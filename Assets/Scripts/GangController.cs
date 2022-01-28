using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangController : MonoBehaviour
{
    public enum Gangs
    {
        Cats,
        Dogs,
    }

    public class AIPool
    {
        public Gangs gang;

        public GameObject prefab;

        public List<GameObject> aiPoolUsed;
        public List<GameObject> aiPoolWaiting;

        public AIPool(Gangs _gang, GameObject _prefab, List<GameObject> _aiPoolUsed, List<GameObject> _aiPoolWaiting)
        {
            gang = _gang;
            prefab = _prefab;
            aiPoolUsed = _aiPoolUsed;
            aiPoolWaiting = _aiPoolWaiting;
        }
    }

    public GameObject catPrefab;
    public GameObject dogPrefab;

    public Transform poolParent;

    AIPool catPool;
    AIPool dogPool;

    List<AIPool> pooling;

    public int totalAIPerGang;

    private void Start()
    {
        catPool = new AIPool(Gangs.Cats, catPrefab, new List<GameObject>(), new List<GameObject>());
        dogPool = new AIPool(Gangs.Dogs, dogPrefab, new List<GameObject>(), new List<GameObject>());

        pooling.Add(catPool);
        pooling.Add(dogPool);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < totalAIPerGang; j++)
            {
                var aiObj = Instantiate(pooling[i].prefab, new Vector3(0, 0, 0), Quaternion.identity);
                aiObj.transform.SetParent(poolParent);
                pooling[i].aiPoolWaiting.Add(aiObj);
            }
        }
    }
}
