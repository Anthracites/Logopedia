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
        public bool IsStoryCreartionStart = false;
        public bool IsStoryEdit;
        public bool IsNewCtory;
        public Story CurrentStory;
        public int CurrentStorySceneIndex;
        public bool IsStorySave;

        public void Initialize()
        {

        }
    }
}
