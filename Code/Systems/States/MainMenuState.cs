namespace Thrust.Systems.State
{
    public class MainMenuState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.MainMenu;
        public override string ScenePath => "res://Scenes/Menus/MainMenu.tscn";

        public MainMenuState()
        {
            AddTargetState(GameStateType.InGame);
            AddTargetState(GameStateType.Settings);
        }
    }
}