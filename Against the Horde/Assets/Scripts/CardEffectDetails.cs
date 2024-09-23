using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectDetails : MonoBehaviour
{
    public CardData cardData;
    private Dictionary<TriggerType, List<Effect>> effectsByTrigger;

    void Start()
    {
        InitialiseEffects();
    }

    private void InitialiseEffects()
    {
        effectsByTrigger = new Dictionary<TriggerType, List<Effect>>();



        if (cardData.effects != null && cardData.effects.Count > 0)
        {
            foreach (Effect effect in cardData.effects)
            {
                if (!effectsByTrigger.ContainsKey(effect.triggerType))
                {
                    effectsByTrigger[effect.triggerType] = new List<Effect>();
                }
                effectsByTrigger[effect.triggerType].Add(effect);
            }
        }
    }
    public void TriggerEffects(TriggerType triggerType)
    {
        if (effectsByTrigger != null && effectsByTrigger.ContainsKey(triggerType))
        {
            Debug.Log($"[CardEffectDetails] Triggering {triggerType} effects for {gameObject.name}");
            foreach (Effect effect in effectsByTrigger[triggerType])
            {
                effect.Activate(gameObject);
            }
        }
        else
        {
            Debug.Log($"[CardEffectDetails] No {triggerType} effects to trigger for {gameObject.name}");
        }
    }
}
