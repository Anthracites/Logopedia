using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using Doozy.Engine;
using Logopedia.UIConnection;

namespace Logopedia.UserInterface
{
    public class CharacterTemplate : MonoBehaviour
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
            _itemsManager.Character.GetComponent<Image>().sprite = _sprite;
            _itemsManager.Character.GetComponent<Image>().sprite.name = gameObject.name;

            GameEventMessage.SendEvent(EventsLibrary.CharacterSpriteChanged);
        }

        public class Factory : PlaceholderFactory<string, CharacterTemplate>
        {

        }
    }
}
