using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine;
using Logopedia.UIConnection;

namespace Logopedia.GamePlay
{
    public class Item : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;

        private CanvasGroup _canvasGroup;
        [SerializeField]
        private GameObject _slot, _garment;
        [SerializeField]
        private Outline _outline;



        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StartMove();
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            StartMove();
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
        }

        void StartMove()
        {
            _outline.enabled = true;
            _itemsManager.CurrentGarment = _garment;
            _itemsManager.CurrentItem = gameObject;
            _itemsManager.CurrentItemShadow = _slot;
            _canvasGroup.blocksRaycasts = false;
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
        }


        public void OnDrop(PointerEventData eventData)
        {    
            _canvasGroup.blocksRaycasts = true;
        }

            public void OnDrag(PointerEventData eventData)
        {
               var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                //gameObject.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
        }

        public void SelectItem()
        {
            _itemsManager.CurrentItem = gameObject;
            _itemsManager.CurrentItemShadow = _slot;
            OutlineItem();
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
        }

        public void OutlineItem()
        {
            if (_itemsManager.CurrentItem != gameObject)
            {
                _canvasGroup.blocksRaycasts = true;
                _outline.enabled = false;
            }
        }

    }
}
