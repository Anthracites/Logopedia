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
        private GameObject[] _scrolls, _contents;

        private void Start()
        {
            _controlButton.onClick.AddListener(MaximazePanel);
        }

        public void ResizeContent()//Подписать на событие SwichTopic
        {
            foreach (GameObject _content in _contents)
            {
                float f = _content.GetComponentsInChildren<Transform>(false).GetLength(0);
                float x = _content.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.x;
                float k = _content.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale.x;
                float s = _content.GetComponent<HorizontalLayoutGroup>().spacing;
                float p = _content.GetComponent<HorizontalLayoutGroup>().padding.left;
                float y = _content.GetComponent<RectTransform>().sizeDelta.y;

                Vector2 _sampleContentSize = new Vector2(((f * ((x * k) + s)) + (p * 2)) - s, y);
                _content.GetComponent<RectTransform>().sizeDelta = _sampleContentSize;
//                Debug.Log("Child count  = " + f);
            }
        }

        public void MaximazePanel()
        {
            _scrolls[0].SetActive(true);
            _scrolls[1].SetActive(true);
            _scrolls[2].SetActive(true);
            _scrolls[3].SetActive(true);
            _controlButtonImage.sprite = _up;
            _controlButton.onClick.RemoveAllListeners();
            _controlButton.onClick.AddListener(MinimazePanel);
        }

        public void MinimazePanel()
        {
            _scrolls[0].SetActive(false);
            _scrolls[1].SetActive(false);
            _scrolls[2].SetActive(false);
            _scrolls[3].SetActive(false);
            _controlButtonImage.sprite = _down;
            _controlButton.onClick.RemoveAllListeners();
            _controlButton.onClick.AddListener(MaximazePanel);
        }

    }
}
