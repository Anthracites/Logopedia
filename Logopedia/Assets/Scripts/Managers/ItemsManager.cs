using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Logopedia.UIConnection
{
    public class ItemsManager : IInitializable
    {
        public GameObject CurrentGarment, Character, Background, CurrentItem, CurrentItemShadow, PreviewButton;
        public GameObject GarmenScenePanel, SplashScreenPanel;
        public List<GameObject> Garments = new List<GameObject>();
        public int ItemCount;

        public void Initialize()
        {

        }
    }
}
