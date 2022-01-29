using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 10, Player.transform.position.z);   
    }
}
