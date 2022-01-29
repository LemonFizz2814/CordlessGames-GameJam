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

        //Shoot Gun
        if (Input.GetMouseButton(0))    //Left Click
        {
            BulletPool.SharedInstance.Shoot(this.gameObject, new Vector3(1, 0, 0));

            //GameObject bullet = BulletPool.SharedInstance.GetBullet();

            //if (bullet != null)
            //{
            //    bullet.transform.position = transform.position;
            //    bullet.transform.rotation = transform.rotation;
            //    bullet.SetActive(true);

            //    VScreen.x = Input.mousePosition.x;
            //    VScreen.y = Input.mousePosition.y;
            //    VScreen.z = PlayerCam.transform.position.z;

            //    //Rotate to always face the mouse
            //    Vector3 Direction = PlayerCam.ScreenToWorldPoint(VScreen);

            //    PhysicalMouse.transform.position = new Vector3(Direction.x, transform.position.y, Direction.z);

            //    transform.LookAt(PhysicalMouse.transform);

            //    bullet.GetComponent<Rigidbody>().AddForce(transform.right * BulletForce, ForceMode.Impulse);
            //}
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
