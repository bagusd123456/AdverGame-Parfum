using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ObjectData
{
    public bool isInitialized = false;
    public ObjectData_SO objectInitialData;
    public string objectName = "Object_Undefined";
    public Sprite objectSprite;
    [FormerlySerializedAs("objectQuality")] [Space]
    public Fragrance objectFragrance = Fragrance.Fruity;

    [FormerlySerializedAs("objectEffect")] public Strength objectStrength = Strength.Durable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitData()
    {
        objectName = objectInitialData.objectName;
        objectSprite = objectInitialData.objectSprite;
        objectFragrance = objectInitialData.objectFragrance;
        objectStrength = objectInitialData.objectStrength;
    }
}
