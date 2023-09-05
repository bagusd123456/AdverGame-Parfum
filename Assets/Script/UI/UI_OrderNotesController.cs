using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class UI_OrderNotesController : MonoBehaviour
{
    public static Action OnIndexChange;
    public static Action OnUpdateInfo;
    public int notesIndex;

    public TMP_Text orderQuality;
    public TMP_Text orderEffect;

    private void OnEnable()
    {
        OnUpdateInfo += UpdateNotesInfo;
    }

    private void OnDisable()
    {
        OnUpdateInfo -= UpdateNotesInfo;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateNotesInfo();
    }

    // Update is called once per frame
    void Update()
    {
        //notesIndex = transform.GetSiblingIndex();
    }

    public void UpdateNotesInfo()
    {
        orderQuality.text = CustomerOrderManager.Instance.currentOrder[notesIndex].objectQuality.ToString();
        orderEffect.text = CustomerOrderManager.Instance.currentOrder[notesIndex].objectEffect.ToString();
    }

    public void HideNotes()
    {
        gameObject.SetActive(false);
    }

    public void ShowNotes()
    {
        gameObject.SetActive(true);
    }
}
