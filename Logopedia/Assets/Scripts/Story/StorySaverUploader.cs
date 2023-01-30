using System.IO;
using System.Collections.Generic;
using System.Collections;
using Logopedia.UIConnection;
using Logopedia.UserInterface;
using Spine;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Zenject;
using Newtonsoft.Json;
using Unity.VisualScripting;
using Doozy.Engine;

namespace Logopedia.GamePlay
{
    public class StorySaverUploader : MonoBehaviour
    {
        [Inject]
        StoryManager _storyManager;
        [Inject]
        PopUpManager _popUpManager;

        private int _currentStorySceneIndex;
        private Sprite _character, _backGround;
        private Story _currentStory;
        private int _currentSceneIndex;

        private void Start()
        {
            ChechContentFolder();
        }

        void ChechContentFolder()
        {
#if !UNITY_EDITOR
            var path = Application.dataPath + "/Resources/Stories";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources/Sprites/GamePlaySprites/Garments");
                Directory.CreateDirectory(Application.dataPath + "/Resources/Sprites/GamePlaySprites/BackGrounds");
                Directory.CreateDirectory(Application.dataPath + "/Resources/Sprites/GamePlaySprites/Characters");
                Directory.CreateDirectory(Application.dataPath + "/Resources/Stories");
            }
#endif
        }

        public void ResetSaverUploader()
        {
            _currentStory = null;
            _currentSceneIndex = 0;
        }


        public void SaveStory()
        {
            _currentStory = _storyManager.CurrentStory;
            var path = Path.Combine(Application.dataPath + "/Resources/Stories", _currentStory.StoryName + ".json");
            var _storyForSave = _storyManager.CurrentStory;
            var _storyString = JsonConvert.SerializeObject(_storyForSave, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText(path, _storyString);
            Debug.Log(_storyString.ToString());
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.StorySaved;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
        }
        }
    }

