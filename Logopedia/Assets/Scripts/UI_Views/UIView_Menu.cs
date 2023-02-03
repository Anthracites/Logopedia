using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine;
using Zenject;
using Logopedia.UIConnection;

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

    }
}
