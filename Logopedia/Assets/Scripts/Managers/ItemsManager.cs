using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Logopedia.UIConnection
{
    public class ItemsManager : IInitializable
    {
        public List<GameObject> CurrentGarment = new List<GameObject>();
        public GameObject Character, Background, CurrentItem, CurrentItemShadow, PreviewButton, CharacterAnimation;
        public GameObject GarmenScenePanel, SplashScreenPanel;
        public List<GameObject> Garments = new List<GameObject>();
        public int ItemCount;

        public void Initialize()
        {

        }
    }
}
