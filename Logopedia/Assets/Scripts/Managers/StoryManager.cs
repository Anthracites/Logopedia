using UnityEngine;
using Zenject;
using System;
using System.Collections;
using System.Collections.Generic;
using Logopedia.GamePlay;
using Spine;

namespace Logopedia.UIConnection
{
    [Serializable]
    public class StoryManager : IInitializable
    {
        public Story CurrentStory;
        public int CurrentStorySceneIndex;
        public List<StoryScene> CurrentStoryScenes;
        public GameObject Chacter, BackGround;


        public void Initialize()
        {

        }
    }
}
