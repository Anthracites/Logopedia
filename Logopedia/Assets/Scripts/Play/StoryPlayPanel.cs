using Logopedia.UIConnection;
using UnityEngine;
using Zenject;
using Doozy.Engine;
using Logopedia;
using System.Collections.Generic;


public class StoryPlayPanel : MonoBehaviour
{
    [Inject]
    ItemsManager _itemsManager;

    public int ItemCount;
    private int _putItemCount;

    private void Start()
    {
        _itemsManager.ItemCount = ItemCount;
        Debug.Log("Items count in panel: " + _itemsManager.Garments.Count);
    }
    public void SwichItemCount() // Подписать на событие "ItemInSlot"
    {
        if (_putItemCount < ItemCount)
        {
            _putItemCount++;
            Debug.Log("Item count: " + ItemCount + ", Put item count: " + _putItemCount);
            if (_putItemCount == ItemCount)
            {
                GameEventMessage.SendEvent(EventsLibrary.LevelComplete);
                Debug.Log("Level complete");

            }
        }

    }

    public class Factory : PlaceholderFactory<string, StoryPlayPanel>
    {

    }
}
