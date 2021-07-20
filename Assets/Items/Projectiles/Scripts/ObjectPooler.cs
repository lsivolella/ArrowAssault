using System.Collections.Generic;
using UnityEngine;
using static PoolConfigurationSO;

public class ObjectPooler
{
    private readonly ObjectPoolerMonoBehaviour objectPooler;

    public ObjectPooler(GameObject caller, PoolConfigurationSO poolConfigSO)
    {
        objectPooler = caller.AddComponent<ObjectPoolerMonoBehaviour>();
        objectPooler.Setup(poolConfigSO);
    }

    public GameObject DequeueObject(string tag)
    {
        var objectDequeued = objectPooler.DequeueObject(tag);
        return objectDequeued;
    }

    public void EnqueueObject(string tag, GameObject objectToEnqueue)
    {
        objectPooler.EnqueueObject(tag, objectToEnqueue);
    }

    public string GetDictionaryTag(int index)
    {
        return objectPooler.tagsList[index];
    }

    public string GenerateRandomTag()
    {
        var randomIndex = Random.Range(0, TagsListSize);

        return objectPooler.tagsList[randomIndex];
    }

    public int TagsListSize { get { return objectPooler.tagsList.Count; } }

    private class ObjectPoolerMonoBehaviour : MonoBehaviour
    {
        private readonly List<Pool> poolConfigSO = new List<Pool>();
        private readonly Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
        private GameObject poolTransformParent;

        public readonly List<string> tagsList = new List<string>();

        public void Setup(PoolConfigurationSO poolConfigSO)
        {
            foreach (var pool in poolConfigSO.Pools)
            {
                this.poolConfigSO.Add(pool);
            }
        }

        private void Start()
        {
            poolTransformParent = new GameObject(gameObject.name + "'s_object_pool");

            InstantiatePoolObjects();
        }

        private void InstantiatePoolObjects()
        {
            foreach (var pool in poolConfigSO)
            {
                Queue<GameObject> objectsPool = new Queue<GameObject>();
                for (int i = 0; i < pool.Size; i++)
                {
                    GameObject newObject = Instantiate(pool.ObjectPrefab);
                    newObject.SetActive(false);
                    newObject.transform.SetParent(poolTransformParent.transform);
                    objectsPool.Enqueue(newObject);
                }
                tagsList.Add(pool.Tag);
                poolDictionary.Add(pool.Tag, objectsPool);
            }
        }

        public GameObject DequeueObject(string tag)
        {
            if (!poolDictionary.ContainsKey(tag)) return null;
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            return objectToSpawn;
        }

        public void EnqueueObject(string tag, GameObject objectToEnqueue)
        {
            poolDictionary[tag].Enqueue(objectToEnqueue);
        }
    }
}