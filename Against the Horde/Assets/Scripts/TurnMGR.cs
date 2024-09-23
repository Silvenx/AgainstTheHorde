using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnMGR : MonoBehaviour
{

    public Button endTurnButton;
    public PlayerDeckMGR playerDeckMGR;
    public int currentEnergy;
    public int maxEnergy; //The total starting energy for a turn
    public int energyCap; //The cap of Energy overall
    public int turnCounter;
    public HordeDeckMGR hordeDeckMGR;
    public CombatMGR combatMGR;
    public int playerLifeforce;
    public int hordeLifeforce;
    public TMP_Text playerLifeforceText;
    public TMP_Text hordeLifeforceText;
    public TMP_Text playerCurrentEnergy;
    public TMP_Text playerMaxEnergy;

    void Start()
    {
        Debug.Log("Start Phase Start");
        endTurnButton.onClick.AddListener(EndTurnPress);
        Debug.Log("Setting energy to 1 and cap to 10 and turn counter to 1");
        currentEnergy = 1;
        maxEnergy = 1;
        energyCap = 10;
        turnCounter = 1;
        Debug.Log($"Current Energy: " + currentEnergy + " | Max Energy: " + maxEnergy + " | Turn Counter: " + turnCounter);
        playerLifeforce = 30; //need to change these later
        hordeLifeforce = 30; //need to change these later
        UpdateLifeforceUI();
        UpdateEnergyUI();
        Debug.Log($"Player Lifeforce: " + playerLifeforce + " | Horde Lifeforce: " + hordeLifeforce);

        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        Debug.Log("Start Phase Begin Game");
        yield return new WaitForSeconds(1f);
        FirstTurnPhase();
    }

    void FirstTurnPhase()
    {
        Debug.Log("Start Phase First Turn");

        //Player first draw
        for (int i = 0; i < 5; i++)
        {
            playerDeckMGR.DrawCard();
        }

        hordeDeckMGR.HordeDrawCard();
        hordeDeckMGR.PrintHordeDeckOrder(); // Print deck order after shuffling
    }

    void StartPhase()
    {
        Debug.Log("Start Phase Start Phase");
        //Turn start effects here
        GainMaxEnergy();
        currentEnergy = maxEnergy; //reset energy to the turn max
        UpdateEnergyUI();
        turnCounter += 1;
        playerDeckMGR.DrawCard();
        hordeDeckMGR.HordeDrawCard();
        Debug.Log($"Current Energy: " + currentEnergy + " | Max Energy: " + maxEnergy + " | Turn Counter: " + turnCounter);
        hordeDeckMGR.PrintHordeDeckOrder(); // Print deck order after shuffling
    }

    void EndTurnPress()
    {
        //button effects here?
        CombatPhase();
    }

    void CombatPhase()
    {
        combatMGR.StartCombat();
        //EndPhase(); //called once the Combat phase ends
    }

    public void EndPhase()
    {
        //Turn effect here
        StartPhase();
    }



    void GainMaxEnergy()
    {
        if (maxEnergy < energyCap)
        {
            maxEnergy += 1;
            UpdateEnergyUI();
        }

    }

    void GainCurrentEnergy()
    {
        if (currentEnergy < energyCap)
        {
            currentEnergy += 1;
            UpdateEnergyUI();
        }
    }

    public void UpdateLifeforceUI()
    {

        playerLifeforceText.text = playerLifeforce.ToString();
        hordeLifeforceText.text = hordeLifeforce.ToString();

        // Check for game over conditions
        if (playerLifeforce <= 0)
        {
            Debug.Log("Player has been defeated!");
            // Implement game over logic for player defeat
        }

        if (hordeLifeforce <= 0)
        {
            Debug.Log("Horde has been defeated!");
            // Implement game over logic for horde defeat
        }

    }
    public void UpdateEnergyUI()
    {
        playerCurrentEnergy.text = currentEnergy.ToString();
        playerMaxEnergy.text = maxEnergy.ToString();
    }



}
