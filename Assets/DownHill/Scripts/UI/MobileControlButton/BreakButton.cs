using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
[RequireComponent(typeof(Collider2D))]
*/
public class BreakButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData) {
        PlayerControl.Instance.OnBreakVal(1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerControl.Instance.OnBreakVal(0f);
    }

}
