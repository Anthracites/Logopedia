using System.Collections;
using System.Collections.Generic;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine;
using UnityEngine.EventSystems;

namespace Logopedia.GamePlay
{
    public class Character : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;


        [SerializeField]
        private Outline _outline;

        void Start()
        {
            _itemsManager.CurrentItem = gameObject;
            StartMove();
        }

        void StartMove()
        {
            _outline.enabled = true;
            _itemsManager.CurrentItem = gameObject;
            _itemsManager.CurrentItemShadow = null;
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
//            Debug.Log("Chacter selected!!!");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StartMove();
        }

        public void OnDrag(PointerEventData eventData)
        {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            StartMove();
        }

        public void DeselectItem()
        {
            if(_itemsManager.CurrentItem != gameObject)
            {
                _outline.enabled = false;
            }
        }
    }
}
