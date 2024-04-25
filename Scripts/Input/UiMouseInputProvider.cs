using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WinzardyDemo.Input
{
    public interface IMouseInput :
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        //clicks
        new Action<PointerEventData> OnPointerClick { get; set; }
        Action<PointerEventData> OnLeftMouseDown { get; set; }
        Action<PointerEventData> OnRightMouseDown { get; set; }
        Action<PointerEventData> OnLeftMouseUp { get; set; }
        Action<PointerEventData> OnRightMouseUp { get; set; }

        //enter
        new Action<PointerEventData, GameObject> OnPointerEnter { get; set; }
        new Action<PointerEventData, GameObject> OnPointerExit { get; set; }
    }
    
    public class UiMouseInputProvider : MonoBehaviour, IMouseInput
    {
        #region Delegates
        Action<PointerEventData> IMouseInput.OnPointerClick { get; set; }
        Action<PointerEventData> IMouseInput.OnLeftMouseDown { get; set; }
        Action<PointerEventData> IMouseInput.OnRightMouseDown { get; set; }
        Action<PointerEventData> IMouseInput.OnLeftMouseUp { get; set; }
        Action<PointerEventData> IMouseInput.OnRightMouseUp { get; set; }
        Action<PointerEventData, GameObject> IMouseInput.OnPointerEnter { get; set; }
        Action<PointerEventData, GameObject> IMouseInput.OnPointerExit { get; set; }
        #endregion

        #region Unity Mouse Events
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerClick?.Invoke(eventData);
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                ((IMouseInput)this).OnLeftMouseDown?.Invoke(eventData);
            else if (eventData.button == PointerEventData.InputButton.Right)
                ((IMouseInput)this).OnRightMouseDown?.Invoke(eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                ((IMouseInput)this).OnLeftMouseUp?.Invoke(eventData);
            else if (eventData.button == PointerEventData.InputButton.Right)
                ((IMouseInput)this).OnRightMouseUp?.Invoke(eventData);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerEnter?.Invoke(eventData, gameObject);

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) =>
            ((IMouseInput)this).OnPointerExit?.Invoke(eventData, gameObject);
        #endregion
    }
}