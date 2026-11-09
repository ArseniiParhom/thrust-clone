using System;
using Godot;
using Thrust.Combat;
using Thrust.Configuration;
using Thrust.Effects;
using Thrust.Systems;

namespace Thrust.Characters
{
	public partial class Rocket : RigidBody2D, IDamageable
	{
		[Signal] public delegate void HealthChangedEventHandler(int current, int max);
		[Signal] public delegate void ExplodedEventHandler();
		[Export] public float ThrustForce = 300f;
		[Export] public float RotationTorque = 900f;

		[Export] public float MaxLinearSpeed = 160f;
		[Export] public float MaxAngularSpeed = 100f;
		[Export] private Gun _gun ;
		[Export] private PackedScene _explosionScene;

		[Export] private int _maxHealth = 3;
		private int _health;

		[Export] private CpuParticles2D _flameParticles;
		[Export] public float ThrustRampSpeed = 4;
		private float _thrustLevel = 0;

		private float _rocketSize = 1f;

		[Export(PropertyHint.Range, "0.4,1.2,0.05")]
		public float RocketSize
		{
			get => _rocketSize;
			set
			{
				_rocketSize = Mathf.Clamp(value, 0.4f, 1.2f);

				if (IsNodeReady())
					ApplyRocketSize();
			}
		}

		public int MaxHealth
		{
			get { return _maxHealth;}
		}

		public int CurrentHealth
		{
			get { return _health; }
			private set { _health = value; }
		}

		public bool IsDead
		{
			get {return _health <= 0;}
		}

		public override void _Ready()
		{
			ApplyRocketSize();
			CurrentHealth = MaxHealth;

		}

		private void ApplyRocketSize()
		{
			float size = RocketSize;

			var sprite = GetNode<Sprite2D>("Sprite2D");
			var body = GetNode<CollisionShape2D>("BodyCollision");
			var leftFoot = GetNode<CollisionShape2D>("LeftFootCollision");
			var rightFoot = GetNode<CollisionShape2D>("RightFootCollision");

			// Scale the visual representation.
			sprite.Scale = Vector2.One * size;

			// Resize the body collision without scaling the physics node.
			body.Position = new Vector2(0, -4) * size;
			body.Shape = new CapsuleShape2D
			{
				Radius = 11f * size,
				Height = 74f * size
			};

			// Resize and reposition the landing legs.
			ResizeFoot(
				leftFoot,
				new Vector2(-18f, 39.5f),
				size
			);

			ResizeFoot(
				rightFoot,
				new Vector2(19f, 40f),
				size
			);

			_flameParticles.Position = new Vector2(0, 40f) * size;
			_flameParticles.Scale = Vector2.One * size;
		}

		private static void ResizeFoot(
			CollisionShape2D foot,
			Vector2 originalPosition,
			float size)
		{
			foot.Position = originalPosition * size;
			foot.Shape = new RectangleShape2D
			{
				Size = new Vector2(18f, 7f) * size
			};
		}

		public override void _PhysicsProcess(double delta)
		{
			if (IsDead){return;}

			float rotationInput = Input.GetAxis(
				Config.RotateLeftActionName,
				Config.RotateRightActionName
			);

			if (Mathf.Abs(rotationInput) > 0.01f)
			{
				ApplyTorque(rotationInput * RotationTorque);
			}

			bool isThrusting = Input.IsActionPressed(Config.RocketThrustActionName);
			float target = isThrusting ? 1 : 0;
			_thrustLevel = Mathf.MoveToward(_thrustLevel, target, ThrustRampSpeed * (float)delta);
			if (isThrusting)
			{
				Vector2 thrustDirection = -GlobalTransform.Y;
				ApplyCentralForce(thrustDirection * ThrustForce);
			}
			UpdateFlameParticles(_thrustLevel);

			if (!IsDead && _gun != null && Input.IsActionPressed(Config.ShootActionName))
			{
				_gun.Fire(-GlobalTransform.Y);
			}
		}

		public override void _IntegrateForces(
			PhysicsDirectBodyState2D state)
		{

			if (MaxLinearSpeed > 0f)
			{
				state.LinearVelocity =
					state.LinearVelocity.LimitLength(MaxLinearSpeed);
			}

			if (MaxAngularSpeed > 0f)
			{
				float maxAngularRadians =
					Mathf.DegToRad(MaxAngularSpeed);

				state.AngularVelocity = Mathf.Clamp(
					state.AngularVelocity,
					-maxAngularRadians,
					maxAngularRadians
				);
			}
		}

		public void TakeDamage(int amount)
		{
			GD.Print($"Rocket hit. Damage: {amount}");
			if (IsDead){ return; }
			CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
			EmitSignal(SignalName.HealthChanged, CurrentHealth, MaxHealth);

			if (CurrentHealth == 0)
			{
				Explode();
			}
		}

		private void Explode()
		{
			EmitSignal(SignalName.Exploded);

			SetDeferred(RigidBody2D.PropertyName.Freeze, true);
			SetDeferred(CollisionObject2D.PropertyName.CollisionLayer,0);

			if(_explosionScene == null)
			{
				GameManager.Instance.EndGame();
				return;
			}

			Explosion explosion = _explosionScene.Instantiate<Explosion>();
			explosion.GlobalPosition = GlobalPosition;
			explosion.Finished += OnExplosionFinished;
			GetTree().CurrentScene.AddChild(explosion);
			
		}

		private void OnExplosionFinished()
		{
			GameManager.Instance.EndGame();
		}

		private void UpdateFlameParticles(float thrustLevel)
		{
			_flameParticles.Emitting = thrustLevel > 0.01f;
			_flameParticles.InitialVelocityMin = Mathf.Lerp(250,350, thrustLevel);
			_flameParticles.InitialVelocityMax = Mathf.Lerp(300,400, thrustLevel);
			// _flameParticles.ScaleAmountMax = Mathf.Lerp(0.5f, 1.2f, thrustLevel);
		}
	}
}
