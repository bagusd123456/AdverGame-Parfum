using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TimingController : MonoBehaviour
{
    public Slider timingSlider;

    public bool timingActive = false;
    public float timingSliderSpeed = 2f;

    public float targetTimingMin;
    public float targetTimingMax;
    public RectTransform targetFill;
    public float fillSizeValue;

    private float timingTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnValidate()
    {
        //targetFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fillSize);
        
    }

    // Update is called once per frame
    void Update()
    {
        timingTime += Time.deltaTime * timingSliderSpeed;
        if (timingActive)
        {
            //LeanTween.value(gameObject, timingSlider.value, timingSlider.maxValue, Time.time).setLoopPingPong();
            timingSlider.value = Mathf.PingPong(timingTime, timingSlider.maxValue);
        }

        SetFill();
    }

    public void SetFill()
    {
        var size = math.remap(0f, 1f,0f, targetFill.rect.size.x, fillSizeValue);

        targetFill.offsetMin = new Vector2(size * -1, targetFill.offsetMin.y);
        targetFill.offsetMax = new Vector2(size, targetFill.offsetMin.y);
    }
}
