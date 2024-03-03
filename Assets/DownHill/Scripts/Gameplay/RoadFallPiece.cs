using System.Collections;   
using System.Collections.Generic;
using UnityEngine;

public class RoadFallPiece : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float maxShakeAngle = 2f;
    [SerializeField] private float shakeDuration = 3f;
    [SerializeField] private float startTime = -1f;
    [SerializeField] private float fallDurationBeforeDestroy = 3f;
    private float anglRt;

    public void Awake()
    {
        anglRt = 2 * maxShakeAngle / shakeDuration;
    }
    public void Update()
    {
        if (startTime == -1) return;
        if (Time.time < startTime + shakeDuration / 2)
        {
            Shaking(true);
        }
        else if (Time.time < startTime + shakeDuration)
        {
            Shaking(false);
        }
        else if (Time.time < startTime + fallDurationBeforeDestroy + shakeDuration)
        {
            Falling();
        }
        else { 
            Destroy(gameObject);
        }

    }
    public void StartFall() {
        startTime = Time.time;
    }

    public void Shaking(bool side) {
        if (side)
        {
            transform.Rotate(0, 0, anglRt * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, -anglRt * Time.deltaTime);
        }
    }

    public void Falling() {
        transform.Translate(Vector3.down * fallSpeed* Time.deltaTime, Space.World);
    }

}
