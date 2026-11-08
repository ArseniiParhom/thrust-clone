using Godot;

namespace Thrust.Systems.State
{
    public class InGameState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.InGame;
        public override string ScenePath => "res://Scenes/Levels/MainLevel.tscn";
        public InGameState() : base()
        {
            AddTargetState(GameStateType.Pause);
            AddTargetState(GameStateType.Win);
            AddTargetState(GameStateType.GameOver);
            AddTargetState(GameStateType.InGame); // For level completion
        }

        public override void OnEnter(bool forceLoad = false)
        {
            base.OnEnter(forceLoad);

            GameManager.Instance.SceneTree.SetDeferred(SceneTree.PropertyName.CurrentScene, _scene);
        }

        public override void OnExit(bool keepLoaded)
        {
            // Call parent class's implementation first.
            base.OnExit(keepLoaded);

            // Additional logic for entering the InGame state can be added here.
        }
    }
}