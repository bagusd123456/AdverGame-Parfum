using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectController : MonoBehaviour
{
    public ObjectData_SO objectInitialData;
    public ObjectData objectCurrentData = new ObjectData();

    private Vector3 _defaultPosition;

    public Transform defaultParent;
    public Transform secondParent;
    public bool isDragging = false;
    private LeanSelectableByFinger selectableByFinger;
    private void Awake()
    {
        selectableByFinger = GetComponent<LeanSelectableByFinger>();

        _defaultPosition = transform.localPosition;
        defaultParent = transform.parent;
    }

    private void OnEnable()
    {
        if (!objectCurrentData.isInitialized)
        {
            objectCurrentData = new ObjectData();
            objectCurrentData.objectInitialData = objectInitialData;
            objectCurrentData.InitData();
        }


        LeanTouch.OnFingerDown += SelectHandler;
        LeanTouch.OnFingerUpdate += DragHandler;
        LeanTouch.OnFingerUp += DropHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (secondParent != null && !isDragging)
        {
            
        }
    }

    public void SelectHandler(LeanFinger finger)
    {
        
        if (isDragging)
        {
            
        }
    }

    public void DragHandler(LeanFinger finger)
    {
        if (!selectableByFinger.IsSelectedBy(finger)) return;

        //Debug.Log($"Is Dragging {gameObject.name}");
        isDragging = true;

        if (isDragging)
        {
            
        }
    }

    public void DropHandler(LeanFinger finger)
    {
        if (!selectableByFinger.IsSelectedBy(finger)) return;

        if (secondParent != null)
        {
            //if (CustomerOrderManager.Instance.CheckOrderCompletionRate(objectCurrentData))
            //{
            //    transform.SetParent(secondParent);
            //    CustomerOrderManager.OnOrderAccepted?.Invoke(objectCurrentData);
            //}
            //else
            //{
            //    transform.SetParent(defaultParent);
            //    transform.localPosition = _defaultPosition;
            //}

            transform.SetParent(secondParent);
            CustomerOrderManager.OnOrderAccepted?.Invoke(objectCurrentData);
        }
        else
        {
            transform.SetParent(defaultParent);
            transform.localPosition = _defaultPosition;
        }

        isDragging = false;
    }
}
