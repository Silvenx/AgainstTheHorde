using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/EfBlessingOfKings")]
public class EfBlessingOfKings : Effect
{
    //Card Description: Give a monster 3 power and 3 life
    //Card goes to the spell slot location and stays there 
    //Targets hovered monster
    //Give it 3 power 3 life
    //discard

    public int powerIncrease = 3;
    public int lifeIncrease = 3;
    public Transform spellHoldArea;
    public TurnMGR turnMGR;
    public int cardEnergyCost;
    public Texture2D targetCursorTexture;
    public CardDetails cardDetails;
    public PlayerFieldMGR playerFieldMGR;

    public override void Activate(GameObject cardGameObject)
    {
        //Find the spell slot location, then set it as parent and place it there
        GameObject spellHoldAreaObject = GameObject.Find("SpellHoldArea");
        spellHoldArea = spellHoldAreaObject.transform;
        cardGameObject.transform.SetParent(spellHoldArea, false);
        cardGameObject.transform.position = spellHoldArea.position;

        playerFieldMGR = FindObjectOfType<PlayerFieldMGR>();

        TargetMGR targetingManager = FindObjectOfType<TargetMGR>();
        if (targetingManager != null)
        {
            targetingManager.StartTargeting(this);
        }

        //turnMGR.currentEnergy = turnMGR.currentEnergy - cardEnergyCost; // take the cost away from current energy
        //turnMGR.UpdateEnergyUI();
    }

    public void ApplyEffect(CardDetails targetCard)
    {

        targetCard.cardPowerInt += powerIncrease;
        targetCard.cardPower.text = targetCard.cardPowerInt.ToString();
        targetCard.cardLifeInt += lifeIncrease;
        targetCard.cardLife.text = targetCard.cardLifeInt.ToString();

        targetCard.UpdateCardUI();
        Debug.Log("Effect applied to monster: +3 Power, +3 Life.");

        GameObject spellCard = spellHoldArea.GetChild(0).gameObject;

        if (playerFieldMGR != null)
        {
            Debug.Log("Sending spell card to graveyard: " + spellCard.name);
            playerFieldMGR.PlayerSpellToGraveyard(spellCard);  // Call the method on the instance
        }
        else
        {
            Debug.LogError("PlayerFieldMGR instance is null!");
        }
    }



}
