using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerFieldMGR : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{

    private CardStateCTRL cardStateCTRL;
    private PlayerHandMGR playerHandMGR;
    private TurnMGR turnMGR;
    //private bool isOverFieldZone1 = false; // To check if the card is over the field - not useful anymore
    private CardDetails cardDetails;
    private GameObject currentSlot = null; // tracks the slot the card is hovering over
    public List<GameObject> playerSlots; //Player slots are assigned in the inspector
    public CombatMGR combatMGR;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    void Start()
    {
        playerSlots = new List<GameObject>();

        cardStateCTRL = GetComponent<CardStateCTRL>();
        playerHandMGR = FindObjectOfType<PlayerHandMGR>();
        turnMGR = FindObjectOfType<TurnMGR>();
        cardDetails = GetComponent<CardDetails>(); // Get the CardDetails component to access the card's energy cost
        combatMGR = FindObjectOfType<CombatMGR>();

        //Ensure raycaster is set on Canvas object
        raycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();

        //This just runs through 1 - 5 and finds PlayerSlot plus the value then adds it to the list, easily can expand field here
        for (int i = 1; i <= 5; i++)
        {

            GameObject slot = GameObject.Find("PlayerSlot" + i);

            if (slot != null)
            {
                playerSlots.Add(slot);
            }
        }

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        int cardEnergyCost = int.Parse(cardDetails.cardEnergy.text); //converts the text mesh to int then the next line checks energy
        if (cardEnergyCost <= turnMGR.currentEnergy &&
        (cardStateCTRL.currentState == CombatReferences.CardState.Hand || cardStateCTRL.currentState == CombatReferences.CardState.Dragging))
        {

            Debug.Log("starting draggin");
            //This sets variables, sets the state to dragging and sets the position to the cursor
            cardStateCTRL.SetCardState(CombatReferences.CardState.Dragging);
            //transform.SetParent(transform.root); - removed, wasn't helping

            //This removes the card from the hand list so the cards fix up in hand
            playerHandMGR.RemoveCardFromHand(gameObject);



        }
        Debug.Log("Trigger hit");
    }


    public void OnDrag(PointerEventData eventData)
    {

        int cardEnergyCost = int.Parse(cardDetails.cardEnergy.text); //converts the text mesh to int then the next line checks energy
        if (cardEnergyCost <= turnMGR.currentEnergy &&
        (cardStateCTRL.currentState == CombatReferences.CardState.Hand || cardStateCTRL.currentState == CombatReferences.CardState.Dragging))
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //will need an if statement here eventually to add in drop
        int cardEnergyCost = int.Parse(cardDetails.cardEnergy.text); //converts the text mesh to int

        if (cardEnergyCost <= turnMGR.currentEnergy &&
                    (cardStateCTRL.currentState == CombatReferences.CardState.Hand || cardStateCTRL.currentState == CombatReferences.CardState.Dragging))
        {
            switch (cardDetails.cardType)
            {
                case CardType.Monster:
                    playMonsterCard(cardEnergyCost);
                    break;

                case CardType.Spell:
                    playSpellCard(cardEnergyCost);
                    break;

                case CardType.Equipment:
                    //playEquipmentCard();
                    break;

                case CardType.Enchantment:
                    //playEnchantmentCard();
                    break;

                case CardType.Field:
                    //playFieldCard();
                    break;

                default:
                    Debug.LogWarning("Unknown card type.");
                    playerHandMGR.AddCardToHand(gameObject); // If card type is unknown, return to hand
                    break;

            }
        }
    }

    public void playMonsterCard(int cardEnergyCost)
    {
        if (currentSlot != null) // Check if the card is over a valid slot
        {
            cardStateCTRL.SetCardState(CombatReferences.CardState.Field);

            transform.SetParent(currentSlot.transform, false);
            transform.position = currentSlot.transform.position;
            turnMGR.currentEnergy = turnMGR.currentEnergy - cardEnergyCost; // take the cost away from current energy
            turnMGR.UpdateEnergyUI();

            int slotIndex = playerSlots.IndexOf(currentSlot);
            combatMGR.playerMonsters[slotIndex] = gameObject;

            TriggerOnPlaySound(); //trigger the on play sound
            TriggerPlayEffect(); //trigger the on play effect
        }
        else
        {
            playerHandMGR.AddCardToHand(gameObject);
        }
    }

    public void playSpellCard(int cardEnergyCost)
    {
        //check if over play area
        if (IsOverPlayArea())
        {
            Debug.Log("Card is over play area");
            cardStateCTRL.SetCardState(CombatReferences.CardState.Field); //set cardstate to field

            TriggerOnPlaySound();//play audio clip
            TriggerPlayEffect();//play card effect on play
        }

        //else put card back in hand
        else
        {
            Debug.Log("Card is not over play area!!");
            playerHandMGR.AddCardToHand(gameObject);
        }



    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerSlots.Contains(other.gameObject))
        {
            currentSlot = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentSlot == other.gameObject)
        {
            currentSlot = null;
        }
    }

    private bool IsOverPlayArea()
    {
        // Set up the PointerEventData with the current mouse position
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // Create a list to store the results of the raycast
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast using the GraphicRaycaster and mouse position
        raycaster.Raycast(pointerEventData, results);

        // Check if any UI element was hit by the raycast
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("SpellPlayableArea"))
            {
                return true;
            }
        }
        return false;
    }


    private void TriggerOnPlaySound()
    {
        AudioClip playSound = cardDetails.cardData.cardPlaySound;

        if (playSound != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // If there is no AudioSource, add one to the GameObject
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.PlayOneShot(playSound);
        }
        else
        {
            Debug.LogWarning($"{cardDetails.cardName.text} does not have an assigned play sound.");
        }
    }

    private void TriggerPlayEffect()
    {
        //Playing the effect of a card on play
        CardEffectDetails cardEffectDetails = gameObject.GetComponent<CardEffectDetails>();
        if (cardEffectDetails != null)
        {
            cardEffectDetails.TriggerEffects(TriggerType.PLAY);
        }
    }

}
