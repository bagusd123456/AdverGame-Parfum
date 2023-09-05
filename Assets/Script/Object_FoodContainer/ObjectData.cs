using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectData
{
    public bool isInitialized = false;
    public ObjectData_SO objectInitialData;
    public string objectName = "Object_Undefined";
    public Sprite objectSprite;
    [Space]
    public Quality objectQuality = Quality.Common;

    public Effect objectEffect = Effect.Single;

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
        objectQuality = objectInitialData.objectQuality;
        objectEffect = objectInitialData.objectEffect;
    }
}
