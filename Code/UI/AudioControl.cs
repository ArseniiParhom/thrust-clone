using Godot;

namespace Thrust.UI
{
    public partial class AudioControl : HBoxContainer
    {
        [Signal] public delegate void VolumeChangedEventHandler(string audioBus, float decibel);
        
        [Export] private Slider _volumeSlider = null;
        [Export] private Label _nameLabel = null;
        [Export] private Label _volumeLabel = null;

        private string _busName = null;

        public override void _EnterTree()
        {
        }

        public override void _ExitTree()
        {
            if (_volumeSlider != null)
            {
                _volumeSlider.ValueChanged -= OnVolumeChanged;
            }
        }
        public void Setup(string busName, string displayName, float decibelVolume)
        {
            _busName = busName;
            _nameLabel.Text = displayName;
            SetVolume(decibelVolume);

            if (_volumeSlider != null)
            {
                _volumeSlider.ValueChanged += OnVolumeChanged;
            }
        }
        public void SetVolume(float decibelVolume)
        {
            float linearVolume = Mathf.DbToLinear(decibelVolume);
            _volumeSlider.Value = linearVolume;
            _volumeLabel.Text = ((int)(linearVolume * 100)).ToString() + "%";
        }

        private void OnVolumeChanged(double value)
        {
            UpdateVolume();
        }
        public void UpdateVolume()
        {
            // Volume on a linear scale [0.0,1.0]
            float linearVolume = (float)_volumeSlider.Value;

            // Volume on a decibel scale [-80.0, 0.0]
            float decibelVolume = Mathf.LinearToDb(linearVolume);

            _volumeLabel.Text = ((int)(linearVolume * 100)).ToString() + "%";

            EmitSignal(SignalName.VolumeChanged, _busName, decibelVolume);
        }
    }
}