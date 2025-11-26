using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float score = 10f;
    private float force = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = this.gameObject.transform.position;
        if (collision.collider.gameObject.CompareTag("Pinball"))
        {
            Vector3 pos2 = collision.gameObject.transform.position;
            collision.rigidbody.AddForce(/*TODO: Get the value of where the ball should bounce based on impact of collision??*/ pos - pos2 * force ,ForceMode.Impulse);
            //Do something with the score here
        }
    }
}
