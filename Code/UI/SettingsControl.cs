using Godot;
using System.Collections.Generic;
using Thrust.Systems;
using Thrust.Systems.State;


namespace Thrust.UI
{
    public partial class SettingsControl : Control
    {
        #region Audio
        [Export] private AudioControl _masterAudioControl = null;
        [Export] private AudioControl _musicAudioControl = null;
        [Export] private AudioControl _sfxAudioControl = null;

        [Export] private string _masterBusName = null;
        [Export] private string _musicBusName = null;
        [Export] private string _sfxBusName = null;

        [Export] private string _masterDisplayName = null;
        [Export] private string _musicDisplayName = null;
        [Export] private string _sfxDisplayName = null;
        #endregion Audio
        #region Video
        #endregion Video
        #region Buttons
        [Export] private Button _mainMenuButton = null;
        [Export] private Button _cancelButton = null;
        [Export] private Button _saveButton = null;
        [Export] private Button _okButton = null;
        #endregion Buttons

        private SettingsData _data = null;

        #region Enter/Exit Tree
        public override void _EnterTree()
        {
            if (_masterAudioControl != null)
            {
            _masterAudioControl.VolumeChanged += OnVolumeChanged;
            }
            if (_musicAudioControl != null)
            {
                _musicAudioControl.VolumeChanged += OnVolumeChanged;
            }
            if (_sfxAudioControl != null)
            {
                _sfxAudioControl.VolumeChanged += OnVolumeChanged;
            }

            if (_mainMenuButton != null)
            {
                _mainMenuButton.Pressed += MainMenu;
            }
            _saveButton.Pressed += Save;
            _cancelButton.Pressed += Close;

            _okButton.Pressed += Save;
            _okButton.Pressed += Close;
        }
        public override void _ExitTree()
        {
            if (_masterAudioControl != null)
            {
            _masterAudioControl.VolumeChanged -= OnVolumeChanged;
            }
            if (_musicAudioControl != null)
            {
                _musicAudioControl.VolumeChanged -= OnVolumeChanged;
            }
            if (_sfxAudioControl != null)
            {
                _sfxAudioControl.VolumeChanged -= OnVolumeChanged;
            }

            if (_mainMenuButton != null)
            {
                _mainMenuButton.Pressed -= MainMenu;
            }
            _saveButton.Pressed -= Save;
            _cancelButton.Pressed -= Close;

            _okButton.Pressed -= Save;
            _okButton.Pressed -= Close;
        }
        #endregion Enter/Exit Tree

        public override void _Ready()
        {
            _data = new SettingsData(GameManager.Instance.Settings.Data);

            if (_masterAudioControl != null)
            {
                SetUpAudioControl(_masterAudioControl, _masterBusName, _masterDisplayName);
            }
            if (_musicAudioControl != null)
            {
                SetUpAudioControl(_musicAudioControl, _musicBusName, _musicDisplayName);
            }
            if (_sfxAudioControl != null)
            {
                SetUpAudioControl(_sfxAudioControl, _sfxBusName, _sfxDisplayName);
            }
        }

        #region Audio Control
        private void SetUpAudioControl(AudioControl audioControl, string busName, string displayName)
        {
            int audioBusIndex = AudioServer.GetBusIndex(busName);
            if (audioBusIndex < 0) return;
            float volumeDb = AudioServer.GetBusVolumeDb(audioBusIndex);
            audioControl.Setup(busName, displayName, volumeDb);
        }

        private void OnVolumeChanged(string audioBus, float decibel)
        {
            int audioBusIndex = AudioServer.GetBusIndex(audioBus);
            if (audioBusIndex < 0) return;

            switch(audioBus)
            {
                case "Master":
                    _data.MasterVolume = decibel;
                    break;
                case "Music":
                    _data.MusicVolume = decibel;
                    break;
                case "SFX":
                    _data.SfxVolume = decibel;
                    break;
            }

            Settings.ApplySettings(_data);
        }
        #endregion Audio Control
        #region Video Control
        #endregion Video Control
        #region Buttons
        public void MainMenu()
        {
            if (!GameManager.Instance.ChangeState(GameStateType.MainMenu))
            {
                GD.PushError("Failed to change state to MainMenu.");
            }
        }
        public void Close()
        {
            Settings.ApplySettings(GameManager.Instance.Settings.Data);
            GameManager.Instance.ActivatePreviousState();
        }
        public void Save()
        {
            GameManager.Instance.Settings.Data = _data;
            GameManager.Instance.Settings.Save();
        }
        #endregion Buttons
    }
}