using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Mixing_MiniGameManager : MonoBehaviour
{
    public enum MixingState {Idle, PickQuality, PickEffect, Mix, Finish}
    public static Mixing_MiniGameManager Instance { get; private set; }

    public MixingState gameState = MixingState.Idle;

    public static Action OnVariableChange;

    public MixingState GameState
    {
        get { return gameState; }

        set
        {
            if (gameState == value) return;
            gameState = value;
            OnVariableChange?.Invoke();
        }
    }

    public GameObject mixingQualityPanel;
    public GameObject mixingEffectPanel;
    public GameObject mixingStatePanel;
    public bool animationPlayed = false;

    public Slider mixingSlider;
    public Image sliderFill;
    public float mixingSliderValue;
    public float mixingSliderSpeedMultiplier = 1f;
    public float mixingSliderDuration = 1f;

    public ObjectData currentMixingData = new ObjectData();
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
        LeanTouch.OnFingerDown += OnConfirm;
        OnVariableChange += PreviewUI;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnConfirm;
        OnVariableChange -= PreviewUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (mixingStatePanel.activeSelf)
        {
            UpdateSliderValue();
            if (mixingSliderValue > 0.3 && mixingSliderValue < 0.7)
            {
                sliderFill.color = Color.green;
            }
            else
            {
                sliderFill.color = Color.white;
            }
        }
        PreviewUI();
    }

    [Button("Preview UI")]
    public void PreviewUI()
    {
        if (animationPlayed) return;

        mixingQualityPanel.SetActive(false);
        mixingEffectPanel.SetActive(false);
        mixingStatePanel.SetActive(false);

        if (gameState == MixingState.PickQuality)
        {
            ShowPanel(mixingQualityPanel.GetComponent<RectTransform>());
        }
        else if (gameState == MixingState.PickEffect)
        {
            ShowPanel(mixingEffectPanel.GetComponent<RectTransform>());
        }
        else if (gameState == MixingState.Mix)
        {
            mixingStatePanel.SetActive(true);
        }

        animationPlayed = true;
    }

    [Button("Change State")]
    public void ChangeState()
    {
        animationPlayed = false;
    }

    public void ShowPanel(RectTransform targeTransform)
    {
        targeTransform.gameObject.SetActive(true);
        targeTransform.anchoredPosition = Vector3.down * targeTransform.sizeDelta.y;
        //Show the panel
        Tween anim = targeTransform.DOAnchorPos(Vector3.zero, .5f);
        anim.SetEase(Ease.InOutSine);
    }

    public void HidePanel(RectTransform targeTransform)
    {
        //hide the panel
        Tween anim = targeTransform.DOAnchorPos(Vector3.down * targeTransform.sizeDelta.y, .5f);
        anim.SetEase(Ease.InOutSine);
        anim.onComplete += () => targeTransform.gameObject.SetActive(false);
    }

    public void UpdateSliderValue()
    {
        mixingSliderValue = Mathf.PingPong(Time.time / mixingSliderDuration * mixingSliderSpeedMultiplier, 1);
        mixingSlider.value = mixingSliderValue;
    }

    public void ChangeStatePickQuality()
    {
        gameState = MixingState.PickQuality;
        ChangeState();
    }

    public void ChangeStatePickEffect()
    {
        gameState = MixingState.PickEffect;
        ChangeState();
    }

    public void ChangeStateMix()
    {
        gameState = MixingState.Mix;
        ChangeState();
    }

    public void ConfirmPickFragrance(Fragrance fragrance)
    {
        currentMixingData.objectFragrance = fragrance;
    }

    public void ConfirmPickStrength(Strength strength)
    {
        currentMixingData.objectStrength = strength;
    }

    public void ClearSelection()
    {
        currentMixingData = new ObjectData();
        gameState = MixingState.Idle;
        ChangeState();
    }

    public void OnConfirm(LeanFinger finger)
    {
        if (gameState == MixingState.Mix)
        {
            if (mixingSliderValue > 0.3 && mixingSliderValue < 0.7)
            {
                gameState = MixingState.Finish;
                CustomerOrderManager.Instance.AcceptOrder(currentMixingData);
                ChangeState();
                Debug.Log("Mixing Success");
            }
            else
            {
                Debug.LogWarning("Mixing Failed");
            }
        }
    }
}
