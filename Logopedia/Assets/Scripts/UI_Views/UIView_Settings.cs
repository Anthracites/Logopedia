using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using Doozy.Engine.Soundy;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Logopedia.GamePlay;
using Zenject;
using Logopedia.UIConnection;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Networking;
using System;
using UniRx;

namespace Logopedia.UserInterface
{
    public class UIView_Settings : MonoBehaviour
    {
        [Inject]
        SettingsManager _settingsManager;

        [SerializeField]
        private TMP_Text MuteButtonLabel;
        [SerializeField]
        private bool _isMute;
        [SerializeField]
        private Slider _slider, _generalVolumeSlider;
        [SerializeField]
        private List<AudioClip> _sounds;
        [SerializeField]
        private TMPro.TMP_Dropdown[] _soundsDropDown;
        [SerializeField]
        private string _spriteFolder;
        [SerializeField]
        private SoundyData _soundyData; //            SoundyManager.Play(_soundData);
        private string _databaseName = "Logopedia";
        [SerializeField]
        private AudioMixerGroup _audioMixerGroup;
        [SerializeField]
        private float _lastGeneralVolume, _generalVolume;
        [SerializeField]
        private AudioMixerGroup _general;
        [SerializeField]
        public enum SoundKind {general, BG, take, GoNext};
        public SoundKind _mykind;


        private void Awake()
        {
            DownloadSettings();
            PlayBGMusic();
        }

        private void Start()
        {
            _lastGeneralVolume = _generalVolumeSlider.value;
            ReloadAudioFiles();
        }

        private void PlayBGMusic()
        {
            _soundyData.DatabaseName = _settingsManager.DataBaseName;
            _soundyData.SoundName = _settingsManager.BGMusic;
            if (_soundyData.SoundName != "No Sound")
            {
                var source = SoundyManager.Play(_soundyData).AudioSource;
                source.volume = _generalVolume;
                source.loop = true;
            };
        }

        private void OnEnable()
        {
            ReloadAudioFiles();
        }

        public void RefrashFileButtonHandler()
        {
            GameEventMessage.SendEvent(EventsLibrary.ClipsAddedToFolder);
        }

        public void ReloadAudioFiles()
        {
            GetSoundsFromManager();
            AddToDropDown();
            ApplyStartSettings();
        }

        public void GetSoundsFromManager()
        {
            _sounds = _settingsManager.Sounds;

        }

        public void SwichMute()
        {
            _isMute = !_isMute;
            string _buttonLabel;
            if (_isMute == true)
            {
                _generalVolume = -80f;
                _buttonLabel = "Включить звук";
            }

            else
            {
                _generalVolume = _lastGeneralVolume;
                _buttonLabel = "Отключить звук";
            }

            MuteButtonLabel.text = _buttonLabel;
            _general.audioMixer.SetFloat("GeneralVolume", _generalVolume);
            _generalVolumeSlider.value = _generalVolume;

        }

        public void ChangeGeneralVolume()
        {
            _generalVolume = _generalVolumeSlider.value;
            _general.audioMixer.SetFloat("GeneralVolume", _generalVolume);

            if (_generalVolume > -80)
            {
                _lastGeneralVolume = _generalVolumeSlider.value;
                _isMute = true;
                SwichMute();
            }
            else
            {
                _isMute = false;
                SwichMute();
            }

        }

        public void PlaySound(TMP_Dropdown _dropDown)
        {
            SoundyManager.StopAllSounds();
            string _soundName = _dropDown.options[_dropDown.value].text;
            _soundyData.DatabaseName = _databaseName;
            _soundyData.SoundName = _soundName;
            SoundyManager.Play(_soundyData);
        }

        public void SetSound(GameObject _soundSetting)
        {
            TMP_Dropdown _setting = _soundSetting.GetComponent<TMP_Dropdown>();
            string _soundNameSetting = _soundSetting.name;
            string soundName = _setting.options[_setting.value].text;

            switch (_soundNameSetting)
            {
                case ("CorrectAnswer"):
                    _settingsManager.CorrectAnswer = soundName;
                    break;
                case ("BGMusic"):
                    _settingsManager.BGMusic = soundName;
                    if (soundName != "No Sound")
                    {
                        var source = SoundyManager.Play(_soundyData).AudioSource;
                        source.volume = _generalVolume;
                        source.loop = true;
                    };
                    break;
                case ("GoNextScene"):
                    _settingsManager.GoNextScene = soundName;
                    break;
                case ("TakeItem"):
                    _settingsManager.TakeItem = soundName;
                    break;
            }
//            UnityEngine.Debug.Log(_settingsManager.CorrectAnswer);

        }


        public void AddToDropDown()
        {
            foreach (TMP_Dropdown _setting in _soundsDropDown)
            {
                _setting.options.Clear();
                _setting.options.Add(new TMP_Dropdown.OptionData() { text = "No Sound" });
                foreach (AudioClip _clip in _sounds)
                {
                    string _soundName = _clip.name.Replace(".mp3", "");
                    _setting.options.Add(new TMP_Dropdown.OptionData() { text = _soundName });

                }
            }
        }


        public void SaveSettings()
        {
            #region PlayerPrefs.Set***
            PlayerPrefs.SetString("CorrectAnswer", _settingsManager.CorrectAnswer);
            PlayerPrefs.SetString("BGMusic", _settingsManager.BGMusic);
            PlayerPrefs.SetString("GoNextScene", _settingsManager.GoNextScene);
            PlayerPrefs.SetString("TakeItem", _settingsManager.TakeItem);

            PlayerPrefs.SetFloat("_generalVolume", _generalVolume);
            #endregion
            UnityEngine.Debug.Log("Settings uploaded to Windows Registry");
        }

        void DownloadSettings()
        {
            #region PlayerPrefs.Get***
            _settingsManager.CorrectAnswer = PlayerPrefs.GetString("CorrectAnswer");
            _settingsManager.BGMusic = PlayerPrefs.GetString("BGMusic");
            _settingsManager.GoNextScene = PlayerPrefs.GetString("GoNextScene");
            _settingsManager.TakeItem = PlayerPrefs.GetString("TakeItem");

            _generalVolume = PlayerPrefs.GetFloat("_generalVolume");

            //применение настроек на страрте
            #endregion
        }

        void ApplyStartSettings()
        {
            foreach (TMP_Dropdown _setting in _soundsDropDown)
            {

                string _name = _setting.gameObject.name;

                int i = 0;
                switch (_name)
                {
                    case ("CorrectAnswer"):
                        i = 0;
                        foreach (TMP_Dropdown.OptionData _text in _setting.options)
                        {
                            if(_text.text == _settingsManager.CorrectAnswer)
                            {
                                _setting.value = i;
                            }
                            i++;
                        }
                        break;
                    case ("BGMusic"):
                         i = 0;
                        foreach (TMP_Dropdown.OptionData _text in _setting.options)
                        {
                            if (_text.text == _settingsManager.BGMusic)
                            {
                                _setting.value = i;

                            }
                            i++;
                        }
                        break;
                    case ("GoNextScene"):
                        i = 0;
                        foreach (TMP_Dropdown.OptionData _text in _setting.options)
                        {
                            if (_text.text == _settingsManager.GoNextScene)
                            {
                                _setting.value = i;
                            }
                            i++;
                        }
                        break;
                    case ("TakeItem"):
                        i = 0;
                        foreach (TMP_Dropdown.OptionData _text in _setting.options)
                        {
                            if (_text.text == _settingsManager.TakeItem)
                            {
                                _setting.value = i;
                            }
                            i++;
                        }
                        break;
                }
            }
            _generalVolumeSlider.value = _generalVolume;
            ChangeGeneralVolume();
        }

        #region UploadingFiles
        public void UploadGarments()
        {
            OpenSpriteFolder(Application.dataPath + SpritesPathLibrary.GarmentSprites);
        }

        public void UploadBG()
        {
            OpenSpriteFolder(Application.dataPath + SpritesPathLibrary.BGSprites);

        }

        public void UploadCharacters()
        {
            OpenSpriteFolder(Application.dataPath + SpritesPathLibrary.CharacterSprites);

        }

        public void UploadAnimations()
        {
            OpenSpriteFolder(Application.dataPath + SpritesPathLibrary.CharacterAnimations);

        }

        public void UploadSounds()
        {
            OpenSpriteFolder(Application.dataPath + SpritesPathLibrary.Sounds);

        }

        public void OpenSpriteFolder(string path)
        {
            Screen.fullScreen = false;
            Process.Start(path);

        }

        private void OnDisable()
        {
            Screen.fullScreen = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEventMessage.SendEvent(EventsLibrary.GoToMenu);
            }
        }
        #endregion
    }
}
