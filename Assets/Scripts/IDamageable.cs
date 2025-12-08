// IDamageable, an interface that contains universal properties and methods for components that has removable health in mind.

public interface IDamageable
{
    public float health { get; set; } // Current Amount of Health
    public float maxHealth { get; } // Max amount of health, should be used to set the health value on Start or Awake.

    /// <summary>
    /// Lose health for the component attached to this game object
    /// </summary>
    /// <param name="damage">Amount of health to remove</param>
    /// <param name="DamType">Damage Type to read certain properties inside of it, like Weakpoint damage multipliers, or having damage over time</param>
    public void Damage(float damage, DamType DamType = null);
}
