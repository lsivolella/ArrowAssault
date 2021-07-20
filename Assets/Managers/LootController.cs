using System.Collections.Generic;
using UnityEngine;

public class LootController
{
    public LootController(EnemyBase caller, LootDropConfigurationSO lootConfigSO)
    {
        LootControllerMonoBehaviour lootController;

        lootController = caller.gameObject.AddComponent<LootControllerMonoBehaviour>();
        lootController.Setup(caller, lootConfigSO);
    }

    private class LootControllerMonoBehaviour : MonoBehaviour
    {
        private readonly Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
        private EnemyBase enemyBase;
        private LootDropConfigurationSO lootConfigSO;
        private GameObject itemTransformParent;
        private int totalWeight = -1;

        public readonly List<string> tagsList = new List<string>();
        private Collectable droppedItem;
        private string droppedItemTag;

        public void Setup(EnemyBase caller, LootDropConfigurationSO lootConfigSO)
        {
            enemyBase = caller;
            enemyBase.onDeath += DrawItem;
            this.lootConfigSO = lootConfigSO;
        }

        private void Start()
        {
            itemTransformParent = new GameObject(gameObject.name + "'s_drop_pool");
            InstantiateItems();
        }

        private void InstantiateItems()
        {
            foreach (var drop in lootConfigSO.Drops)
            {
                Queue<GameObject> itemsPool = new Queue<GameObject>();
                for (int i = 0; i < drop.PoolSize; i++)
                {
                    if (!drop.ItemPrefab)
                        continue;

                    GameObject newItem = Instantiate(drop.ItemPrefab);
                    newItem.SetActive(false);
                    newItem.transform.SetParent(itemTransformParent.transform);
                    itemsPool.Enqueue(newItem);
                }
                tagsList.Add(drop.Tag);
                poolDictionary.Add(drop.Tag, itemsPool);

            }
        }

        public int TotalWeight
        {
            get
            {
                if (totalWeight == -1) { GetTotalWeight(); }
                return totalWeight;
            }
        }

        private void GetTotalWeight()
        {
            foreach (var item in lootConfigSO.Drops)
            {
                totalWeight += item.Weight;
            }
        }

        public void DrawItem()
        {
            int roll = Random.Range(0, TotalWeight);

            for (int i = 0; i < lootConfigSO.Drops.Count; i++)
            {
                roll -= lootConfigSO.Drops[i].Weight;
                if (roll < 0)
                {
                    if (!lootConfigSO.Drops[i].ItemPrefab)
                        break;
                    droppedItem = DequeueItem(lootConfigSO.Drops[i].Tag).GetComponent<Collectable>();
                    droppedItem.ActivateItem(transform.position);
                    droppedItem.onCollected += EnqueueObject;
                    droppedItemTag = lootConfigSO.Drops[i].Tag;
                    break;
                }
            }
        }

        private GameObject DequeueItem(string tag)
        {
            if (!poolDictionary.ContainsKey(tag)) return null;
            GameObject itemToSpawn = poolDictionary[tag].Dequeue();
            return itemToSpawn;
        }

        public void EnqueueObject()
        {
            poolDictionary[droppedItemTag].Enqueue(droppedItem.gameObject);
            droppedItem.onCollected -= EnqueueObject;
        }

        private void OnDisable()
        {
            enemyBase.onDeath -= DrawItem;
        }
    }
}

//public class LootController : MonoBehaviour
//{
//    [SerializeField] LootDropConfigurationSO dropTable;

//    private CharacterBase enemyBase;
//    private int totalWeight = -1;

//    private void Awake()
//    {
//        enemyBase = GetComponent<CharacterBase>();
//        enemyBase.onDeath += DrawItem;
//    }

//    public int TotalWeight
//    {
//        get
//        {
//            if (totalWeight == -1) { GetTotalWeight(); }
//            return totalWeight;
//        }
//    }

//    private void GetTotalWeight()
//    {
//        foreach (var item in dropTable.Drops)
//        {
//            totalWeight += item.Weight;
//        }
//    }

//    public void DrawItem()
//    {
//        int roll = Random.Range(0, TotalWeight);

//        for (int i = 0; i < dropTable.Drops.Count; i++)
//        {
//            roll -= dropTable.Drops[i].Weight;
//            if (roll < 0)
//            {
//                if (!dropTable.Drops[i].Item)
//                    break;
//                Instantiate(dropTable.Drops[i].Item, transform.position, dropTable.Drops[i].Item.transform.rotation);
//                break;
//            }
//        }
//    }

//    private void OnDisable()
//    {
//        enemyBase.onDeath -= DrawItem;
//    }
//}
