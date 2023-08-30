using System.Collections;
using System.Collections.Generic;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine;
using UnityEngine.EventSystems;
using UniRx;

namespace Logopedia.GamePlay
{
    public class Character : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;


        [SerializeField]
        private float _startX, _startY;
        [SerializeField]
        private Outline _outline;
        [SerializeField]
        private GameObject _animation, _animationParent;
        private float _modifyParametr;
        [SerializeField]
        private GameObject _shadow;


        [SerializeField]
        private GameEventListener[] _listeners;
        private CompositeDisposable _disposable = new CompositeDisposable();


        void Start()
        {
            StartMove();
        }

        void StartMove()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _startX = mousePos.x - transform.position.x;
            _startY = mousePos.y - transform.position.y;
            _outline.enabled = true;
            _itemsManager.CharacterAnimation = _animation;
            _itemsManager.SelectedGarments.Clear();
            _itemsManager.SelectedGarments.RemoveAll(x => x == null);
            _itemsManager.SelectedGarments.Add(gameObject);
            Debug.Log("Selected garment count: " + _itemsManager.SelectedGarments.Count.ToString());
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);

            _itemsManager.SelectedGarments.Clear();
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


        public void OnPointerClick(PointerEventData pointerEventData)
        {
            _itemsManager.SelectedGarments.Clear();
            _itemsManager.SelectedGarments.RemoveAll(x => x == null);
            StartMove();
            SwichSelectCharacter(true);
        }


        public void SwichSelectCharacter(bool _isCharacterSelected)
        {
            _outline.enabled = _isCharacterSelected;
            foreach (GameEventListener _listener in _listeners)
            {
                _listener.enabled = _isCharacterSelected;
            }

            if ((_isCharacterSelected == true) & (_disposable.Count == 0))
            {
                UserInterface.UIView_Creation.ItemScale.SkipLatestValueOnSubscribe().Subscribe(_ => ScaleItem()).AddTo(_disposable);
                UserInterface.UIView_Creation.ItemRotation.SkipLatestValueOnSubscribe().Subscribe(_ => RotateItem()).AddTo(_disposable);
            }
            else if (_isCharacterSelected == false)
            {
                _disposable.Clear();
            }
        }

        public void RotateItem()
        {
            _modifyParametr = _itemsManager.UI_Parametr;
            if (_modifyParametr != 0)
            {

                float _curretnRotation = gameObject.transform.eulerAngles.z;
                float newRotation = _curretnRotation + (0.5f * _modifyParametr);
                gameObject.transform.eulerAngles = new Vector3(0, 0, newRotation);
                _shadow.transform.eulerAngles = new Vector3(0, 0, newRotation);

            }
            else
            {
                float newRotation = UserInterface.UIView_Creation.ItemRotation.Value;
                gameObject.transform.eulerAngles = new Vector3(0, 0, newRotation);
                _shadow.transform.eulerAngles = new Vector3(0, 0, newRotation);
            }
            Debug.Log("Item rotated!!!" + _itemsManager.UI_Parametr.ToString());

        }

        public void ScaleItem()
        {
            _modifyParametr = _itemsManager.UI_Parametr;
            float a = Mathf.Sign(gameObject.transform.localScale.x);
            if (_modifyParametr != 0)
            {
                float _currentScale = gameObject.transform.localScale.y;
                float newScale = _currentScale + (0.1f * _modifyParametr);

                gameObject.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                _shadow.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                Debug.Log("Item " + gameObject.name + " scaled1!!! a = " + a.ToString());

            }
            else
            {
                float newScale = UserInterface.UIView_Creation.ItemScale.Value;

                gameObject.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                _shadow.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                Debug.Log("Item " + gameObject.name + "  scaled2!!!" + "Scele index: " + newScale.ToString());
            }
        }

        public void MirrorCharacter()
        {
            Vector3 _scale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
            _shadow.transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
        }

        public void ResetCharacterRotation()
        {
            gameObject.transform.eulerAngles = Vector3.zero;
            _shadow.transform.eulerAngles = Vector3.zero;
        }

    }
}

