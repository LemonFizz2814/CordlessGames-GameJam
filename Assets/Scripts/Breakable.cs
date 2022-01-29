using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject BrokenObjectPrefab;
    private GameObject BrokenObject;

    public List<Transform> childs = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        BrokenObject = Instantiate(BrokenObjectPrefab, transform.position, transform.rotation);
        BrokenObject.transform.parent = transform;

        BrokenObject.transform.position = transform.position;
        BrokenObject.transform.rotation = transform.rotation;

        BrokenObject.SetActive(false);

        FindEveryChild(gameObject.transform);

        for (int i = 0; i < childs.Count; i++)
        {
            FindEveryChild(childs[i]);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].gameObject.AddComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        BrokenObject.transform.position = transform.position;
        BrokenObject.transform.rotation = transform.rotation;
    }

    public void FindEveryChild(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            childs.Add(parent.GetChild(i));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            BrokenObject.SetActive(false);
        }
    }

}
