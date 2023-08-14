using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Logopedia.UIConnection
{
    public class ItemsManager : IInitializable
    {
        public List<GameObject> SelectedGarments = new List<GameObject>(); //выделенные предметы на сцене
        public GameObject Character, Background, PreviewButton, CharacterAnimation;
        public GameObject GarmenScenePanel, SplashScreenPanel;
        public List<GameObject> Garments = new List<GameObject>(); // все предметы на сцене
        public int ItemCount;
        public float UI_Parametr;

        public void Initialize()
        {

        }
    }
}
