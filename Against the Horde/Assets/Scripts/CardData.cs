using UnityEngine;
using System.Collections.Generic;

public enum CardType
{
    Monster,
    Spell,
    Field,
    Equipment,
    Enchantment
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card Data", order = 1)] //creates the scriptable object in unity to use!


public class CardData : ScriptableObject
{
    public string cardName;  // The name of the card
    public string description;  // The card's description or effect
    public int energy;  // The energy cost to play the card
    public int power;  // The card's power stat
    public int life;  // The card's life stat
    public CardType cardType;  // Type of the card 
    public string cardAttribute;  // Additional attributes or keywords 
    public Sprite cardImage;  // The image displayed on the card
    public int cardBuffTough;
    public int cardBuffDivine;
    public List<Effect> effects;
    public AudioClip cardPlaySound;

}
