using UnityEngine;
using Zenject;
using System;
using System.Collections;
using System.Collections.Generic;
using Logopedia.GamePlay;
using Spine;
using Doozy.Engine.Soundy;

namespace Logopedia.UIConnection
{

    public class SettingsManager : IInitializable
    {
        public List<AudioClip> Sounds;
        public AudioClip CurrentClip;
        public float _generalVolume, _correctAnswerSoundVolume, _BGSoundVolume, _goNextSceneSoundVolume, _takeItemSoundVolume;
        public string CorrectAnswer, BGMusic, GoNextScene, TakeItem;
        public string DataBaseName = "Logopedia";

        public void Initialize()
        {

        }
    }
}
