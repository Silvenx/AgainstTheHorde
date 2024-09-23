using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/EfCroak")]
public class EfCroak : Effect
{
    //Card Description: Deal 4 damage to the left most player monster
    //Targets left most
    //Deals 4 damage

    public override void Activate(GameObject cardGameObject)
    {
        CombatMGR combatMGR = FindObjectOfType<CombatMGR>();
        HordeFieldMGR hordeFieldMGR = FindObjectOfType<HordeFieldMGR>();

        (GameObject targetCard, int cardIndex) = TargetLeftMost(hordeFieldMGR);
        int damage = 4;
        //If finds a card deals damage
        if (targetCard != null)
        {

            CardDetails targetCardStats = targetCard.GetComponent<CardDetails>();

            Debug.Log($"Croak deals {damage} damage to '{targetCardStats.cardName.text}'");

            combatMGR.PlayerMonsterTakesDamage(targetCard, damage, cardIndex);
        }
    }

    private (GameObject, int) TargetLeftMost(HordeFieldMGR hordeFieldMGR)
    {
        //Searches slots and finds left most available target
        for (int i = 0; i < hordeFieldMGR.playerSlots.Count; i++)
        {

            GameObject slotGameObject = hordeFieldMGR.playerSlots[i];
            Transform slotTransform = slotGameObject.transform;
            if (slotTransform.childCount > 0)
            {
                GameObject playerMonster = slotTransform.GetChild(0).gameObject;
                return (playerMonster, i);
            }
        }
        return (null, -1); // Returns nothing if no target is found
    }
}
