using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Doozy;
using Doozy.Engine;
using Spine.Unity;
using Zenject;
using Logopedia.UIConnection;

namespace Logopedia.GamePlay
{
    public class ItemSlot : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;

        [SerializeField]
        private GameObject _item, _starsObj, _garment;
        [SerializeField]
        private SkeletonGraphic _stars;

        void Start()
        {
            _stars = _starsObj.GetComponent<SkeletonGraphic>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SendItem();
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            SendItem();
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
        }
        public void OnDrag(PointerEventData eventData)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        public void OnDrop( PointerEventData eventData)
        {
            var other = eventData.pointerDrag.transform;
            if (other != transform)
            {
                if (other.gameObject == _item)
                {
                    //other.SetParent(transform);
                    other.transform.position = transform.position;
                    ShowAnimation();
                }
                else
                {
                    //other.transform.position = _startDragPosition;
                }
            }
        }

        void SendItem()
        {
            _itemsManager.CurrentGarment = _garment;
            _itemsManager.CurrentItem = _item;
            _itemsManager.CurrentItemShadow = gameObject;
        }

        public void ShowAnimation()
        {
            _stars.enabled = true;
            _stars.AnimationState.SetAnimation(0, "correct_answer", false);
        }
    }
}
