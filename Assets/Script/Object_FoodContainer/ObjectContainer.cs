using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectContainer : MonoBehaviour
{
    [NaughtyAttributes.Tag]
    public string objectTag;

    public Fragrance objectTargetFragrance = Fragrance.Fruity;

    public TMP_Text ObjectRequirementText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectRequirementText != null)
        {
            ObjectRequirementText.text = $"Quality Target: {objectTargetFragrance}";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent<ObjectController>(out var controller);
        if (controller != null)
        {
            Debug.Log($"Colliding with: {gameObject.name}");
            //var controller = other.GetComponent<ObjectController>();
            //if(controller.objectCurrentData.objectQuality != objectTargetQuality) return;

            controller.secondParent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.TryGetComponent<ObjectController>(out var controller);
        if (controller != null)
        {
            other.GetComponent<ObjectController>().secondParent = null;
        }
    }
}
