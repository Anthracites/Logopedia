using System.Collections;
using System.Collections.Generic;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;


public class ItemForPlay : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [Inject]
    ItemsManager _itemsManager;

    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private GameObject _garment;

    public void OnDrop(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        int _sibIndex = _itemsManager.ItemCount + 2;
        _garment.transform.SetSiblingIndex(_sibIndex);
        Debug.Log("Sibling index: " + _sibIndex.ToString() + ", Items count: " + _itemsManager.ItemCount);
    }
}
