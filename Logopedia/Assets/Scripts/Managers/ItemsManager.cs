using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace Logopedia.UIConnection
{
    public class ItemsManager : IInitializable
    {
        public GameObject CurrentGarment, CurrentItem, CurrentItemShadow;
        public GameObject MiddleScenePanel;
        public List<GameObject> Garments = new List<GameObject>();

        public void Initialize()
        {

        }
    }
}
