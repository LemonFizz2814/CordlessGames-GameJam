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
    public GameObject arrowObj;

    public Transform[] bossLocations;

    //Movement
    public float moveLimiter = 0.7f;    //Percentage
    public float runSpeed = 20.0f;

    int gangArrowPoint;

    bool milk;
    bool bone;
    bool milkDelivered;
    bool boneDelivered;
    bool arrow;

    float health = 3;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        ShowArrow(false, 0);
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

        if(arrow)
        {
            Vector3 pos = new Vector3(bossLocations[gangArrowPoint].position.x, 0, bossLocations[gangArrowPoint].position.z);
            arrowObj.transform.LookAt(pos);
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
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    public void ShowArrow(bool _show, int _boss)
    {
        arrowObj.SetActive(_show);
        arrow = _show;
        gangArrowPoint = _boss;
    }

    public bool GetMilk()
    {
        return milk;
    }
    public bool GetBone()
    {
        return bone;
    }
    public void HitByBullet(float _damage)
    {
        health -= _damage;
        uiManager.UpdateHeartImages(health);

        HealthCheck();
    }

    void HealthCheck()
    {
        if (health <= 0)
        {
            uiManager.ShowGameOverScreen();
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Milk") && !milk && !bone)
        {
            milk = true;
            uiManager.PickedUpImage(1);
            uiManager.TextAnimation("Picked up milk");
            ShowArrow(true, 0);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Bone") && !milk && !bone)
        {
            bone = true;
            uiManager.PickedUpImage(2);
            uiManager.TextAnimation("Picked up bone");
            ShowArrow(true, 1);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("CatBoss") && milk)
        {
            uiManager.TextAnimation("Delivered milk to cat boss!");
            uiManager.PickedUpImage(0);
            milk = false;
            milkDelivered = true;
            ShowArrow(false, 0);
        }
        if (other.CompareTag("DogBoss") && bone)
        {
            uiManager.TextAnimation("Delivered bone to dog boss!");
            uiManager.PickedUpImage(0);
            bone = false;
            boneDelivered = true;
            ShowArrow(false, 0);
        }
        if (other.CompareTag("Start") && milkDelivered && boneDelivered)
        {
            uiManager.ShowWinScreen();
            Time.timeScale = 0;
        }
        if (other.CompareTag("Bullet"))
        {
            HitByBullet(1);
        }
    }
}
