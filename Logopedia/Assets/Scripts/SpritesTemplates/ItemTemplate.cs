using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Logopedia.GamePlay;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine;

namespace Logopedia.UserInterface
{

    public class ItemTemplate : MonoBehaviour
    {
        [Inject]
        Garment.Factory _garmentFactory;
        [Inject]
        ItemsManager _itemsManager;


        [SerializeField]
        private Sprite _sprite;

        private void Start()
        {
            GetSprite();
            //CreatMirroredSprite();
        }
        void GetSprite()
        {
            _sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        }

        public void CreateNewGerment()
        {
            var _item = _garmentFactory.Create(PrefabsPathLibrary.Item).gameObject;
            _item.transform.SetParent(_itemsManager.GarmenScenePanel.transform);
            _item.name = gameObject.name;
            _item.transform.localScale = new Vector3(1, 1, 1);
            var pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 pos1 = Camera.main.ScreenToWorldPoint(pos);
            _item.transform.position = new Vector3(pos1.x, pos1.y, 0);
            _item.transform.GetChild(0).GetComponent<Image>().sprite = _sprite;
            _item.transform.GetChild(1).GetComponent<Image>().sprite = _sprite;

            _itemsManager.SelectedGarments.Clear();

            _itemsManager.SelectedGarments.Add(_item);
            _itemsManager.Garments.Add(_item);
            _item.GetComponent<Image>().raycastTarget = true;
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
        }

        public class Factory : PlaceholderFactory<string, ItemTemplate>
        {

        }
    }
}
