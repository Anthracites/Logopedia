using System.IO;
using System.Collections.Generic;
using Logopedia.UIConnection;
using Spine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Newtonsoft.Json;
using Unity.VisualScripting;

namespace Logopedia.GamePlay
{
    public class StorySaverUploader : MonoBehaviour
    {
        [Inject]
        StoryManager _storyManager;
        [Inject]
        ItemsManager _itemManager;

        private int _currentStorySceneIndex;
        private Sprite _character, _backGround;
        private Story _currentStory;
        private int _currentSceneIndex;

        public void ResetSaverUploader()
        {
            _currentStory = null;
            _currentSceneIndex = 0;
    }

        public void Save()
        {
            GetStoryParts();
            AddScene();
            SaveStory();
        }

        private void GetStoryParts()
        {
            _currentStory = _storyManager.CurrentStory;
            _currentStorySceneIndex = _storyManager.CurrentStorySceneIndex;
            _character = _storyManager.Chacter.GetComponent<Image>().sprite;
            _backGround = _storyManager.BackGround.GetComponent<Image>().sprite;
        }

        private void AddScene() 
        {
            var _scene = new StoryScene();
            _scene.SceneNumberInStory = _currentSceneIndex;
//            _scene.Character = _storyManager.Chacter.GetComponent<Image>().sprite;
            _scene.CharacterPosition = new StoryScene.PositionForSave(_storyManager.Chacter.transform.position);
            _scene.CharacterRotation = new StoryScene.PositionForSave(_storyManager.Chacter.transform.eulerAngles);
            _scene.CharacterScale = new StoryScene.PositionForSave(_storyManager.Chacter.transform.localScale);
//            _scene.BG = _storyManager.BackGround.GetComponent<Image>().sprite;

            _scene.Items = new List<StoryScene.SceneItem>();
            _storyManager.CurrentStory.Scenes = new List<StoryScene>();

            var _itemsList = _itemManager.Garments;

            foreach (GameObject _item in _itemsList)
            {
                if (_item == null){
                    continue;
                }
                //Debug.Log(_itemsList.ToString());
                var _currentItem = new StoryScene.SceneItem();
                var _garmentItem = _item.transform.GetChild(1).gameObject;

 //               _currentItem.ItemSprite = _garmentItem.GetComponent<Image>().sprite;
                _currentItem.ItemPosition = new StoryScene.PositionForSave(_garmentItem.transform.position);
                _currentItem.ItemShadowPosition = new StoryScene.PositionForSave(_item.transform.GetChild(0).gameObject.transform.position);
                _currentItem.ItemRotation = new StoryScene.PositionForSave(_garmentItem.transform.eulerAngles);
                _currentItem.ItemScale = new StoryScene.PositionForSave(_garmentItem.transform.localScale);
                _currentItem.ShadowEnabled = _item.transform.GetChild(0).gameObject.activeSelf;

                Color _transparent = new Color(255, 255, 255, 0);
                Color _currentShadowColor = _item.transform.GetChild(0).gameObject.GetComponent<Image>().color;

                if (_currentShadowColor == _transparent)
                {
                    _currentItem.ShadowVisible = false;
                }
                else
                {
                    _currentItem.ShadowVisible = true;

                }

                _scene.Items.Add(_currentItem);
            }

            _storyManager.CurrentStory.Scenes.Add(_scene);
            _currentSceneIndex++;
        }


        private void SaveStory()
        {
            var path = Path.Combine(Application.dataPath + "/Resources/Stories", _currentStory.StoryName + ".json");
            var _storyForSave = _storyManager.CurrentStory;
            var _storyString = JsonConvert.SerializeObject(_storyForSave, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText(path, _storyString);
            Debug.Log(_storyString.ToString());
        }
    }
}
