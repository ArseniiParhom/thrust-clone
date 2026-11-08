using Godot;
using System;
using System.Collections;
using Thrust.Combat;

public partial class Bullet : Area2D
{
	[Export] private float _speed = 150f;
	[Export] private int _damage = 1;
	[Export] private float _lifetime = 3f;
	[Export] private float _length = 7f;
	[Export] private float _width = 3f;
	[Export] private Color _color = new Color(1f, 0.45f, 0.35f);

	private Vector2 _direction = Vector2.Right;
	private float _age;

	#region Properties 

	public float Speed
	{
		get {return _speed;}
		set {_speed = value;}
	}

	public Vector2 Direction{
		get {return _direction;}
		private set {_direction = value.Normalized();}
	}
	#endregion Properties 


	public void Launch(Vector2 position, Vector2 direction)
	{
		GlobalPosition = position;
		Direction = direction;
		Rotation = Direction.Angle();
	}

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
    }


    public override void _ExitTree()
    {
        BodyEntered -= OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaTime = (float) delta;

		GlobalPosition += Direction * Speed * deltaTime;

		_age += deltaTime;

		// vanishes, when no collision occurs during its lifetime
		if (_age >= _lifetime){
			QueueFree();
		}

    }

    public override void _Draw()
    {
		DrawLine(new Vector2(-_length, 0), Vector2.Zero, _color, _width);
    }
	
	
	private void OnBodyEntered(Node2D body)
    {
		if (body is IDamageable damageableTarget)
		{
			damageableTarget.TakeDamage(_damage);
		}

		QueueFree();
    }


}
