using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logopedia.GamePlay
{
    [Serializable]
    public class Story
    {
        public string StoryName;
        public List<StoryScene> Scenes = new List<StoryScene>();
    }
}
