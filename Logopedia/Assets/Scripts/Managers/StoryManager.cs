using UnityEngine;
using Zenject;
using System;
using System.Collections;
using System.Collections.Generic;
using Logopedia.GamePlay;

namespace Logopedia.UIConnection
{
    [Serializable]
    public class StoryManager : IInitializable
    {
        public Story CurrentStory;
        public string StoryName;
        public StoryScene CurrentStoryScene;
        public List<StoryScene> CurrentStoryScenes;
        public Sprite Chacter, BackGround;
        public List<Sprite> GarmentsSprites;

        public void Initialize()
        {

        }
    }
}
