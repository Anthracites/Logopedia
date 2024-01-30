using System.Collections;
using System.Collections.Generic;
using Logopedia.UIConnection;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Spine;
using Spine.Unity;
using Doozy.Engine;
using DG.Tweening;
using System.IO;
using System.Linq;



namespace Logopedia.UserInterface
{
    public class AnimationTemplate : MonoBehaviour, IPointerClickHandler
    {
        [Inject]
        ItemsManager _itemsManager;

        [SerializeField]
        private SkeletonGraphic _animation;

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            _itemsManager.CharacterAnimation.GetComponent<SkeletonGraphic>().skeletonDataAsset = _animation.skeletonDataAsset;
            _itemsManager.CharacterAnimation.GetComponent<SkeletonGraphic>().initialSkinName = _animation.initialSkinName;
            _itemsManager.CharacterAnimation.GetComponent<SkeletonGraphic>().startingAnimation = _animation.startingAnimation;
            _itemsManager.CharacterAnimation.name = gameObject.name;
            _itemsManager.CharacterAnimation.GetComponent<SkeletonGraphic>().Initialize(true);

            GameEventMessage.SendEvent(EventsLibrary.AnimationChanged);
            //Debug.Log(gameObject.name);
        }

        public class Factory : PlaceholderFactory<string, AnimationTemplate>
        {

        }

    }
}
