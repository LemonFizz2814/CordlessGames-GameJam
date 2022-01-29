using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody body;

    private float horizontal;
    private float vertical;

    private GameObject PhysicalMouse;

    private Vector3 VScreen = new Vector3();

    //Player's Camera
    public Camera PlayerCam;
    public GameObject Gun;

    //Movement
    public float moveLimiter = 0.7f;    //Percentage
    public float runSpeed = 20.0f;

    void Start()
    {
        body = GetComponent<Rigidbody>();

        PhysicalMouse = new GameObject();
        PhysicalMouse.name = "PhysicalMouse";
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxis("Horizontal"); // -1 is left
        vertical = Input.GetAxis("Vertical"); // -1 is down

        VScreen.x = Input.mousePosition.x;
        VScreen.y = Input.mousePosition.y;
        VScreen.z = PlayerCam.transform.position.z;

        //Rotate to always face the mouse
        Vector3 LookDirection = PlayerCam.ScreenToWorldPoint(VScreen);
        PhysicalMouse.transform.position = new Vector3(LookDirection.x, transform.position.y, LookDirection.z);

        //Shoot Gun
        if (Input.GetMouseButtonDown(0))    //Left Click
        {
            transform.LookAt(PhysicalMouse.transform);

            Vector3 ShootDirection = PhysicalMouse.transform.position - transform.position;
            ShootDirection = Vector3.Normalize(ShootDirection);

            BulletPool.SharedInstance.Shoot(Gun, ShootDirection);
        }
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector3(horizontal * runSpeed, 0, vertical * runSpeed);
    }
}
