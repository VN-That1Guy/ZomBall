using UnityEngine;

// Bullet Tracer class, only used for visual FX for the weapons.
public class BulletTracer : MonoBehaviour
{
    public float lifeTime = 0.2f;
    private float startTime;
    public float speed { get; private set; } = 200f;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time > startTime + lifeTime)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        gameObject.transform.Translate(Vector3.forward * (Time.deltaTime * speed));
    }
}
