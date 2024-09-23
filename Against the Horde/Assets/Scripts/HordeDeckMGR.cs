using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeDeckMGR : MonoBehaviour
{
    public List<GameObject> hordeDeck = new List<GameObject>();
    public List<CardData> currentHordeDeckList = new List<CardData>();
    public RectTransform hordeDeckPositionRect;
    public GameObject cardPrefab;
    public Canvas combatCanvas;
    public HordeFieldMGR hordeFieldMGR;
    public CombatMGR combatMGR;
    //public HordeCardEffects hordeCardEffects;

    void Start()
    {
        RectTransform deckPositionRect = GameObject.Find("PlayerDeckArea").GetComponent<RectTransform>();
        LoadHordeDeck();
        ShuffleHordeDeck();
    }

    void LoadHordeDeck()
    {
        currentHordeDeckList = new List<CardData>(Resources.LoadAll<CardData>("Cards/Horde"));

        foreach (CardData cardData in currentHordeDeckList)
        {
            GameObject newCard = Instantiate(cardPrefab, hordeDeckPositionRect.anchoredPosition, Quaternion.Euler(0, 0, 90));
            newCard.name = cardData.cardName;
            CardDetails cardDetails = newCard.GetComponent<CardDetails>();

            cardDetails.cardData = cardData;
            cardDetails.InitialiseCard(
                cardData.cardName,
                cardData.description,
                cardData.energy,
                cardData.power,
                cardData.life,
                cardData.cardType,
                cardData.cardAttribute,
                cardData.cardImage,
                cardData.cardBuffTough,
                cardData.cardBuffDivine,
                cardData.cardPlaySound
            );

            // Assign CardEffectDetails
            CardEffectDetails cardEffectDetails = newCard.GetComponent<CardEffectDetails>();
            if (cardEffectDetails != null)
            {
                cardEffectDetails.cardData = cardData; // Assign CardData
            }
            else
            {
                Debug.LogError("CardEffectDetails component is missing on the instantiated card prefab."); //commeted out because spam, can be used later
            }

            newCard.transform.SetParent(combatCanvas.transform, false);
            hordeDeck.Add(newCard);

        }

    }

    public void ShuffleHordeDeck()
    {
        //This uses a pre-defined shuffling method, not sure how this works
        System.Random rng = new System.Random();
        int n = hordeDeck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = hordeDeck[k];
            hordeDeck[k] = hordeDeck[n];
            hordeDeck[n] = value;
        }

    }


    public void HordeDrawCard()
    {
        if (hordeDeck.Count > 0)
        {
            GameObject drawnCard = hordeDeck[0];
            hordeDeck.RemoveAt(0);

            CardDetails cardDetails = drawnCard.GetComponent<CardDetails>();
            if (cardDetails.cardType == CardType.Monster)
            {
                Debug.Log("Horde Monster Drawn");
                hordeFieldMGR.PlayHordeMonster(drawnCard);
                //play card effect
                CardEffectDetails cardComponent = drawnCard.GetComponent<CardEffectDetails>();
                if (cardComponent != null)
                {
                    cardComponent.TriggerEffects(TriggerType.PLAY);
                }
                else
                {
                    Debug.LogWarning($"[{drawnCard.name}] CardEffectDetails component is missing.");
                }
            }
            else if (cardDetails.cardType == CardType.Spell)
            {
                //play draw effect
                CardEffectDetails cardComponent = drawnCard.GetComponent<CardEffectDetails>();
                if (cardComponent != null)
                {
                    cardComponent.TriggerEffects(TriggerType.DRAW);
                }
                else
                {
                    Debug.LogWarning($"[{drawnCard.name}] CardEffectDetails component is missing.");
                }

                //hordeCardEffects.ExecuteHordeCardEffect(cardDetails);
                combatMGR.HordeCardToGraveyard(drawnCard);
            }
        }

        if (hordeDeck.Count == 0)
        {
            //hordeDeckBacking.SetActive(false); fix this later
        }
    }
    public void PrintHordeDeckOrder()
    {
        // Initialize a string to build the deck order with a header
        string deckOrder = "Current Horde Deck Order:\n";

        // Loop through the horde deck and append each card name with numbering
        for (int i = 0; i < hordeDeck.Count; i++)
        {
            GameObject cardObject = hordeDeck[i];
            CardDetails cardDetails = cardObject.GetComponent<CardDetails>();
            if (cardDetails != null)
            {
                string cardName = cardDetails.cardName.text; // Use the cardNameString field
                deckOrder += $"{i + 1}. {cardName}\n"; // Add line break after each card
            }
            else
            {
                deckOrder += $"{i + 1}. [Unknown Card at index {i}]\n"; // Handle missing CardDetails
            }
        }

        // Log the entire deck order
        Debug.Log(deckOrder);
    }

}
