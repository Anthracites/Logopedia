using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine;
using Logopedia.UIConnection;
using Doozy.Engine.Soundy;


namespace Logopedia.GamePlay
{
    public class Item : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;
        [Inject]
        SettingsManager _settingsManager;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private GameObject _slot, _garment;
        [SerializeField]
        private Outline _outline;
        [SerializeField]
        private SoundyData _takeItem;
        [SerializeField]
        private float _startX, _startY;

        void Start()
        {
            GetTakeSound();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StartMove();
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if ((Input.GetKey(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.RightShift)))
            {
                Debug.Log("Shift key is pressed.");
            }
            SelectItem();
            _outline.enabled = true;
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
        }

        void StartMove()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startX = mousePos.x - transform.position.x;
            _startY = mousePos.y - transform.position.y;
            SoundyManager.Play(_takeItem);
            _outline.enabled = true;
            _itemsManager.SelectedGarments.Clear();
            _itemsManager.SelectedGarments.Add(_garment);
            _canvasGroup.blocksRaycasts = false;
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
            SelectItem();
        }


        public void OnDrop(PointerEventData eventData)
        {    
            _canvasGroup.blocksRaycasts = true;
        }

            public void OnDrag(PointerEventData eventData)
        {
            SelectItem();

               var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x - _startX, mousePos.y - _startY, 0);
            //gameObject.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
        }

        public void SelectItem()
        {
            _itemsManager.SelectedGarments.Clear();
            _itemsManager.SelectedGarments.RemoveAll(x => x == null);
            _itemsManager.SelectedGarments.Add(_garment);
            _garment.transform.SetSiblingIndex(_itemsManager.Garments.Count + 1);
            Debug.Log("Item selected");
            if (_itemsManager.SelectedGarments.Contains(_garment) == true)
            {
                //Debug.Log("Item " + _garment.name + " selected");
            }
        }

        public void OutlineItem()
        {
            bool b = _itemsManager.SelectedGarments.Contains(_garment);

            _outline.enabled = b;

//            Debug.Log("Item outlined" + b.ToString());
        }

        public void GetTakeSound()
        {
            _takeItem.DatabaseName = _settingsManager.DataBaseName;
            _takeItem.SoundName = _settingsManager.TakeItem;
        }

    }
}
