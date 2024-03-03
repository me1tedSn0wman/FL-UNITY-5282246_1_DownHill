using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GasButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerControl.Instance.OnGasVal(1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerControl.Instance.OnGasVal(0f);
    }
}
