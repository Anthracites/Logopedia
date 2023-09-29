using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine;
using Zenject;
using Logopedia.UIConnection;
using TMPro;
using Assets.SimpleLocalization.Scripts;

namespace Logopedia.UserInterface
{
    public class UIView_Menu : MonoBehaviour
    {
        [Inject]
        PopUpManager _popUpManager;
        [Inject]
        StoryManager _storyManager;

        [SerializeField]
        private Button _create;
        [SerializeField]
        private TMP_Dropdown _dropdown;
        [SerializeField]
        private string _currentLanguage;

        private void Awake()
        {
            LocalizationManager.Read();
            _currentLanguage = null;
        }

        private void Start()
        {
            #region PlayerPrefs.Get***
            _currentLanguage = PlayerPrefs.GetString("_currentLanguage");
            #endregion

            LocalizationManager.Read();
            LocalizationManager.Language = _currentLanguage;
            Showlanguages();
        }

        public void SelectLanguage()
        {
            string l = _dropdown.options[_dropdown.value].text;
            LocalizationManager.Language = l;
            _currentLanguage = l;
            GameEventMessage.SendEvent(EventsLibrary.LanguageSwich);
            SaveLanguageSetting();
        }

        void Showlanguages()
        {
            _dropdown.options.Clear();
            List<string> _allLanguages = new List<string>();
            int i = 0;
            foreach (string _language in LocalizationManager.languages)
            {
                if (i == 0)
                {
                    i++;
                }
                else
                {
                    _allLanguages.Add(_language);
                    _dropdown.options.Add(new TMP_Dropdown.OptionData() { text = _language });

                    if (_language == _currentLanguage)
                    {
                        _dropdown.value = i;
                    }
                    i++;
                }
            }
        }

        public void ExitFromApplication()
        {
            Application.Quit();
        }

       public void PlayNewGame()
        {
            _storyManager.IsStoryEdit = false;
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.PlayGame;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
        }

        public void ContinuePlayGame()
        {
            GameEventMessage.SendEvent(EventsLibrary.GoToContinueGame);
        }

        public void CreateStory()
        {
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.NewStory;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
        }

        public void EditStory()
        {
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.EditStory;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
        }

        void SaveLanguageSetting()
        {
            #region PlayerPrefs.Set***
            PlayerPrefs.SetString("_currentLanguage", _currentLanguage);
            #endregion
        }

    }
}
