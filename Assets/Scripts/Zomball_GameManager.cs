using UnityEngine;

public class Zomball_GameManager : MonoBehaviour
{
    static public Zomball_GameManager S;
    static public int LIVES = 0;

    [SerializeField] private int defaultLives = 5;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        S = this;
        LIVES = defaultLives;
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    void LateUpdate()
    {
        

    }

    static public void LoseLife()
    {
        if (LIVES <= 0)
        {

            return;
        }
        LIVES--;
    }
    static public void AddLife()
    {
        LIVES++;
    }
}
