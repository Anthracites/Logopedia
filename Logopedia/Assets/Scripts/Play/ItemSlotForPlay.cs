using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;
using UnityEngine.UI;
using Logopedia;
using Doozy.Engine;

public class ItemSlotForPlay : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject _garment, _item, _starsObj;
    [SerializeField]
    private SkeletonGraphic _stars;
    [SerializeField]
    private Image _itemImage;

    void Start()
    {
        _stars = _starsObj.GetComponent<SkeletonGraphic>();
    }

    public void OnDrop(PointerEventData eventData)
    {
       
        var other = eventData.pointerDrag.transform;
        if (other != transform)
        {
            if (other.gameObject == _item)
            {
                other.transform.position = transform.position;
                ShowAnimation();
                _itemImage.raycastTarget = false;
                GameEventMessage.SendEvent(EventsLibrary.ItemInSlot);
            }
        }
    }

    public void ShowAnimation()
    {
        _stars.enabled = true;
        _stars.AnimationState.SetAnimation(0, "correct_answer", false);
    }
}
