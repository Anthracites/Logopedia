using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Logopedia.GamePlay;

namespace Logopedia.UIConnection
{
    public class SpritesManager : IInitializable
    {
        public List<Topic> Topics = new List<Topic>();
        public Topic CurrentTopic;

        public void Initialize()
        {

        }
    }
}
