using UnityEngine;

public class Launcher : MonoBehaviour
{
    protected Vector3 restPos;
    protected Vector3 launchPos;

    protected float launcherPullPower = 0.25f;
    protected float launcherPower = 2000f;
    protected float letGoTime;
    protected float letGoCurrTime;

    protected bool reachMaxDist = false;
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
        //transform.Translate(0, (-1 * Time.deltaTime) * launcherPullPower, 0, Space.Self);
        letGo = true;
        launchPos = this.transform.position;
        letGoTime = Time.time;
    }
}
