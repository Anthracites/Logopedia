using Logopedia.UIConnection;
using UnityEngine;
using Zenject;
using Doozy.Engine;
using Logopedia;
using System.Collections.Generic;
using System;


public class StoryPlayPanel : MonoBehaviour
{
    [Inject]
    ItemsManager _itemsManager;

    public int ItemCount;
    private int _putItemCount, _sceneNumber;
    public bool IsSplashScreen;

    private void Start()
    {
        _sceneNumber = Int32.Parse(gameObject.name);

        if ((IsSplashScreen == true) & (_sceneNumber == 0))
            {
                GameEventMessage.SendEvent(EventsLibrary.LevelComplete);
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

    public class Factory : PlaceholderFactory<string, StoryPlayPanel>
    {

    }
}
