using UnityEngine;

[RequireComponent(typeof(Weapon_Fire))]
public class BaseWeapon : MonoBehaviour
{
    [Header("Inscribed")]
    public int ammoCapacity = 0;
    public float reloadTime = 0f;

    public float aimLag = 1;
    public float selectTime = 2f;
    public float holsterTime = 2f;

    [Header("Dynamic")]
    [SerializeField] private Weapon_Fire fireClass = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireClass = GetComponent<Weapon_Fire>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireWeapon()
    {
        fireClass.Fire();
    }

    public void StopFiringWeapon()
    {
        fireClass.StopFiring();
    }
}
