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
            var path = Application.dataPath + SpritesPathLibrary.ContentFolderPath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Application.dataPath + SpritesPathLibrary.GarmentSprites);
                Directory.CreateDirectory(Application.dataPath + SpritesPathLibrary.BGSprites);
                Directory.CreateDirectory(Application.dataPath + SpritesPathLibrary.CharacterSprites);
                Directory.CreateDirectory(Application.dataPath + SpritesPathLibrary.CharacterAnimations);
                Directory.CreateDirectory(Application.dataPath + SpritesPathLibrary.Sounds);

                _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.NoSpritesNotification;
                GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);

            }
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
            //Debug.Log(_storyString.ToString());
            if (_popUpManager.ExitToMenu == true)
            {
                GameEventMessage.SendEvent(EventsLibrary.GoToMenu);
                _popUpManager.ExitToMenu = false;
            }

            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.StorySaved;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);

        }
        }
    }

