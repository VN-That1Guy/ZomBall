using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour, IDamageable
{
    private BlinkColorOnHit blnk;

    public float health { get; set; }

    [Header("Inscribed")]
    [field:SerializeField]public float maxHealth { get; set; } = 100f;
    [SerializeField] private GameObject head;
    public float speed = 5f;
    public int defaultMaxDotCount = 5;


    [Header("Dynamic")]
    // Damage Over Time stuff
    [SerializeField] private bool dotActive = false;
    [SerializeField] private float dotInterval = 1f;
    [SerializeField] private int dotCount;
    [SerializeField] private int maxDotCount;

    [SerializeField] private bool isHeadGone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        blnk = GetComponent<BlinkColorOnHit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHeadGone && head == null)
        {
            isHeadGone = true;
            maxDotCount = 999;
            // Bleedout until death
            ApplyDamageOverTime(10);
        }
    }

    public void Damage(float damage, DamType DamType = null)
    {
        blnk.SetColors();

        health -= damage;

        if (health <= 0)
        {
            Destroy(this.gameObject);
            StopAllCoroutines();
            return;
        }

        if (DamType != null && DamType.damageOverTime)
        {
            maxDotCount = DamType.maxDotCount > defaultMaxDotCount ? DamType.maxDotCount : defaultMaxDotCount;
            ApplyDamageOverTime(DamType.damageOverTimeDamage);
        }
    }   
    
    public void ApplyDamageOverTime(float damage)
    {
        if (dotActive) return;
        StartCoroutine(DoT());

        IEnumerator DoT()
        {
            dotActive = true;
            while (dotCount > maxDotCount)
            {
                yield return new WaitForSeconds(dotInterval);
                dotCount++;
                Damage(damage);
            }
            dotActive = false;
            dotCount = 0;
            yield break;
        }
    }
}
