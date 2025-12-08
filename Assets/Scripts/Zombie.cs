using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Zombie - An enemy that walks towards the players. Slowly.

public enum ZombieState
{
    Idle,
    Stunned,
    Moving,
    Attacking,
    Stumbling,
    Dead
}

public class Zombie : MonoBehaviour, IDamageable
{
    private BlinkColorOnHit blnk;


    [Header("Inscribed")]
    [field:SerializeField] public float maxHealth { get; set; } = 100f;
    [SerializeField] private GameObject head;
    public float speed = 1f;
    public int defaultMaxDotCount = 5;

    [Header("Dynamic")]
    public Transform initialTarget;
    [field:SerializeField] public float health { get; set; }
    public ZombieState state = ZombieState.Idle; 

    // Damage Over Time stuff
    [SerializeField] private bool dotActive = false;
    [SerializeField] private float dotInterval = 1f;
    [SerializeField] private int dotCount = 0; // How many times have the damage over time triggered
    [SerializeField] private int maxDotCount; // Max amount of times that the damage over time can trigger

    [SerializeField] private bool isHeadGone = false;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 destinationPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        blnk = GetComponent<BlinkColorOnHit>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        initialTarget = GameObject.Find("Barricade").transform;
        if (initialTarget != null)
        {
            Target = initialTarget;
            Bounds bounds = Target.gameObject.GetComponent<Collider>().bounds;
            Vector3 initDestination = Utils.GetRandomPointInBounds(bounds);
            initDestination.y = 0;
            destinationPoint = Target.position + initDestination;
            agent.SetDestination(destinationPoint);
            Invoke(nameof(Think), 0.1f);
        }
    }

    // What should this Zombie do when they are thinking in certain states
    void Think()
    {
        if (agent == null || !agent.enabled) return;

        switch(state)
        {
            case ZombieState.Dead:
                agent.destination = this.gameObject.transform.position;
                agent.velocity = Vector3.zero;
                agent.enabled = false;
                return;
            case ZombieState.Moving:
                // Make a check here and call upon a method to attack said target
                if (Vector3.Distance(agent.transform.position, Target.position) < 2f)
                {
                    agent.isStopped = true;
                    // Attack!
                }
                break;
            default:
                break;
        }

        if (Target.gameObject == null) // The zombies are trying to break the barricade down (Which, at the moment does not have health yet). When the barricade is gone, they will now progress to the player. Currently, they can't go to the player since the barricade can't be destroyed.
        { 
            Target = FindFirstObjectByType<Player>().gameObject.transform;
            destinationPoint = Target.position;
        }

        if (isHeadGone) // Head is gone, forget about the target and walk around aimlessly
        {
            destinationPoint =  agent.transform.position + -(Random.insideUnitSphere * 2f);
            destinationPoint.y = agent.transform.position.y;
            agent.SetDestination(destinationPoint);
            Invoke(nameof(Think), 1f);
            return;
        }

        agent.SetDestination(destinationPoint);
        Invoke(nameof(Think), 0.25f); // Think again every .25 seconds
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ZombieState.Dead) return;

        if (!isHeadGone && head == null) // The zombie lost it's head! Start bleed out until death.
        {
            isHeadGone = true;
            maxDotCount = 999;
            ApplyDamageOverTime(10);
        }

        if ((agent.velocity.x != 0 || agent.velocity.y != 0) && (state != ZombieState.Attacking || state != ZombieState.Stumbling)) 
        {
            state = ZombieState.Moving;
        }
    }

    public void Damage(float damage, DamType DamType = null)
    {
        if (state == ZombieState.Dead) return; // Do not execute the code below if already dead.

        blnk.SetColors();

        health -= damage;

        if (health <= 0)
        {
            state = ZombieState.Dead;
            Destroy(this.gameObject,5f);
            gameObject.AddComponent<Rigidbody>();
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).AddComponent<Rigidbody>();
                Destroy(gameObject.transform.GetChild(i).gameObject, 4f);
            }
            gameObject.transform.DetachChildren();
            StopAllCoroutines();
            return;
        }

        if (DamType != null && DamType.damageOverTime && !dotActive)
        {
            maxDotCount = DamType.maxDotCount > defaultMaxDotCount ? DamType.maxDotCount : defaultMaxDotCount;
            ApplyDamageOverTime(DamType.damageOverTimeDamage);
        }
    }   
    
    public void ApplyDamageOverTime(float damage)
    {
        if (dotActive) return;
        StartCoroutine(DoT(damage));
    }

    private IEnumerator DoT(float damage)
    {
        dotActive = true;
        while (dotCount < maxDotCount)
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
