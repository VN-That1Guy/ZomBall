using UnityEngine;

public class Trigger_RemovePinball : Trigger
{
    override protected void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Pinball"))
        {
            Destroy(collision.gameObject);
            Zomball_GameManager.LoseLife();
            //if (Zomball_GameManager.LIVES <= 0) return; // Uncomment when game is fully coded
            Pinball_Game.S.DelayRespawn();
        }
    }
}
