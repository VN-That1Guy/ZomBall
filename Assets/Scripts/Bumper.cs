using System;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public int score = 10;
    public float force = 5f;

    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Pinball has hit this object, bounce the pinball away based on the normal of the surface that was hit, ignore any momentum or previous velocity it had
    public void Bump(Vector3 norm, Rigidbody colliderRB)
    {
        colliderRB.AddForce(norm * force, ForceMode.VelocityChange);
        player.wallet.AddPoints(score); // Give player points when this bumper is hit
    }

    // Old code from when I thought this method works when the pinball hits this collision, not the other way around
    /*private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = this.gameObject.transform.position;
        if (collision.collider.gameObject.CompareTag("Pinball"))
        {
            Vector3 pos2 = collision.gameObject.transform.position;
            collision.rigidbody.AddForce(*//*TODO: Get the value of where the ball should bounce based on impact of collision??*//* pos - pos2 * force ,ForceMode.Impulse);
            //Do something with the score here
            Player.score += score;
        }
    }*/

}
