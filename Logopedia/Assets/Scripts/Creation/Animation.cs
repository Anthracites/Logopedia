using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Doozy.Engine;
using Logopedia.UIConnection;
using UnityEngine.EventSystems;
using Spine.Unity;
using DG.Tweening;

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
        [SerializeField]
        private bool _isAnimationActive;

        public void GetAnimation()
        {
            StartCoroutine(ApplyAnimation());
        }

        IEnumerator ApplyAnimation()
        {
            yield return new WaitForEndOfFrame();
            _animation.skeletonDataAsset = _itemsManager.CharacterAnimation.GetComponent<SkeletonGraphic>().skeletonDataAsset;
            Debug.Log("Animation applaed!");
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
            _isAnimationActive = _animation.gameObject.activeSelf;
            SendToManager();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startX = mousePos.x - transform.position.x;
            _startY = mousePos.y - transform.position.y;
            SendToManager();
        }

        public void PlayAnimation()
        {
            _animation.AnimationState.SetAnimation(0, "action", false);
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            SendToManager();
        }

        public void SwichAnimation()
        {
            _isAnimationActive = !_isAnimationActive;
            _animation.gameObject.SetActive(_isAnimationActive);
        }
    }
}
