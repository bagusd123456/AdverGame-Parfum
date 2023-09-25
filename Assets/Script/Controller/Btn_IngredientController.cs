using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Btn_IngredientController : MonoBehaviour
{

    public bool useFragrance, useStrength;
    [ShowIf("useFragrance")]
    public Fragrance objectFragrance = Fragrance.Fruity;
    [ShowIf("useStrength")]
    public Strength objectStrength = Strength.Durable;

    public void SetFragrance()
    {
        Mixing_MiniGameManager.Instance.currentMixingData.objectFragrance = objectFragrance;
        Mixing_MiniGameManager.Instance.gameState++;
        Mixing_MiniGameManager.Instance.ChangeState();
    }

    public void SetStrength()
    {
        Mixing_MiniGameManager.Instance.currentMixingData.objectStrength = objectStrength;
        Mixing_MiniGameManager.Instance.gameState++;
        Mixing_MiniGameManager.Instance.ChangeState();
    }
}
