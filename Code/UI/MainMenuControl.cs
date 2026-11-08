using Godot;
using Thrust.Systems;
using Thrust.Systems.State;

namespace Thrust.UI
{
    public partial class MainMenuControl : Control
    {
        [Export] private Button _newGameButton = null;
        [Export] private Button _settingsButton = null;
        [Export] private Button _quitGameButton = null;

        public override void _EnterTree()
        {
            // TODO: Subscribe to the rest of the buttons pressed signals as well.
            _newGameButton.Pressed += OnNewGamePressed;
            _settingsButton.Pressed += OnSettingsPressed;
            _quitGameButton.Pressed += OnQuitGamePressed;
        }

        public override void _ExitTree()
        {
            _newGameButton.Pressed -= OnNewGamePressed;
            _settingsButton.Pressed -= OnSettingsPressed;
            _quitGameButton.Pressed -= OnQuitGamePressed;
        }

        private void OnNewGamePressed()
        {
            if (!GameManager.Instance.StartNewGame())
            {
                GD.PushError("Failed to start a new game.");
            }
        }

        private void OnSettingsPressed()
        {
            if (!GameManager.Instance.ChangeState(GameStateType.Settings))
            {
                GD.PushError("Failed to change state to Settings.");
            }
        }

        private void OnQuitGamePressed()
        {
            GetTree().Quit();
        }
    }
}