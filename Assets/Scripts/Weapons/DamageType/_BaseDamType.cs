using UnityEngine;

[CreateAssetMenu(fileName = "DamType", menuName = "Scriptable Objects/DamType")]
public class DamType : ScriptableObject
{
    public float weakPointMult = 1.1f;


    public bool damageOverTime = false;
    public float damageOverTimeDamage = 0f;
    public int maxDotCount = 5;
}
