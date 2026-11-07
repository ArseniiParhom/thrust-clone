using Godot;
using System.Collections.Generic;


namespace Thrust.Systems.State
{
    public abstract class GameStateBase
    {
        /// <summary>
        /// The type of the game state
        /// </summary>
        public abstract GameStateType StateType { get; }
        /// <summary>
        /// The path to the scene file associated with this state.
        /// </summary>
        public abstract string ScenePath { get; }
        /// <summary>
        /// Should the associated scene be loaded additively or not.
        /// </summary>
        public virtual bool IsAdditive => false;

        /// <summary>
        /// The list of states this state can transition to.
        /// </summary>
        private HashSet<GameStateType> _targetStates = new HashSet<GameStateType>();

        /// <summary>
        /// The reference to the scene resource associeated with this state.
        /// </summary>
        private PackedScene _sceneResource = null;

        /// <summary>
        /// A reference to the scenen assoviated with this state
        /// </summary>
        private Node _scene = null;
        protected GameStateBase()
        {

            // Preload the scene file and cache the result.
            // Makes loading states a bit faster sinche they are already in memory.
            _sceneResource = GD.Load<PackedScene>(ScenePath);
            if (_sceneResource == null)
            {
                GD.PrintErr($"Failed to load scene resource for state {StateType} at path {ScenePath}");
            }
            // GameStateBase constructor, after GD.Load
            GD.Print($"[{StateType}] scene loaded: {_sceneResource != null} ({ScenePath})");
        }

        protected bool AddTargetState(GameStateType gameStateType)
        {
            return _targetStates.Add(gameStateType);
        }
         protected bool RemoveTargetState(GameStateType gameStateType)
        {
            return _targetStates.Remove(gameStateType);    
        }

        public bool CanTransitionTo(GameStateType targetStateType)
        {
            return _targetStates.Contains(targetStateType);
        }

        /// <summary>
        /// State activation logic. This should be run just after the state is activated.
        /// </summary>
        /// <param name="forceLoad"></param>
        public virtual void OnEnter(bool forceLoad = false)
        {
            if (forceLoad || _scene == null)
            {
                // Scene is not loaded or reloading it is forced
                if (_scene != null)
                {
                    _scene.QueueFree();
                }
            
                _scene = _sceneResource.Instantiate();
            
                // TODO: Show what happens if scene addition is not delayed.
                GameManager.Instance.SceneTree.Root.CallDeferred(Node.MethodName.AddChild, _scene);

                // TODO: Threaded level loading: https://docs.godotengine.org/en/stable/tutorials/io/background_loading.html
            }
            // GetTree().Root.AddChild(_score);
        }

        /// <summary>
        /// State deactivation logic. This should be run just before the state is deactivated.
        /// </summary>
        /// <param name="keepLoaded"></param>
        public virtual void OnExit(bool keepLoaded)
        {
            if (!keepLoaded && _scene != null)
            {
                // This scene should be unloaded and it is still loaded. Unload it.
                _scene.QueueFree();
                _scene = null;
            }
        }
    }
}