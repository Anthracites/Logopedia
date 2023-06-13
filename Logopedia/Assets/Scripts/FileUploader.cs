using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Logopedia.UIConnection;
using System.Linq;
using System.IO;
using System.Drawing;

namespace Logopedia.GamePlay
{
    public class FileUploader : MonoBehaviour
    {
        [Inject] SpritesManager spritesManager;


        void Awake()
        {
            UploadTopics();
        }

        public void UploadSprites(string _topicPartName, List<Sprite> _spriteList)
        {
            DirectoryInfo _contentDirectory = new DirectoryInfo(Application.dataPath + "/" +_topicPartName);
            FileInfo[] _files = new string[] { "*.jpg", "*jpeg", "*.png" }.SelectMany(ext => _contentDirectory.GetFiles(ext, SearchOption.TopDirectoryOnly)).ToArray();

            int f = 0;
            foreach (FileInfo _file in _files)
            {
                WWW _www = new WWW("file://" + _file.FullName);
                Rect _rect = new Rect(0, 0, _www.texture.width, _www.texture.height);

                Sprite _sprite = Sprite.Create(_www.texture, _rect, new Vector2(0.5f, 0.5f));
                _sprite.name = _file.FullName;
                _spriteList.Add(_sprite);
                f++;
            }
        }


        public void UploadTopics()
        {
            string _contentPath = "/Resources/Sprites/GamePlaySprites/";
            DirectoryInfo _contentDirectory = new DirectoryInfo(Application.dataPath + _contentPath);

            DirectoryInfo[] _topicDirectories = new string[] {"*.*"}.SelectMany(ext => _contentDirectory.GetDirectories(ext, SearchOption.TopDirectoryOnly)).ToArray(); //Получение списка папок по темам

            foreach (DirectoryInfo _topicDirectory in _topicDirectories)
            {
                Topic _topic = new Topic();

                _topic.TopicName = _topicDirectory.Name;
                UploadSprites(_contentPath + _topicDirectory.Name + "/BackGrounds", _topic.BackGrounds);
                UploadSprites(_contentPath + _topicDirectory.Name + "/Characters", _topic.Characters);
                UploadSprites(_contentPath + _topicDirectory.Name + "/Objects", _topic.Objects);

                spritesManager.Topics.Add(_topic);
            }
        }
        }
    }