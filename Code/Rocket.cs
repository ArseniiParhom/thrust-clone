using Godot;
using Thrust.Configuration;


namespace Thrust.Characters {
public partial class Rocket : RigidBody2D
{
	[Export] public float ThrustForce = 300f;
	[Export] public float RotationTorque = 900f;

	[Export] public float MaxLinearSpeed = 160f;
	[Export] public float MaxAngularSpeed = 100f;

	public override void _PhysicsProcess(double delta)
	{
		// Rotate the rocket using physics torque
		float rotationInput = Input.GetAxis(
			Config.RotateLeftActionName,
			Config.RotateRightActionName
		);

		if (Mathf.Abs(rotationInput) > 0.01f)
		{
			ApplyTorque(rotationInput * RotationTorque);
		}

		// Apply thrust in the rocket's forward direction
		if (Input.IsActionPressed(Config.RocketThrustActionName))
		{
			Vector2 thrustDirection = -GlobalTransform.Y;
			ApplyCentralForce(thrustDirection * ThrustForce);
		}
	}

	public override void _IntegrateForces(
		PhysicsDirectBodyState2D state)
	{
		// Limit the rocket's linear speed
		if (MaxLinearSpeed > 0f)
		{
			state.LinearVelocity =
				state.LinearVelocity.LimitLength(MaxLinearSpeed);
		}

		// Limit the rocket's angular speed
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
}
}
