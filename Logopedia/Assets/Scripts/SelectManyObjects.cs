using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Logopedia.UIConnection;
using UnityEngine.EventSystems;
using Doozy.Engine;


namespace Logopedia.UserInterface
{
	public class SelectManyObjects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler ,IDragHandler
	{
		[Inject]
		ItemsManager _itemsManager;

		[SerializeField]
		public List<GameObject> unit; // массив всех юнитов, которых мы можем выделить
		[SerializeField]
		public List<GameObject> unitSelected; // массив выделенных юнитов
		//[SerializeField]
		//private bool isMouse;


		public GUISkin skin;
		private Rect rect;
		[SerializeField]
		private bool draw;
		private Vector2 startPos;
		private Vector2 endPos;

		void Start()
        {
			unit.Clear();
			unitSelected.Clear();
			_itemsManager.SelectedGarments.Clear();
			_itemsManager.SelectedGarments.RemoveAll(x => x == null);
			foreach (GameObject u in _itemsManager.Garments)
			{
				unit.Add(u.transform.GetChild(1).transform.gameObject);
			}
			Debug.Log("Click on BG, garments count: " + _itemsManager.Garments.Count);
			GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
		}

		public void OnPointerUp(PointerEventData eventData)
        {
			draw = false;
			//isMouse = false;
        }
		public void OnBeginDrag(PointerEventData eventData)
        {
			draw = true;
			_itemsManager.SelectedGarments.Clear();
			_itemsManager.SelectedGarments.RemoveAll(x => x == null);
			startPos = Input.mousePosition;
			GameEventMessage.SendEvent(EventsLibrary.ItemSelected);

		}

        public void OnDrag(PointerEventData eventData)
        {
            draw = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
			draw = false;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			unit.Clear();
			unitSelected.Clear();
			_itemsManager.SelectedGarments.Clear();
			_itemsManager.SelectedGarments.RemoveAll(x => x == null);
			foreach (GameObject u in _itemsManager.Garments)
			{
				unit.Add(u.transform.GetChild(1).transform.gameObject);
			}
			//Debug.Log("Click on BG, garments count: " + _itemsManager.Garments.Count);
			GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//isMouse = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			//isMouse = false;
		}

		void Awake()
		{
			unit = new List<GameObject>();
			unitSelected = new List<GameObject>();
		}

		// проверка, добавлен объект или нет

        void OnGUI()
        {
            GUI.skin = skin;
            GUI.depth = -1;

            if (draw == true)
            {
                unitSelected.Clear();
				_itemsManager.SelectedGarments.Clear();
				endPos = Input.mousePosition;
                if (startPos == endPos) return;

                rect = new Rect(Mathf.Min(endPos.x, startPos.x),
                                Screen.height - Mathf.Max(endPos.y, startPos.y),
                                Mathf.Max(endPos.x, startPos.x) - Mathf.Min(endPos.x, startPos.x),
                                Mathf.Max(endPos.y, startPos.y) - Mathf.Min(endPos.y, startPos.y)
                                );

                GUI.Box(rect, "");
                foreach (GameObject u in unit)
                {
                    // трансформируем позицию объекта из мирового пространства, в пространство экрана
                    Vector2 tmp = new Vector2(Camera.main.WorldToScreenPoint(u.transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(u.transform.position).y);

                    if (rect.Contains(tmp) & (_itemsManager.SelectedGarments.Contains(u) != true)) // проверка, находится-ли текущий объект в рамке и добавлен ли он в пул менеджера
                    {
                        unitSelected.Add(u.transform.parent.gameObject);
                        _itemsManager.SelectedGarments.Add(u.transform.parent.gameObject);
                        GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
                    }
					//else
					//	if (rect.Contains(tmp) != true & (_itemsManager.SelectedGarments.Contains(u) == true))

					//{
					//	unitSelected.Remove(u.transform.parent.gameObject);
					//	unitSelected.RemoveAll(x => x == null);
					//	_itemsManager.SelectedGarments.Remove(u.transform.parent.gameObject);
					//	_itemsManager.SelectedGarments.RemoveAll(x => x == null);

					//	GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
					//}
                }
            }
        }

    }
}
	