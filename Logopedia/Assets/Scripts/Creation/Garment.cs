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
using Spine.Unity;
using UniRx;
using System.Security.Policy;
using System;
using System.Linq;

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
        [SerializeField]
        private GameEventListener[] _listeners;
        [SerializeField]
        private bool _isShadowHiden;
        [SerializeField]
        private SkeletonGraphic _stars;
        private float _modifyParametr;

        private UnityEngine.Color _transparent = new Color(0, 0, 0, 0), _opaque = new Color(0, 0, 0, 0.5f);
        private CompositeDisposable _disposable = new CompositeDisposable();


        private void Awake()
        {
            GameEventMessage.SendEvent(EventsLibrary.ItemCreated);
            _moveWithMouse = MoveWhithMouse();
            if (_storyManager.IsStoryEdit == false)
            {
                StartCoroutine(_moveWithMouse);
            }
            gameObject.GetComponent<Image>().raycastTarget = false;

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
            _itemsManager.SelectedGarments.Remove(gameObject);
            _itemsManager.SelectedGarments.RemoveAll(x => x == null);
        }

        public void MirrorGarment()
        {
            Vector3 _scale = _item.transform.localScale;
            _item.transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
            _itemShadow.transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
        }

        public void SelectObject(bool _isSelected)
        {
            _isSelected = _itemsManager.SelectedGarments.Contains(gameObject);

            foreach (GameEventListener _listener in _listeners)
            {
                _listener.enabled = _isSelected;
            }

            if ((_isSelected == true) & (_disposable.Count == 0))
            {
                 //var a = UserInterface.UIView_Creation.ItemScale.SkipLatestValueOnSubscribe().Subscribe(_ => ScaleItem()).AddTo(_disposable);
                 //var b = UserInterface.UIView_Creation.ItemRotation.SkipLatestValueOnSubscribe().Subscribe(_ => RotateItem()).AddTo(_disposable);
                Debug.Log("Item " + gameObject.name + " subscribed!!!" + "Disposable count: " + _disposable.Count.ToString());
            }
            else if (_isSelected == false)
            {
                _disposable.Clear();
                Debug.Log("Item " + gameObject.name +  " unsubscribed!!!");
            }
        }

        public void DeselectObject()
        {
            _itemsManager.SelectedGarments.Clear();
            _itemsManager.SelectedGarments.Remove(gameObject);
            _itemsManager.SelectedGarments.RemoveAll(x => x == null);
            SelectObject(false);
        }

        public void SetShadowOnItem()
        {
            _itemShadow.transform.position = _item.transform.position;
        }

        public void ResetItemRotation()
        {
            _item.transform.eulerAngles = Vector3.zero;
            _itemShadow.transform.eulerAngles = Vector3.zero;
        }

        public void SwichItemShadow()
        {
                bool _isActive = !_itemShadow.activeSelf;

                {
                    _itemShadow.SetActive(_isActive);
                }
        }

        public void SwichSahdowVisible()
        {
            var _currentColor = new Color();
            if (_isShadowHiden)
            {
                _currentColor = _opaque;
            }
            else
            {
                _currentColor = _transparent;
            }
            _itemShadow.GetComponent<Image>().color = _currentColor;
            _isShadowHiden = !_isShadowHiden;
        }

        public void SearchShadow()
        {
            _stars.enabled = true;
            _stars.AnimationState.SetAnimation(0, "correct_answer", false);
        }

        public void RotateItem()
        {
            _modifyParametr = _itemsManager.UI_Parametr;
            if (_modifyParametr != 0)
            {

                float _curretnRotation = _item.transform.eulerAngles.z;
                float newRotation = _curretnRotation + (0.5f * _modifyParametr);
                _item.transform.eulerAngles = new Vector3(0, 0, newRotation);
                _itemShadow.transform.eulerAngles = new Vector3(0, 0, newRotation);
            }
            else
            {
                float newRotation = UserInterface.UIView_Creation.ItemRotation.Value;
                _item.transform.eulerAngles = new Vector3(0, 0, newRotation);
                _itemShadow.transform.eulerAngles = new Vector3(0, 0, newRotation);

            }
            Debug.Log("Item rotated!!!" + _itemsManager.UI_Parametr.ToString());

        }

        public void ScaleItem()
        {
            _modifyParametr = _itemsManager.UI_Parametr;
            float a = Mathf.Sign(_item.transform.localScale.x);
            if (_modifyParametr != 0)
            {
                float _currentScale = _item.transform.localScale.y;
                float newScale = _currentScale + (0.1f * _modifyParametr);

                _item.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                _itemShadow.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                Debug.Log("Item " + gameObject.name + " scaled1!!! a = " + a.ToString());

            }
            else
            {
                float newScale = UserInterface.UIView_Creation.ItemScale.Value;

                _item.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                _itemShadow.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                 Debug.Log("Item " + gameObject.name + "  scaled2!!!"+ "Scele index: " + newScale.ToString());

            }
        }

        public class Factory : PlaceholderFactory<string, Garment>
        {

        }
    }
}
