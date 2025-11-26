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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bumper"))
        {
            Vector3 pos = this.gameObject.transform.position;
            Vector3 pos2 = collision.gameObject.transform.position;

            ContactPoint contact = collision.GetContact(0);
            Bumper bumper = collision.gameObject.GetComponentInParent<Bumper>();
            Vector3 norm = contact.normal;
            
            rb.AddForce(/*TODO: Get the value of where the ball should bounce based on impact of collision??*/ /*pos - pos2*/ norm * bumper.force, ForceMode.Impulse);
            //Do something with the score here
            Player.score += bumper.score;
            //player.wallet.AddPoints(bumper.score);
        }
    }
}
