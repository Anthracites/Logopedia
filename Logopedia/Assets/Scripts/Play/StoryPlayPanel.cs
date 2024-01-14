using Logopedia.UIConnection;
using UnityEngine;
using Zenject;
using Doozy.Engine;
using Logopedia;
using System.Collections.Generic;
using System;
using Spine.Unity;
using System.Drawing;

public class StoryPlayPanel : MonoBehaviour
{
    [Inject]
    ItemsManager _itemsManager;

    public int ItemCount;
    private int _putItemCount, _sceneNumber;
    public bool IsSplashScreen, IsAnimated;
    [SerializeField]
    private UnityEngine.UI.Image _characterSprite;
    [SerializeField]
    private GameObject _garmentPanel;
    [SerializeField]
    private SkeletonGraphic _animation;


    private void Start()
    {
        _sceneNumber = Int32.Parse(gameObject.name);

        if ((IsSplashScreen == true) & (_sceneNumber == 0))
            {
                GameEventMessage.SendEvent(EventsLibrary.LevelComplete);
            }

        foreach (Transform child in _garmentPanel.transform)
        {
            child.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    private void OnEnable()
    {
       if (IsSplashScreen == true)
        {
            GameEventMessage.SendEvent(EventsLibrary.LevelComplete);
        }
        _itemsManager.ItemCount = ItemCount;
    }

    public void SwichItemCount()
    {
        if (_putItemCount < ItemCount)
        {
            _putItemCount++;
            Debug.Log("Item count: " + ItemCount + ", Put item count: " + _putItemCount);
            if (_putItemCount == ItemCount)
            {
                GameEventMessage.SendEvent(EventsLibrary.LevelComplete);
                Debug.Log("Level complite");
            }
        }

    }

    public void PlayAnimation()
    {
        if (IsAnimated == true)
        {
            _characterSprite.color = new UnityEngine.Color(0, 0, 0, 0);
            _garmentPanel.SetActive(false);
            _animation.gameObject.SetActive(true);
            _animation.AnimationState.SetAnimation(0, "action", false);
        }
    }


    public class Factory : PlaceholderFactory<string, StoryPlayPanel>
    {

    }
}
