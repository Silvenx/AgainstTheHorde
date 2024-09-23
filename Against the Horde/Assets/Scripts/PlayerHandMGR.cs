using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerHandMGR : MonoBehaviour
{
    public List<GameObject> hand = new List<GameObject>();
    public RectTransform handPositionRect;
    public float cardSpacing;
    public GameObject handArea;

    public void AddCardToHand(GameObject addedCard)
    {
        hand.Add(addedCard);
        addedCard.transform.SetParent(handPositionRect.transform, false);
        addedCard.transform.SetAsLastSibling();
        PositionCardsInHand();

    }

    public void PositionCardsInHand()
    {

        //Calculate the width of all cards including the spacing
        float totalHandWidth = 0f; //Resets the width to 0 to recalculate
        for (int i = 0; i < hand.Count; i++)
        {
            RectTransform cardRectTransform = hand[i].GetComponent<RectTransform>(); //gets the width of each card 1 by 1 
            totalHandWidth += cardRectTransform.rect.width; //and adds them to the total value
            if (i < hand.Count - 1)
            {
                totalHandWidth += cardSpacing; //the if statement checks if there are any more cards after this one, if yes: adds more spacing
            }
        }

        //Calculate starting position of the cards
        float startX = -totalHandWidth / 2f;

        //Position all of the cards based on the starting position
        for (int i = 0; i < hand.Count; i++)
        {
            RectTransform cardRectTransform = hand[i].GetComponent<RectTransform>();
            float cardWitdth = cardRectTransform.rect.width;

            cardRectTransform.anchoredPosition = new Vector2(startX + cardWitdth / 2f, 0f);
            cardRectTransform.rotation = Quaternion.Euler(0, 0, 0);

            startX += cardWitdth + cardSpacing;

            //Change the current state of the cards in hand, this is so I can drag/hover and other things without breaking them
            CardStateCTRL cardStateCTRL = hand[i].GetComponent<CardStateCTRL>();
            if (cardStateCTRL != null)
            {
                cardStateCTRL.SetCardState(CombatReferences.CardState.Hand);
            }

        }

    }

    public void RemoveCardFromHand(GameObject removedCard)
    {
        //This runs when a card is removed from the hand, and reorganises the cards by running position cards in hand
        hand.Remove(removedCard);
        PositionCardsInHand();
    }

    void Update()
    {
        //PositionCardsInHand(); - might need to sort this out again - because unless I force an update later? it may mess up order on drag
    }

}
