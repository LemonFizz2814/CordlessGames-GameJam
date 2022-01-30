using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletUnactivate : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DogBoss") && !other.CompareTag("CatBoss") && !other.CompareTag("Milk") && !other.CompareTag("Bone") && !other.CompareTag("Bullet") && !other.CompareTag("Health") && !other.CompareTag("Ammo") && !other.CompareTag("Start"))
        {
            print("triggered");
            gameObject.SetActive(false);
        }
    }
}
