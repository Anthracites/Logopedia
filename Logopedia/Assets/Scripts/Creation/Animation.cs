using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Doozy.Engine;
using Logopedia.UIConnection;
using UnityEngine.EventSystems;
using Spine.Unity;


namespace Logopedia.GamePlay
{
    public class Animation : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;

        [SerializeField]
        private SkeletonGraphic _animation;
        [SerializeField]
        private float _startX, _startY;

        public void GetAnimation()
        {
            _animation.skeletonDataAsset = _itemsManager.CharacterAnimation.GetComponent<SkeletonGraphic>().skeletonDataAsset;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float _z = transform.position.z;
            transform.position = new Vector3(mousePos.x - _startX, mousePos.y - _startY, _z);
        }

        void SendToManager()
        {
            _itemsManager.CharacterAnimation = _animation.gameObject;
        }

        private void Start()
        {
            SendToManager();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startX = mousePos.x - transform.position.x;
            _startY = mousePos.y - transform.position.y;
            _itemsManager.CurrentItem = gameObject;
            SendToManager();
        }
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            _itemsManager.CurrentItem = gameObject;
            SendToManager();
        }
    }
}
