using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsFireButtonHeldOn {get; set;}

    public void OnPointerDown(PointerEventData data) {
        IsFireButtonHeldOn = true;
    }

    public void OnPointerUp(PointerEventData data) {
        IsFireButtonHeldOn = false;
    }
}
