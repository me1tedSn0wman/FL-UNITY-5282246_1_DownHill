using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SpeedometerUI : MonoBehaviour
{
    public GameObject arrowImageGO;

    public float minValue;
    public float maxValue;

    public float moveTimeDuration = 1.5f;
    private float timeStartMove = -1f;

    private float rateValue
    {
        set {
            timeStartMove = Time.time;
            pts = new List<float>() { rateImmediate, value };
        }
    }
    private List<float> pts = new List<float>();

    public float rateImmediate = 0;

    public void SetValue(float rate) {
        rateValue = rate;
    }

    public void Update()
    {
        if (timeStartMove == -1) return;
        float rateTime = (Time.time - timeStartMove) / moveTimeDuration;
        rateTime = Mathf.Clamp01(rateTime);
        rateImmediate = Util.Bezier(rateTime, pts);

        if (rateTime == 1) {
            timeStartMove = -1;
        }

        float angleValue = minValue + rateImmediate * (maxValue - minValue);
        arrowImageGO.transform.localRotation = Quaternion.Euler(0, 0, angleValue);
    }
}
