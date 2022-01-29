using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletUnactivate : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
