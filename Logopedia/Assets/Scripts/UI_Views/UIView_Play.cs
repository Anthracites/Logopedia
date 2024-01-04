using System.Collections;
using System.Collections.Generic;
using Logopedia;
using UnityEngine;
using Doozy.Engine;
using Zenject;
using Logopedia.UIConnection;
using Logopedia.GamePlay;
using UnityEngine.UI;
using Unity.VisualScripting;
using Doozy.Engine.UI;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using Spine.Unity;



namespace Logopedia.UserInterface
{
    public class UIView_Play : MonoBehaviour
    {
        [Inject]
        StoryManager _storyManager;
        [Inject]
        PopUpManager _popUpManager;
        [Inject]
        StoryPlayPanel.Factory _storyPlayPanel;
        [Inject]
        GarmentForPlay.Factory _garmentForPlayFactory;
        [Inject]
        ItemsManager _itemsManager;

        [SerializeField]
        private List<GameObject> _scenePanels;
        private Color _transparent = new Color(0, 0, 0, 0), _opaque = new Color(0, 0, 0, 0.5f);
        private int _currentScene;


        private void OnDisable()
        {
            foreach (GameObject panel in _scenePanels)
            {
                Destroy(panel);
            }
            _scenePanels.Clear();

        }

        private void OnEnable()
        {
            if (_storyManager.CurrentStory != null)
            {
                OpenStory();
            }
        }

        void OpenStory()
        { 
            _currentScene = 0;
            _scenePanels = new List<GameObject>();

                var currentStory = _storyManager.CurrentStory;

            foreach(StoryScene _scene in currentStory.Scenes)
            {
                var _scenePanel = _storyPlayPanel.Create(PrefabsPathLibrary.PlayPanel).gameObject;
                _scenePanel.transform.SetParent(transform);
                _scenePanel.transform.position = Vector3.zero;
                _scenePanel.GetComponent<StoryPlayPanel>().ItemCount = _scene.ActiveItemCount;
                _scenePanel.GetComponent<StoryPlayPanel>().ItemCount = _scene.ActiveItemCount;
                _scenePanel.GetComponent<StoryPlayPanel>().IsSplashScreen = _scene.IsSceneSplashScreen;
                _scenePanel.GetComponent<StoryPlayPanel>().IsAnimated = _scene.SceneCharacter.IsAnimated;



                _scenePanel.name = _scene.SceneNumberInStory.ToString();
                _scenePanels.Add(_scenePanel);


                var _bg = _scenePanel.transform.GetChild(0).gameObject;

                WWW _BGwww = new WWW("file://" + _scene.CurrentBGForSave);

                Rect _BGrect = new Rect(0, 0, _BGwww.texture.width, _BGwww.texture.height);
                Sprite _bgSprite = Sprite.Create(_BGwww.texture, _BGrect, new Vector2(0.5f, 0.5f));

                _bg.GetComponent<Image>().sprite = _bgSprite;

                if (_scene.SceneCharacter.IsChacterActive == true)
                {
                    var _character = _scenePanel.transform.GetChild(1).gameObject;
                    _character.SetActive(true);
                    string _characterPath = _scene.SceneCharacter.CharacterSprite;

                    WWW _Characterwww = new WWW("file://" + _characterPath);
                    Rect _Characterrect = new Rect(0, 0, _Characterwww.texture.width, _Characterwww.texture.height);
                    Sprite _characterSprite = Sprite.Create(_Characterwww.texture, _Characterrect, new Vector2(0.5f, 0.5f));

                    _character.GetComponent<Image>().sprite = _characterSprite;
                    _character.transform.localPosition = new Vector3(_scene.SceneCharacter.CharacterPosition.x, _scene.SceneCharacter.CharacterPosition.y, _scene.SceneCharacter.CharacterPosition.z);
                    _character.transform.localEulerAngles = new Vector3(_scene.SceneCharacter.CharacterRotation.x, _scene.SceneCharacter.CharacterRotation.y, _scene.SceneCharacter.CharacterRotation.z);
                    _character.transform.localScale = new Vector3(_scene.SceneCharacter.CharacterScale.x, _scene.SceneCharacter.CharacterScale.y, _scene.SceneCharacter.CharacterScale.z);

                    SkeletonDataAsset _characterAnim = Resources.Load<SkeletonDataAsset>(_scene.SceneCharacter.AnimationAsset);
                    var _animationCharacter = _character.transform.GetChild(0).gameObject.GetComponent<SkeletonGraphic>();

                    _animationCharacter.skeletonDataAsset = _characterAnim;
                    _animationCharacter.initialSkinName = _scene.SceneCharacter.AnimationSkin;
                    _animationCharacter.gameObject.name = _scene.SceneCharacter.AnimationAsset;
                    //_animationCharacter.AnimationState.SetAnimation(0, "action", false);
                    _animationCharacter.Initialize(true);
                    _animationCharacter.gameObject.SetActive(false);

                }
                else
                {
                    _scenePanel.transform.GetChild(1).gameObject.SetActive(false);
                }

                var _garmentPanel = _scenePanel.transform.GetChild(2);
                foreach (StoryScene.SceneItem _sceneItem in _scene.Items)
                {
                    var _garment = _garmentForPlayFactory.Create(PrefabsPathLibrary.ItemForPlay).gameObject;
                    _garment.transform.SetParent(_garmentPanel);
                    _garment.transform.localScale = Vector3.one;
                    _garment.transform.localPosition = new Vector3(_sceneItem.GarmentPosition.x, _sceneItem.GarmentPosition.y, _sceneItem.GarmentPosition.z);
                    var _item = _garment.transform.GetChild(1).gameObject;
                    var _itemShadow = _garment.transform.GetChild(0).gameObject;


                    WWW _Itemwww = new WWW("file://" + _sceneItem.ItemSprite);
                    Rect _Itemrect = new Rect(0, 0, _Itemwww.texture.width, _Itemwww.texture.height);
                    Sprite _itemSprite = Sprite.Create(_Itemwww.texture, _Itemrect, new Vector2(0.5f, 0.5f));


                    _item.GetComponent<Image>().sprite = _itemSprite;
                    _itemShadow.GetComponent<Image>().sprite = _itemSprite;

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

                    bool _isActive = _sceneItem.ShadowEnabled;
                            _itemShadow.SetActive(_isActive);


                    bool _isVisible = _sceneItem.ShadowVisible;

                    var _currentColor = _opaque;

                        if (_isVisible)
                        {
                            _currentColor = _opaque;
                        }
                        else
                        {
                            _currentColor = _transparent;
                        }
                        _itemShadow.GetComponent<Image>().color = _currentColor;

                }
            }

            foreach(GameObject _panel in _scenePanels)
            {
                _panel.GetComponent<UIView>().Hide();
            }
            _itemsManager.ItemCount = _scenePanels[0].GetComponent<StoryPlayPanel>().ItemCount;
            _scenePanels[0].GetComponent<UIView>().Show();
        }

        public void GoToNextScene()
        {
            StartCoroutine(GoToNextSceneCoroutine());

        }

        IEnumerator GoToNextSceneCoroutine()
        {
            yield return new WaitForSeconds(2);

            _scenePanels[_currentScene].GetComponent<UIView>().Hide();
            Debug.Log("Level" + _currentScene.ToString() + "complite");

            _currentScene += 1;
            if (_currentScene < _scenePanels.Count)
            {
                _scenePanels[_currentScene].GetComponent<UIView>().Show();
            }
            else if (_currentScene == _scenePanels.Count) //Добавить финальную заставку
            {
                GameEventMessage.SendEvent(EventsLibrary.GoToMenu);
            }
        }


            private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _popUpManager.CurrentPopUpConfig = PopUpConfigLibrary.ConfirmExitToMenuFromGame;
                GameEventMessage.SendEvent(EventsLibrary.ShowPopUp);
            }
        }
    }
}
