using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody body;

    private float horizontal;
    private float vertical;

    public UIManager uiManager;

    public GameObject Gun;

    //Movement
    public float moveLimiter = 0.7f;    //Percentage
    public float runSpeed = 20.0f;

    bool milk;
    bool bone;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        //Shoot Gun
        if (Input.GetMouseButtonDown(0))    //Left Click
        {
            BulletPool.SharedInstance.Shoot(Gun, Gun.transform.forward);
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

        //Rotate to always face the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            transform.LookAt(hit.point);
        }
    }

    public bool GetMilk()
    {
        return milk;
    }
    public bool GetBone()
    {
        return bone;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Milk") && !milk && !bone)
        {
            milk = true;
            uiManager.PickedUpImage(1);
            Destroy(other.gameObject);
        }
        if(other.CompareTag("Bone") && !milk && !bone)
        {
            bone = true;
            uiManager.PickedUpImage(2);
            Destroy(other.gameObject);
        }
        if(other.CompareTag("CatBoss") && milk)
        {
            milk = false;
        }
        if(other.CompareTag("DogBoss") && bone)
        {
            bone = false;
        }
    }
}
