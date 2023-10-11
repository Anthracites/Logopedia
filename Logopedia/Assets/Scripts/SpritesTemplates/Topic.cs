using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Logopedia.GamePlay
{
    public class Topic
    {
        public string TopicName;
        public List<Sprite> Objects = new List<Sprite>();
        public List<Sprite> BackGrounds = new List<Sprite>();
        public List<Sprite> Characters = new List<Sprite>();
        public List<string> CharacterAnimation = new List<string>();
    }
}
