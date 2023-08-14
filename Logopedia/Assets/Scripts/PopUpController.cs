using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine.UI;
using TMPro;
using Doozy.Engine;
using System.IO;
using Logopedia.GamePlay;
using Logopedia.UIConnection;
using Spine;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Linq;

namespace Logopedia.UserInterface
{ 
public class PopUpController : MonoBehaviour
{
        [Inject]
        StoryManager _storyManager;
        [Inject]
        PopUpManager _popUpManager;
        [Inject]
        ItemsManager _itemsManager;


    [SerializeField]
    private PopUpConfig _currentPopUpConfug;

    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Button ButtonOk, ButtonPaste, _cancelButton, _closeButton, ButtonNo;
    [SerializeField]
    private UIPopup _thisPopUp;
    [SerializeField]
    private TMP_InputField _inpupField;
        [SerializeField]
        private GameObject _inpupPanel;
        [SerializeField]
    private TMP_Dropdown _dropdown;
    [SerializeField]
    private bool _isInoutActive;
    [SerializeField]
    private TMP_Text _title;
    [SerializeField]
    private string _popUpName;
        [SerializeField]
        private List<Story> _stories;

    public void ShowThisPopUp()
    {
        GetKind();
        _inpupField.text = "Новый сюжет";
        _thisPopUp.Show();
          
    }


    void GetKind()
    {
        _currentPopUpConfug = _popUpManager.CurrentPopUpConfig;
        ConfigPopUp(_currentPopUpConfug);

            ButtonOk.onClick.RemoveAllListeners();
            ButtonNo.onClick.RemoveAllListeners();

            switch (_popUpName)
            {
                case ("NewStory"):
                    ButtonOk.onClick.AddListener(GoToCreateNewStory);
                    break;

                case ("PlayNewGame"):
                    LoadStories();
                    ButtonOk.onClick.AddListener(GoToPlayNewGame);
                    break;

                case ("ConfirmExitToMenuFromGame"):
                    ButtonOk.onClick.AddListener(ExitWhithoutSave);
                    break;

                case ("EditStory"):
                    LoadStories();
                    ButtonOk.onClick.AddListener(GoToEditStory);
                    break;
                case ("СonfirmClearScene"):
                    ButtonOk.onClick.AddListener(CleanScene);
                    break;
                case ("СonfirmRemoveScene"):
                    ButtonOk.onClick.AddListener(RemoveScene);
                    break;
                case ("StorySaved"):
                    ButtonOk.onClick.AddListener(ClosePopUp);
                    break;
                case ("СonfirmSaveStory"):
                    ButtonOk.onClick.AddListener(SaveStory);
                    ButtonNo.onClick.AddListener(ExitWhithoutSave);
                    break;
                case ("NoSpritesNotification"):
                    ButtonOk.onClick.AddListener(GoToUploadSprites);
                    break;
                default:
                ButtonOk.onClick.AddListener(ClosePopUp);
                break;
        }
    }

        void GoToUploadSprites()
        {
            GameEventMessage.SendEvent(EventsLibrary.GoToSettings);
            ClosePopUp();
        }

        void ExitWhithoutSave()
        {
            GameEventMessage.SendEvent(EventsLibrary.GoToMenu);
            ClosePopUp();
            _storyManager.IsStoryCreartionStart = false;
        }

        void SaveStory()
        {
            _storyManager.IsStorySave = true;
            GameEventMessage.SendEvent(EventsLibrary.SaveStory);
            _storyManager.IsStoryCreartionStart = false;
        }

        void CleanScene()
        {
            var _garments = _itemsManager.Garments;
            foreach (GameObject garment in _garments)
            {
                Destroy(garment);
                _itemsManager.Garments.Clear();
                _itemsManager.Garments.RemoveAll(x => x == null);
            }
            ClosePopUp();
        }

        void RemoveScene()
        {
            GameEventMessage.SendEvent(EventsLibrary.RemoveCurrentScene);
            ClosePopUp();
        }

        void GoToEditStory()
        {
            GameEventMessage.SendEvent(EventsLibrary.GoToEditStory);
            string _storyJsonName = _dropdown.options[_dropdown.value].text;
            var _storyJson = File.ReadAllText(Application.dataPath + "/Resources/Stories/" + _storyJsonName);
            var _choosenStory = JsonConvert.DeserializeObject<Story>(_storyJson);
            _storyManager.CurrentStory = _choosenStory;
            _storyManager.IsStoryCreartionStart = false;
            _storyManager.IsStoryEdit = true;
            GameEventMessage.SendEvent(EventsLibrary.GoToNewGame);
            ClosePopUp();
        }

        void LoadStories()
        {
            _dropdown.options.Clear();

            DirectoryInfo _contentDirectory = new DirectoryInfo(Application.dataPath + "/Resources/Stories");
            FileInfo[] _storiesName = new string[] { "*.json" }.SelectMany(ext => _contentDirectory.GetFiles(ext, SearchOption.TopDirectoryOnly)).ToArray();

            foreach (FileInfo _name in _storiesName)
            {
                _dropdown.options.Add(new TMP_Dropdown.OptionData() { text = _name.Name});
            }
        }
        public void GoToPlayNewGame()
        {
            string _storyJsonName = _dropdown.options[_dropdown.value].text;
            var _storyJson = File.ReadAllText(Application.dataPath + "/Resources/Stories/" + _storyJsonName);
            var _choosenStory = JsonConvert.DeserializeObject<Story>(_storyJson);
            _storyManager.CurrentStory = _choosenStory;
            GameEventMessage.SendEvent(EventsLibrary.GoToNewGame);
            ClosePopUp();
        }

        public void GoToCreateNewStory()
        {
            if(_inpupField.text != "")
            {
                int i = 0;

                string _newStoryName = _inpupField.text;
                m1:
                var path = Path.Combine(Application.dataPath + "/Resources/Stories", _newStoryName + ".json");
                bool _fileExist = File.Exists(path);

                if (_fileExist)
                {
                    i++;
                    _newStoryName = _inpupField.text + "(" + i.ToString() + ")";
                    goto m1;
                }

                else
                {
                    var _storyName = _inpupField.text + "(" + i.ToString() + ")";
                    if (i == 0)
                    {
                        _storyName = _inpupField.text;
                    }

                    var _story = new Story();
                    _story.StoryName = _storyName;
                    _storyManager.CurrentStory = _story;
                    _storyManager.IsStoryCreartionStart = true;
                }

                GameEventMessage.SendEvent(EventsLibrary.GoToNewStoryCreation);
            }
            ClosePopUp();
        }

    void ConfigPopUp(PopUpConfig _config)
    {
        _popUpName = _config.PopUpName;
        _title.text = _config.Title;
        _icon.sprite = Resources.Load<Sprite>(_config.IconWay);
            ButtonNo.gameObject.SetActive(_config.IsActiveNoButton);
        _closeButton.gameObject.SetActive(_config.IsActiveCloseButton);
        _dropdown.gameObject.SetActive(_config.IsActiveDropDown);
            _inpupPanel.SetActive(_config.IsActiveInputField);
        _cancelButton.gameObject.SetActive(_config.IsActiveCancelButton);
        _thisPopUp.HideOnClickOverlay = (_config.CloseAnywareClick);
        _thisPopUp.HideOnClickContainer = (_config.CloseAnywareClick);
    }

    public void PasteFromBuffer()
    {
        TextEditor _text = new TextEditor();
        _text.Paste();
        _inpupField.text = _text.text;
    }

    public void ClosePopUp()
    {

            _thisPopUp.Hide();
    }
}
}

