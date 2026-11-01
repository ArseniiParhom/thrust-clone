using Godot;
using Thrust.Configuration;

namespace Thrust.Characters
{
    [Tool]
    public partial class Rocket : RigidBody2D
    {
        [Export] public float ThrustForce = 300f;
        [Export] public float RotationTorque = 900f;

        [Export] public float MaxLinearSpeed = 160f;
        [Export] public float MaxAngularSpeed = 100f;

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

        public override void _Ready()
        {
            ApplyRocketSize();
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
            if (Engine.IsEditorHint())
                return;

            float rotationInput = Input.GetAxis(
                Config.RotateLeftActionName,
                Config.RotateRightActionName
            );

            if (Mathf.Abs(rotationInput) > 0.01f)
            {
                ApplyTorque(rotationInput * RotationTorque);
            }

            if (Input.IsActionPressed(Config.RocketThrustActionName))
            {
                Vector2 thrustDirection = -GlobalTransform.Y;
                ApplyCentralForce(thrustDirection * ThrustForce);
            }
        }

        public override void _IntegrateForces(
            PhysicsDirectBodyState2D state)
        {
            if (Engine.IsEditorHint())
                return;

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
    }
}
