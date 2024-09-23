using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/EfBlessingOfKings")]
public class EfBlessingOfKings : Effect
{
    //Card Description: Give a monster 3 power and 3 life
    //Card goes to the spell slot location and stays there 
    //Targets hovered monster
    //Give it 3 power 3 life
    //discard

    public int powerIncrease = 3;
    public int lifeIncrease = 3;
    public Transform spellHoldArea;
    public TurnMGR turnMGR;
    public int cardEnergyCost;
    public Texture2D targetCursorTexture;
    private bool isTargeting = false;
    public CardDetails cardDetails;
    private GameObject currentCardGameObject;


    public override void Activate(GameObject cardGameObject)
    {
        //Find the spell slot location, then set it as parent and place it there
        GameObject spellHoldAreaObject = GameObject.Find("SpellHoldArea");
        spellHoldArea = spellHoldAreaObject.transform;
        cardGameObject.transform.SetParent(spellHoldArea, false);
        cardGameObject.transform.position = spellHoldArea.position;

        StartTargeting();

        //turnMGR.currentEnergy = turnMGR.currentEnergy - cardEnergyCost; // take the cost away from current energy
        //turnMGR.UpdateEnergyUI();
    }

    private void StartTargeting()
    {
        isTargeting = true;
        Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (isTargeting && Input.GetMouseButton(0))
        {
            Debug.Log("Click Registered");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("MonsterCard"))
                {
                    Debug.Log("Found Monster Tag, registering effect");
                    ApplyEffect(hitObject);
                    EndTargeting();
                }
            }
        }
    }

    private void ApplyEffect(GameObject targetCard)
    {
        CardDetails stats = targetCard.GetComponent<CardDetails>();
        stats.cardPowerInt += powerIncrease;
        stats.cardLifeInt += lifeIncrease;

        stats.UpdateCardUI();
    }

    private void EndTargeting()
    {
        isTargeting = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Reset the cursor
        //PlayerFieldMGR.PlayerSpellToGraveyard(currentCardGameObject);
    }

}
