using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Logopedia.UIConnection
{
    public class SpritesManager : IInitializable
    {
        public List<Sprite> Objects = new List<Sprite>();
        public List<Sprite> BackGrounds = new List<Sprite>();
        public List<Sprite> Characters = new List<Sprite>();


        public void Initialize()
        {

        }
    }
}
