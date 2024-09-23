using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/EfCommander")]
public class EfCommander : Effect
{
    //Card Description: Summon: give adjacent monsters 2 power
    //Targets adjacent two monster slots
    //Give them both 2 power

    public override void Activate(GameObject cardGameObject)
    {
        CardDetails playerCard = cardGameObject.GetComponent<CardDetails>();
        PlayerFieldMGR playerFieldMGR = FindObjectOfType<PlayerFieldMGR>();

        int powerIncrease = 2;

        int slotIndex = playerFieldMGR.playerSlots.IndexOf(cardGameObject.transform.parent.gameObject);

        ApplyCommanderEffect(playerFieldMGR, slotIndex, powerIncrease);

    }


    private void ApplyCommanderEffect(PlayerFieldMGR playerFieldMGR, int slotIndex, int powerIncrease)
    {
        int leftSlotIndex = slotIndex - 1;
        int rightSlotIndex = slotIndex + 1;

        if (leftSlotIndex >= 0)
        {
            ApplyPowerBonusToSlot(playerFieldMGR, leftSlotIndex, powerIncrease);
        }

        if (rightSlotIndex < playerFieldMGR.playerSlots.Count)
        {
            ApplyPowerBonusToSlot(playerFieldMGR, rightSlotIndex, powerIncrease);
        }
    }

    private void ApplyPowerBonusToSlot(PlayerFieldMGR playerFieldMGR, int slotIndex, int powerIncrease)
    {
        GameObject slot = playerFieldMGR.playerSlots[slotIndex];
        if (slot.transform.childCount > 0)
        {

            GameObject adjacentMonster = slot.transform.GetChild(0).gameObject;
            CardDetails adjacentMonsterDetails = adjacentMonster.GetComponent<CardDetails>();
            if (adjacentMonsterDetails != null)
            {
                adjacentMonsterDetails.cardPowerInt += powerIncrease;
                adjacentMonsterDetails.cardPower.text = adjacentMonsterDetails.cardPowerInt.ToString();

            }
        }
    }
}
