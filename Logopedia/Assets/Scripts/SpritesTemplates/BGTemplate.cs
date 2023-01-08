using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using Doozy.Engine;
using Logopedia.UIConnection;

namespace Logopedia.UserInterface
{
    public class BGTemplate : MonoBehaviour
    {
        [Inject]
        StoryManager _storyManager;


        [SerializeField]
        private Sprite _sprite;

        private void Start()
        {
            GetSprite();
        }
        void GetSprite()
        {
            _sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        }

        public void SendSpriteToManager()
        {
            _storyManager.BackGround.GetComponent<Image>().sprite = _sprite;
            GameEventMessage.SendEvent(EventsLibrary.BGSpriteChanged);
        }

        public class Factory : PlaceholderFactory<string, BGTemplate>
        {

        }
    }
}
