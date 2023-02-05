using System.Collections;
using System.Collections.Generic;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Doozy.Engine.Soundy;


public class ItemForPlay : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [Inject]
    ItemsManager _itemsManager;
    [Inject]
    SettingsManager _settingsManager;

    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private GameObject _garment;
    [SerializeField]
    private SoundyData _takeItem;
    [SerializeField]
    private float _startX, _startY;

    void Start()
    {
        GetTakeSound();
    }

    public void OnDrop(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x - _startX, mousePos.y - _startY, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _startX = mousePos.x - transform.position.x;
        _startY = mousePos.y - transform.position.y;
        _canvasGroup.blocksRaycasts = false;
        int _sibIndex = _itemsManager.ItemCount + 2;
        _garment.transform.SetSiblingIndex(_sibIndex);
        SoundyManager.Play(_takeItem);
        Debug.Log("Sibling index: " + _sibIndex.ToString() + ", Items count: " + _itemsManager.ItemCount);
    }

    public void GetTakeSound()
    {
        _takeItem.DatabaseName = _settingsManager.DataBaseName;
        _takeItem.SoundName = _settingsManager.TakeItem;
    }
}
