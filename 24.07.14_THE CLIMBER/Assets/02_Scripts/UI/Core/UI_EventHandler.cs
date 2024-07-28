using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultSetting
{
    public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        public Action<PointerEventData> OnClickHandler = null;
        public Action<PointerEventData> OnBeginDragHandler = null;
        public Action<PointerEventData> OnDragHandler = null;
        public Action<PointerEventData> OnEndDragHandler = null;
        public Action<PointerEventData> OnEnterHandler = null;
        public Action<PointerEventData> OnExitHandler = null;
        public Action<PointerEventData> OnUpHandler = null;
        public Action<PointerEventData> OnDownHandler = null;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClickHandler != null)
                OnClickHandler.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (OnBeginDragHandler != null)
                OnBeginDragHandler.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragHandler != null)
                OnDragHandler.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (OnEndDragHandler != null)
                OnEndDragHandler.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnEnterHandler != null)
                OnEnterHandler.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExitHandler != null)
                OnExitHandler.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnUpHandler != null)
                OnUpHandler.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (OnDownHandler != null)
                OnDownHandler.Invoke(eventData);
        }
    }
}
