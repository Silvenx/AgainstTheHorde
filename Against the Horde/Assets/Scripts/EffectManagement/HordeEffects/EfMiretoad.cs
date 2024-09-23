using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/EfMiretoad")]
public class EfMiretoad : Effect
{

    //Card Description: Summon: gain divine shield
    //Give self divine shield

    public override void Activate(GameObject cardGameObject)
    {
        CardDetails cardDetails = cardGameObject.GetComponent<CardDetails>();

        if (cardDetails == null)
        {
            Debug.LogError("CardDetails component is missing from the summoned card.");
            return;
        }
        cardDetails.cardBuffDivine += 1;

        Debug.Log($"{cardDetails.cardName.text} has Divine Shield: {cardDetails.cardBuffDivine}");
    }


}
