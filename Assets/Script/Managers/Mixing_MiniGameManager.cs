using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Mixing_MiniGameManager : MonoBehaviour
{
    public enum MixingState {Idle, PickQuality, PickEffect, Mix, Finish}
    public static Mixing_MiniGameManager Instance { get; private set; }

    public MixingState gameState = MixingState.Idle;


    public GameObject mixingQualityPanel;
    public GameObject mixingEffectPanel;
    public GameObject mixingStatePanel;

    public Slider mixingSlider;
    public float mixingSliderValue;
    public float mixingSliderSpeedMultiplier = 1f;
    public float mixingSliderDuration = 1f;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        if (gameState == MixingState.Mix)
        {
            //LeanTouch.OnFingerDown += OnConfirm;
        }
        else
        {
            //LeanTouch.OnFingerDown -= OnConfirm;
        }
    }

    private void OnDisable()
    {
        //LeanTouch.OnFingerDown -= OnConfirm;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == MixingState.PickQuality)
        {
            mixingQualityPanel.SetActive(true);
            mixingEffectPanel.SetActive(false);
            mixingStatePanel.SetActive(false);
        }
        else if (gameState == MixingState.PickEffect)
        {
            mixingQualityPanel.SetActive(false);
            mixingEffectPanel.SetActive(true);
            mixingStatePanel.SetActive(false);
        }
        else if (gameState == MixingState.Mix)
        {
            mixingEffectPanel.SetActive(false);
            mixingQualityPanel.SetActive(false);
            mixingStatePanel.SetActive(true);
            UpdateSliderValue();

        }
        else if (gameState == MixingState.Finish)
        {
            mixingQualityPanel.SetActive(false);
            mixingEffectPanel.SetActive(false);
            mixingStatePanel.SetActive(false);
        }
        else if (gameState == MixingState.Idle)
        {
            mixingQualityPanel.SetActive(false);
            mixingEffectPanel.SetActive(false);
            mixingStatePanel.SetActive(false);
        }
    }

    public void UpdateSliderValue()
    {
        mixingSliderValue = Mathf.PingPong(Time.time / mixingSliderDuration * mixingSliderSpeedMultiplier, 1);
        mixingSlider.value = mixingSliderValue;
    }

    public void OnConfirm(LeanFinger finger)
    {
        if (gameState == MixingState.Mix)
        {
            if (mixingSliderValue > 0.3 && mixingSliderValue < 0.7)
            {
                gameState = MixingState.Finish;
                Debug.Log("Mixing Success");
            }
            else
            {
                Debug.LogWarning("Mixing Failed");
            }
        }
    }
}
