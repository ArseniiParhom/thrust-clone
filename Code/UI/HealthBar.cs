using Godot;
using System;
using Thrust.Characters;
namespace Thrust.UI{
public partial class HealthBar : TextureProgressBar
	{
		private Rocket _rocket;

		public void Bind(Rocket rocket)
		{
			if(rocket == null)
			{
				GD.PushError("No rocket found to assign the health bar");
				return;
			}

			_rocket = rocket;

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
