using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Logopedia.GamePlay;

namespace Logopedia.GamePlay
{
    [Serializable]
    public class StoryScene : MonoBehaviour
    {
        public int SceneNumberInStory;
        public Sprite Character, BG;

        public struct Item
        {
            public Sprite ItemSprite;
            public Vector3 ItemPosition;
            public Vector3 ItemShadowPosition;
            public Vector3 ItemRotation;
            public Vector3 ItemScale;
        }
        public Item[] Items;

    }
}
