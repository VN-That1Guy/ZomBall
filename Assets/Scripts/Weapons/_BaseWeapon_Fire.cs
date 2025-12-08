using NUnit.Framework.Constraints;
using System.Collections;
using UnityEditor.Timeline;
using UnityEngine;


public class Weapon_Fire : MonoBehaviour, IFire
{
    [Header("Inscribed")]
    [field:SerializeField] public float damage { get; set; } = 0f;
    [field:SerializeField] public int ammoPerShot { get; set; } = 0;
    [field:SerializeField] public int bulletsPerShot { get; set; } = 1;
    [field: SerializeField] public float fireRate { get; set; } = 0.1f;
    [field:SerializeField] public float spread { get; set; } = 0f;
    [field:SerializeField] public bool singleFire { get; set; } = true;

    [field: SerializeField] public DamType damageType { get; set; }

    [SerializeField] protected Transform firingPoint; // Not used yet but will serve as a point where Bullet Tracers spawn from

    public LayerMask layerMask = 64;

    [SerializeField] protected GameObject tracerPrefab;

    [Header("Dynamic")]
    [SerializeField] protected bool firing;
    [SerializeField] protected float lastFireTime;
    [SerializeField] protected float refireTime;
    [SerializeField] protected Player player;
    [SerializeField] protected BaseWeapon weapon; // Not used but serves as a reference to the weapon's ammo capacity

    private Ray ray;
    private RaycastHit hit;

    // Can this weapon fire? Why it won't could be a variety of reasons, like it already fired in a specific time window from the last time it was firing, the player is reloading, the gun does not have ammo, and so on.
    virtual public bool AllowFire()
    {
        lastFireTime = Time.time;

        if (firing && singleFire)
        {
            return false;
        }

        if (lastFireTime > refireTime)
        {
            refireTime = Time.time + fireRate;
            return true;
        }

        // Many more checks to do here when things are more fleshed out

        return false;
    }

    virtual public void Fire()
    {
        if (!AllowFire()) return;
        firing = true;
        lastFireTime = Time.time;

        Debug.Log("Firing!");

        ConsumeRound();

        // Do all the visuals and stuff here
        int i = 0;
        while (i < bulletsPerShot)
        {
            DoTrace();
            i++;
        }

        //if (!singleFire && firing)
        //    Invoke(nameof(Fire), fireRate);
    }

    virtual public void StopFiring()
    {
        if (firing)
            firing = false;
        //CancelInvoke(nameof(Fire));
    }

    virtual public void ConsumeRound()
    {
        // Not Implemented
        // If it was, it'd grab the weapon property and decrement it's ammoCapacity property.
    }

    // Scan from when the player fires the gun pointing at something and check if it's something that can be damaged.
    virtual public void DoTrace()
    {
        // TODO: Set to the player's AimOffset property later on when the UI is more established
        Vector3 Spread = new(Random.Range(-spread, spread), Random.Range(-spread, spread));
        ray = player.cam.ScreenPointToRay(Input.mousePosition + Spread);

        GameObject tracer = Instantiate<GameObject>(tracerPrefab, firingPoint.position, firingPoint.rotation);
        tracer.transform.LookAt(ray.GetPoint(1000f));

        //Vector3 fwd = player.cam.transform.forward;
        //fwd += player.cam.transform.TransformDirection(new(Random.Range(-spread,spread), Random.Range(-spread,spread)));
        //fwd += player.aimPos;
        // Raycast stuff here
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            if (hit.transform.TryGetComponent<IDamageable>(out IDamageable damageActor))
            {
                tracer.transform.LookAt(hit.point);
                BulletTracer bulletTracer = tracer.GetComponent<BulletTracer>();
                bulletTracer.lifeTime = Vector3.Distance(firingPoint.position, hit.transform.position) / bulletTracer.speed;
                Debug.Log("Hit! " + hit.transform.gameObject.name);
                damageActor.Damage(damage, damageType);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = this.gameObject.transform.root.GetComponent<Player>();
        weapon = GetComponent<BaseWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firing)
        {
            if (Time.time > refireTime)
            {
                Fire();
            }
        }
            
    }
}

public interface IFire
{
    public float damage { get; set; }
    public int ammoPerShot { get; set; }
    public int bulletsPerShot { get; set; }
    public float fireRate { get; set; }
    public float spread { get; set; }
    public bool singleFire { get; set; }

    public DamType damageType { get; set; }

    public void Fire();

    public void ConsumeRound();
}