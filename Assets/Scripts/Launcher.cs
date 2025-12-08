using UnityEngine;

// A GameObject component made to launch pinballs. Unlike several Unity tutorials I've watched, this one actually utilizes rigidbody collisions/force,
// whereas the the tutorials used UI and script to add force artificially to the pinball.
public class Launcher : MonoBehaviour
{
    protected Vector3 restPos; // Set in the Start method, 
    protected Vector3 launchPos; // Position of the launcher when the player lets go of it

    protected float launcherPullPower = 0.25f; // How fast should the launcher pull
    protected float launcherPower = 2000f; // How fast should the launcher go back to it's rest position, launching the pinball
    
    // Values for storing time and calculating time
    protected float letGoTime;
    protected float letGoCurrTime;

    protected bool reachMaxDist = false; // The max distance distance of this is set in scene via trigger volume with a tag (See OnTrigger methods).
    protected bool letGo = false;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restPos = this.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (letGo)
        {
            letGoCurrTime = Time.time - letGoTime;
            //this.transform.position = Vector3.Lerp(launchPos,restPos, letGoCurrTime * (launcherPower * Time.fixedDeltaTime));
            //transform.Translate(0, (-1 * Time.fixedDeltaTime) * launcherPower, 0, Space.Self);
            //Rigidbody rb = GetComponent<Rigidbody>();
            //rb.MovePosition(restPos * launcherPower);
            rb.MovePosition(Vector3.Lerp(launchPos, restPos, letGoCurrTime * (launcherPower * Time.fixedDeltaTime)));
            if (Vector3.Distance(this.transform.position, restPos) <= 0)
            { 
                letGo = false; 
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Pinball") && rb.linearVelocity.magnitude > 3)
    //    {
    //        collision.rigidbody.AddForce(this.rb.linearVelocity, ForceMode.VelocityChange);
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LauncherStopPoint"))
        {
            reachMaxDist = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LauncherStopPoint"))
        {
            reachMaxDist = false;
        }
    }

    virtual public void Pull()
    {
        if (reachMaxDist) return;

        transform.Translate(0, (1 * Time.fixedDeltaTime) * launcherPullPower, 0, Space.Self);
    }

    virtual public void LetGo()
    {
        letGo = true;
        launchPos = this.transform.position;
        letGoTime = Time.time;
    }
}
