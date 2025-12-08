using UnityEngine;

// Weapon class: Responsible for all the methods related to the properties of the weapon and using it
[RequireComponent(typeof(Weapon_Fire))]
public class BaseWeapon : MonoBehaviour
{
    [Header("WIP - Not Implemented")]
    public int ammoCapacity = 0;
    public float reloadTime = 0f;

    [Header("Inscribed")]
    // These variables will be read from the player class.
    [Tooltip("Aim Lag - How much does the weapon's crosshair lag behind the actual cursor position? (Crosshair not implemented)")]
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
