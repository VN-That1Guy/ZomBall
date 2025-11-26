using UnityEngine;

public class Player : MonoBehaviour
{
    static private int _score;
    static public int score {
        get {  return _score; }
        set
        {
            _score = value;
            HighScore.TRY_SET_HIGH_SCORE(_score);
            return; 
        }
    }

    public Player_Points wallet = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
