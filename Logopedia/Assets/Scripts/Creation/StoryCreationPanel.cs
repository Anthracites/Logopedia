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
        private GameObject _character, _bg, _garmentPanel;
        [SerializeField]
        private List<GameObject> _garments;
        [SerializeField]
        StoryScene _scene;
        [SerializeField]
        int _sceneNumber;


        private void Start()
        {
            _sceneNumber = Int32.Parse(gameObject.name);
            _scene = new StoryScene();
            _scene.SceneNumberInStory = _sceneNumber;
            _scene.Items = new List<StoryScene.SceneItem>();
            _garments = new List<GameObject>();
            _itemsManager.MiddleScenePanel = _garmentPanel;
            _storyManager.CurrentStory.Scenes.Add(_scene);
            Debug.Log("Scene number in story: " + _storyManager.CurrentStorySceneIndex.ToString());
        }

        private void OnEnable()
        {
            _itemsManager.Character = _character;
            _itemsManager.Background = _bg;
        }

        private void OnDisable()
        {
            ConvertToScene();
        }


        public void ConvertToScene()

        {
            _sceneNumber = Int32.Parse(gameObject.name);
            _scene.SceneNumberInStory = _sceneNumber;
            _scene.ActiveItemCount = 0;
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
