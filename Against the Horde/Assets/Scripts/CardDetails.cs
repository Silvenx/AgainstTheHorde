using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;



public class CardDetails : MonoBehaviour
{
    public CardData cardData;

    public Image cardImage;
    public Image cardBaseImage;
    //public Image cardBGEnergy; - no longer needed
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardEnergy;
    public TextMeshProUGUI cardPower;
    public TextMeshProUGUI cardLife;
    public TextMeshProUGUI cardTypeText;
    public TextMeshProUGUI cardAttribute;
    public string cardFaction;
    public int cardPowerInt;
    public int cardLifeInt;
    public CardType cardType;
    public int cardBuffTough;
    public int cardBuffDivine;
    public List<Effect> effects;
    public AudioClip cardPlaySound;

    void Start()
    {
        UpdateCardUI();
    }

    public void InitialiseCard
        (
        string cname,
        string cdesc,
        int cenergy,
        int cpower,
        int clife,
        CardType ctype,
        string catt,
        Sprite cimage,
        int cBuffTough,
        int cBuffDivine,
        AudioClip playSound
        )
    {
        cardPowerInt = cpower;
        cardLifeInt = clife;
        cardName.text = cname.ToString();
        cardDescription.text = cdesc;
        cardEnergy.text = cenergy.ToString();
        cardPower.text = cpower.ToString();
        cardLife.text = clife.ToString();
        cardType = ctype;
        cardTypeText.text = ctype.ToString();
        cardAttribute.text = catt;
        cardImage.sprite = cimage;
        cardBuffTough = cBuffTough;
        cardBuffDivine = cBuffDivine;
        effects = cardData.effects;
        cardPlaySound = playSound;

    }

    void UpdateCardUI()
    {
        // Refresh the UI elements to match the current card stats in the database
        cardEnergy.text = cardEnergy.text;
        cardPower.text = cardPower.text;
        cardLife.text = cardLife.text;
        cardName.text = cardName.text;
        cardDescription.text = cardDescription.text;
        cardTypeText.text = cardType.ToString();
        cardAttribute.text = cardAttribute.text;
        cardImage.sprite = cardImage.sprite;
    }

    public void ChangeCardUI()
    {
        cardLife.text = cardLifeInt.ToString();
    }

}
