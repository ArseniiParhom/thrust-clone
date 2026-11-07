namespace Thrust.Systems.State
{
    public class SettingsState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.Settings;
        public override string ScenePath => "res://Scenes/Menus/Settings.tscn";

        public SettingsState()
        {
            AddTargetState(GameStateType.MainMenu);
        }
    }
}