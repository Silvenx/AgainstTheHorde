using UnityEngine;

public enum TriggerType
{
    OnDraw,
    OnPlay,
    OnDeath,
    // Add more as needed
}
public abstract class Effect : ScriptableObject
{
    public TriggerType triggerType;
    public abstract void Activate(GameObject cardGameObject);

}




