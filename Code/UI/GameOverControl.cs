using Godot;
using Thrust.Systems;
using Thrust.Systems.State;

namespace Thrust.UI
{
    public partial class GameOverControl : Control
    {
        [Export] private Button _retryButton = null;
        [Export] private Button _mainMenuButton = null;
        [Export] private Button _quitButton = null;

        public override void _EnterTree()
        {
            if (_retryButton != null)
            {
                _retryButton.Pressed += OnRetryPressed;
            }
            if (_mainMenuButton != null)
            {
                _mainMenuButton.Pressed += OnMainMenuPressed;
            }
            if (_quitButton != null)
            {
                _quitButton.Pressed += OnQuitPressed;
            }
        }

        public override void _ExitTree()
        {
            if (_retryButton != null)
            {
                _retryButton.Pressed -= OnRetryPressed;
            }
            if (_mainMenuButton != null)
            {
                _mainMenuButton.Pressed -= OnMainMenuPressed;
            }
            if (_quitButton != null)
            {
                _quitButton.Pressed -= OnQuitPressed;
            }
        }

        private void OnRetryPressed()
        {
            if (!GameManager.Instance.StartNewGame())
            {
                GD.PushError("Failed to start a new game.");
            }
        } 

        private void OnMainMenuPressed()
        {
            if (!GameManager.Instance.ChangeState(GameStateType.MainMenu))
            {
                GD.PushError("Failed to change state to MainMenu.");
            }
        }

        private void OnQuitPressed() { GetTree().Quit(); }
    }
}