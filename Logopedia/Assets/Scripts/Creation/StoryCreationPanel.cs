using Zenject;
using UnityEngine;
using Logopedia.UIConnection;
using Logopedia.GamePlay;
using System.Collections.Generic;
using Doozy.Engine;
using System;
using UnityEngine.UI;

namespace Logopedia.UserInterface
{
    public class StoryCreationPanel : MonoBehaviour
    {
        [Inject]
        ItemsManager _itemsManager;
        [Inject]
        StoryManager _storyManager;

        [SerializeField]
        private GameObject _character, _bg, _garmentPanel, _previewSwichButton, _splashScreenPanel;
        [SerializeField]
        private List<GameObject> _garments;
        [SerializeField]
        StoryScene _scene;
        [SerializeField]
        int _sceneNumber;
        private bool _isCharacterHidden;

        private void Awake()
        {
            //if (_storyManager.IsStoryEdit == false)
            //{
            //    _itemsManager.GarmenScenePanel = _garmentPanel;
            //    //if (_storyManager.IsStoryEdit == false)
            //    //{
            //        _sceneNumber = Int32.Parse(gameObject.name);
            //        _scene = new StoryScene();
            //        _scene.SceneNumberInStory = _sceneNumber;
            //        _scene.Items = new List<StoryScene.SceneItem>();
            //        _garments = new List<GameObject>();
            //        _storyManager.CurrentStory.Scenes.Add(_scene);
            //        Debug.Log("Send scene" + gameObject.name);
            //    //}
            //}
        }

        public void SwichCharacter()//пописан на событие CharacterSwiched на кнопре HideCharacter в UIView_Creation

        {
            _isCharacterHidden = !_isCharacterHidden;
            _character.gameObject.SetActive(_isCharacterHidden);
        }

        private void Start()
        {
            if ((_storyManager.IsStoryEdit == false)&(_storyManager.IsStoryCreartionStart == true))
            {
                _itemsManager.GarmenScenePanel = _garmentPanel;
                //if (_storyManager.IsStoryEdit == false)
                //{
                _sceneNumber = Int32.Parse(gameObject.name);
                _scene = new StoryScene();
                _scene.SceneNumberInStory = _sceneNumber;
                _scene.Items = new List<StoryScene.SceneItem>();
                _garments = new List<GameObject>();
                _storyManager.CurrentStory.Scenes.Add(_scene);
//                Debug.Log("Send scene" + gameObject.name);
                //}
            }
        }

        private void OnEnable()
        {
            _isCharacterHidden = _character.activeSelf;
            _itemsManager.GarmenScenePanel = _garmentPanel;
            _itemsManager.BackgroundSprite = _bg.GetComponent<Image>().sprite;
            _itemsManager.SplashScreenPanel = _splashScreenPanel;
            _itemsManager.PreviewButton = _previewSwichButton;
        }

        private void OnDisable()
        {
            ConvertToScene();
        }

        public void PreviewButtonActivate()
        {
            GameEventMessage.SendEvent(EventsLibrary.PreviewScene);
        }

        public void ConvertToScene()

        {
            _sceneNumber = Int32.Parse(gameObject.name);
            _scene.SceneNumberInStory = _sceneNumber;
            _scene.ActiveItemCount = 0;
            _scene.IsSceneSplashScreen = _splashScreenPanel.activeSelf;
            _scene.Items.Clear();
            var _garmentsArray = _garmentPanel.GetComponentsInChildren<Garment>();

            foreach (Garment tr in _garmentsArray)
            {
                var _newItem = new StoryScene.SceneItem(tr.gameObject);
                _scene.Items.Add(_newItem);

                if (_newItem.ShadowEnabled == true)
                {
                    _scene.ActiveItemCount++;
                }
            }


            _scene.CurrentBGForSave =  _scene.BGForSave(_bg.gameObject);

            _scene.SceneCharacter = new StoryScene.CharacterForSave(_character.gameObject);

//            Debug.Log("BG: " + _scene.CurrentBGForSave + "Character: " + _scene.SceneCharacter);

                _storyManager.CurrentStory.Scenes[_sceneNumber] = _scene;

            _garments.Clear();
            if (_storyManager.IsStorySave == true)
            {
                GameEventMessage.SendEvent(EventsLibrary.StoryConvertedForSave);
                _storyManager.IsStorySave = false;
            }
        }

        public void SwichBGSprite()
        {
            _bg.GetComponent<Image>().sprite = _itemsManager.BackgroundSprite;
            _bg.name = _itemsManager.BackgroundSprite.name;
        }

        public class Factory : PlaceholderFactory<string, StoryCreationPanel>
        {

        }
    }
}
