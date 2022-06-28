namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    public class Platformer2DControllerView : MonoBehaviour
    {
        [SerializeField] private Platformer2DController _platformer2DController = null;
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private Animator _animator = null;
        
        private PlayerController PlayerController => _platformer2DController as PlayerController;
        
        private void OnJumped(bool airborne)
        {
            _animator.ResetTrigger("Idle");
        }

        private void OnLanded(bool impact)
        {
            _animator.SetTrigger("Idle");
        }

        private void OnLandImpactOver()
        {
        }
        
        private void OnDashStarted(DashController.DashStartedEventArgs args)
        {
            _animator.ResetTrigger("Idle");
            _animator.SetTrigger(args.Airborne ? "Dash" : "Slide");
        }
        
        private void OnDashOver(DashController.DashOverEventArgs args)
        {
            _animator.SetTrigger("Idle");
        }

        private System.Collections.IEnumerator Start()
        {
            yield return new WaitUntil(() => _platformer2DController.Initialized);
            
            _platformer2DController.JumpController.Jumped += OnJumped;
            _platformer2DController.JumpController.Landed += OnLanded;
            _platformer2DController.JumpController.LandedImpactOver += OnLandImpactOver;
            _platformer2DController.DashController.DashStarted += OnDashStarted;
            _platformer2DController.DashController.DashOver += OnDashOver;
        }

        private void Update()
        {
            _spriteRenderer.flipX = _platformer2DController.CurrentDirection < 0f;
            
            _animator.SetBool("IsRunning", _platformer2DController.CollisionsController.CurrentState.Grounded && PlayerController.InputController.Horizontal != 0f);
            _animator.SetBool("IsGrounded", _platformer2DController.CollisionsController.CurrentState.Grounded);
            _animator.SetFloat("VelocityY", _platformer2DController.CurrentVelocity.y);
        }

        private void OnDestroy()
        {
            _platformer2DController.JumpController.Jumped -= OnJumped;
            _platformer2DController.JumpController.Landed -= OnLanded;
            _platformer2DController.JumpController.LandedImpactOver -= OnLandImpactOver;
            _platformer2DController.DashController.DashStarted -= OnDashStarted;
            _platformer2DController.DashController.DashOver -= OnDashOver;
        }
    }
}