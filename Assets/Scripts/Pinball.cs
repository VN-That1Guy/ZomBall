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
        // Add a constant downforce to the pinball, simulating the weight of the object since the mass only affects how much the object move when force is applied, but not how fast it falls.
        // Also, do not continuously apply downforce if the object is already on the ground or something is stopping the object from going down.
        // In short, this line of code makes the object feel less "floaty" like flat paper when it's falling down.
        if (rb.linearVelocity.y >= 1)
            rb.AddForce(0, -weight, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bumper"))
        {
            // Try getting the bumper component in the parent, if it has a simplified collision. I think this also works with objects that aren't parented
            Bumper bumper = collision.gameObject.GetComponentInParent<Bumper>();

            // In case it does not, get it from the collision's game object
            if (bumper == null)
                bumper = collision.gameObject.GetComponent<Bumper>();

            // Get the point of contact and the normal of the surface where the rigid body collided
            ContactPoint contact = collision.GetContact(0);
            Vector3 norm = contact.normal;
            
            // Signal the bumper that we hit it
            bumper.Bump(norm, rb);
        }
    }
}
