using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quality
{
    Common, Uncommon, Rare,
    Epic, Legendary
}
public enum Effect
{
    None, Single, Double,
    Triple, Quadruple, Quintuple,
    Sextuple
}

[CreateAssetMenu(fileName = "ObjectName_objectSO", menuName = "SO/Create NewObject")]
public class ObjectData_SO : ScriptableObject
{
    public string objectName = "Object_Undefined";
    public Sprite objectSprite;
    [Space]
    public Quality objectQuality = Quality.Common;

    public Effect objectEffect = Effect.Single;
}
