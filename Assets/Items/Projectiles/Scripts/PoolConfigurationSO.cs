using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object Pool Configuration")]
public class PoolConfigurationSO : ScriptableObject
{
    [Serializable]
    public class Pool
    {
        [SerializeField] string tag;
        [SerializeField] GameObject objectPrefab;
        [SerializeField] int size;

        public string Tag { get { return tag; } }
        public GameObject ObjectPrefab { get { return objectPrefab; } }
        public int Size { get { return size; } }
    }

    [SerializeField] List<Pool> pools;

    public List<Pool> Pools { get { return pools; } }
}
