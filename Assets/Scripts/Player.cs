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

    public GameObject gunObject; // the GameObject that holds the overall "viewmodel". Used for hiding/showing the gun.
    public BaseWeapon currentWeapon;

    public Transform gunPosGO; // The origin/pivot gameObject of the "viewmodel". Used to control where the gun is looking
    // Note: These should be localized coordinates to the GameObject since the gunObject is a child of the GO that this script is attached to
    public Vector3 gunOnPos = new(0.36f, -0.2f, 0f);
    public Vector3 gunOffPos = new(0.36f, -0.2f, -1.25f);

    // For the weapon "lag" from the cursor position
    public float maxDistFromCursor = 3f;

    public bool gunActive = false;

    // Animation curves to make animation transitions and aim lag smooth
    public AnimationCurve aimLagSmoothCurve = new AnimationCurve();
    public AnimationCurve equipHolsterSmoothCurve = new AnimationCurve();

    [Header("Dynamic")]
    public bool transitioning { get; private set; } = false;
    [SerializeField] private float aimLag = 0f;
    [SerializeField] private float aimEval = 0f;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 currPos;
    [field: SerializeField] public Vector3 aimOff{ get; private set; }
    [field: SerializeField] public Vector3 aimPos { get; private set; }
    [SerializeField] private Vector3 gunRot;
    [field: SerializeField] public Camera cam { get; private set; }
    
    // Not used, still figuring out.
    [SerializeField] private Ray ray;
    [SerializeField] private RaycastHit hit;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        currentWeapon = gunObject.GetComponentInChildren<BaseWeapon>();
        cam = Camera.main;

        gunPosGO.transform.localPosition = gunOffPos; // Set the gun off screen
    }

    // Update is called once per frame
    void Update()
    {
        if (!gunActive) return;

        // Controls
        if (Input.GetMouseButtonDown(0)) currentWeapon.FireWeapon();
        if (Input.GetMouseButtonUp(0)) currentWeapon.StopFiringWeapon();

        // Cursor position
        mousePos = Input.mousePosition;
        mousePos.z += cam.farClipPlane;
        aimPos = cam.ScreenToWorldPoint(mousePos);
        
        // Aim Lag - Used for the gun to "Lag" behind the player's cursor position, weapon firing method calls will rely on aimOff one day.
        aimLag = maxDistFromCursor * currentWeapon.aimLag;
        aimEval = aimLagSmoothCurve.Evaluate(Mathf.Clamp(Mathf.Abs(aimPos.sqrMagnitude - aimOff.sqrMagnitude), 0, aimLag));
        aimOff = Vector3.Lerp(aimOff, aimPos, 1 - ((aimLag - aimEval) / Mathf.Clamp(aimLag, 1, aimLag)));

        // Make the gun look at where the cursor is at
        if (gunPosGO != null && !transitioning)
        {
            gunPosGO.transform.LookAt(aimOff);
            gunRot = gunPosGO.transform.localRotation.eulerAngles;
            gunRot.z = 0; // Do not rotate the Z axis (it looks really weird when looking up and down)
            gunPosGO.transform.localRotation = Quaternion.Euler(gunRot);
        }
    }

    // Handle the gun moving in and off screen when switching views
    public void GunActive(bool active)
    {
        gunActive = active;
        transitioning = true;
        if (gunActive) { StartCoroutine(MoveOnScreen()); }
        else { StartCoroutine(MoveOffScreen()); }
    }

    private void HideGunGO()
    {
        gunObject.SetActive(false);
    }
    private void ShowGunGO()
    {
        gunObject.SetActive(true);
    }

    // Animating the gun Moving On or Off the screen
    private IEnumerator MoveOnScreen()
    {
        CancelInvoke(nameof(HideGunGO));
        ShowGunGO();
        float u = 0;
        float step = 0;
        float eval = 0;
        while (u <= currentWeapon.selectTime)
        {
            step = Time.deltaTime * 1;
            u += step;
            eval = equipHolsterSmoothCurve.Evaluate(u / currentWeapon.selectTime);
            gunPosGO.transform.localPosition = Vector3.Lerp(gunPosGO.transform.localPosition, gunOnPos, eval);
            if (Vector3.Distance(gunPosGO.transform.localPosition, gunOnPos) < 0.0001f)
                break;
            yield return null;
        }
        transitioning = false;
        yield break;
    }
    private IEnumerator MoveOffScreen()
    {
        Invoke(nameof(HideGunGO), 2);
        float u = 0;
        float step = 0;
        float eval = 0;
        while (u <= currentWeapon.holsterTime)
        {
            step = Time.deltaTime * 1;
            u += step;
            eval = equipHolsterSmoothCurve.Evaluate(u / currentWeapon.holsterTime);
            gunPosGO.transform.localPosition = Vector3.Lerp(gunPosGO.transform.localPosition, gunOffPos, eval);
            if (Vector3.Distance(gunPosGO.transform.localPosition, gunOffPos) < 0.0001f)
                break;
            yield return null;
        }
        transitioning = false;
        yield break;
    }

    // Not Implemented: No other weapons than the pistol exist
    // Switch to another weapon when called
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
