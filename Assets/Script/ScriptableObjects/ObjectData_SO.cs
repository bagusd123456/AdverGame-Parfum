using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Fragrance
{
    None, Fruity, Aromatic, Woody,
    Floral, Citrus
}
public enum Strength
{
    None, Durable, Momentary
}

[CreateAssetMenu(fileName = "ObjectName_objectSO", menuName = "SO/Create NewObject")]
public class ObjectData_SO : ScriptableObject
{
    public string objectName = "Object_Undefined";
    public Sprite objectSprite;
    [Space]
    public Fragrance objectFragrance = Fragrance.Fruity;

    public Strength objectStrength = Strength.Durable;
}
