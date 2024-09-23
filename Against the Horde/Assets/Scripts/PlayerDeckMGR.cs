using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerDeckMGR : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<GameObject> deck = new List<GameObject>();
    public List<CardData> allCards = new List<CardData>();
    public Canvas combatCanvas;
    public RectTransform deckPositionRect;
    public PlayerHandMGR playerHandMGR; //Refernce the hand script for drawing
    public GameObject deckBacking;


    void Start()
    {
        RectTransform deckPositionRect = GameObject.Find("PlayerDeckArea").GetComponent<RectTransform>(); //I can set this in declared variables but keep this for reference
        LoadDeck();
        ShuffleDeck();

        //Starting Hand Draw, this may have to go into a different place - moved to turn manager
        //for (int i = 0; i < 5; i++)
        //{
        //    DrawCard();
        //}
    }

    void LoadDeck()
    {
        allCards = new List<CardData>(Resources.LoadAll<CardData>("Cards/Player/LegionStarter"));

        foreach (CardData cardData in allCards)
        {
            GameObject newCard = Instantiate(cardPrefab, deckPositionRect.anchoredPosition, Quaternion.Euler(0, 0, 90));
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
            deck.Add(newCard);
        }
        deckBacking.transform.SetAsLastSibling();
        //GameObject firstCard = Instantiate(cardPrefab, deckPositionRect.anchoredPosition, Quaternion.Euler(0, 0, 90));

    }

    //Dictate what drawing a card is, then we need to do multiple cards
    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            GameObject drawnCard = deck[0];
            deck.RemoveAt(0);

            playerHandMGR.AddCardToHand(drawnCard);

        }
        if (deck.Count == 0)
        {
            deckBacking.SetActive(false);
        }
    }


    public void ShuffleDeck()
    {
        //This uses a pre-defined shuffling method, not sure how this works
        System.Random rng = new System.Random();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }

    }
}
