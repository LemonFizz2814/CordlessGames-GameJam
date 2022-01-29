using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletUnactivate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DogBoss") && !other.CompareTag("CatBoss") && !other.CompareTag("Milk") && !other.CompareTag("Bone") && !other.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
        }
    }
}
