namespace RSLib.Jumble.Platformer2D
{
    using RSLib.Extensions;
    using UnityEngine;

    public class CollisionsController : RaycastsController
    {
        /// <summary>
        /// Handles the collisions state for a controller.
        /// Stores the collision info for each collision type.
        /// </summary>
        public class CollisionsState
        {
            private System.Collections.Generic.Dictionary<CollisionOrigin, bool> _state = new System.Collections.Generic.Dictionary<CollisionOrigin, bool>(new RSLib.Framework.Comparers.EnumComparer<CollisionOrigin>())
            {
                { CollisionOrigin.ABOVE, false },
                { CollisionOrigin.BELOW, false },
                { CollisionOrigin.LEFT, false },
                { CollisionOrigin.RIGHT, false },
                { CollisionOrigin.EDGE, false },
                { CollisionOrigin.SLOPE_CLIMB, false },
                { CollisionOrigin.SLOPE_DESCEND, false }
            };

            private CollisionOrigin[] _keys;
            
            public CollisionsState()
            {
                int i = 0;
                _keys = new CollisionOrigin[_state.Count];
                foreach (System.Collections.Generic.KeyValuePair<CollisionOrigin, bool> state in _state)
                    _keys[i++] = state.Key;
            }

            public float SlopeAngle = -1f;
            
            #region READONLY HELPER PROPERTIES
            
            public bool Above => Get(CollisionOrigin.ABOVE);
            public bool Below => Get(CollisionOrigin.BELOW);
            public bool Left => Get(CollisionOrigin.LEFT);
            public bool Right => Get(CollisionOrigin.RIGHT);
            
            public bool Edge => Get(CollisionOrigin.EDGE);
            
            public bool SlopeClimb => Get(CollisionOrigin.SLOPE_CLIMB);
            public bool SlopeDescend => Get(CollisionOrigin.SLOPE_DESCEND);
            public bool Slope => SlopeClimb || SlopeDescend;
            
            public bool Horizontal => Get(CollisionOrigin.RIGHT) || Get(CollisionOrigin.LEFT);
            public bool Vertical => Get(CollisionOrigin.ABOVE) || Get(CollisionOrigin.BELOW);

            public bool Grounded => Below || SlopeClimb || SlopeDescend;
            
            #endregion
            
            public void Reset()
            {
                for (int i = 0; i < _keys.Length; ++i)
                    _state[_keys[i]] = false;

                SlopeAngle = -1f;
            }

            public void CopyFrom(CollisionsState source)
            {
                for (int i = 0; i < _keys.Length; ++i)
                    _state[_keys[i]] = source.Get(_keys[i]);

                SlopeAngle = source.SlopeAngle;
            }

            public void Set(CollisionOrigin origin, bool state = true)
            {
                UnityEngine.Assertions.Assert.IsTrue(origin != CollisionOrigin.NONE);
                _state[origin] = state;
            }

            public bool Get(CollisionOrigin origin)
            {
                UnityEngine.Assertions.Assert.IsTrue(origin != CollisionOrigin.NONE);
                return _state[origin];
            }

            public override string ToString()
            {
                string collisionsStr = string.Empty;
                foreach (System.Collections.Generic.KeyValuePair<CollisionOrigin,bool> state in _state)
                    if (state.Value)
                        collisionsStr += $"{state.Key},";
                
                collisionsStr = collisionsStr.RemoveLast(1);
                return collisionsStr;
            }
        }

        /// <summary>
        /// Arguments stored to handle detected collision events.
        /// </summary>
        public class CollisionDetectedEventArgs : System.EventArgs
        {
            public CollisionDetectedEventArgs(CollisionOrigin origin, RaycastHit2D hit)
            {
                Origin = origin;
                Hit = hit;
            }

            public CollisionOrigin Origin { get; }
            public RaycastHit2D Hit { get; }
        }

        /// <summary>
        /// Parameters used when checking controller collisions.
        /// </summary>
        public struct CollisionComputationInfo
        {
            public bool DontResetCurrentCollisionState;
            public bool TriggerEvents;
            public bool CheckEdge;
            public bool EffectorDown;
            public bool OnMovingPlatform;

            /// <summary>
            /// Default computation info.
            /// </summary>
            public static CollisionComputationInfo Default => new CollisionComputationInfo()
            {
                TriggerEvents = true
            };
        }
        
        [System.Flags]
        public enum CollisionOrigin
        {
            NONE,
            ABOVE,
            BELOW,
            LEFT,
            RIGHT,
            EDGE,
            SLOPE_CLIMB,
            SLOPE_DESCEND
        }
        
        public CollisionsController(BoxCollider2D boxCollider2D, Platformer2DController controller, LayerMask collisionMask, LayerMask dashCollisionMask) : base(boxCollider2D)
        {
            _controller = controller;
            _collisionMask = collisionMask;
            _dashCollisionMask = dashCollisionMask;
        }
        
        private Platformer2DController _controller;
        private LayerMask _collisionMask;
        private LayerMask _dashCollisionMask;

        // Shared data.
        private static System.Collections.Generic.Dictionary<Collider2D, SideTriggerOverrider> s_sharedKnownSideTriggerOverriders = new System.Collections.Generic.Dictionary<Collider2D, SideTriggerOverrider>();
        private static System.Collections.Generic.Dictionary<Collider2D, PlatformEffector> s_sharedKnownEffectors = new System.Collections.Generic.Dictionary<Collider2D, PlatformEffector>();

        // Values stored to do something after movement has been applied and collision state updated.
        private System.Collections.Generic.Queue<CollisionDetectedEventArgs> _detectedCollisionsForEvent = new System.Collections.Generic.Queue<CollisionDetectedEventArgs>();

        public delegate void CollisionDetectedEventHandler(CollisionDetectedEventArgs args);
        public delegate void EffectorDownEventHandler(PlatformEffector effector);
        public event CollisionDetectedEventHandler CollisionDetected;
        public event EffectorDownEventHandler EffectorDown;

        public CollisionsState CurrentState { get; } = new CollisionsState();
        public CollisionsState PreviousState { get; } = new CollisionsState();
        
        public bool AboveEffector { get; private set; }
        
        /// <summary>
        /// Computes the mask to use for collisions detection, depending on the current controller state.
        /// Can be overriden to specify collision mask even more.
        /// </summary>
        /// <returns>LayerMask used for collisions detection.</returns>
        protected virtual LayerMask ComputeCollisionMask()
        {
            return _controller.DashController.IsDashing ? _dashCollisionMask : _collisionMask;
        }

        /// <summary>
        /// Process all the detected collisions once all possible collisions have been computed.
        /// </summary>
        public void ProcessDetectedCollisions()
        {
            while (_detectedCollisionsForEvent.Count > 0)
            {
                // Done in two separate instructions, else loop will be infinite if event has no listener.
                CollisionDetectedEventArgs collision = _detectedCollisionsForEvent.Dequeue();
                CollisionDetected?.Invoke(collision);
            }
        }

        /// <summary>
        /// Instantly sets the controller position to the nearest ground detected below if one is found.
        /// Can be used on game start to initialize the controller position.
        /// </summary>
        /// <param name="transform">Controller's transform.</param>
        /// <param name="triggerEvent">Should trigger collision detection events.</param>
        public void Ground(Transform transform, bool triggerEvent = false)
        {
            for (int i = 0; i < VerticalRaycastsCount; ++i)
            {
                Vector2 rayOrigin = RaycastsOrigins.BottomLeft + Vector2.right * i * VerticalRaycastsSpacing;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, _collisionMask);
                if (!hit)
                    continue;
                
                CurrentState.Set(CollisionOrigin.BELOW);
                if (triggerEvent)
                    CollisionDetected?.Invoke(new CollisionDetectedEventArgs(CollisionOrigin.BELOW, hit));

                transform.Translate(new Vector3(0f, -hit.distance + SKIN_WIDTH));
                return;
            }

            CProLogger.LogWarning(this, $"No ground has been found to ground {transform.name}.");
        }

        /// <summary>
        /// Backups the current collisions controller state.
        /// Should be called at the beginning of each frame before doing movement computations.
        /// </summary>
        public void BackupCurrentState()
        {
            PreviousState.CopyFrom(CurrentState);
        }

        #region COLLISIONS COMPUTATION

        /// <summary>
        /// Registers a collision info when detected, by enqueuing it.
        /// Registered collisions can be used afterwards to perform some behaviour once all possible collisions have been computed.
        /// </summary>
        /// <param name="args">Collision to register info.</param>
        private void RegisterCollision(CollisionDetectedEventArgs args)
        {
            _detectedCollisionsForEvent.Enqueue(args);
        }
        
        /// <summary>
        /// Modifies a velocity vector based on current controller position and detected collisions of all types.
        /// </summary>
        /// <param name="velocity">Movement velocity (scaled by Time.deltaTime beforehand).</param>
        /// <param name="collisionComputationInfo">Custom collision computation parameters.</param>
        /// <returns>Modified velocity vector.</returns>
        public Vector3 ComputeCollisions(Vector3 velocity, CollisionComputationInfo collisionComputationInfo)
        {
            ComputeRaycastOrigins();
            
            if (!collisionComputationInfo.DontResetCurrentCollisionState)
                CurrentState.Reset();

            if (velocity.y < 0f)
                DescendSlope(ref velocity);
            
            if (velocity.x != 0f)
                ComputeHorizontalCollisions(ref velocity, collisionComputationInfo);

            if (velocity.y != 0f)
                ComputeVerticalCollisions(ref velocity, collisionComputationInfo);

            if (collisionComputationInfo.OnMovingPlatform)
                CurrentState.Set(CollisionOrigin.BELOW);

            return velocity;
        }

        /// <summary>
        /// Modifies a velocity vector based on current controller position and detected horizontal collisions.
        /// </summary>
        /// <param name="velocity">Movement velocity (scaled by Time.deltaTime beforehand).</param>
        /// <param name="collisionComputationInfo">Custom collision computation parameters.</param>
        /// <returns>Modified velocity vector.</returns>
        public void ComputeHorizontalCollisions(ref Vector3 velocity, CollisionComputationInfo collisionComputationInfo)
        {
            float sign = Mathf.Sign(velocity.x);
            float length = velocity.x * sign + SKIN_WIDTH;

            for (int i = 0; i < HorizontalRaycastsCount; ++i)
            {
                Vector2 rayOrigin = (sign > 0f ? RaycastsOrigins.BottomRight : RaycastsOrigins.BottomLeft) + Vector2.up * i * HorizontalRaycastsSpacing;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * sign, length, ComputeCollisionMask());

                if (!hit)
                {
                    // No wall or slope -> there may be an edge.
                    if (collisionComputationInfo.CheckEdge)
                        ComputeEdgeCollisions(ref velocity, collisionComputationInfo);

                    Debug.DrawRay(rayOrigin, Vector2.right * sign, Color.yellow);
                }
                
                if (hit.distance <= Mathf.Epsilon)
                    continue;
                
                bool checkSlope = i == 0;
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (!ComputeHorizontalHit(hit, sign))
                    continue;

                Debug.DrawRay(rayOrigin, Vector2.right * sign, Color.red);

                if (checkSlope)
                    ComputeSlopeHit(hit, sign, ref velocity, slopeAngle);

                if (!CurrentState.SlopeClimb || slopeAngle > _controller.ControllerData.MaxSlopeClimbAngle)
                {
                    length = hit.distance;
                    velocity.x = (length - SKIN_WIDTH) * sign;

                    // Handle wall collision when climbing a slope.
                    if (CurrentState.SlopeClimb)
                        velocity.y = Mathf.Tan(CurrentState.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    
                    CurrentState.Set(CollisionOrigin.LEFT, sign < 0f);
                    CurrentState.Set(CollisionOrigin.RIGHT, sign > 0f);

                    if (collisionComputationInfo.TriggerEvents)
                        RegisterCollision(new CollisionDetectedEventArgs(CurrentState.Get(CollisionOrigin.LEFT) ? CollisionOrigin.LEFT : CollisionOrigin.RIGHT, hit));
                }
            }
        }

        /// <summary>
        /// Modifies a velocity vector based on current controller position and detected vertical collisions.
        /// </summary>
        /// <param name="velocity">Movement velocity (scaled by Time.deltaTime beforehand).</param>
        /// <param name="collisionComputationInfo">Custom collision computation parameters.</param>
        /// <returns>Modified velocity vector.</returns>
        public void ComputeVerticalCollisions(ref Vector3 velocity, CollisionComputationInfo collisionComputationInfo)
        {
            float sign = Mathf.Sign(velocity.y);
            float length = velocity.y * sign + SKIN_WIDTH;

            for (int i = 0; i < VerticalRaycastsCount; ++i)
            {
                Vector2 rayOrigin = (sign > 0f ? RaycastsOrigins.TopLeft : RaycastsOrigins.BottomLeft) + Vector2.right * i * VerticalRaycastsSpacing;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * sign, length, ComputeCollisionMask());

                if (!hit)
                {
                    Debug.DrawRay(rayOrigin, Vector2.up * sign, Color.yellow);
                    continue;
                }
                
                if (hit.distance <= Mathf.Epsilon)
                    continue;
                
                if (!ComputeVerticalHit(hit, sign, collisionComputationInfo.EffectorDown))
                    continue;

                Debug.DrawRay(rayOrigin, Vector2.up * sign, Color.red);

                length = hit.distance;
                velocity.y = (length - SKIN_WIDTH) * sign;

                // Handle ceil collision when climbing a slope.
                if (CurrentState.SlopeClimb)
                    velocity.x = velocity.y / Mathf.Tan(CurrentState.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                
                CurrentState.Set(CollisionOrigin.ABOVE, sign > 0f);
                CurrentState.Set(CollisionOrigin.BELOW, sign < 0f);

                if (collisionComputationInfo.TriggerEvents)
                    RegisterCollision(new CollisionDetectedEventArgs(CurrentState.Get(CollisionOrigin.ABOVE) ? CollisionOrigin.ABOVE : CollisionOrigin.BELOW, hit));
            }

            // Handle multiple following slopes.
            if (CurrentState.SlopeClimb)
            {
                float signX = Mathf.Sign(velocity.x);
                length = Mathf.Abs(velocity.x) + SKIN_WIDTH;
                Vector2 rayOrigin = (signX < 0f ? RaycastsOrigins.BottomLeft : RaycastsOrigins.BottomRight) + Vector2.up * velocity.y;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * signX, length, ComputeCollisionMask());

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (slopeAngle != CurrentState.SlopeAngle)
                    {
                        velocity.x = (hit.distance - SKIN_WIDTH) * signX;
                        CurrentState.SlopeAngle = slopeAngle;
                    }
                }
            }
        }

        /// <summary>
        /// Modifies a velocity vector based on current controller position and detected platform edge.
        /// Basically nullifies horizontal velocity if controller is on a platform's edge.
        /// </summary>
        /// <param name="velocity">Movement velocity (scaled by Time.deltaTime beforehand).</param>
        /// <param name="collisionComputationInfo">Custom collision computation parameters.</param>
        /// <returns>Modified velocity vector.</returns>
        public void ComputeEdgeCollisions(ref Vector3 velocity, CollisionComputationInfo collisionComputationInfo)
        {
            float sign = Mathf.Sign(velocity.x);
            Vector2 rayOrigin = (sign > 0f ? RaycastsOrigins.BottomRight : RaycastsOrigins.BottomLeft) + new Vector2(velocity.x, 0f);
            Debug.DrawRay(rayOrigin, Vector2.down, Color.green);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, EDGE_MIN_HEIGHT, ComputeCollisionMask());
            if (!hit)
            {
                velocity.x = 0f;
                CurrentState.Set(CollisionOrigin.EDGE);

                if (collisionComputationInfo.TriggerEvents)
                    RegisterCollision(new CollisionDetectedEventArgs(CollisionOrigin.EDGE, hit));
            }
        }
        
        #endregion // COLLISIONS COMPUTATION

        #region RAYCAST HIT COMPUTATION

        /// <summary>
        /// Computes info related to any hit, vertical or horizontal.
        /// Checks if specifics components are found on colliders, to handle effector, etc. and do the early
        /// return checks that are not related to a specific hit direction.
        /// </summary>
        /// <param name="hit">RaycastHit2D to compute.</param>
        /// <returns>True if hit should stop movement, else false.</returns>
        private bool ComputeHitBase(RaycastHit2D hit)
        {
            if (!s_sharedKnownSideTriggerOverriders.TryGetValue(hit.collider, out SideTriggerOverrider sideTriggerOverrider))
                if (hit.collider.TryGetComponent(out sideTriggerOverrider))
                    s_sharedKnownSideTriggerOverriders.Add(hit.collider, sideTriggerOverrider);

            if (!s_sharedKnownEffectors.TryGetValue(hit.collider, out PlatformEffector effector))
                if (hit.collider.TryGetComponent(out effector))
                    s_sharedKnownEffectors.Add(hit.collider, effector);

            if (hit.collider.isTrigger)
                return false;

            return true;
        }

        /// <summary>
        /// Computes info related to horizontal movement.
        /// First checks base computations, then do specific ones.
        /// </summary>
        /// <param name="hit">RaycastHit2D to compute.</param>
        /// <param name="sign">Direction sign.</param>
        /// <returns>True if hit should stop movement, else false.</returns>
        private bool ComputeHorizontalHit(RaycastHit2D hit, float sign)
        {
            if (!ComputeHitBase(hit))
                return false;

            s_sharedKnownSideTriggerOverriders.TryGetValue(hit.collider, out SideTriggerOverrider sideTriggerOverrider);
            s_sharedKnownEffectors.TryGetValue(hit.collider, out PlatformEffector effector);

            // Side trigger overrider detected with a valid trigger direction -> go through.
            if (sideTriggerOverrider != null && sideTriggerOverrider.IsSideSetAsTrigger(sign > 0f ? CollisionOrigin.LEFT : CollisionOrigin.RIGHT))
                return false;

            if (effector != null)
                return false;
            
            return true;
        }
        
        /// <summary>
        /// Computes info related to vertical movement.
        /// First checks base computations, then do specific ones.
        /// </summary>
        /// <param name="hit">RaycastHit2D to compute.</param>
        /// <param name="sign">Direction sign.</param>
        /// <param name="downEffector">Is the player trying to get down through a platform effector.</param>
        /// <returns>True if hit should stop movement, else false.</returns>
        private bool ComputeVerticalHit(RaycastHit2D hit, float sign, bool downEffector)
        {
            if (!ComputeHitBase(hit))
                return false;

            s_sharedKnownSideTriggerOverriders.TryGetValue(hit.collider, out SideTriggerOverrider sideTriggerOverrider);
            s_sharedKnownEffectors.TryGetValue(hit.collider, out PlatformEffector effector);

            // Side trigger overrider detected with a valid trigger direction -> go through.
            if (sideTriggerOverrider != null && sideTriggerOverrider.IsSideSetAsTrigger(sign > 0f ? CollisionOrigin.BELOW : CollisionOrigin.ABOVE))
                return false;

            // Effector detected but going up -> go through.
            if (effector != null && sign > 0f)
                return false;

            AboveEffector = sign < 0f && effector != null;
            if (downEffector && AboveEffector && effector != null && !effector.BlockDown)
            {
                EffectorDown?.Invoke(effector);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Computes info related to slope climbing (going up).
        /// </summary>
        /// <param name="hit">RaycastHit2D to compute.</param>
        /// <param name="sign">Direction sign.</param>
        /// <param name="velocity">Velocity of the controller before hitting the slope.</param>
        /// <param name="slopeAngle">Slope angle of slope normal to up vector.</param>
        private void ComputeSlopeHit(RaycastHit2D hit, float sign, ref Vector3 velocity, float slopeAngle)
        {
            if (slopeAngle > _controller.ControllerData.MaxSlopeClimbAngle)
                return;
            
            float slopeDistanceX = 0f;
            if (Mathf.Abs(slopeAngle - PreviousState.SlopeAngle) > 0.001f)
            {
                slopeDistanceX = hit.distance - SKIN_WIDTH;
                velocity.x -= slopeDistanceX * sign;
            }
                
            ClimbSlope(ref velocity, slopeAngle);
            velocity.x += slopeDistanceX * sign;
        }
        
        /// <summary>
        /// Computes slope climbing (going up) velocity.
        /// </summary>
        /// <param name="velocity">Velocity of the controller before descending the slope.</param>
        /// <param name="slopeAngle">Slope angle of slope normal to up vector.</param>
        private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
        {
            float moveDistance = Mathf.Abs(velocity.x);
            float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
           
            // Use slope velocity only if controller is not jumping.
            if (climbVelocityY > velocity.y)
            {
                velocity.y = climbVelocityY;
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                
                CurrentState.Set(CollisionOrigin.BELOW);
                CurrentState.Set(CollisionOrigin.SLOPE_CLIMB);
                CurrentState.SlopeAngle = slopeAngle;
            }
        }

        /// <summary>
        /// Computes slope descending (going down) velocity.
        /// </summary>
        /// <param name="velocity">Velocity of the controller before descending the slope.</param>
        public void DescendSlope(ref Vector3 velocity)
        {
            float sign = Mathf.Sign(velocity.x);
            Vector2 rayOrigin = sign < 0f ? RaycastsOrigins.BottomRight : RaycastsOrigins.BottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, ComputeCollisionMask());

            if (hit)
            {
                float slopeAngle = Vector2.Angle(Vector2.up, hit.normal);
                if (slopeAngle != 0f && slopeAngle <= _controller.ControllerData.MaxSlopeDescendAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == sign)
                    {
                        if (hit.distance - SKIN_WIDTH <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.y))
                        {
                            float moveDistance = Mathf.Abs(velocity.x);
                            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * sign;
                            velocity.y -= descendVelocityY;

                            CurrentState.Set(CollisionOrigin.BELOW);
                            CurrentState.Set(CollisionOrigin.SLOPE_DESCEND);
                            CurrentState.SlopeAngle = slopeAngle;
                        }
                    }
                }
            }
        }
        
        #endregion // RAYCAST HIT COMPUTATION
    }
}