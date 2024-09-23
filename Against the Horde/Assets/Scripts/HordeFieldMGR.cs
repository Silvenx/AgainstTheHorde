using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HordeFieldMGR : MonoBehaviour
{
    //public RectTransform hordeSlot;
    public GameObject hordePlayArea;
    public CombatMGR combatMGR;
    public List<GameObject> hordeSlots = new List<GameObject>(); //The slot list, not the list of cards
    public List<GameObject> playerSlots = new List<GameObject>(); //The slot list, not the list of cards

    void Start()
    {
        for (int i = 1; i <= 5; i++)
        {
            GameObject slot = GameObject.Find("PlayerSlot" + i);
            if (slot != null)
            {
                playerSlots.Add(slot);
            }
        }
        for (int i = 1; i <= 5; i++)
        {
            GameObject slot = GameObject.Find("HordeSlot" + i);
            if (slot != null)
            {
                hordeSlots.Add(slot);
            }
        }
    }

    public void PlayHordeMonster(GameObject addedCard)
    {

        PositionHordeMonster(addedCard);
    }

    public void PositionHordeMonster(GameObject card)
    {
        Debug.Log($"PositionHordeCard called for card '{card.name}'");
        //Try to place the monster against a player's left-most monster if it's free
        for (int i = 0; i < playerSlots.Count; i++)
        {
            if (playerSlots[i].transform.childCount > 0 && hordeSlots[i].transform.childCount == 0)
            {
                card.transform.rotation = Quaternion.identity;
                card.transform.SetParent(hordeSlots[i].transform, false);
                card.transform.localPosition = Vector3.zero;
                card.transform.SetAsLastSibling();

                combatMGR.hordeMonsters[i] = card; //assign the card to the slot in the array

                return;
            }
        }

        //Then try to place it against the left-most free slot
        for (int i = 0; i < hordeSlots.Count; i++)
        {
            if (hordeSlots[i].transform.childCount == 0)
            {

                card.transform.rotation = Quaternion.identity;
                card.transform.SetParent(hordeSlots[i].transform, false);
                card.transform.localPosition = Vector3.zero;
                card.transform.SetAsLastSibling();

                combatMGR.hordeMonsters[i] = card; //assign the card to the slot in the array

                return;
            }
        }

        //Discard the card if none are free

        //RectTransform hordeSlot1RectTransform = GameObject.Find("HordeSlot1").GetComponent<RectTransform>();
        //card.transform.position = hordeSlot1RectTransform.position;
        //card.transform.rotation = Quaternion.identity;

    }


}
