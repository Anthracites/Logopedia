using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;
using UnityEngine.UI;
using Logopedia;
using Doozy.Engine;
using Doozy.Engine.Soundy;
using Zenject;
using Logopedia.UIConnection;



public class ItemSlotForPlay : MonoBehaviour, IDropHandler
{
    [Inject]
    SettingsManager _settingsManager;

    [SerializeField]
    private GameObject _garment, _item, _starsObj;
    [SerializeField]
    private SkeletonGraphic _stars;
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private SoundyData _putItem;

    void Start()
    {
        _stars = _starsObj.GetComponent<SkeletonGraphic>();
        GetSound();
    }

    public void OnDrop(PointerEventData eventData)
    {
       
        var other = eventData.pointerDrag.transform;
        if (other != transform)
        {
            if (other.gameObject == _item)
            {
                other.transform.position = transform.position;
                SoundyManager.Play(_putItem);
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

    public void GetSound()
    {
        _putItem.DatabaseName = _settingsManager.DataBaseName;
        _putItem.SoundName = _settingsManager.CorrectAnswer;
    }
}
