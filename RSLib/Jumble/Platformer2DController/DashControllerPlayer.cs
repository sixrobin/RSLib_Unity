namespace RSLib.Jumble.Platformer2D
{
    public class DashControllerPlayer : DashController
    {
        private PlayerController _playerController;

        public DashControllerPlayer(Platformer2DController controller) : base(controller)
        {
            _playerController = _controller as PlayerController;
            UnityEngine.Assertions.Assert.IsNotNull(_playerController, $"Could not cast {nameof(_controller)} as a {nameof(PlayerController)}!");
        }

        public override bool CanDash()
        {
            return base.CanDash() && _playerController.InputController.CheckInput(InputController.ButtonCategory.DASH);
        }
    }
}