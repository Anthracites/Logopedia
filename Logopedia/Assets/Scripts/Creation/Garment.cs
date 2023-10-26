 using System.Collections;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Doozy.Engine;
using Spine.Unity;
using UniRx;


namespace Logopedia.GamePlay
{
    public class Garment : MonoBehaviour, IDropHandler, IPointerUpHandler, IEndDragHandler, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;
        [Inject]
        StoryManager _storyManager;
        [Inject]
        Garment.Factory _garmentFactory;

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
        private float _currentScale;
        private float _curretnRotation;
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
            _item.GetComponent<Image>().raycastTarget = true;
            _itemShadow.GetComponent<Image>().raycastTarget = true;

        }


        public void OnEndDrag(PointerEventData eventData)
        {
            StopCoroutine(_moveWithMouse);
        }

        public void OnPointerUp(PointerEventData eventData)
        { 
            StopCoroutine(_moveWithMouse);
            _item.GetComponent<Image>().raycastTarget = true;
            _itemShadow.GetComponent<Image>().raycastTarget = true;
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
                _curretnRotation = _item.transform.eulerAngles.z;
                _currentScale = _item.transform.localScale.y;
                UserInterface.UIView_Creation.ItemScale.SkipLatestValueOnSubscribe().Subscribe(_ => ScaleItem()).AddTo(_disposable);
                UserInterface.UIView_Creation.ItemRotation.SkipLatestValueOnSubscribe().Subscribe(_ => RotateItem()).AddTo(_disposable);
            }
            else if (_isSelected == false)
            {
                _disposable.Clear();
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
            _curretnRotation = _item.transform.eulerAngles.z;

            if (_itemsManager.SelectedGarments.Count <= 1)
            {
                if (_modifyParametr != 0)
                {
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
            }
            else
            {
                float f = UserInterface.UIView_Creation.ItemRotation.Value;
                float newRotation = f + _curretnRotation;
                _item.transform.eulerAngles = new Vector3(0, 0, newRotation);
                _itemShadow.transform.eulerAngles = new Vector3(0, 0, newRotation);

            }
        }

        public void ScaleItem()
        {
            _modifyParametr = _itemsManager.UI_Parametr;
            float a = Mathf.Sign(_item.transform.localScale.x);
            _currentScale = _item.transform.localScale.y;

            if (_itemsManager.SelectedGarments.Count <= 1)
            {
                if (_modifyParametr != 0)
                {
                    float newScale = _currentScale + (0.1f * _modifyParametr);
                    _item.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                    _itemShadow.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                    Debug.Log("Item scaled1!!! a = " + a.ToString()+ ", modify parametr: " + _modifyParametr.ToString());

                }
                else
                {
                    float newScale = UserInterface.UIView_Creation.ItemScale.Value;
                    _item.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                    _itemShadow.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                    Debug.Log("Item " + gameObject.name + "  scaled2!!!" + "Scele index: " + newScale.ToString());

                }
            }
            else
            {
                float f = UserInterface.UIView_Creation.ItemScale.Value - 1;
                float newScale = f + _currentScale;
                _item.transform.localScale = new Vector3(newScale * a, newScale, newScale);
                _itemShadow.transform.localScale = new Vector3(newScale * a, newScale, newScale);

            }
        }

        public void CopyItem()
        {
            if (_itemsManager.SelectedGarments.Count > 0)
            {
                    GameObject obj = gameObject;
                    string _name = obj.name;
                    Sprite _itemSprite = _item.GetComponent<UnityEngine.UI.Image>().sprite;
                    Vector3 _itemScale = _item.transform.localScale;
                    Vector3 _itemRotation = _item.transform.localEulerAngles;
                    bool _isShadowEnable = _itemShadow.activeSelf;
                    UnityEngine.Color _itemShadowColor = _itemShadow.GetComponent<UnityEngine.UI.Image>().color;


                    var _garment = _garmentFactory.Create(PrefabsPathLibrary.Item).gameObject;
                    _garment.transform.SetParent(_itemsManager.GarmenScenePanel.transform);
                    Debug.Log(_name);
                    _garment.name = _name;
                    var _copyItem = _garment.transform.GetChild(1).gameObject;
                    var _copyItemShadow = _garment.transform.GetChild(0).gameObject;

                    _garment.transform.localScale = Vector3.one;
                    _garment.transform.localPosition = Input.mousePosition;

                _garment.GetComponent<Image>().raycastTarget = true;
                _copyItem.GetComponent<Image>().raycastTarget = false;
                _copyItemShadow.GetComponent<Image>().raycastTarget = false;



                _copyItem.GetComponent<UnityEngine.UI.Image>().sprite = _itemSprite;
                    _copyItemShadow.GetComponent<UnityEngine.UI.Image>().sprite = _itemSprite;

                   // _copyItem.transform.localPosition = _garmentPosition;
                   // _copyItemShadow.transform.localPosition = _garmentPosition;

                    _copyItem.transform.localScale = _itemScale;
                    _copyItemShadow.transform.localScale = _itemScale;

                    _copyItem.transform.localEulerAngles = _itemRotation;
                    _copyItemShadow.transform.localEulerAngles = _itemRotation;

                    _copyItemShadow.GetComponent<UnityEngine.UI.Image>().color = _itemShadowColor;
                    _copyItemShadow.SetActive(_isShadowEnable);

                    _itemsManager.Garments.Add(obj);
            }
        }

        void OnDestroy()
        {
            DeselectObject();
            _itemsManager.Garments.Remove(gameObject);
            _itemsManager.Garments.RemoveAll(x => x == null);
        }

        public class Factory : PlaceholderFactory<string, Garment>
        {

        }
    }
}
