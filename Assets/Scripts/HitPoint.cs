using Unity.VisualScripting;
using UnityEngine;

// Hitpoint - Can be an extension of a living gameObject to act as a hitbox, or a standalone object that can be broken.
// Uses include: Being a destructable prop in the background, a removable limb, a damage resistant hitpoint, and so on.
public class HitPoint : MonoBehaviour, IDamageable
{
    [Header("Inscribed")]
    [field: SerializeField]public float maxHealth { get; set; } = 50f;

    public bool isWeakpoint = false;

    [Tooltip("If hit, how much of the base damage taken is modified to do more or less damage?  1 = No Modification, <1 = Less Damage, >1 = More Damage")]
    public float damageMultiplier = 1f;

    [Tooltip("Take extra damage based on Percentage of Max HP when this limb is removed (0 - No damage,  1 - Instant Kill)")]
    [SerializeField] [Range(0,1)] private float pctHealthLostOnRemoval = 0f;

    [Tooltip("Is this hitpoint or object of hitpoint removable?")]
    public bool isRemovable = true;

    [Tooltip("Is this an object or simply a collider volume?")]
    public bool isObject = true;

    [Tooltip("If isObject = true, get the set hitParentObject to destroy later when this component detects that the health value is less than or equal to zero")]
    public GameObject hitParentObject;

    [Header("Dynamic")]
    [field: SerializeField] public float health { get; set; }

    private IDamageable parentDamageable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        // Is this a hit point for an existing game object that has it's own health or is this a game object itself that is representing it's own health (NOTE: The root object has to have a IDamageable component)
        if (gameObject.transform.root.gameObject == this.gameObject) return;
        // If this is a hitpoint attached to a gameObject (Like a Zombie), get it's IDamageable component
        parentDamageable = gameObject.transform.root.gameObject.GetComponent<IDamageable>();
    }

    public void Damage(float damage, DamType DamType = null)
    {
        if (health <= 0) return;

        //if (DamType != null)
        //    damage *= (damageMultiplier * DamType.weakPointMult);
        //else
        //    damage *= damageMultiplier;

        if (isWeakpoint) // Is this a weak point, if so, check if the damage type can actually hit weakpoints to decide if it should apply the damage type multiplier on top of the existing one or don't modify at all.
            damage = DamType != null ? damage = DamType.canHitWeakPoints ? damage *= (damageMultiplier * DamType.weakPointMult) : damage : damage *= damageMultiplier;

        if (isRemovable)
            health -= damage;

        if (parentDamageable != null)
            parentDamageable.Damage(damage, DamType);

        if (health <= 0 )
        {
            if (parentDamageable != null)
                parentDamageable.Damage(parentDamageable.maxHealth * pctHealthLostOnRemoval, DamType);
            if (isObject)
                Destroy(gameObject);
            else
            {
                if (hitParentObject != null)
                    Destroy(hitParentObject);
                else
                    Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
