using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Logopedia.UIConnection
{
    public class ItemsManager : IInitializable
    {
        public List<GameObject> SelectedGarments = new List<GameObject>(); //выделенные предметы на сцене
        public Sprite CharacterSprite, BackgroundSprite;
        public GameObject GarmenScenePanel, SplashScreenPanel, PreviewButton, CharacterAnimation;
        public List<GameObject> Garments = new List<GameObject>(); // все предметы на сцене
        public int ItemCount;

        public void Initialize()
        {

        }
    }
}
