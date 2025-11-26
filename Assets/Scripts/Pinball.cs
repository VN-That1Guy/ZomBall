using UnityEngine;

public class Pinball : MonoBehaviour
{
    [SerializeField] private float weight = 5;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.AddForce(0, -weight, 0);
    }
}
