 using System.Collections;
using System.Collections.Generic;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine;
using Unity.VisualScripting;
using Spine;

namespace Logopedia.GamePlay
{
    public class Garment : MonoBehaviour, IDropHandler, IPointerUpHandler, IEndDragHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;
        [Inject]
        StoryManager _storyManager;

        [SerializeField]
        private GameObject _item, _itemShadow;
        [SerializeField]
        private IEnumerator _moveWithMouse;


        private void Awake()
        {
            _itemsManager.CurrentGarment = gameObject;
            _itemsManager.CurrentItem = _item;
            _itemsManager.CurrentItemShadow = _itemShadow;
            _itemsManager.Garments.Add(gameObject);
            GameEventMessage.SendEvent(EventsLibrary.ItemCreated);
            _moveWithMouse = MoveWhithMouse();
            if (_storyManager.IsStoryEdit == false)
            {
                StartCoroutine(_moveWithMouse);
            }
        }

        private IEnumerator MoveWhithMouse()
        {
            while (true)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                yield return null;
            }
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            StopCoroutine(_moveWithMouse);
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            StopCoroutine(_moveWithMouse);
        }

        public void OnPointerUp(PointerEventData eventData)
        { 
            StopCoroutine(_moveWithMouse);
        }

        public void OnDrop(PointerEventData eventData)
        {
            StopCoroutine(_moveWithMouse);
        }

        public void DeleteGarment()
        {
            Destroy(gameObject);
        }

        public void MirrrorGarment()
        {
           // _item.GetComponent<Image>().sprite.
        }
        public void SetItemPosition()
        {
            _itemShadow.transform.position = _item.transform.position;
        }

        public class Factory : PlaceholderFactory<string, Garment>
        {

        }
    }
}
