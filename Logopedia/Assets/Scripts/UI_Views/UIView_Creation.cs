using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Logopedia.GamePlay;
using Logopedia.UIConnection;
using System.Runtime.InteropServices;
using TMPro;
using System.IO;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using Doozy.Engine;

namespace Logopedia.UserInterface
{
    public class UIView_Creation : MonoBehaviour
    {
        [Inject]
        ItemsManager _itemsManager;
        [Inject]
        StoryManager _storyManager;
        [Inject]
        PopUpManager _popUpManager;

        [Inject]
        Garment.Factory _garmentFactory;
        [Inject]
        ItemTemplate.Factory _itemTemplateFactory;
        [Inject]
        BGTemplate.Factory _bgTemplateFactory;
        [Inject]
        CharacterTemplate.Factory _characterTemplateFactory;


        [SerializeField]
        private Image _bg, _character;
        [SerializeField]
        private Slider _scaleSlider, _rotationSlider;
        [SerializeField]
        private GameObject _middlePanel, _samplePrefab, _garment, _item, _itemShadow, _header, _footer, _backFromPreviewButton;
        [SerializeField]
        private GameObject _garmentSamplesContent, _backgroundSamplesContent, _characterSamplesContent;
        [SerializeField]
        private Vector2 _screenCenter;
        [SerializeField]
        private int _instCount;
        [SerializeField]
        private bool _isPreview, _isShadowHiden;
        [SerializeField]
        private TMP_Text _storyName, _sceneNumber;
        [SerializeField]
        private Story _newStory;
        [SerializeField]
        private StoryScene _storyScene;
        private Color _transparent = new Color(255, 255, 255, 0), _opaque = new Color(0, 0, 0, 0.5f);
        [SerializeField]
        private int _lastStoryID = 0, _currentSceneNumber;

        private void Start()
        {
            float x = Screen.currentResolution.width;
            float y = Screen.currentResolution.height;
            _screenCenter = new Vector2(x / 2, y / 2);
            _instCount = 0;
            _itemsManager.MiddleScenePanel = _middlePanel;
            _isPreview = false;
            CreateSamples();
            _currentSceneNumber = 0;
        }

        private void OnEnable()
        {
            if (_storyManager.StoryName != null)
            {
                _storyName.text = _storyManager.StoryName;
            }
        }
        public void PreviewScene()
        {
            _isPreview = !_isPreview;
            _header.SetActive(_isPreview);
            _footer.SetActive(_isPreview);
            _backFromPreviewButton.SetActive(!_isPreview);
        }

        public void HideShadow()
        {
            GetCurrentGarment();
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                var _currentColor = _opaque;
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
        }

        public void SwichShadow()
        {
            GetCurrentGarment();
            GetCurrentGarment();
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                bool _isActive = !_itemShadow.activeSelf;

                {
                    _itemShadow.SetActive(_isActive);
                }
            }
        }

        public void ScaleItem()
        {
            GetCurrentGarment();
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                _item.transform.localScale = new Vector3(_scaleSlider.value, _scaleSlider.value, _scaleSlider.value);
                _itemShadow.transform.localScale = new Vector3(_scaleSlider.value, _scaleSlider.value, _scaleSlider.value);
            }
            else
            {
                _character.transform.localScale = new Vector3(_scaleSlider.value, _scaleSlider.value, _scaleSlider.value);
            }
        }

        public void RotateItem()
        {
            GetCurrentGarment();
            if ((_item != _character) && (_itemShadow != null))
            {
                _item.transform.localRotation = Quaternion.Euler(0, 0, _rotationSlider.value);
                _itemShadow.transform.localRotation = Quaternion.Euler(0, 0, _rotationSlider.value);
            }
            else
            {
                _character.transform.localEulerAngles = new Vector3(0,0, _rotationSlider.value);
            }
        }

        private void GetCurrentGarment()
        {
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                _item = _itemsManager.CurrentItem;
                _itemShadow = _itemsManager.CurrentItemShadow;
                if (_itemShadow.GetComponent<Image>().color == _transparent)
                {
                    _isShadowHiden = true;
                }
                else
                {
                    _isShadowHiden = false;
                }
            }
            else
            {
                _item = _character.gameObject;
            }
        }

        void CreateSamples()
        {
            CreateSpritesSamples("Sprites/GamePlaySprites/Garments", PrefabsPathLibrary.GarmentSample, _garmentSamplesContent, _itemTemplateFactory);
            CreateSpritesSamples("Sprites/GamePlaySprites/BackGrounds", PrefabsPathLibrary.BackGroundSample, _backgroundSamplesContent, _bgTemplateFactory);
            CreateSpritesSamples("Sprites/GamePlaySprites/Characters", PrefabsPathLibrary.CharacterSample, _characterSamplesContent, _characterTemplateFactory);
        }



        void CreateSpritesSamples(string _spriteFolder, string _spriteSample, GameObject _samplesContent, dynamic _factory)
        {
            for (int i = _samplesContent.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(_samplesContent.transform.GetChild(0).gameObject);
            }

            Sprite[] _items = Resources.LoadAll<Sprite>(_spriteFolder);
            float f = 0;
            foreach (Sprite _item in _items)
            {
                var _itemSample = _factory.Create(_spriteSample).gameObject;
                _itemSample.transform.SetParent(_samplesContent.transform);
                _itemSample.transform.localScale = new Vector3(1, 1, 1);
                _itemSample.transform.GetChild(0).GetComponent<Image>().sprite = _item;
                f++;
            }
            float x = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x;
            float k = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale.x;
            float y = _samplesContent.GetComponent<RectTransform>().sizeDelta.y;
            _samplesContent.GetComponent<RectTransform>().sizeDelta = new Vector2(f * x * k, y);
        }

        public void SetTargetPosition()
        {
            GetCurrentGarment();
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                _itemShadow.transform.position = _item.transform.position;
            }
        }

        public void CreateItem()
        {
            var _item = _garmentFactory.Create(PrefabsPathLibrary.Item).gameObject;
            _item.transform.localPosition = _screenCenter;
            _item.transform.SetParent(_middlePanel.transform);
            _item.transform.localScale = (_character.transform.localScale / 4);
            _item.name += _instCount.ToString();
            _instCount++;
        }

        public void DeleteItem()
        {
            GetCurrentGarment();
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                var _currentItem = _itemsManager.CurrentGarment;
                Destroy(_currentItem);
            }
        }

        public void ChangeCharacter()
        {
            var _currentCharacter = _storyManager.Chacter;
            ChangeSprite(_currentCharacter, _character);
        }

        public void ChangeBG()
        {
            var _currentBG = _storyManager.BackGround;
            ChangeSprite(_currentBG, _bg);
        }

        public void ResetControl()
        {
            GetCurrentGarment();
            if (_item != null)
            {
                _scaleSlider.value = _item.transform.localScale.x;
                var y = _item.transform.localRotation.z;
                _rotationSlider.value = y;
                Debug.Log("Angle: " + y.ToString());
            }
        }

        public void ChangeSprite(Sprite _currentSprite, Image _image)
        {
            _image.sprite = _currentSprite;
        }

        public void SearchShadow()
        {
            GetCurrentGarment();
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                _itemShadow.GetComponent<ItemSlot>().ShowAnimation();
            }
        }

        public void CleanScene()
        {
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.СonfirmRemove;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
        }
    }
}
