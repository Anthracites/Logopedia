using System.Collections;
using System.Collections.Generic;
using Logopedia.GamePlay;
using UnityEngine;
using Zenject;
using Doozy.Engine;
using TMPro;
using System.Drawing;

namespace Logopedia.UIConnection
{

    public class TopicIcon : MonoBehaviour
    {
        [Inject]
        SpritesManager spritesManager;

        [SerializeField]
        private TMP_Text topicNameLabel;
        [SerializeField]
        private GameObject clickBlock;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private bool a = true;

        public Topic topic;
        public List<GameObject> topicPartIcons = new List<GameObject>();

        private void Start()
        {
            topicNameLabel.text = topic.TopicName;
            SwichTopic();
        }

        public void IconClickHandle()
        {
            SendTopicToManager();
            SwichTopic();
        }

        public void SwichTopic() 
        {
            bool _b = (spritesManager.CurrentTopic == topic);

            foreach (GameObject _p in topicPartIcons)
            {
                _p.SetActive(_b);
            }
            if (_b == true) { }
            canvasGroup.blocksRaycasts = !_b;
            clickBlock.SetActive(_b);
            GameEventMessage.SendEvent(EventsLibrary.TopicSwiched);
        }

        void SendTopicToManager()
        {
            spritesManager.CurrentTopic = topic;
            GameEventMessage.SendEvent(EventsLibrary.TopicSwich);
        }

        public class Factory : PlaceholderFactory<string, TopicIcon>
        {

        }
    }
}
