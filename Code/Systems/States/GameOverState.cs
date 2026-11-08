namespace Thrust.Systems.State
{
    public class GameOverState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.GameOver;
        public override string ScenePath => "res://Scenes/Menus/GameOver.tscn";

        public GameOverState()
        {
            AddTargetState(GameStateType.InGame);
            AddTargetState(GameStateType.MainMenu);
        }
    }
}