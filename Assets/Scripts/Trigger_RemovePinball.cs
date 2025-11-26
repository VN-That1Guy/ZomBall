using UnityEngine;

public class Trigger_RemovePinball : Trigger
{
    override protected void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Pinball"))
        {
            Destroy(collision.gameObject);
            Zomball_GameManager.LoseLife();
        }
    }
}
