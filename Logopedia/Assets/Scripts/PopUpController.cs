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
    private Button ButtonOk, ButtonPaste, _cancelButton, _closeButton;
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
        switch (_popUpName)
        {
                case ("NewStory"):
                    ButtonOk.onClick.AddListener(GoToCreateNewStory);
                break;

                case ("PlayNewGame"):
                    LoadStories();
                    ButtonOk.onClick.AddListener(GoToPlayNewGame);
                    break;

                case ("EditStory"):
                    LoadStories();
                    ButtonOk.onClick.AddListener(GoToEditStory);
                    break;
                case ("СonfirmRemove"):
                    ButtonOk.onClick.AddListener(CleanScene);
                    break;
                default:
                ButtonOk.onClick.AddListener(ClosePopUp);
                break;
        }
    }

        void CleanScene()
        {
            var _garments = _itemsManager.Garments;
            foreach (GameObject garment in _garments)
            {
                Destroy(garment);
            }
            ClosePopUp();
        }


        void GoToEditStory()
        {
            GameEventMessage.SendEvent(EventsLibrary.GoToEditStory);
        }

        void LoadStories()
        {
            var _jsons = Resources.LoadAll("Stories");
            string[] _storiesName = new string[_jsons.Length];

            int i = 0;
            foreach (string _name in _storiesName)
            {
                _storiesName[i] = _jsons[i].name;
                _dropdown.options.Add(new TMP_Dropdown.OptionData() { text = _jsons[i].name});
                i++;
            }
        }
        public void GoToPlayNewGame()
        {
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
                string jsonString = _newStoryName;
                var path = Path.Combine(Application.dataPath + "/Resources/Stories", _newStoryName + ".json");
                bool _fileExist = File.Exists(path);

                if (_fileExist)
                {
                    _newStoryName += i.ToString();
                    i++;
                    goto m1;
                }
                else
                {
                    File.WriteAllText(path, jsonString);
                    var _storyName = _inpupField.text + "(" + i.ToString() + ")";
                    var _story = new Story();
                    _story.StoryName = _storyName;
                    _storyManager.CurrentStory = _story;
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

