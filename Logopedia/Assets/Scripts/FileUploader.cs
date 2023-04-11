using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Logopedia.UIConnection;
using System.Linq;
using System.IO;

namespace Logopedia.GamePlay
{
    public class FileUploader : MonoBehaviour
    {
        [Inject] SpritesManager spritesManager;

        void Awake()
        {
            GetFiles();
        }

        public void GetFiles()
        {
            CreateSpritesSamples(SpritesPathLibrary.GarmentSprites, PrefabsPathLibrary.GarmentSample, spritesManager.Objects);
            CreateSpritesSamples(SpritesPathLibrary.BGSprites, PrefabsPathLibrary.BackGroundSample, spritesManager.BackGrounds);
            CreateSpritesSamples(SpritesPathLibrary.CharacterSprites, PrefabsPathLibrary.CharacterSample, spritesManager.Characters);
        }


        public void CreateSpritesSamples(string _spriteFolder, string _spriteSample, List<Sprite> _spriteList)
        {
            DirectoryInfo _contentDirectory = new DirectoryInfo(Application.dataPath + _spriteFolder);
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
    }
}

