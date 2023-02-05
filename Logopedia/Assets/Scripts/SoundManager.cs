using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Logopedia.GamePlay;
using UnityEngine.Networking;
using Zenject;
using Doozy.Engine;

namespace Logopedia.UIConnection
{
    public class SoundManager : MonoBehaviour
    {
        [Inject]
        SettingsManager _settingsManager;

        private void Awake()
        {
            GetAudiopClips();
        }

        public void GetAudiopClips()
        {
            StartCoroutine(GetAudioClip());
            GameEventMessage.SendEvent(EventsLibrary.ClipsAddedToGame);
        }

        IEnumerator GetAudioClip()
        {
            DirectoryInfo _contentDirectory = new DirectoryInfo(Application.dataPath + SpritesPathLibrary.Sounds);
            FileInfo[] _files = new string[] { "*.mp3" }.SelectMany(ext => _contentDirectory.GetFiles(ext, SearchOption.TopDirectoryOnly)).ToArray();

            var _soundfiles = new List<AudioClip>();
            foreach (FileInfo _file in _files)
            {

                WWW _www = new WWW("file://" + _file.FullName);
                var _myClip = _www.GetAudioClip();
                        _myClip.name = _file.Name;
                        _soundfiles.Add(_myClip);
            }
            yield return null;

                    //var _soundfiles = new List<AudioClip>();
                    //foreach (FileInfo _file in _files)
                    //{
                    //    using (UnityWebRequest _www = UnityWebRequestMultimedia.GetAudioClip("file://" + _file.FullName, AudioType.MPEG))
                    //    {
                    //        yield return _www.SendWebRequest();

                    //        if (_www.result == UnityWebRequest.Result.ConnectionError)
                    //        {
                    //            UnityEngine.Debug.Log(_www.error);
                    //        }
                    //        else
                    //        {
                    //            AudioClip _myClip = DownloadHandlerAudioClip.GetContent(_www);
                    //            _myClip.name = _file.Name;
                    //            _soundfiles.Add(_myClip);
                    //        }
                    //    }
                    //}
                    _settingsManager.Sounds = _soundfiles;
        }
    }
}
