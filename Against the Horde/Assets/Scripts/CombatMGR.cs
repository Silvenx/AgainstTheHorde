using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMGR : MonoBehaviour
{

    //public List<GameObject> playerMonsters = new List<GameObject>(5);
    //public List<GameObject> hordeMonsters = new List<GameObject>(5);
    public GameObject[] playerMonsters = new GameObject[5];
    public GameObject[] hordeMonsters = new GameObject[5];

    public CardDetails cardDetails;
    public GraveyardMGR graveyardMGR;
    public TurnMGR turnMGR;



    void Awake()
    {
        if (playerMonsters == null || playerMonsters.Length != 5)
        {
            playerMonsters = new GameObject[5];
        }

        if (hordeMonsters == null || hordeMonsters.Length != 5)
        {
            hordeMonsters = new GameObject[5];
        }
    }

    void Start()
    {
        graveyardMGR = FindObjectOfType<GraveyardMGR>();
        turnMGR = FindObjectOfType<TurnMGR>();
    }

    public void StartCombat()
    {
        //I needed to split this up because I couldn't call the IEnumerator below from another script
        //plus it can't run itself
        StartCoroutine(CombatProcess());
    }

    IEnumerator CombatProcess()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject playerMonster = playerMonsters[i];
            GameObject hordeMonster = hordeMonsters[i];

            //combat only happens if both monsters in a slot
            if (playerMonster != null && hordeMonster != null)
            {
                Debug.Log($"Slot {i + 1}: Monsters in both spot, call Monsters Fight");
                yield return StartCoroutine(MonstersFight(playerMonster, hordeMonster, i));
            }
            else if (playerMonster != null && hordeMonster == null)
            {
                Debug.Log($"Slot {i + 1}: Player Deals Lifeforce damage");
                yield return StartCoroutine(PlayerDealLifeforce(playerMonster, hordeMonster, i));
            }
            else if (playerMonster == null && hordeMonster != null)
            {
                Debug.Log($"Slot {i + 1}: Horde Deals Lifeforce damage");
                yield return StartCoroutine(HordeDealLifeforce(playerMonster, hordeMonster, i));
            }
            else
            {
                Debug.Log($"Slot {i + 1}: No monster combat, one side is empty.");
                yield return new WaitForSeconds(1f);
            }
        }
        turnMGR.EndPhase();
    }

    IEnumerator MonstersFight(GameObject playerMonster, GameObject hordeMonster, int index)
    {
        Debug.Log("Combat calculation starting");

        CardDetails playerMonsterStats = playerMonster.GetComponent<CardDetails>();
        CardDetails hordeMonsterStats = hordeMonster.GetComponent<CardDetails>();

        Debug.Log($"Initial Player Life: {playerMonsterStats.cardLifeInt}, Horde Life: {hordeMonsterStats.cardLifeInt}");

        int playerDamageTaken = hordeMonsterStats.cardPowerInt;
        int hordeDamageTaken = playerMonsterStats.cardPowerInt;

        Debug.Log($"Player takes {playerDamageTaken} damage, Horde takes {hordeDamageTaken} damage");

        PlayerMonsterTakesDamage(playerMonster, playerDamageTaken, index);
        HordeMonsterTakesDamage(hordeMonster, hordeDamageTaken, index);

        yield return new WaitForSeconds(1f);

    }

    IEnumerator PlayerDealLifeforce(GameObject playerMonster, GameObject hordeMonster, int index)
    {
        CardDetails playerMonsterStats = playerMonster.GetComponent<CardDetails>();
        int damage = playerMonsterStats.cardPowerInt;

        turnMGR.hordeLifeforce -= damage;
        turnMGR.UpdateLifeforceUI();

        yield return new WaitForSeconds(1f);
    }

    IEnumerator HordeDealLifeforce(GameObject playerMonster, GameObject hordeMonster, int index)
    {
        CardDetails hordeMonsterStats = hordeMonster.GetComponent<CardDetails>();
        int damage = hordeMonsterStats.cardPowerInt;

        turnMGR.playerLifeforce -= damage;
        turnMGR.UpdateLifeforceUI();

        yield return new WaitForSeconds(1f);
    }

    public void HordeCardToGraveyard(GameObject hordeMonster)
    {

        RectTransform hordeGraveyard = GameObject.Find("HordeGraveyard").GetComponent<RectTransform>();

        hordeMonster.transform.SetParent(hordeGraveyard); //Set Graveyard as Parent
        hordeMonster.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //move to graveyard

        //reset stats
        //change state to horde graveyard
        graveyardMGR.hordeGraveyard.Add(hordeMonster); //add to graveyard list
        Debug.Log("Horde monster dies");
    }

    public void PlayerMonsterToGraveYard(GameObject playerMonster, int index)
    {
        Debug.Log("Player monster dies");
        RectTransform playerGraveyard = GameObject.Find("PlayerGraveyard").GetComponent<RectTransform>();

        playerMonster.transform.SetParent(playerGraveyard); //Set Graveyard as Parent
        playerMonster.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //move to graveyard
        //playerMonsters.Remove(playerMonster);//remove from list
        playerMonster.GetComponent<CardDetails>();
        playerMonsters[index] = null;

        //set state
        //reset stats
        //change state to player graveyard
        graveyardMGR.playerGraveyard.Add(playerMonster); //add to graveyard list
    }

    public void PlayerMonsterTakesDamage(GameObject playerMonster, int damage, int index)
    {
        CardDetails playerMonsterStats = playerMonster.GetComponent<CardDetails>();

        Debug.Log($"Initial Player Monster: {playerMonsterStats.cardName.text} Life: {playerMonsterStats.cardLifeInt}");
        Debug.Log($"Player Monster: {playerMonsterStats.cardName.text} takes {damage} damage");

        //Tough
        if (playerMonsterStats.cardBuffTough > 0)
        {
            damage -= 1;
            Debug.Log($"Player {playerMonsterStats.cardName.text} has toughness and reduces the damage to {damage}");
        }

        //Divine Shield
        if (playerMonsterStats.cardBuffDivine > 0)
        {
            damage = 0;
            playerMonsterStats.cardBuffDivine = 0;
            Debug.Log($"Player {playerMonsterStats.cardName.text} has divine shield and reduces the damage to {damage}");
        }

        playerMonsterStats.cardLifeInt -= damage;

        Debug.Log($"Post-damage Player Monster: {playerMonsterStats.cardName.text} Life: {playerMonsterStats.cardLifeInt}");

        playerMonster.GetComponent<CardDetails>().ChangeCardUI();

        if (playerMonsterStats.cardLifeInt <= 0)
        {
            PlayerMonsterToGraveYard(playerMonster, index);
        }

        Debug.Log($"Player Monster: {playerMonsterStats.cardName.text} Life: {playerMonsterStats.cardLifeInt}");
    }


    public void HordeMonsterTakesDamage(GameObject hordeMonster, int damage, int index)
    {
        CardDetails hordeMonsterStats = hordeMonster.GetComponent<CardDetails>();

        Debug.Log($"Initial Horde Monster: {hordeMonsterStats.cardName.text} Life: {hordeMonsterStats.cardLifeInt}");
        Debug.Log($"Horde Monster: {hordeMonsterStats.cardName.text} takes {damage} damage");

        //Tough
        if (hordeMonsterStats.cardBuffTough > 0)
        {
            damage -= 1;
            Debug.Log($"Horde {hordeMonsterStats.cardName.text} has toughness and reduces the damage to {damage}");
        }

        //Divine Shield
        if (hordeMonsterStats.cardBuffDivine > 0)
        {
            damage = 0;
            hordeMonsterStats.cardBuffDivine = 0;
            Debug.Log($"Horde {hordeMonsterStats.cardName.text} has divine shield and reduces the damage to {damage}");
        }

        hordeMonsterStats.cardLifeInt -= damage;

        Debug.Log($"Post-damage Horde Monster: {hordeMonsterStats.cardName.text} Life: {hordeMonsterStats.cardLifeInt}");

        hordeMonster.GetComponent<CardDetails>().ChangeCardUI();

        if (hordeMonsterStats.cardLifeInt <= 0)
        {
            hordeMonsters[index] = null;  //remove from array
            HordeCardToGraveyard(hordeMonster);
        }

        Debug.Log($"Horde Monster: {hordeMonsterStats.cardName.text} Life: {hordeMonsterStats.cardLifeInt}");
    }

}
