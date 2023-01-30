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
using UnityEngine.SceneManagement;
using Doozy.Engine.UI;
using System.Linq;
using System.Drawing;

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
        [Inject]
        StoryCreationPanel.Factory _storyCreationPanelFactory;


        [SerializeField]
        private UnityEngine.UI.Image _bg, _character;
        [SerializeField]
        private Slider _scaleSlider, _rotationSlider;
        [SerializeField]
        private GameObject _middlePanel, _samplePrefab, _garment, _item, _itemShadow, _header, _footer, _backFromPreviewButton, _headerMiddlePanel;
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

        private UnityEngine.Color _transparent = new UnityEngine.Color(255, 255, 255, 0), _opaque = new UnityEngine.Color(0, 0, 0, 0.5f);
        [SerializeField]
        private int _lastStoryID = 0, _currentSceneNumber;
        [SerializeField]
        private Button PreviousSceneButton, NextSceneButton;
        [SerializeField]
        private List<UIView> _storyScenes = new List<UIView>();
        [SerializeField]
        private UnityEngine.UI.Image _addSwich;
        private FileInfo[] _files;



        private void OnEnable()
        {
            if (_storyManager.IsStoryCreartionStart == true)
            {
                CreateSceneBlank();
                _instCount = 0;
                _itemsManager.MiddleScenePanel = _middlePanel;
                _storyManager.IsStorySave = false;
                _isPreview = false;
                CreateSamples();
                _currentSceneNumber = 0;
                _storyManager.CurrentStory.Scenes = new List<StoryScene>();
                _storyManager.CurrentStorySceneIndex = 0;
                var _newScene = new StoryScene();
                _newScene.SceneNumberInStory = 0;

                if (_storyManager.CurrentStory.StoryName != null)
                {
                    _storyName.text = _storyManager.CurrentStory.StoryName;
                }
                ConfigSwichButton();
                ShowCurrentSceneNumber();
                _storyScenes[0].Show();
                SwichScene();
            }
        }

        public void ShowCurrentSceneNumber()
        {
            _sceneNumber.text = ("Scene number " + (_storyManager.CurrentStorySceneIndex + 1)).ToString();
        }

        private void OnDisable()
        {
            var _allItems = _itemsManager.Garments;
            foreach (GameObject _go in _allItems)
            {
                Destroy(_go);
            }
            _allItems.Clear();
            foreach (UIView _sceneBlank in _storyScenes)
            {
                Destroy(_sceneBlank.gameObject);
            }
            _storyScenes.Clear();
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
                _itemShadow.GetComponent<UnityEngine.UI.Image>().color = _currentColor;
                _isShadowHiden = !_isShadowHiden;
            }
        }

        public void SwichShadow()
        {
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
            if ((_item != _character.gameObject) && (_itemShadow != null))
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
            if ((_item != _character.gameObject) && (_itemShadow != null))
            {
                _item.transform.eulerAngles = new Vector3(0, 0, _rotationSlider.value);
                _itemShadow.transform.eulerAngles = new Vector3(0, 0, _rotationSlider.value);
            }
            else
            {
                _character.transform.eulerAngles = new Vector3(0, 0, _rotationSlider.value);
            }
        }

        public void MirrowItem()
        {
            GetCurrentGarment();

            if ((_item != _character.gameObject) && (_itemShadow != null))
            {
                Vector3 _scale = _item.transform.localScale;
                _item.transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
                _itemShadow.transform.localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
            }
            else
            {
                Vector3 _scale = _character.transform.localScale;
                _character.transform.eulerAngles = new Vector3(-_scale.x, _scale.y, _scale.z);
            }

        }

        private void GetCurrentGarment()
        {
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                _item = _itemsManager.CurrentItem;
                _itemShadow = _itemsManager.CurrentItemShadow;
                if (_itemShadow.GetComponent<UnityEngine.UI.Image>().color == _transparent)
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
            CreateSpritesSamples(SpritesPathLibrary.GarmentSprites, PrefabsPathLibrary.GarmentSample, _garmentSamplesContent, _itemTemplateFactory);
            CreateSpritesSamples(SpritesPathLibrary.BGSprites, PrefabsPathLibrary.BackGroundSample, _backgroundSamplesContent, _bgTemplateFactory);
            CreateSpritesSamples(SpritesPathLibrary.CharacterSprites, PrefabsPathLibrary.CharacterSample, _characterSamplesContent, _characterTemplateFactory);
        }



        void CreateSpritesSamples(string _spriteFolder, string _spriteSample, GameObject _samplesContent, dynamic _factory)
        {
            for (int i = _samplesContent.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(_samplesContent.transform.GetChild(0).gameObject);
            }

            DirectoryInfo _contentDirectory =  new DirectoryInfo(Application.dataPath + "/Resources/" + _spriteFolder);
            FileInfo[] _files = new string[] { "*.jpg", "*jpeg", "*.png" }.SelectMany(ext => _contentDirectory.GetFiles(ext, SearchOption.TopDirectoryOnly)).ToArray();

            int f = 0;
            foreach (FileInfo _file in _files)
            {
                var _itemSample = _factory.Create(_spriteSample).gameObject;
                _itemSample.name = _file.FullName;
                _itemSample.transform.SetParent(_samplesContent.transform);
                _itemSample.transform.localScale = new Vector3(1, 1, 1);


                WWW _www = new WWW("file://" + _file.FullName);
                Debug.Log(_www.url);
                   Rect _rect = new Rect(0, 0, _www.texture.width, _www.texture.height);

                Sprite _sprite = Sprite.Create(_www.texture, _rect, new Vector2(0.5f, 0.5f));

                _itemSample.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = _sprite;
                f++;
            }

            float x = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x;
            float k = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale.x;
            float s = _samplesContent.GetComponent<HorizontalLayoutGroup>().spacing;
            float p = _samplesContent.GetComponent<HorizontalLayoutGroup>().padding.left;
            float y = _samplesContent.GetComponent<RectTransform>().sizeDelta.y;

            _samplesContent.GetComponent<RectTransform>().sizeDelta = new Vector2(((f * ((x * k) + s)) + (p*2)) - s, y);
        }

        public void SetTargetPosition()
        {
            GetCurrentGarment();
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                _itemShadow.transform.position = _item.transform.position;
            }
        }

        public void ResetItemRotation()
        {
            GetCurrentGarment();

            if ((_item != _character.gameObject) && (_itemShadow != null))
            {
                _item.transform.eulerAngles = Vector3.zero;
                _itemShadow.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                _character.transform.eulerAngles = Vector3.zero;
            }
            ResetControl();
        }

        public void SlowRotaion(int k)
        {
            GetCurrentGarment();

            float currentRotation = _item.transform.eulerAngles.z;
            float newRotation = currentRotation + (0.5f * k);

            if ((_item != _character.gameObject) && (_itemShadow != null))
            {
                _item.transform.eulerAngles = new Vector3(0, 0, newRotation);
                _itemShadow.transform.eulerAngles = new Vector3(0, 0, newRotation);
            }
            else
            {
                _character.transform.eulerAngles = new Vector3(0, 0, newRotation);
            }
            ResetControl();
        }

        public void SlowScale(int k)
        {
            GetCurrentGarment();

            float currentScale = _item.transform.localScale.z;
            float newScale = currentScale + (0.1f * k);

            if ((_item != _character.gameObject) && (_itemShadow != null))
            {
                _item.transform.localScale = new Vector3(newScale, newScale, newScale);
                _itemShadow.transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            else
            {
                _character.transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            ResetControl();
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
                _itemsManager.Garments.Remove(_currentItem);
                _itemsManager.Garments.RemoveAll(x => x == null);
            }

        }

        public void ChangeCharacter()
        {
            var _currentCharacter = _itemsManager.Character.GetComponent<UnityEngine.UI.Image>().sprite;
            ChangeSprite(_currentCharacter, _character);
            _character.gameObject.name = _currentCharacter.name;
        }

        public void ChangeBG()
        {
            var _currentBG = _itemsManager.Background.GetComponent<UnityEngine.UI.Image>().sprite;
            ChangeSprite(_currentBG, _bg);
            _bg.gameObject.name = _currentBG.name;
        }

        public void ResetControl()
        {
            GetCurrentGarment();
            if (_item != null)
            {
                _scaleSlider.value = _item.transform.localScale.x;
                var a = _item.transform.eulerAngles.z;
                a = Mathf.Repeat(a + 180, 360) - 180;
                _rotationSlider.value = a;
            }
        }

        public void ChangeSprite(Sprite _currentSprite, UnityEngine.UI.Image _image)
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
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.СonfirmCleanScene;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
        }

        public void SaveStory()
        {
            _storyManager.IsStorySave = true;
            GameEventMessage.SendEvent(EventsLibrary.SaveStory);
        }

        public void ConfigSwichButton()
        {
            _currentSceneNumber = _storyManager.CurrentStorySceneIndex;
            PreviousSceneButton.gameObject.SetActive(!(_currentSceneNumber == 0));

            Sprite _buttonSprite;

            if (_currentSceneNumber == _storyScenes.Count - 1)
            {
                _buttonSprite = Resources.Load<Sprite>("Sprites/UI/AddNew");
            }
            else
            {
                _buttonSprite = Resources.Load<Sprite>("Sprites/UI/angleRight");
            }
//            Debug.Log("Current scene number: " + _currentSceneNumber.ToString() + " , Story scenes count: " + (_storyScenes.Count - 1).ToString());
            _addSwich.sprite = _buttonSprite;
        }

        void SwichScene()
        {
            _character = _itemsManager.Character.GetComponent<UnityEngine.UI.Image>();
            _bg = _itemsManager.Background.GetComponent<UnityEngine.UI.Image>();
        }

        public void GoToNextScene()
        {
           // SaveStory();
            _currentSceneNumber = _storyManager.CurrentStorySceneIndex;
            if (_storyScenes.Count - 1 > _currentSceneNumber)
            {
                _storyScenes[_currentSceneNumber].HideBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
                _storyScenes[_currentSceneNumber].Hide();
                _storyScenes[_currentSceneNumber + 1].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
                _storyScenes[_currentSceneNumber + 1].Show();
            }
            else
            {
                CreateSceneBlank();
                _storyScenes[_currentSceneNumber].HideBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
                _storyScenes[_currentSceneNumber].Hide();
                _storyScenes[_currentSceneNumber + 1].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
                _storyScenes[_currentSceneNumber + 1].Show();
            }
            _storyManager.CurrentStorySceneIndex = _currentSceneNumber + 1;
            ConfigSwichButton();
            _middlePanel = _storyScenes[_currentSceneNumber].gameObject;
            _itemsManager.MiddleScenePanel = _middlePanel;
            SwichScene();
            ShowCurrentSceneNumber();

        }

        private void CreateSceneBlank()
        {
            var _newStoryScenePanel = _storyCreationPanelFactory.Create(PrefabsPathLibrary.StoryCreationPanel).gameObject.GetComponent<UIView>();
            _newStoryScenePanel.gameObject.transform.SetParent(_headerMiddlePanel.transform);
            _newStoryScenePanel.Hide();
            _header.transform.SetSiblingIndex(_storyScenes.Count + 1);
            _storyScenes.Add(_newStoryScenePanel);
            _newStoryScenePanel.gameObject.name = (_storyScenes.Count - 1).ToString();
            _middlePanel = _newStoryScenePanel.gameObject;
        }

        public void GoToPreviousScene()
        {
            _currentSceneNumber = _storyManager.CurrentStorySceneIndex;
            _storyScenes[_currentSceneNumber].HideBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
            _storyScenes[_currentSceneNumber].Hide();
            _storyScenes[_currentSceneNumber - 1].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
            _storyScenes[_currentSceneNumber - 1].Show();
            _storyManager.CurrentStorySceneIndex = _currentSceneNumber - 1;
            ConfigSwichButton();
            _middlePanel = _storyScenes[_currentSceneNumber].gameObject;
            _itemsManager.MiddleScenePanel = _middlePanel;
            SwichScene();
            ShowCurrentSceneNumber();
        }

        public void DeleteSceneButton()
        {
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.СonfirmRemoveScene;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);

        }

        public void DeleteScene()
        {
            if (_storyScenes.Count > 1)
            {
                var _sceneForDelete = _storyScenes[_currentSceneNumber];
                _storyScenes.Remove(_sceneForDelete);
                Destroy(_sceneForDelete.gameObject);
                _storyManager.CurrentStory.Scenes.Remove(_storyManager.CurrentStory.Scenes[_currentSceneNumber]);
                if (_currentSceneNumber == 0)
                { 
                    //_currentSceneNumber++;
                    _storyScenes[_currentSceneNumber].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
                    _storyScenes[_currentSceneNumber].Show();
                    _storyManager.CurrentStorySceneIndex = _currentSceneNumber;
                    ShowCurrentSceneNumber();
                }
                else
                {
                    _currentSceneNumber--;
                    _storyScenes[_currentSceneNumber].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
                    _storyScenes[_currentSceneNumber].Show();
                    _storyManager.CurrentStorySceneIndex = _currentSceneNumber;
                    ShowCurrentSceneNumber();
                }
                ConfigSwichButton();
            }

            int i = 0;
            foreach (UIView _scenePanel in _storyScenes)
            {
                _scenePanel.gameObject.name = i.ToString();
                i++;
            }
        }


        public void CopyItem()
        {
            if ((_itemsManager.CurrentItem != _character) && (_itemsManager.CurrentItemShadow != null))
            {
                GetCurrentGarment();
                var _garment = _garmentFactory.Create(PrefabsPathLibrary.Item).gameObject;
                _garment.transform.SetParent(_itemsManager.MiddleScenePanel.transform);
                var _copyItem = _garment.transform.GetChild(1).gameObject;
                var _copyItemShadow = _garment.transform.GetChild(0).gameObject;

                Sprite _itemSprite = _item.GetComponent<UnityEngine.UI.Image>().sprite;
                Vector3 _garmentPosition = _itemsManager.CurrentGarment.transform.localPosition;
                Vector3 _itemPosition = _item.transform.localPosition;
                Vector3 _itemShadowPosition = _itemShadow.transform.localPosition;
                Vector3 _itemScale = _item.transform.localScale;
                Vector3 _itemRotation = _item.transform.localEulerAngles;
                bool _isShadowEnable = _itemShadow.activeSelf;
                UnityEngine.Color _itemShadowColor = _itemShadow.GetComponent<UnityEngine.UI.Image>().color;

                _garment.transform.localScale = Vector3.one;
                _garment.transform.localPosition = _garmentPosition;


                _copyItem.GetComponent<UnityEngine.UI.Image>().sprite = _itemSprite;
                _copyItemShadow.GetComponent<UnityEngine.UI.Image>().sprite = _itemSprite;

                _copyItem.transform.localPosition = _itemPosition;
                _copyItemShadow.transform.localPosition = _itemShadowPosition;

                _copyItem.transform.localScale = _itemScale;
                _copyItemShadow.transform.localScale = _itemScale;

                _copyItem.transform.localEulerAngles = _itemRotation;
                _copyItemShadow.transform.localEulerAngles = _itemRotation;

                _copyItemShadow.GetComponent<UnityEngine.UI.Image>().color = _itemShadowColor;
                _copyItemShadow.SetActive(_isShadowEnable);

                _itemsManager.Garments.Add(_item);
            }
        }

    }
}
