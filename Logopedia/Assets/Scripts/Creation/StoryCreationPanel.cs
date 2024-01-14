using Zenject;
using System.Collections;
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
        private List<GameObject> _garments = new List<GameObject>();
        [SerializeField]
        public StoryScene _scene;
        public int _sceneNumber;
        private bool _isCharacterHidden;
        public bool SceneDeleted;

        private void Awake()
        {
            //StartCoroutine(AddGarments());

            _isCharacterHidden = _character.activeSelf;
            _itemsManager.GarmenScenePanel = _garmentPanel;
            _itemsManager.BackgroundSprite = _bg.GetComponent<Image>().sprite;
            _itemsManager.SplashScreenPanel = _splashScreenPanel;
            _itemsManager.PreviewButton = _previewSwichButton;
        }

        public void SwichCharacter()//пописан на событие CharacterSwiched на кнопре HideCharacter в UIView_Creation

        {
            _isCharacterHidden = !_isCharacterHidden;
            _character.gameObject.SetActive(_isCharacterHidden);
        }

        private void Start()
        {
            SceneDeleted = false;
            _sceneNumber = Int32.Parse(gameObject.name);

            if ((_storyManager.IsStoryEdit == false)&(_storyManager.IsStoryCreartionStart == true))
            {
                _itemsManager.GarmenScenePanel = _garmentPanel;
                _scene = new StoryScene();
                _scene.SceneNumberInStory = _sceneNumber;
                _scene.Items = new List<StoryScene.SceneItem>();
                _storyManager.CurrentStory.Scenes.Add(_scene);
            }
            foreach (Transform child in _garmentPanel.transform)
            {
                child.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }


        IEnumerator AddGarments()
        {
            yield return new WaitForEndOfFrame();
            _garments.Clear();
            _garments.RemoveAll(x => x == null);
            _itemsManager.Garments.Clear();
            _itemsManager.Garments.RemoveAll(x => x == null);

            int i = 0;
            foreach (Transform child in _garmentPanel.transform)
            {
                _garments.Add(child.gameObject);
                _itemsManager.Garments.Add(child.gameObject);
                i++;
            }
//            Debug.Log("Garments count: " + _itemsManager.Garments.Count.ToString() + ", i = " + i.ToString());
        }

        private void OnEnable()
        {
            StartCoroutine(AddGarments());

            _isCharacterHidden = _character.activeSelf;
            _itemsManager.GarmenScenePanel = _garmentPanel;
            _itemsManager.BackgroundSprite = _bg.GetComponent<Image>().sprite;
            _itemsManager.SplashScreenPanel = _splashScreenPanel;
            _itemsManager.PreviewButton = _previewSwichButton;
            ConvertToScene();
           Debug.Log("Scene show " + _sceneNumber.ToString());

        }

        private void OnDisable()
        {
            ConvertToScene();
        }

        private void OnDestroy()
        {
            if (SceneDeleted == false)
            {
                ConvertToScene();
            }
        }

        public void PreviewButtonActivate()
        {
            GameEventMessage.SendEvent(EventsLibrary.PreviewScene);
        }

        public void ConvertToScene()

        {
            _sceneNumber = Int32.Parse(gameObject.name);
            _scene.SceneNumberInStory = _sceneNumber;

            if (_storyManager.CurrentStory.Scenes.Count < _sceneNumber + 1)
            {
                _storyManager.CurrentStory.Scenes.Add(_scene);
            }

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

                _storyManager.CurrentStory.Scenes[_sceneNumber] = _scene;

            _garments.Clear();
            if (_storyManager.IsStorySave == true)
            {
                GameEventMessage.SendEvent(EventsLibrary.StoryConvertedForSave);
                _storyManager.IsStorySave = false;
            }
//            Debug.Log("Panel converted to scene, scene number:" + _sceneNumber.ToString() + ". Story lenght:" + _storyManager.CurrentStory.Scenes.Count.ToString());

        }

        public void SwichBGSprite()
        {
            _bg.GetComponent<Image>().sprite = _itemsManager.BackgroundSprite;
            _bg.name = _itemsManager.BackgroundSprite.name;
        }

        public void AddNewGarment()
        {
            StartCoroutine(AddGarments());

        }

        public class Factory : PlaceholderFactory<string, StoryCreationPanel>
        {

        }
    }
}
