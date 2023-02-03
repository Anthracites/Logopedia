using Zenject;
using UnityEngine;
using Logopedia.UIConnection;
using Logopedia.GamePlay;
using System.Collections.Generic;
using Doozy.Engine;
using System;

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

        private void Awake()
        {
            if (_storyManager.IsStoryEdit == false)
            {
                _itemsManager.MiddleScenePanel = _garmentPanel;

            }
        }

        private void Start()
        {
            if (_storyManager.IsStoryEdit == false)
            {
                _sceneNumber = Int32.Parse(gameObject.name);
                _scene = new StoryScene();
                _scene.SceneNumberInStory = _sceneNumber;
                _scene.Items = new List<StoryScene.SceneItem>();
                _garments = new List<GameObject>();
                _storyManager.CurrentStory.Scenes.Add(_scene);
            }
        }

        private void OnEnable()
        {
            _itemsManager.Character = _character;
            _itemsManager.Background = _bg;
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

            Debug.Log("BG: " + _scene.CurrentBGForSave + "Character: " + _scene.SceneCharacter);

            _storyManager.CurrentStory.Scenes[_sceneNumber] = _scene;
            _garments.Clear();
            if (_storyManager.IsStorySave == true)
            {
                GameEventMessage.SendEvent(EventsLibrary.StoryConvertedForSave);
                _storyManager.IsStorySave = false;
            }
        }

        public class Factory : PlaceholderFactory<string, StoryCreationPanel>
        {

        }
    }
}
