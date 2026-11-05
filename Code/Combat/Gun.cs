using Godot;
using System;

namespace Thrust.Combat{
public partial class Gun : Node2D
{
	[Export] private PackedScene _bulletScene = null;
	[Export] private float _cooldown = 1.2f;
	
	private float _cooldownLeft;
	public bool CanFire
	{
		get
		{
			return _bulletScene != null && _cooldownLeft <= 0f;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaTime = (float) delta;

		if(_cooldownLeft > 0f)
		{
			_cooldownLeft -= deltaTime;
		}
	}


	public bool Fire(Vector2 direction)
	{
		if (!CanFire)
		{
			return false;
		}


		Bullet bullet = _bulletScene.Instantiate<Bullet>();

		GetTree().CurrentScene.AddChild(bullet);
		bullet.Launch(GlobalPosition, direction);

		_cooldownLeft = _cooldown;
		
		return true;
	}
}
}
