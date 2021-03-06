using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objecttoPool;
    public int amounttoPool;

    //Bullets
    public float BulletForce = 2.0f;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;

        for (int i = 0; i < amounttoPool; i++)
        {
            tmp = Instantiate(objecttoPool);


            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < amounttoPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    public void Shoot(GameObject Parent, Vector3 Direction, string tag)
    {
        GameObject bullet = BulletPool.SharedInstance.GetBullet();

        bullet.transform.tag = tag;

        if (bullet != null)
        {
            bullet.transform.position = Parent.transform.position;
            bullet.transform.rotation = Parent.transform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            bullet.GetComponent<TrailRenderer>().Clear();

            bullet.transform.forward = Direction;

            //bullet.GetComponent<Rigidbody>().AddForce(Direction * BulletForce, ForceMode.Impulse);
            //bullet.transform.forward = bullet.GetComponent<Rigidbody>().velocity;
        }
    }
}
