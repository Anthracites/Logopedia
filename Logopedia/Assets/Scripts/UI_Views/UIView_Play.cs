using System.Collections;
using System.Collections.Generic;
using Logopedia;
using UnityEngine;
using Doozy.Engine;

namespace Logopedia.UserInterface
{
    public class UIView_Play : MonoBehaviour
    {

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEventMessage.SendEvent(EventsLibrary.GoToMenu);
            }
        }
    }
}
