using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Logopedia.GamePlay
{
    [Serializable]
    public class StoryScene
    {
        public int SceneNumberInStory;
        public Sprite Character, BG;
        public PositionForSave CharacterPosition, CharacterRotation, CharacterScale;

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
            public Sprite ItemSprite;
            public PositionForSave ItemPosition;
            public PositionForSave ItemShadowPosition;
            public PositionForSave ItemRotation;
            public PositionForSave ItemScale;
            public bool ShadowVisible, ShadowEnabled;
        }

        public List<SceneItem> Items;
    }
}
