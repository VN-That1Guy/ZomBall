using UnityEngine;

public class Trigger_RemovePinball : Trigger
{
    override protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pinball"))
        {
            Zomball_GameManager.LoseLife();
            Destroy(collision.gameObject);
        }
    }
}
