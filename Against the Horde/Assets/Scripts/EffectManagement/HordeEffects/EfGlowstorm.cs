using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/EfGlowstorm")]
public class EfGlowStorm : Effect
{

    //Card Description: Deal 3 damage to all player monsters
    //Targets all player monsters
    //Deals 3 damage to each

    public override void Activate(GameObject cardGameObject)
    {
        CombatMGR combatMGR = FindObjectOfType<CombatMGR>();
        HordeFieldMGR hordeFieldMGR = FindObjectOfType<HordeFieldMGR>();
        //Description: Deal 3 damage to all player monsters
        int damage = 3;
        bool anyTargets = false;

        //Goes through each one and does damage
        for (int i = 0; i < hordeFieldMGR.playerSlots.Count; i++)
        {
            Transform slotTransform = hordeFieldMGR.playerSlots[i].transform;
            if (slotTransform.childCount > 0)
            {
                GameObject playerMonster = slotTransform.GetChild(0).gameObject;
                CardDetails playerMonsterStats = playerMonster.GetComponent<CardDetails>();
                if (playerMonsterStats != null)
                {
                    playerMonsterStats.cardLifeInt -= damage;
                    playerMonsterStats.ChangeCardUI();
                    Debug.Log($"Glowstorm deals {damage} damage to '{playerMonsterStats.cardName.text}'");
                    if (playerMonsterStats.cardLifeInt <= 0)
                    {
                        combatMGR.PlayerMonsterToGraveYard(playerMonster, i);
                    }

                    anyTargets = true;
                }
            }
            if (!anyTargets)
            {
                Debug.Log("Glowstorm was cast, but no player monsters are on the field.");
            }
        }
    }


}
