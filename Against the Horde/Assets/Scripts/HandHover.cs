using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Vector3 originalCardScale;
    private Vector3 originalCardPosition;
    private Quaternion originalCardRotation;
    private int originalCardSiblingIndex;
    private CardStateCTRL cardStateCTRL;
    public PlayerHandMGR playerHandMGR;

    void Start()
    {
        cardStateCTRL = GetComponent<CardStateCTRL>();

        playerHandMGR = FindObjectOfType<PlayerHandMGR>();
    }

    public void OnPointerEnter(PointerEventData eventData) //This name is based on the system class, can't rename
    {
        if (cardStateCTRL != null && cardStateCTRL.currentState == CombatReferences.CardState.Hand)
        {
            originalCardPosition = transform.localPosition;
            originalCardSiblingIndex = transform.GetSiblingIndex();
            //originalCardRotation = transform.localRotation; //not needed yet until i fan out the cards
            originalCardScale = transform.localScale;

            transform.localScale = originalCardScale * 1f;
            transform.SetAsLastSibling();
            // transform.localRotation
            transform.localPosition = originalCardPosition + new Vector3(0, 75, 2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData) //This name is based on the system class, can't rename
    {
        if (cardStateCTRL != null && cardStateCTRL.currentState == CombatReferences.CardState.Hand)
        {
            transform.localScale = originalCardScale;
            transform.localPosition = originalCardPosition;
            transform.SetSiblingIndex(originalCardSiblingIndex);
            playerHandMGR.PositionCardsInHand();
        }
    }

    void Update()
    {

    }
}
