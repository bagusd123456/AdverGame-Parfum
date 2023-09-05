using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerOrderManager : MonoBehaviour
{
    public static CustomerOrderManager Instance { get; private set; }
    public static Action OnOrderGenerated;
    public static Action OnOrderCompleted;
    public static Action OnOrderFailed;
    public static Action<ObjectData> OnOrderAccepted;

    public List<ObjectData> finishedOrder = new List<ObjectData>();
    public List<ObjectData> failedOrder = new List<ObjectData>();
    public List<ObjectData> currentOrder = new List<ObjectData>();

    public int orderCapacity;

    public void OnEnable()
    {
        OnOrderAccepted += AcceptOrder;
    }

    public void OnDisable()
    {
        OnOrderAccepted -= AcceptOrder;
    }

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
        UpdateOrderNotes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI_OrderNotes

    public void UpdateOrderNotes()
    {
        for (int i = 0; i < currentOrder.Count; i++)
        {
            var order = Managers_OrderNotesUI.Instance.orderNotesControllers[i];
            order.notesIndex = i;
            order.UpdateNotesInfo();
        }
    }

    #endregion

    public void AcceptOrder(ObjectData objectData)
    {
        //Debug.Log($"Order Placed: {objectData.objectName}");
        for (int i = 0; i < currentOrder.Count; i++)
        {
            var order = Managers_OrderNotesUI.Instance.orderNotesControllers[i];
            if (CheckOrderCompletionRate(currentOrder[i], objectData))
            {
                
                order.HideNotes();
                Managers_OrderNotesUI.Instance.orderNotesControllers.Remove(order);

                UpdateOrderNotes();
                break;
            }
            else
            {
                if (i == currentOrder.Count - 1)
                {
                    Debug.LogWarning($"Order Effect: {order.orderEffect.text} is Failed");
                }
            }

        }
    }

    public void CompleteOrder(ObjectData objectData)
    {
        currentOrder.Remove(objectData);
        finishedOrder.Add(objectData);
    }

    public void FailOrder(ObjectData objectData)
    {
        currentOrder.Remove(objectData);
        failedOrder.Add(objectData);
    }

    [Button("Generate Order")]
    public void GenerateOrder()
    {
        for (int i = 0; i < currentOrder.Count; i++)
        {

            if (currentOrder[i] == null)
            {
                ObjectData customerOrderData = new ObjectData();
                currentOrder.Add(customerOrderData);
            }
            else
            {
                currentOrder[i] = GenerateRandomData();
                //GenerateRandomData(currentOrder[i]);
            }
        }

        UI_OrderNotesController.OnUpdateInfo?.Invoke();
    }

    public ObjectData GenerateRandomData()
    {
        ObjectData data = new ObjectData();

        //data.objectEffect = (Effect)Random.Range(0, (int)Effect.Sextuple + 1);
        data.objectQuality = (Quality)Random.Range(0, (int)Quality.Legendary + 1);

        //targetData = data;
        return data;
    }

    public bool CheckOrderCompletionRate(ObjectData orderData, ObjectData objectData)
    {
        int completionRate = 0;
        if (orderData.objectQuality == objectData.objectQuality)
        {
            completionRate++;
        }

        if (orderData.objectEffect == objectData.objectEffect)
        {
            //completionRate++;
        }

        if (completionRate > 0)
        {
            CompleteOrder(orderData);
            Debug.Log($"Order Name: {orderData.objectName} is Success");
            return true;
        }
        else
        {
            //FailOrder(orderData);
            //Debug.Log($"Order Name: {orderData.objectName} is Failed");
            return false;
        }
    }

    public static bool IsAnyOrderFound()
    {
        return false;
    }
}
