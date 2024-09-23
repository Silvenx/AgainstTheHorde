using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStateCTRL : MonoBehaviour
{
    public CombatReferences.CardState currentState;

    public void SetCardState(CombatReferences.CardState newState)
    {
        currentState = newState;
    }
}
