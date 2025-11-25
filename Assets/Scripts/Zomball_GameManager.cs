using UnityEngine;

public class Zomball_GameManager : MonoBehaviour
{
    static public Zomball_GameManager S;
    static public int LIVES = 0;

    [SerializeField] private int defaultLives = 3;
    [SerializeField] private GameObject launcher;
    [SerializeField] private GameObject pulley;
    [SerializeField] private GameObject flipperRight;
    [SerializeField] private GameObject flipperLeft;
    private float flipperSpeed = 1f;
    private float flipperAngle = 45f;
    private bool flipperInputL = false;
    private bool flipperInputR = false;
    private float flipperLeftTime;
    private float flipperRightTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LIVES = defaultLives;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            flipperInputL = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            flipperInputL = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            flipperInputR = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            flipperInputR = false;
        }


    }

    void LateUpdate()
    {
        if (flipperInputR)
        {

        }
        if (flipperInputL)
        {

        }

    }

    static public void LoseLife()
    {
        LIVES--;
    }
    static public void AddLife()
    {
        LIVES++;
    }
}
