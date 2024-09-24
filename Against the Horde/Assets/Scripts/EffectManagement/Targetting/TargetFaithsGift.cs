using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // For checking UI interactions

public class TargetFaithsGift : MonoBehaviour
{
    public Texture2D targetCursorTexture;
    private bool isTargeting = false;
    private EfFaithsGift FaithsGiftEffect;
    public Camera mainCamera;

    void Update()
    {
        // If targeting is active, handle the input
        if (isTargeting)
        {
            Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);

            if (Input.GetMouseButtonDown(0)) // On left click
            {
                // Create a ray from the camera through the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Use GetRayIntersection for 2D physics
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null)
                {
                    Debug.Log("Raycast2D hit: " + hit.collider.gameObject.name);
                    GameObject hitObject = hit.collider.gameObject;
                    CardDetails cardDetails = hitObject.GetComponent<CardDetails>();

                    if (cardDetails != null && cardDetails.cardType == CardType.Monster)
                    {
                        Debug.Log("Monster found, applying effect.");
                        FaithsGiftEffect.ApplyEffect(cardDetails); // Apply effect via ScriptableObject
                        EndTargeting();
                    }
                    else
                    {
                        Debug.Log("CardDetails not found or not a Monster.");
                    }
                }
                else
                {
                    Debug.Log("No collider hit by GetRayIntersection.");
                }
            }
        }
    }

    public void StartTargeting(EfFaithsGift effect)
    {
        isTargeting = true;
        FaithsGiftEffect = effect;
        Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void EndTargeting()
    {
        isTargeting = false;
        FaithsGiftEffect = null;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Reset cursor
    }
}
