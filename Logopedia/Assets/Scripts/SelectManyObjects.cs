using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Logopedia.UIConnection;
using UnityEngine.EventSystems;
using Doozy.Engine;


namespace Logopedia.UserInterface
{
	public class SelectManyObjects : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[Inject]
		ItemsManager _itemsManager;

		[SerializeField]
		public List<GameObject> unit; // массив всех юнитов, которых мы можем выделить
		[SerializeField]
		public List<GameObject> unitSelected; // массив выделенных юнитов
		[SerializeField]
		private bool isMouse;


		public GUISkin skin;
		private Rect rect;
		private bool draw;
		private Vector2 startPos;
		private Vector2 endPos;

		public void OnDrag(PointerEventData eventData)
		{

		}

		public void OnPointerClick(PointerEventData pointerEventData)
		{
			_itemsManager.SelectedGarments.Clear();
			_itemsManager.SelectedGarments.RemoveAll(x => x == null);
			GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			isMouse = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			isMouse = false;
		}

		void Awake()
		{
			unit = new List<GameObject>();
			unitSelected = new List<GameObject>();
		}

		// проверка, добавлен объект или нет
		bool CheckUnit(GameObject unit)
		{
			bool result = false;
			foreach (GameObject u in unitSelected)
			{
				result = true;
			}
			return result;
		}

		void Select()
		{
			if (unitSelected.Count > 0)
			{
				for (int j = 0; j < unitSelected.Count; j++)
				{
					
				}
			}
		}

		void Deselect()
		{
			if (unitSelected.Count > 0)
			{
				for (int j = 0; j < unitSelected.Count; j++)
				{

				}
			}
		}

        void OnGUI()
        {
            GUI.skin = skin;
            GUI.depth = -1;

            unit = _itemsManager.SelectedGarments;

            if (Input.GetMouseButtonDown(0) & (isMouse == true))
            {
                Deselect();
                startPos = Input.mousePosition;
                draw = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                draw = false;
                Select();
            }

            if (draw)
            {
                unitSelected.Clear();
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

                    if (rect.Contains(tmp) | (_itemsManager.SelectedGarments.Contains(u) != true)) // проверка, находится-ли текущий объект в рамке и добавлен ли он в пул менеджера
                    {
                        unitSelected.Add(u);
                        _itemsManager.SelectedGarments.Add(u);
						GameEventMessage.SendEvent(EventsLibrary.ItemSelected);
					}
				}
            }
        }
    }
}
	