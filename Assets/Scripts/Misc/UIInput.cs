using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * This class lets us have mouseDown and mouseUp listeners
 * which is important for the HotkeyButtons
 */

public class UIInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public List<Action<PointerEventData>> OnMouseDownListeners = new List<Action<PointerEventData>>();
    public List<Action<PointerEventData>> OnMouseUpListeners = new List<Action<PointerEventData>>();

    public void OnPointerDown(PointerEventData eventData) {
        foreach (var callback in OnMouseDownListeners) {
            callback(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        foreach (var callback in OnMouseUpListeners) {
            callback(eventData);
        }
    }

    public void AddOnMouseDownListener(Action<PointerEventData> action) {
        OnMouseDownListeners.Add(action);
    }

    public void AddOnMouseUpListener(Action<PointerEventData> action) {
        OnMouseUpListeners.Add(action);
    }
}