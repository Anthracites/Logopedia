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
        private float _startX, _startY;
        [SerializeField]
        private Outline _outline;
        [SerializeField]
        private GameObject _animation, _animationParent;

        void Start()
        {
            //this.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            _itemsManager.CurrentItem = gameObject;
            StartMove();
        }

        void StartMove()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startX = mousePos.x - transform.position.x;
            _startY = mousePos.y - transform.position.y;
            _outline.enabled = true;
            _itemsManager.CurrentItem = gameObject;
            _itemsManager.CurrentItemShadow = null;
            _itemsManager.CharacterAnimation = _animation;
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
//            Debug.Log("Chacter selected!!!" + gameObject.name);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StartMove();
        }

        public void OnDrag(PointerEventData eventData)
        {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float _z = transform.position.z;
            transform.position = new Vector3(mousePos.x - _startX, mousePos.y - _startY, _z);
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

