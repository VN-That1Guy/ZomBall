using UnityEngine;

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

public class Weapon_Fire : MonoBehaviour, IFire
{
    [Header("Inscribed")]
    [field:SerializeField] public float damage { get; set; } = 0f;
    [field:SerializeField] public int ammoPerShot { get; set; } = 0;
    [field:SerializeField] public int bulletsPerShot { get; set; } = 1;
    [field: SerializeField] public float fireRate { get; set; } = 0.1f;
    [field:SerializeField] public float spread { get; set; } = 0f;
    [field:SerializeField] public bool singleFire { get; set; } = true;

    [field: SerializeField] public DamType damageType { get; set; } = new();

    [SerializeField] protected Transform firingPoint;

    [Header("Dynamic")]
    [SerializeField] protected bool firing;
    [SerializeField] protected float lastFireTime;
    [SerializeField] protected float refireTime;
    [SerializeField] protected Player player;
    [SerializeField] protected BaseWeapon weapon;

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

        return false;
    }

    virtual public void Fire()
    {
        if (!AllowFire()) return;
        firing = true;

        ConsumeRound();

        // Do all the visuals and stuff here
        DoTrace();
    }

    virtual public void StopFiring()
    {
        if (firing)
            firing = false;
    }

    virtual public void ConsumeRound()
    {

    }

    virtual public void DoTrace()
    {
        // Raycast stuff here
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = this.gameObject.transform.root.GetComponent<Player>();
        weapon = GetComponent<BaseWeapon>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
