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

    public class AIData
    {
        public Gangs gang;

        public GameObject prefab;

        public List<GameObject> aiPoolUsed;
        public List<GameObject> aiPoolWaiting;

        public Transform spawnLocations;

        public AIData(Gangs _gang, GameObject _prefab, List<GameObject> _aiPoolUsed, List<GameObject> _aiPoolWaiting, Transform _spawnLocations)
        {
            gang = _gang;
            prefab = _prefab;
            aiPoolUsed = _aiPoolUsed;
            aiPoolWaiting = _aiPoolWaiting;
            spawnLocations = _spawnLocations;
        }
    }

    [Header("References")]
    public GameObject catPrefab;
    public GameObject dogPrefab;

    public Transform catSpawnLocs;
    public Transform dogSpawnLocs;

    public Transform poolParent;

    AIData catPool;
    AIData dogPool;

    List<AIData> aiData = new List<AIData>();

    [Header("AI Spawning Settings")]
    public int totalAIPerGang;
    public float spawnWait;
    public float spawningClusterMin;
    public float spawningClusterMax;

    private void Start()
    {
        catPool = new AIData(Gangs.Cats, catPrefab, new List<GameObject>(), new List<GameObject>(), catSpawnLocs);
        dogPool = new AIData(Gangs.Dogs, dogPrefab, new List<GameObject>(), new List<GameObject>(), dogSpawnLocs);

        aiData.Add(catPool);
        aiData.Add(dogPool);

        for (int gang = 0; gang < 2; gang++)
        {
            for (int j = 0; j < totalAIPerGang; j++)
            {
                var aiObj = Instantiate(aiData[gang].prefab, new Vector3(0, 0, 0), Quaternion.identity);
                aiObj.transform.SetParent(poolParent.GetChild(gang));
                aiObj.transform.localPosition = new Vector3(0, 0, 0);

                aiData[gang].aiPoolWaiting.Add(aiObj);
            }

            aiSpawnLoop(gang);
        }
    }

    private IEnumerator aiSpawnLoop(int _gang)
    {
        yield return new WaitForSeconds(spawnWait);

        float cluster = 0;

        if (aiData[_gang].aiPoolWaiting.Count != 0)
        {
            if (aiData[_gang].aiPoolWaiting.Count < spawningClusterMax)
            {
                cluster = aiData[_gang].aiPoolWaiting.Count;
            }
            else
            { cluster = Random.Range(spawningClusterMin, spawningClusterMax); }
        }

        SpawnInAI(_gang, cluster);

        aiSpawnLoop(_gang);
    }

    void SpawnInAI(int _gang, float _cluster)
    {
        Transform spawnLocation = aiData[_gang].spawnLocations.GetChild(Random.Range(0, aiData[_gang].spawnLocations.childCount));

        for (int i = 0; i < _cluster; i++)
        {
            //remove from waiting and add to 
            var store = aiData[_gang].aiPoolWaiting[0];
            aiData[_gang].aiPoolWaiting.RemoveAt(0);
            aiData[_gang].aiPoolUsed.Add(store);

            //set position
            aiData[_gang].aiPoolUsed[aiData[_gang].aiPoolUsed.Count - 1].transform.localPosition = spawnLocation.position;
        }
    }
}
