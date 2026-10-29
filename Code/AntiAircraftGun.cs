using Godot;
using System;
using Thrust.Configuration;

namespace Thrust.Characters{
public partial class AntiAircraftGun : StaticBody2D
	{
		[Export] private float _range = 300f;
		[Export] private float _speed = 2.5f;
		[Export] private Node2D _headPivot;

		[Export] private Vector2 _direction = new Vector2(1,-1);
		private Rocket _rocket;

		private float _aimAngle = -Mathf.Pi / 2;

		public float Range{get {return _range;}}
		public float Speed{get {return _speed;}}

		public Vector2 Direction{get {return _direction;}}

		

		public override void _Ready()
		{

			if(_headPivot == null)
			{
				GD.PushError("Head pivot is not found!");
			}

			_rocket = GetTree().GetFirstNodeInGroup(Config.RocketGroupName) as Rocket;

			if (_rocket == null)
			{
				GD.PushError("Rocket is not found!");
			}
			
		}

		public override void _PhysicsProcess(double delta)
		{
			float deltaTime = (float)delta;
			if (_rocket == null || _headPivot == null) { return; }

			if (GlobalPosition.DistanceTo(_rocket.GlobalPosition) > _range) { return; }


			Vector2 direction = _rocket.GlobalPosition - _headPivot.GlobalPosition;
			float targetAngle = direction.Angle() - GlobalRotation;


			_aimAngle = Mathf.Wrap(
				Mathf.RotateToward(_aimAngle, targetAngle, Speed * deltaTime),
				-Mathf.Pi, Mathf.Pi);


			_headPivot.Rotation = _aimAngle + Mathf.Pi / 2;
		}

		
	}
}
