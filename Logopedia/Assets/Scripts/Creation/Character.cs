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

        [SerializeField]
        private GameEventListener[] _listeners;

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
            GameEventMessage.SendEvent(EventsLibrary.ItemSelected);

            _itemsManager.SelectedGarments.Clear();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _itemsManager.SelectedGarments.Clear();
            _itemsManager.SelectedGarments.RemoveAll(x => x == null);
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
        }

        public void MirrorCharacter()
        {
            Vector3 _scale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
        }

        public void ResetCharacterRotation()
        {
            gameObject.transform.eulerAngles = Vector3.zero;
        }

    }
}

