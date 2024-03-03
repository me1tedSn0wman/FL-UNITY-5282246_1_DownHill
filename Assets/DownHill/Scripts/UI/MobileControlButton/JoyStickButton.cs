using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickButton : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject joyStickAim;
    [SerializeField] private GameObject joyStickCenter;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform joyStickCenterRectTransform;
    [SerializeField] private RectTransform joyStickAimRectTransform;



    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        joyStickCenterRectTransform = joyStickCenter.GetComponent<RectTransform>();
        joyStickAimRectTransform = joyStickAim.GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData eventData) {
        joyStickAim.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        joyStickAim.SetActive(false);
        PlayerControl.Instance.OnMoveVal(new Vector2(0, 0));
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pointerPosition = eventData.position;

        Vector3 centerPosition = joyStickCenterRectTransform.position;
        Vector2 delta = new Vector2(pointerPosition.x - centerPosition.x, pointerPosition.y - centerPosition.y);
        float radius = rectTransform.rect.width / 2;
        float deltaClampMagnitude = Mathf.Clamp(delta.magnitude, 0, radius);
        Vector2 deltaNormalize = delta.normalized * deltaClampMagnitude;
        joyStickAimRectTransform.anchoredPosition = deltaNormalize;
        PlayerControl.Instance.OnMoveVal(deltaNormalize / radius);
    }



}
