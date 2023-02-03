using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Logopedia.UIConnection;

public class SliderWithButton : MonoBehaviour
{
    [Inject]
    SettingsManager _settingsManager;

    [SerializeField]
    private Button _moreButton, _lessButton;
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private float _volume;

    public void MoreButtonHandler()
    {
        _volume += 0.1f;
        _slider.value = _volume;
    }

    public void LessButtonHandler()
    {
        _volume -= 0.1f;
        _slider.value = _volume;
    }

    public void SetSettings(float _setting)
    {
        _setting = _slider.value;
    }
}
