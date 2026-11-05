using Godot;
using Thrust.Configuration;

namespace Thrust.Systems
{
	public partial class GameManager : Node
	{
		#region Singleton
		public static GameManager Instance { get; private set; }

		public GameManager()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else if (Instance != this)
			{
				GD.PushWarning("GameManager instance already exists. Destroying duplicate.");
				QueueFree();
				return;
			}
		}
		#endregion Singleton
		
		[Signal] public delegate void ScoreChangedEventHandler(int newScore);
		[Signal] public delegate void LivesChangedEventHandler(int newLives);

		private int _score = 0;
		private int _lives = Config.InitialLives;

		#region Properties
		public int Score
		{
			get { return _score; }
			private set
			{
				_score = value;
				EmitSignal(SignalName.ScoreChanged, _score);
			}
		}
		public int Lives
		{
			get { return _lives; }
			private set
			{
				_lives = value;
				EmitSignal(SignalName.LivesChanged, _lives);
			}
		}
		#endregion Properties

		#region Accessors
		// TODO: Add methods here to modify GameManagers Properties
		#endregion Accessors
	}
}