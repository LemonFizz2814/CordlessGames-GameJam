using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objecttoPool;
    public int amounttoPool;

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
}
