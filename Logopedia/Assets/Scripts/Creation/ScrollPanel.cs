using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace Logopedia.UserInterface
{
    public class ScrollPanel : MonoBehaviour
    {
        [SerializeField]
        private Sprite _up, _down;
        [SerializeField]
        private Button _controlButton;
        [SerializeField]
        private UnityEngine.UI.Image _controlButtonImage;
        [SerializeField]
        private GameObject[] _scrolls;

        private void Start()
        {
            _controlButton.onClick.AddListener(MaximazePanel);
        }

        public void MaximazePanel()
        {
            _scrolls[1].SetActive(true);
            _scrolls[2].SetActive(true);
            _controlButtonImage.sprite = _up;
            _controlButton.onClick.RemoveAllListeners();
            _controlButton.onClick.AddListener(MinimazePanel);
        }

        public void MinimazePanel()
        {
            _scrolls[1].SetActive(false);
            _scrolls[2].SetActive(false);
            _controlButtonImage.sprite = _down;
            _controlButton.onClick.RemoveAllListeners();
            _controlButton.onClick.AddListener(MaximazePanel);
        }


    }
}
