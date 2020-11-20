using UnityEngine;
using UnityEngine.UI;

public class PoolStick : MonoBehaviour
{
    [SerializeField] GameObject stick;
    [SerializeField] float maxVel = 20f;
    [SerializeField] float velIncrement = 1f;
    public bool canShoot = true;
    bool InMenu = false;
    Slider slider;

    Sphere sphere;
    float velocity = 0;
    float t = 0;
    Vector3 stickStartLocalPos;

    private void Awake()
    {
        sphere = GetComponentInParent<Sphere>();
        slider = FindObjectOfType<Slider>();
        stickStartLocalPos = stick.transform.localPosition;
        slider.maxValue = maxVel;
    }

    private void OnEnable()
    {
        Menu.InMenu.AddListener(SetInMenu);
    }

    private void OnDisable()
    {
        Menu.InMenu.RemoveListener(SetInMenu);
    }

    private void SetInMenu(bool value)
    {
        InMenu = value;
    }


    private void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.y = transform.position.y;

        Debug.DrawLine(transform.position, point);
        transform.LookAt(point, transform.up);

        if(canShoot && !InMenu)
        {
            stick.SetActive(true);
            if(Input.GetMouseButton(0))
            {
                // move stick minus z on hold
                // on release, push forward
                // add force to ball for every tick mouse was hold
                //Using fixedDelta as there was a deltaTime differed between playing in scene and game view
                t += Time.fixedDeltaTime;

                velocity += velIncrement;
                slider.value = velocity;
                if(velocity < slider.maxValue)
                stick.transform.localPosition = Vector3.Lerp(stick.transform.localPosition, (stick.transform.localPosition -= new Vector3(0, 0, velocity/3000)), t);
            }
            if(Input.GetMouseButtonUp(0))
            {

               sphere.Velocity = (transform.forward * Mathf.Min(velocity, maxVel));
                velocity = 0;
                canShoot = false;
                slider.value = 0;
                stick.transform.localPosition = stickStartLocalPos;
                stick.SetActive(false);
                t = 0;
            }
        }
        Vector3 testVel = new Vector3(sphere.Velocity.x, 0, sphere.Velocity.z);
        if (testVel.magnitude < 0.2f)
        {
            canShoot = true;
            sphere.Velocity = Vector3.zero;
        }
    }

}
