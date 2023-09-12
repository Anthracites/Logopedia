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
using Spine.Unity;
using UniRx;

namespace Logopedia.UserInterface
{
    public class UIView_Creation : MonoBehaviour
    {
        [Inject]
        SpritesManager _spritesManager;
        [Inject]
        ItemsManager _itemsManager;
        [Inject]
        StoryManager _storyManager;
        [Inject]
        PopUpManager _popUpManager;
        [Inject]
        TopicIcon.Factory _topicIconFactory;
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
        private GameObject _middlePanel, _samplePrefab, _itemShadow, _header, _footer, _backFromPreviewButton, _headerMiddlePanel, _splashScreenPanel;
        [SerializeField]
        private List<GameObject> currentItems, currentShadows, currentGarments = new List<GameObject>();
        [SerializeField]
        private GameObject _garmentSamplesContent, _backgroundSamplesContent, _characterSamplesContent, _hiddenCharacterButtonSprite, _isSplashScreenButtonSprite, _topicIconsContent;
        [SerializeField]
        private Vector2 _screenCenter;
        [SerializeField]
        private int _instCount;
        [SerializeField]
        private bool _isPreview, _isShadowHiden, _isCharacterHidden, _isCurrentSceneSplashScreen;
        [SerializeField]
        private TMP_Text _storyName, _sceneNumber;
        [SerializeField]
        private int _lastStoryID = 0, _currentSceneNumber;
        [SerializeField]
        private Button PreviousSceneButton, NextSceneButton;
        [SerializeField]
        private List<UIView> _storyScenes = new List<UIView>();
        [SerializeField]
        private UnityEngine.UI.Image _addSwich;
        [SerializeField]
        private TMP_Dropdown _sceneNavigationDropdown;
        private FileInfo[] _files;
        List<Sprite> _topicIcons = new List<Sprite>();
        private UnityEngine.Color _transparent = new UnityEngine.Color(0, 0, 0, 0), _opaque = new UnityEngine.Color(0, 0, 0, 0.5f);

        public static FloatReactiveProperty ItemScale = new FloatReactiveProperty();
        public static ReactiveProperty<float> ItemRotation = new ReactiveProperty<float>();
        public FloatReactiveProperty be = new FloatReactiveProperty();



        void OpenStory()
        {
            _storyScenes.Clear();
            _currentSceneNumber = 0;
            var currentStory = _storyManager.CurrentStory;

            foreach (StoryScene _scene in currentStory.Scenes)
            {
                var _scenePanel = _storyCreationPanelFactory.Create(PrefabsPathLibrary.StoryCreationPanel).gameObject;
                _scenePanel.gameObject.transform.SetParent(_headerMiddlePanel.transform);
                _scenePanel.transform.position = Vector3.zero;

                _scenePanel.name = _scene.SceneNumberInStory.ToString();
                _storyScenes.Add(_scenePanel.GetComponent<UIView>());


                var _bg = _scenePanel.transform.GetChild(0).gameObject;

                WWW _BGwww = new WWW("file://" + _scene.CurrentBGForSave);

                Rect _BGrect = new Rect(0, 0, _BGwww.texture.width, _BGwww.texture.height);
                Sprite _bgSprite = Sprite.Create(_BGwww.texture, _BGrect, new Vector2(0.5f, 0.5f));

                _bg.GetComponent<UnityEngine.UI.Image>().sprite = _bgSprite;
                _bg.name = _scene.CurrentBGForSave;

                var _character = _scenePanel.transform.GetChild(1).gameObject;
                _character.SetActive(_scene.SceneCharacter.IsChacterActive);
                string _characterPath = _scene.SceneCharacter.CharacterSprite;

                WWW _Characterwww = new WWW("file://" + _characterPath);
                Rect _Characterrect = new Rect(0, 0, _Characterwww.texture.width, _Characterwww.texture.height);
                Sprite _characterSprite = Sprite.Create(_Characterwww.texture, _Characterrect, new Vector2(0.5f, 0.5f));

                _character.GetComponent<UnityEngine.UI.Image>().sprite = _characterSprite;
                _character.name = _characterPath;
                _character.transform.localPosition = new Vector3(_scene.SceneCharacter.CharacterPosition.x, _scene.SceneCharacter.CharacterPosition.y, _scene.SceneCharacter.CharacterPosition.z);
                _character.transform.localEulerAngles = new Vector3(_scene.SceneCharacter.CharacterRotation.x, _scene.SceneCharacter.CharacterRotation.y, _scene.SceneCharacter.CharacterRotation.z);
                _character.transform.localScale = new Vector3(_scene.SceneCharacter.CharacterScale.x, _scene.SceneCharacter.CharacterScale.y, _scene.SceneCharacter.CharacterScale.z);

                GameObject _garmentPanel = _scenePanel.transform.GetChild(2).gameObject;
                _scenePanel.transform.GetChild(3).gameObject.SetActive(_scene.IsSceneSplashScreen);

                foreach (StoryScene.SceneItem _sceneItem in _scene.Items)
                {
                    var _garment = _garmentFactory.Create(PrefabsPathLibrary.Item).gameObject;
                    _garment.transform.SetParent(_garmentPanel.transform);

                    _garment.transform.localScale = Vector3.one;
                    _garment.transform.localPosition = new Vector3(_sceneItem.GarmentPosition.x, _sceneItem.GarmentPosition.y, _sceneItem.GarmentPosition.z);
                    var _item = _garment.transform.GetChild(1).gameObject;
                    var _itemShadow = _garment.transform.GetChild(0).gameObject;


                    WWW _Itemwww = new WWW("file://" + _sceneItem.ItemSprite);
                    Rect _Itemrect = new Rect(0, 0, _Itemwww.texture.width, _Itemwww.texture.height);
                    Sprite _itemSprite = Sprite.Create(_Itemwww.texture, _Itemrect, new Vector2(0.5f, 0.5f));

                    _garment.name = _sceneItem.ItemSprite;

                    _item.GetComponent<UnityEngine.UI.Image>().sprite = _itemSprite;
                    _itemShadow.GetComponent<UnityEngine.UI.Image>().sprite = _itemSprite;

                    var _itemPosition = new Vector3(_sceneItem.ItemPosition.x, _sceneItem.ItemPosition.y, _sceneItem.ItemPosition.z);
                    _item.transform.localPosition = _itemPosition;
                    _itemShadow.transform.localPosition = new Vector3(_sceneItem.ItemShadowPosition.x, _sceneItem.ItemShadowPosition.y, _sceneItem.ItemShadowPosition.z);
                    //                    Debug.Log(_item.transform.position.ToString());

                    Vector3 _scale = new Vector3(_sceneItem.ItemScale.y, _sceneItem.ItemScale.y, _sceneItem.ItemScale.z);
                    _item.transform.localScale = _scale;
                    _itemShadow.transform.localScale = _scale;

                    Vector3 _rotation = new Vector3(_sceneItem.ItemRotation.x, _sceneItem.ItemRotation.y, _sceneItem.ItemRotation.z);
                    _item.transform.localEulerAngles = _rotation;
                    _itemShadow.transform.localEulerAngles = _rotation;
                    //_garment.transform.SetParent(_scenePanel.transform);

                    bool _isActive = _sceneItem.ShadowEnabled;
                    _itemShadow.SetActive(_isActive);


                    bool _isVisible = _sceneItem.ShadowVisible;

                    var _currentColor = _opaque;

                    if (_isVisible == true)
                    {
                        _currentColor = _opaque;
                    }
                    else
                    {
                        _currentColor = _transparent;
                    }
                    _itemShadow.GetComponent<UnityEngine.UI.Image>().color = _currentColor;
                    _itemsManager.Garments.Add(_garment);
                }
            }

            foreach (UIView _panel in _storyScenes)
            {
                _panel.GetComponent<UIView>().Hide();
            }
            _header.transform.SetSiblingIndex(_storyScenes.Count + 1);
            _storyScenes[0].GetComponent<UIView>().Show();
        }

        public void SetSplashScreen()
        {
            _isCurrentSceneSplashScreen = !_isCurrentSceneSplashScreen;
            _splashScreenPanel.SetActive(_isCurrentSceneSplashScreen);
            _isSplashScreenButtonSprite.SetActive(_isCurrentSceneSplashScreen);

        }

        public void SwichCharacterButtonHanler()
        {
            _isCharacterHidden = !_isCharacterHidden;
            GameEventMessage.SendEvent(EventsLibrary.CharacterSwiched);
            _hiddenCharacterButtonSprite.SetActive(_isCharacterHidden);
        }

        public void ExitToMenu()
        {
            _popUpManager.ExitToMenu = true;
            _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.СonfirmSaveStory;
            GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
        }


        private void OnEnable()
        {
            
            ItemScale.Value = 0;
            ItemRotation.Value = 0;
            if ((_storyManager.IsStoryCreartionStart == true) & (_storyManager.IsStoryEdit == false))
            {
                CreateSceneBlank();
                CreateTopics();
                _storyManager.CurrentStory.Scenes = new List<StoryScene>(0);
                var _newScene = new StoryScene();
                _newScene.SceneNumberInStory = 0;
                //_storyManager.CurrentStory.Scenes.Add(_newScene);
                //AddScenesToDropDown();
            }

            if (_storyManager.IsStoryEdit == true)
            {
                //_storyManager.CurrentStory.Scenes = new List<StoryScene>();
                OpenStory();
            }
            _instCount = 0;
            _storyManager.IsStorySave = false;
            CreateTopics();
            _isPreview = true;
            _currentSceneNumber = 0;
            _storyManager.CurrentStorySceneIndex = 0;

            if (_storyManager.CurrentStory.StoryName != null)
            {
                _storyName.text = _storyManager.CurrentStory.StoryName;
            }
            ShowCurrentSceneNumber();
            _storyScenes[0].Show();

            if (_storyManager.IsStoryEdit == true)
            {
                _bg = _storyScenes[0].transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>();
                _character = _storyScenes[0].transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>();
                _itemsManager.GarmenScenePanel = _storyScenes[0].transform.GetChild(2).gameObject;
                _splashScreenPanel = _storyScenes[0].transform.GetChild(3).gameObject;
                _backFromPreviewButton = _storyScenes[0].transform.GetChild(4).gameObject;
//                Debug.Log("First scene !!!");
            }
            SwichScene();
            ConfigSwichButton();
            _storyManager.IsStoryEdit = false;
            _isPreview = false;
            AddScenesToDropDown();
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PreviewScene();
            }
        }

        public void SwichShadowVisible()
        {
            GameEventMessage.SendEvent(EventsLibrary.SwichItemShadowVisible);
        }

        public void SwichShadow()
        {
            GameEventMessage.SendEvent(EventsLibrary.SwichItemShadow);  
        }

        public void ScaleItem()
        {
            _itemsManager.UI_Parametr = 0;
            ItemScale.Value = _scaleSlider.value;
            be.Value = ItemScale.Value;
        }

        public void RotateItem()
        {

            _itemsManager.UI_Parametr = 0;
            ItemRotation.Value = _rotationSlider.value;

            ////if (_itemsManager.SelectedGarments.Count > 0)
            ////{
            ////    foreach (GameObject _item in currentItems)
            ////    {
            ////        _item.transform.eulerAngles = new Vector3(0, 0, _rotationSlider.value);
            ////        _itemShadow.transform.eulerAngles = new Vector3(0, 0, _rotationSlider.value);
            ////    }
            ////}
            ////else
            ////{
            ////    _character.transform.eulerAngles = new Vector3(0, 0, _rotationSlider.value);

            ////}
        }

        public void MirrowItem()
        {
            GameEventMessage.SendEvent(EventsLibrary.MirrorItem);
        }

        void CreateAnimationsSamples(GameObject _samplesContent, string _animSample, dynamic _factory, string _animFolder)
        {
            for (int i = _samplesContent.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(_samplesContent.transform.GetChild(0).gameObject);
            }

            DirectoryInfo _contentDirectory = new DirectoryInfo(Application.dataPath + _animFolder);
            FileInfo[] _files = new string[] { "*.asset" }.SelectMany(ext => _contentDirectory.GetFiles(ext, SearchOption.TopDirectoryOnly)).ToArray();

            int f = 0;
            foreach (FileInfo _file in _files)
            {
                var _itemSample = _factory.Create(_animSample).gameObject;
                _itemSample.name = _file.FullName;
                _itemSample.transform.SetParent(_samplesContent.transform);
                _itemSample.transform.localScale = new Vector3(1, 1, 1);


                WWW _www = new WWW("file://" + _file.FullName);


                var _skeletonData = _www.assetBundle.name;
                _samplesContent.transform.GetChild(0).gameObject.GetComponent<SkeletonGraphic>().SkeletonDataAsset.name = _skeletonData;

                f++;
            }

            float x = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x;
            float k = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale.x;
            float s = _samplesContent.GetComponent<HorizontalLayoutGroup>().spacing;
            float p = _samplesContent.GetComponent<HorizontalLayoutGroup>().padding.left;
            float y = _samplesContent.GetComponent<RectTransform>().sizeDelta.y;

            _samplesContent.GetComponent<RectTransform>().sizeDelta = new Vector2(((f * ((x * k) + s)) + (p * 2)) - s, y);
        }

        public void SetTargetPosition()
        {
            GameEventMessage.SendEvent(EventsLibrary.SetShadowOnItem);
        }

        void CreateTopics()
        {

            ClearContent(_garmentSamplesContent);
            ClearContent(_backgroundSamplesContent);
            ClearContent(_characterSamplesContent);
            ClearContent(_topicIconsContent);


            List<Topic> _topics = _spritesManager.Topics;
            List<GameObject> _topicIcons = new List<GameObject>();
            _spritesManager.CurrentTopic = _topics[0];


            foreach (Topic _t in _topics)
            {
                List<GameObject> _topicPartIcons = new List<GameObject>();
                CreateSmples(_garmentSamplesContent, _itemTemplateFactory, _t.Objects, PrefabsPathLibrary.GarmentSample, _topicPartIcons);
                CreateSmples(_backgroundSamplesContent, _bgTemplateFactory, _t.BackGrounds, PrefabsPathLibrary.BackGroundSample, _topicPartIcons);
                CreateSmples(_characterSamplesContent, _characterTemplateFactory, _t.Characters, PrefabsPathLibrary.CharacterSample, _topicPartIcons);

                GameObject _topicIcon = _topicIconFactory.Create(PrefabsPathLibrary.TopicIcon).gameObject;
                _topicIcon.GetComponent<TopicIcon>().topic = _t;
                _topicIcon.GetComponent<TopicIcon>().topicPartIcons = _topicPartIcons;
                _topicIcon.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = _t.Objects[0];
                _topicIcons.Add(_topicIcon);
                _topicIcon.transform.SetParent(_topicIconsContent.transform);
                _topicIcon.transform.localScale = new Vector3(1, 1, 1);
            }
            GameEventMessage.SendEvent(EventsLibrary.TopicSwiched);
        }

        void ClearContent(GameObject _content)
        {
            for (int i = _content.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(_content.transform.GetChild(0).gameObject);
            }
        }


        void CreateSmples(GameObject _samplesContent, dynamic _factory, List<Sprite> _spritesList, string _spriteSample, List<GameObject> _topicPartIconsList)
        {

            float f = 0;
            foreach (Sprite _sprite in _spritesList)
            {

                var _itemSample = _factory.Create(_spriteSample).gameObject;
                _itemSample.name = _sprite.name;
                _itemSample.transform.SetParent(_samplesContent.transform);
                _itemSample.transform.localScale = new Vector3(1, 1, 1);
                _itemSample.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = _sprite;
                _topicPartIconsList.Add(_itemSample);
                f++;
            }

            float x = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x;
            float k = _samplesContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale.x;
            float s = _samplesContent.GetComponent<HorizontalLayoutGroup>().spacing;
            float p = _samplesContent.GetComponent<HorizontalLayoutGroup>().padding.left;
            float y = _samplesContent.GetComponent<RectTransform>().sizeDelta.y;

            Vector2 _sampleContentSize = new Vector2((f * (x + s) + (p * 2) - s), y);
            _samplesContent.GetComponent<RectTransform>().sizeDelta += _sampleContentSize;

//            Debug.Log(_spriteSample + _sampleContentSize);
        }

        public void ResetItemRotation()
        {
            GameEventMessage.SendEvent(EventsLibrary.ResetRotation);
        }

        public void SlowRotaion(int k)
        {
            _itemsManager.UI_Parametr = k;
            GameEventMessage.SendEvent(EventsLibrary.RotateSelectedItem);
                ResetControl();
        }

        public void SlowScale(int k)
        {
            _itemsManager.UI_Parametr = k;
            GameEventMessage.SendEvent(EventsLibrary.ScaleSelectedItem);
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
            GameEventMessage.SendEvent(EventsLibrary.DeleteItem);

        }

        public void ChangeCharacter()
        {
            //var _currentCharacter = _itemsManager.Character.GetComponent<UnityEngine.UI.Image>().sprite;
            //ChangeSprite(_currentCharacter, _character);
            //_character.gameObject.name = _currentCharacter.name;
        }

        public void ChangeBG()
        {
            var _currentBG = _itemsManager.BackgroundSprite;
            ChangeSprite(_currentBG, _bg);
            _bg.gameObject.name = _currentBG.name;
        }

        private IEnumerator ResetControls()
        {
            yield return new WaitForEndOfFrame();
            var _count = _itemsManager.SelectedGarments.Count;

            switch (_count)
            {
                case 0:
                    _scaleSlider.value = 0;
                    _rotationSlider.value = 0;
                    break;
                case >= 2:
                    _scaleSlider.value = 0;
                    _rotationSlider.value = 0;
                    break;

                default:
                    _itemsManager.UI_Parametr = 0;
                    _itemsManager.UI_Parametr = 0;
                    Debug.Log(_itemsManager.SelectedGarments[0].name);
                    GameObject _item = _itemsManager.SelectedGarments[0].transform.GetChild(0).gameObject;
                    ItemScale.Value = _item.transform.localScale.x;
                    _scaleSlider.value = _item.transform.localScale.x;
                    var a = _item.transform.eulerAngles.z;
                    a = Mathf.Repeat(a + 180, 360) - 180;
                    ItemRotation.Value = a;
                    _rotationSlider.value = a;
                    break;
            }
        }

        public void ResetControl()
        {
            StartCoroutine(ResetControls());
        }

        public void ChangeSprite(Sprite _currentSprite, UnityEngine.UI.Image _image)
        {
            _image.sprite = _currentSprite;
        }

        public void SearchShadow()
        {
            GameEventMessage.SendEvent(EventsLibrary.SearchShadow);

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
            _addSwich.sprite = _buttonSprite;
        }

        void SwichScene()
        {
            if (_storyManager.IsStoryEdit == false)
            {
                //_character = _itemsManager.Character.GetComponent<UnityEngine.UI.Image>();
                _bg.sprite = _itemsManager.BackgroundSprite;
                _splashScreenPanel = _itemsManager.SplashScreenPanel;
            }

            _isCharacterHidden = _character.gameObject.activeSelf;
            _hiddenCharacterButtonSprite.SetActive(_isCharacterHidden);


            _isCurrentSceneSplashScreen = _itemsManager.SplashScreenPanel.activeSelf;
            _isSplashScreenButtonSprite.SetActive(_isCurrentSceneSplashScreen);
            if (_storyManager.IsStoryEdit == false)
            {
                _backFromPreviewButton = _itemsManager.PreviewButton;
            }
        }
        public void GoToNextScene()
        {
            int i = _storyManager.CurrentStorySceneIndex;
            GoToScene(i + 1);
        }

        public void GoToPrevioudScene()
        {
            int i = _storyManager.CurrentStorySceneIndex;
            GoToScene(i - 1);
        }

        public void GoToScene(int TargetSceneNumber)
        {
            _currentSceneNumber = _storyManager.CurrentStorySceneIndex;
            if (TargetSceneNumber > _currentSceneNumber)
            {
                if (_storyScenes.Count - 1 > _currentSceneNumber)
                {
                    _storyScenes[_currentSceneNumber].HideBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
                    _storyScenes[_currentSceneNumber].Hide();
                    _currentSceneNumber = TargetSceneNumber;
                    _storyScenes[_currentSceneNumber].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
                    _storyScenes[_currentSceneNumber].Show();
                }
                else
                {
                    CreateSceneBlank();
                    _storyScenes[_currentSceneNumber].HideBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
                    _storyScenes[_currentSceneNumber].Hide();
                    _currentSceneNumber = TargetSceneNumber;
                    _storyScenes[_currentSceneNumber].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
                    _storyScenes[_currentSceneNumber].Show();
                    _storyManager.CurrentStory.Scenes.Add(new StoryScene());
                }
            }
            else
            {
                _currentSceneNumber = _storyManager.CurrentStorySceneIndex;
                _storyScenes[_currentSceneNumber].HideBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
                _storyScenes[_currentSceneNumber].Hide();
                _currentSceneNumber = TargetSceneNumber;
                _storyScenes[_currentSceneNumber].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
                _storyScenes[_currentSceneNumber].Show();
                _itemsManager.GarmenScenePanel = _storyScenes[_currentSceneNumber].gameObject.transform.GetChild(2).gameObject;
                _storyManager.CurrentStorySceneIndex = _currentSceneNumber;
            }
            _storyManager.CurrentStorySceneIndex = _currentSceneNumber;
            ConfigSwichButton();
            _itemsManager.GarmenScenePanel = _storyScenes[_currentSceneNumber].gameObject.transform.GetChild(2).gameObject;
            SwichScene();
            ShowCurrentSceneNumber();
            AddScenesToDropDown();
            GameEventMessage.SendEvent(EventsLibrary.SceneSwiched);
        }

        //public void GoToPreviousScene()
        //{
        //    _currentSceneNumber = _storyManager.CurrentStorySceneIndex;
        //    _storyScenes[_currentSceneNumber].HideBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Right;
        //    _storyScenes[_currentSceneNumber].Hide();
        //    _currentSceneNumber--;
        //    _storyScenes[_currentSceneNumber].ShowBehavior.Animation.Move.Direction = Doozy.Engine.UI.Animation.Direction.Left;
        //    _storyScenes[_currentSceneNumber].Show();
        //    _itemsManager.GarmenScenePanel = _storyScenes[_currentSceneNumber].gameObject.transform.GetChild(2).gameObject;
        //    _storyManager.CurrentStorySceneIndex = _currentSceneNumber;
        //    SwichScene();
        //    ShowCurrentSceneNumber();
        //    ConfigSwichButton();
        //    AddScenesToDropDown(_currentSceneNumber);

        //}


        private void CreateSceneBlank()
        {
            _isCharacterHidden = false;
            var _newStoryScenePanel = _storyCreationPanelFactory.Create(PrefabsPathLibrary.StoryCreationPanel).gameObject.GetComponent<UIView>();
            _newStoryScenePanel.gameObject.transform.SetParent(_headerMiddlePanel.transform);
            _newStoryScenePanel.Hide();
            _header.transform.SetSiblingIndex(_storyScenes.Count + 2);
            _storyScenes.Add(_newStoryScenePanel);
            _newStoryScenePanel.gameObject.name = (_storyScenes.Count - 1).ToString();
            _middlePanel = _newStoryScenePanel.gameObject.transform.GetChild(2).gameObject;
            _backFromPreviewButton = _itemsManager.PreviewButton;
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
                    _storyScenes[_currentSceneNumber].ShowBehavior.Animation.Move.From.x = -(Screen.currentResolution.width / 2);

                    _storyScenes[_currentSceneNumber].Show();
                    _storyManager.CurrentStorySceneIndex = _currentSceneNumber;
                    ShowCurrentSceneNumber();
                }
            }

            int i = 0;
            foreach (UIView _scenePanel in _storyScenes)
            {
                _scenePanel.gameObject.name = i.ToString();
                i++;
            }
        }

        public void AddScenesToDropDown()
        {
            _sceneNavigationDropdown.options.Clear();

            int i = 0;
            foreach (UIView _scene in _storyScenes)
            {
                _sceneNavigationDropdown.options.Add(new TMP_Dropdown.OptionData() { text = "Лист №" + (i + 1).ToString() });
                i++;
            }

            _sceneNavigationDropdown.value = _storyManager.CurrentStorySceneIndex;
            Debug.Log("Item" + i + "added to DD." + " Current scene: " + _storyManager.CurrentStorySceneIndex);
        }

        public void GoToSceneFronDropDown()
        {
            GoToScene(_sceneNavigationDropdown.value);
        }

        public void CopyItem()
        {
            if (_itemsManager.SelectedGarments.Count > 0)
            {
                foreach (GameObject obj in _itemsManager.SelectedGarments)
                {
                    string _name = obj.name;
                    Sprite _itemSprite = obj.GetComponent<UnityEngine.UI.Image>().sprite;
                    Vector3 _garmentPosition = obj.transform.localPosition;
                    Vector3 _itemPosition = obj.transform.localPosition;
                    Vector3 _itemShadowPosition = _itemShadow.transform.localPosition;
                    Vector3 _itemScale = obj.transform.localScale;
                    Vector3 _itemRotation = obj.transform.localEulerAngles;
                    bool _isShadowEnable = _itemShadow.activeSelf;
                    UnityEngine.Color _itemShadowColor = _itemShadow.GetComponent<UnityEngine.UI.Image>().color;


                    var _garment = _garmentFactory.Create(PrefabsPathLibrary.Item).gameObject;
                    _garment.transform.SetParent(_itemsManager.GarmenScenePanel.transform);
                    Debug.Log(_name);
                    _garment.name = _name;
                    var _copyItem = _garment.transform.GetChild(1).gameObject;
                    var _copyItemShadow = _garment.transform.GetChild(0).gameObject;

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

                    _itemsManager.Garments.Add(obj);
                }
            }
        }

    }
}
