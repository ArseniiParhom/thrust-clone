using Godot;
using System;

namespace Thrust.Characters
{
	public partial class Fuel : StaticBody2D
	{
		[Export] public int _fuelAmount = 10;
		[Export] private Area2D _fuelArea;
		
		public override void _Ready()
		{
			_fuelArea.BodyEntered += OnBodyEntered;
			_fuelArea.BodyExited += OnBodyExited;
		}
		public override void _Process(double delta)
		{
		}

		private void OnBodyEntered(Node2D body)
		{
			if (body is Rocket rocket)
			{
				rocket.Refuel(_fuelAmount);
				QueueFree();
			}
		}
		private void OnBodyExited(Node2D body)
		{
			if (body is Rocket rocket)
			{
				// Do nothing for now
			}
		}
	}
}