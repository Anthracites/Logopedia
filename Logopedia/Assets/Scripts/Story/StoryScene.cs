using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEditor;

namespace Logopedia.GamePlay
{
    [Serializable]
    public class StoryScene
    {
        public bool IsSceneSplashScreen;
        public int SceneNumberInStory, ActiveItemCount;
        public CharacterForSave SceneCharacter;
        public string CurrentBGForSave;

        public string BGForSave(GameObject _bg)
        {
            var s = _bg.name;
            return s;
        }

        public struct CharacterForSave
        {
            public bool IsChacterActive;
            public string CharacterSprite;
            public PositionForSave CharacterPosition;
            public PositionForSave CharacterRotation;
            public PositionForSave CharacterScale;

            public CharacterForSave(GameObject _character)
            {
                IsChacterActive = _character.gameObject.activeSelf;
                CharacterSprite = _character.name;
                CharacterPosition = new PositionForSave(_character.gameObject.transform.localPosition);
                CharacterRotation = new PositionForSave(_character.gameObject.transform.eulerAngles);
                CharacterScale = new PositionForSave(_character.transform.localScale);
            }
    }

        public struct PositionForSave
        {
            public float x, y, z;

            public PositionForSave(Vector3 _vector3)
        {
            x = _vector3.x;
            y = _vector3.y;
            z = _vector3.z;
        }
    }


    public struct SceneItem
        {
            public string ItemSprite;
            public PositionForSave GarmentPosition;
            public PositionForSave ItemPosition;
            public PositionForSave ItemShadowPosition;
            public PositionForSave ItemRotation;
            public PositionForSave ItemScale;
            public bool ShadowVisible, ShadowEnabled;

            public SceneItem(GameObject _garment)
            {
                GameObject _item, _itemShadow;
                _item = _garment.transform.GetChild(1).gameObject;
                _itemShadow = _garment.transform.GetChild(0).gameObject;

                GarmentPosition = new PositionForSave(_garment.gameObject.transform.localPosition);
                ItemSprite = _garment.name;
                ItemPosition = new PositionForSave(_item.gameObject.transform.localPosition);
                ItemShadowPosition = new PositionForSave(_itemShadow.gameObject.transform.localPosition);
                ItemRotation = new PositionForSave(_item.transform.eulerAngles);
                ItemScale = new PositionForSave(_item.transform.localScale);
                ShadowEnabled = _itemShadow.activeSelf;

                Color _transparent = new Color(255, 255, 255, 0);
                Color _currentShadowColor = _itemShadow.GetComponent<Image>().color;

                if (_currentShadowColor == _transparent)
                {
                    ShadowVisible = false;
                }
                else
                {
                    ShadowVisible = true;

                }
            }

        }


        public List<SceneItem> Items = new List<SceneItem>();
    }
}
