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
using Doozy.Engine.Soundy;


namespace Logopedia.GamePlay
{
    public class ItemSlot : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;
        [Inject]
        SettingsManager _settingsManager;

        [SerializeField]
        private GameObject _item, _starsObj, _garment;
        [SerializeField]
        private SkeletonGraphic _stars;
        [SerializeField]
        private SoundyData _putItem, _takeItem;
        [SerializeField]
        private float _startX, _startY;

        void Start()
        {
            _stars = _starsObj.GetComponent<SkeletonGraphic>();
            GetSound();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startX = mousePos.x - transform.position.x;
            _startY = mousePos.y - transform.position.y;
            SendItem();
            SoundyManager.Play(_takeItem);
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
            transform.position = new Vector3(mousePos.x - _startX, mousePos.y - _startY, 0);
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
                    SoundyManager.Play(_putItem);
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
        public void GetSound()
        {
            _putItem.DatabaseName = _settingsManager.DataBaseName;
            _putItem.SoundName = _settingsManager.CorrectAnswer;
            _takeItem.DatabaseName = _settingsManager.DataBaseName;
            _takeItem.SoundName = _settingsManager.TakeItem;
        }
    }
}
