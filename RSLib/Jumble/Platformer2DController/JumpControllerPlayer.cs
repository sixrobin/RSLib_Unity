namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;
    
    public class JumpControllerPlayer : JumpController
    {
        private PlayerController _playerController;
        
        public JumpControllerPlayer(Platformer2DController controller) : base(controller)
        {
            _playerController = _controller as PlayerController;
            UnityEngine.Assertions.Assert.IsNotNull(_playerController, $"Could not cast {nameof(_controller)} as a {nameof(PlayerController)}!");
        }

        public override bool CanJump()
        {
            return base.CanJump() && _playerController.InputController.CheckInput(InputController.ButtonCategory.JUMP);
        }
        
        public override void ApplyVariousVelocity(ref Vector3 velocity, float multiplier = 1f)
        {
            if (_playerController.InputController.CheckJumpInput())
                ApplyMaxVelocity(ref velocity, multiplier);
            else
                ApplyMinVelocity(ref velocity, multiplier);
        }
    }
}