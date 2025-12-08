using System.Collections;
using UnityEngine;

public class Zomball_GameManager : MonoBehaviour
{
    static public Zomball_GameManager S;

    [Header("Inscribed")]
    [SerializeField] private int defaultLives = 5; // Lives system: used but not implemented
    [SerializeField] private Transform cameraPinballMachineView;
    [SerializeField] private Transform cameraBehindView;
    [SerializeField] private float cameraViewTransitionSpeed = 3f;

    [Header("Dynamic")]
    [SerializeField] private Camera cam;
    [SerializeField] private bool lookingForward = true;
    [SerializeField] private bool camTransition = false;
    [SerializeField] private Pinball_Game pinballGame;
    [SerializeField] private Player player;
    static public int LIVES = 0; // Lives system: used but not implemented

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        S = this;
        LIVES = defaultLives;
        cam = Camera.main;
        player = FindFirstObjectByType<Player>();
        pinballGame = FindFirstObjectByType<Pinball_Game>();
    }

    // Look away from or towards the pinball machine, switch "controls" during the process.
    // Is called when the mouse hovers over the "Turn Around" button once. Does not require to be clicked, only that the cursor enters the button's viscinity.
    public void TurnAround()
    {
        if (camTransition || player.transitioning) return; // Can't call upon this if the camera is already trying to transition.

        lookingForward = !lookingForward;

        switch (lookingForward)
        {
            case false:
                pinballGame.GoAwayFromMachine();
                StartCoroutine(CameraTransition(cameraPinballMachineView, cameraBehindView));
                break;
            case true:
                pinballGame.GoToPinballMachine();
                StartCoroutine(CameraTransition(cameraBehindView, cameraPinballMachineView, true));
                break;
        }
        player.GunActive(!lookingForward);
        
    }

    private IEnumerator CameraTransition(Transform a, Transform b, bool leftOrRight = false)
    {
        // Here I want the camera to circle around to the other point instead of linearly transition to the point
        // I just learned about SLerp this week (Spherical Linear Interpolation) so it should do that in theory.
        // Code is mostly taken from the documentation page of the function (https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Vector3.Slerp.html)

        // Center of the arc
        Vector3 center = (a.position + b.position) / 2;
        Vector3 direction = leftOrRight ? Vector3.left : Vector3.right;
        Vector3 rotDirection = leftOrRight ? Vector3.up * 180 : Vector3.down * 180;

        // Offset the center to the side to make the arc horizontal
        // leftOrRight - false for left, true for right
        center.x -= direction.x;
        
        // Interpolate over the arc relative to center
        Vector3 riseRelCenter = a.position - center;
        Vector3 setRelCenter = b.position - center;

        // Interpolate Rotation?

        // Local Time variable here to use since this is not in the update method
        float time = 0f;
        camTransition = true;

        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        while (time <= 1.02f)
        {
            // Timestep
            time += Time.deltaTime * cameraViewTransitionSpeed;

            cam.transform.SetPositionAndRotation(Vector3.Slerp(riseRelCenter, setRelCenter, time) + center, Quaternion.Euler(Vector3.Lerp(a.rotation.eulerAngles + direction, b.rotation.eulerAngles, time) ) );
            yield return null;
        }

        camTransition = false;
        yield break;
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
