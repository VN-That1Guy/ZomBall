

public interface IDamageable
{
    public float health { get; set; }
    public float maxHealth { get; }

    public void Damage(float damage, DamType DamType = null);
}
