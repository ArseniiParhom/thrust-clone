using Godot;
using Thrust.Configuration;

namespace Thrust.Effects
{
    public partial class Explosion : AnimatedSprite2D
    {
        [Signal] public delegate void FinishedEventHandler();


        public override void _Ready()
        {
            AnimationFinished += OnAnimationFinished;
            Play(Config.ExplodeEffectName);
        }

        private void OnAnimationFinished()
        {
            EmitSignal(SignalName.Finished);
            QueueFree();
        }
    }
}