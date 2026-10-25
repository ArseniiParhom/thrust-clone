using Godot;

public partial class Rocket : RigidBody2D
{
    [Export] public float ThrustForce = 1500f;
    [Export] public float RotationTorque = 8000f;

    public override void _PhysicsProcess(double delta)
    {
        // Rotate rocket using physics torque
        float rotationInput = Input.GetAxis(
            "rocket_rotate_left",
            "rocket_rotate_right"
        );

        if (Mathf.Abs(rotationInput) > 0.01f)
        {
            ApplyTorque(rotationInput * RotationTorque);
        }

        // Apply thrust in the direction the rocket is facing
        if (Input.IsActionPressed("rocket_thrust"))
        {
            Vector2 thrustDirection = -GlobalTransform.Y;
            ApplyCentralForce(thrustDirection * ThrustForce);
        }
    }
}
