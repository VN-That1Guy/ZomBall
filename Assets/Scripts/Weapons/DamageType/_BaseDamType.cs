using UnityEngine;

/* Damage Type: A scriptable object that holds the property of a specific type of damage. (e.g, how much damage should be modified on hitting weakpoints,
 *  is this damage type the kind that applies damage over time, etc.)
 *  Currently, only one type of weapon exists but more can be made in the future from this script.
*/
[CreateAssetMenu(fileName = "DamType", menuName = "Scriptable Objects/DamType")]
public class DamType : ScriptableObject
{
    public bool canHitWeakPoints = true;
    public float weakPointMult = 1.1f;


    public bool damageOverTime = false;
    public float damageOverTimeDamage = 0f;
    public int maxDotCount = 5;
}
