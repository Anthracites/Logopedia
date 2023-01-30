using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zenject;
using UnityEngine.UI;
using Doozy.Engine;
using Logopedia.UIConnection;
using Logopedia.GamePlay;

namespace Logopedia.UserInterface
{
    public class BGTemplate : MonoBehaviour
    {
        [Inject]
        ItemsManager _itemsManager;



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
            _itemsManager.Background.GetComponent<Image>().sprite = _sprite;
            _itemsManager.Background.GetComponent<Image>().sprite.name = gameObject.name;

            GameEventMessage.SendEvent(EventsLibrary.BGSpriteChanged);
        }

        public class Factory : PlaceholderFactory<string, BGTemplate>
        {

        }
    }
}
