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


namespace Logopedia.UserInterface
{
    public class UIView_Play : MonoBehaviour
    {
        [Inject]
        StoryManager _storyManager;

        [Inject]
        StoryPlayPanel.Factory _storyPlayPanel;
        [Inject]
        GarmentForPlay.Factory _garmentForPlayFactory;

        [SerializeField]
        private List<GameObject> _scenePanels;
        private Color _transparent = new Color(255, 255, 255, 0), _opaque = new Color(0, 0, 0, 0.5f);
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
                _scenePanel.name = _scene.SceneNumberInStory.ToString();
                _scenePanels.Add(_scenePanel);


                var _bg = _scenePanel.transform.GetChild(0).gameObject;


                //string _bgPath = _scene.CurrentBGForSave;

                //Sprite _bgSprite = Resources.Load<Sprite>(_bgPath);

                // public string url = "https://unity3d.com/files/images/ogimg.jpg";

                //DirectoryInfo _contentDirectory = new DirectoryInfo(Application.dataPath + "/Resources/");

                WWW _BGwww = new WWW("file://" + _scene.CurrentBGForSave);
                Debug.Log("BGWWW: " + _BGwww);

                Rect _BGrect = new Rect(0, 0, _BGwww.texture.width, _BGwww.texture.height);
                Sprite _bgSprite = Sprite.Create(_BGwww.texture, _BGrect, new Vector2(0.5f, 0.5f));

                _bg.GetComponent<Image>().sprite = _bgSprite;

                var _character = _scenePanel.transform.GetChild(1).gameObject;
                string _characterPath = _scene.SceneCharacter.CharacterSprite;

                WWW _Characterwww = new WWW("file://" + _characterPath);
                Rect _Characterrect = new Rect(0, 0, _Characterwww.texture.width, _Characterwww.texture.height);
                Sprite _characterSprite = Sprite.Create(_Characterwww.texture, _Characterrect, new Vector2(0.5f, 0.5f));

                _character.GetComponent<Image>().sprite = _characterSprite;
                _character.transform.localPosition = new Vector3(_scene.SceneCharacter.CharacterPosition.x, _scene.SceneCharacter.CharacterPosition.y, _scene.SceneCharacter.CharacterPosition.z);
                _character.transform.localEulerAngles = new Vector3(_scene.SceneCharacter.CharacterRotation.x, _scene.SceneCharacter.CharacterRotation.y, _scene.SceneCharacter.CharacterRotation.z);
                _character.transform.localScale = new Vector3(_scene.SceneCharacter.CharacterScale.x, _scene.SceneCharacter.CharacterScale.y, _scene.SceneCharacter.CharacterScale.z);

                foreach (StoryScene.SceneItem _sceneItem in _scene.Items)
                {
                    var _garment = _garmentForPlayFactory.Create(PrefabsPathLibrary.ItemForPlay).gameObject;
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
                    _garment.transform.SetParent(_scenePanel.transform);

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
                GameEventMessage.SendEvent(EventsLibrary.GoToMenu);
            }
        }
    }
}
