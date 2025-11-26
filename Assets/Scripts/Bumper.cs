using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private int _score = 10;
    public int score { get { player.wallet.AddPoints(_score); return _score; } }
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
