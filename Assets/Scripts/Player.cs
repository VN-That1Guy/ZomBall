using System.Collections;
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

    [Header("Inscribed")]
    public Player_Points wallet = new();

    public GameObject gunObject;
    public BaseWeapon currentWeapon;

    public GameObject gunPosGO;
    // Note: These are localized coordinates, not world coordinates
    public Vector3 gunOnPos = new(0.36f, -0.2f, 0f);
    public Vector3 gunOffPos = new(0.36f, -0.2f, -1.25f);

    public float aimOffsetBoundsMod = 20f;

    public bool gunActive = false;

    [SerializeField] protected Keyframe[] aimCurvePoints = new Keyframe[4] {
        new Keyframe(0f, 0),
        new Keyframe(0.25f, 0.5f),
        new Keyframe(0.75f, 0.5f),
        new Keyframe(1f, 1)
    };
    [SerializeField] protected Keyframe[] equipHolsterPoints = new Keyframe[4] {
        new Keyframe(0f, 0),
        new Keyframe(0.25f, 0.5f),
        new Keyframe(0.75f, 0.5f),
        new Keyframe(1f, 1)
    };

    [Header("Dynamic")]
    public AnimationCurve aimLagSmoothCurve = new AnimationCurve();
    public AnimationCurve equipHolsterSmoothCurve = new AnimationCurve();
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 aimOff;
    [SerializeField] private Vector3 gunRot;
    [SerializeField] private Camera cam;
    [SerializeField] private Ray ray;
    [SerializeField] private RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        aimLagSmoothCurve = new AnimationCurve(aimCurvePoints);
        equipHolsterSmoothCurve = new AnimationCurve(equipHolsterPoints);
        currentWeapon = gunObject.GetComponentInChildren<BaseWeapon>();
        cam = Camera.main;

        gunPosGO.transform.localPosition = gunOffPos;

        // Temporarily Set
        GunActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gunActive) return;

        mousePos = Input.mousePosition;
        mousePos.z += cam.farClipPlane;



        if (gunPosGO != null) {
            gunPosGO.transform.LookAt(cam.ScreenToWorldPoint(mousePos));
            gunRot = gunPosGO.transform.localRotation.eulerAngles;
            gunRot.z = 0;
            gunPosGO.transform.localRotation = Quaternion.Euler(gunRot);
        }


    }

    public void GunActive(bool active)
    {
        gunActive = active;

        if (gunActive) { StartCoroutine(MoveOnScreen()); StopCoroutine(MoveOffScreen()); }
        else { StartCoroutine(MoveOffScreen()); StopCoroutine(MoveOnScreen()); }
    }

    private IEnumerator MoveOnScreen()
    {
        float u = 0;
        while (u <= currentWeapon.selectTime)
        {
            gunPosGO.transform.localPosition = Vector3.Lerp(gunPosGO.transform.localPosition, gunOnPos, equipHolsterSmoothCurve.Evaluate(u/currentWeapon.selectTime));
            yield return new WaitForSeconds(0.02f);
            u += 0.02f;
        }
        yield break;
    }

    private IEnumerator MoveOffScreen()
    {
        float u = 0;
        while (u <= currentWeapon.holsterTime)
        {
            gunPosGO.transform.localPosition = Vector3.Lerp(gunPosGO.transform.localPosition, gunOffPos, equipHolsterSmoothCurve.Evaluate(u/currentWeapon.holsterTime));
            yield return new WaitForSeconds(0.05f);
            u += 0.05f;
        }
        yield break;
    }

    private IEnumerator SwitchWeapon(BaseWeapon weapon = null)
    {
        if (weapon != null && weapon != currentWeapon)
        {
            StartCoroutine(MoveOffScreen());
            yield return new WaitForSeconds(currentWeapon.holsterTime);
            StartCoroutine(MoveOnScreen());
        }
        yield break;
    }
}
