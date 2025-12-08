using UnityEngine;

public class Plunger : Launcher // This is just a copy of Launcher that only serves as a visual plunger from outside the pinball machine. The GameObject is named pulley but it's actually a plunger.
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restPos = this.transform.position;
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
            this.gameObject.transform.position = Vector3.Lerp(launchPos,restPos, letGoCurrTime * (launcherPower * Time.fixedDeltaTime));
            if (Vector3.Distance(this.transform.position, restPos) <= 0)
            { 
                letGo = false; 
            }
        }
        reachMaxDist = this.gameObject.transform.position.z < -1 ? true : false;
    }

    void OnTriggerEnter(Collider other) { }

    void OnTriggerExit(Collider other) { }

    override public void Pull()
    {
        if (reachMaxDist) return;

        transform.Translate(0, 0, -(1 * Time.fixedDeltaTime) * launcherPullPower, Space.Self);
    }

    //public void LetGo()
    //{
    //    //transform.Translate(0, (-1 * Time.deltaTime) * launcherPullPower, 0, Space.Self);
    //    letGo = true;
    //    launchPos = this.transform.position;
    //    letGoTime = Time.time;
    //}
}
