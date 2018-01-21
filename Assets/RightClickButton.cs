using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class RightClickButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onRightClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
        //    Debug.Log("Left click");
        //else if (eventData.button == PointerEventData.InputButton.Middle)
        //    Debug.Log("Middle click");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRightClick.Invoke();
            Debug.Log("Right click");
        }
    }
}
