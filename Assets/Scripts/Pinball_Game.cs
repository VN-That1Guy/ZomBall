using UnityEngine;

public class Pinball_Game : MonoBehaviour
{
    static public Pinball_Game S;

    public bool isActive = true;
    [SerializeField] private GameObject launcher;
    [SerializeField] private GameObject plunger;
    [SerializeField] private GameObject flipperLeft;
    [SerializeField] private GameObject flipperRight;
    [SerializeField] private GameObject Pinball;

    public GameObject currPinball { get; private set; }
    private HingeJoint flipperHingeL;
    private HingeJoint flipperHingeR;
    private JointSpring flipperSpringL;
    private JointSpring flipperSpringR;
    private float flipperRestAngle = 0f;
    private float flipperAngle = 75f;
    private float hitStrength = 10000f;
    private float flipperDamper = 150f;
    private bool flipperInputL = false;
    private bool flipperInputR = false;

    private Launcher launcherSpring;
    private Plunger plungerSpring;
    
    private bool plungerPull = false;

    private Vector3 pinballRespawnPos;



    private void Start()
    {
        S = this;

        flipperHingeL = flipperLeft.GetComponent<HingeJoint>();
        flipperHingeR = flipperRight.GetComponent<HingeJoint>();

        flipperSpringL = new JointSpring();
        flipperSpringR = new JointSpring();

        flipperSpringL.spring = hitStrength;
        flipperSpringL.damper = flipperDamper;

        flipperSpringR.spring = hitStrength;
        flipperSpringR.damper = flipperDamper;

        flipperHingeL.spring = flipperSpringL;
        flipperHingeR.spring = flipperSpringR;

        launcherSpring = launcher.GetComponent<Launcher>();
        plungerSpring = plunger.GetComponent<Plunger>();

        currPinball = GameObject.Find("Pinball");
        pinballRespawnPos = currPinball.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        //flipperInputL = Input.GetKeyDown(KeyCode.Mouse0) ? true : false;

        //flipperInputR = Input.GetKeyDown(KeyCode.Mouse1) ? true : false;
        if (Input.GetKeyDown(KeyCode.A))
        {
            flipperInputL = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            flipperInputL = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            flipperInputR = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            flipperInputR = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            plungerPull = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            plungerPull = false;
            launcherSpring.LetGo();
            plungerSpring.LetGo();
        }
    }

    void FixedUpdate()
    {
        if (flipperInputL)
        {
            flipperSpringL.targetPosition = -flipperAngle;
            //flipperLeft.transform.Rotate(0f, -(45f * Time.deltaTime) * flipperSpeed, 0);
        }
        else
        {
            flipperSpringL.targetPosition = flipperRestAngle;
        }

        if (flipperInputR)
        {
            flipperSpringR.targetPosition = flipperAngle;
            //flipperRight.transform.Rotate(0f, (45f * Time.deltaTime) * flipperSpeed, 0);
        }
        else
        {
            flipperSpringR.targetPosition = flipperRestAngle;
        }

        flipperHingeL.spring = flipperSpringL;
        flipperHingeR.spring = flipperSpringR;

        if (plungerPull)
        {
            launcherSpring.Pull();
            plungerSpring.Pull();
        }
    }

    public void DelayRespawn()
    {
        Invoke(nameof(Respawn), 3);
    }

    private void Respawn()
    {
        currPinball = Instantiate<GameObject>(Pinball, pinballRespawnPos, Quaternion.identity);
    }
}
