using Godot;
using System;
using Thrust.Characters;
namespace Thrust.UI{
public partial class HealthBar : ProgressBar
	{
		// TODO: dont export the rocket here. Health bar won't be directly on the MainLevel
		[Export] private Rocket _rocket;

		public override void _Ready()
		{
			if(_rocket == null)
			{
				GD.PushError("No rocket found to assign the health bar");
				return;
			}

			MaxValue = _rocket.MaxHealth;
			Value = _rocket.MaxHealth;

			_rocket.HealthChanged += OnHealthChanged;
		}

		public override void _ExitTree()
		{
			if(_rocket != null)
			{
				_rocket.HealthChanged -= OnHealthChanged;
			}
		}

		private void OnHealthChanged(int current, int max)
		{
			MaxValue = max;
			Value = current;
		}
	}
}
