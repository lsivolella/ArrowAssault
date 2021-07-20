using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Loot Drop Configuration")]
public class LootDropConfigurationSO : ScriptableObject
{
    [Serializable]
    public class Drop
    {
        [SerializeField] string tag;
        [SerializeField] GameObject itemPrefab;
        [SerializeField] int poolSize;
        [SerializeField] int weight;

        public string Tag { get { return tag; } }
        public GameObject ItemPrefab { get { return itemPrefab; } }
        public int PoolSize { get { return poolSize; } }
        public int Weight { get { return weight; } }
    }

    [SerializeField] List<Drop> drops;

    public List<Drop> Drops { get { return drops; } }
}
