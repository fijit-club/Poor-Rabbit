using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private List<GameObjects> objectPrefabs;
    [SerializeField] private int objectCount;
    [SerializeField] private Transform objectParent;

    private List<List<GameObject>> objectPools;

    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
        InitializeObjectPools();
    }

    private void InitializeObjectPools()
    {
        objectPools = new List<List<GameObject>>();

        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            List<GameObject> objectPool = new List<GameObject>();
            for (int j = 0; j < objectCount; j++)
            {
                GameObject obj = Instantiate(objectPrefabs[i].prefab, objectParent);
                obj.SetActive(false);
                objectPool.Add(obj);
            }
            objectPools.Add(objectPool);
        }
    }

    public GameObject GetObject(Prefabs objectType)
    {

        List<GameObject> objectPool = objectPools[GetPrefabIndex(objectType)];

        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null;
    }

    private int GetPrefabIndex(Prefabs name)
    {
        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            if (objectPrefabs[i].name == name)
            {
                return i;
            }
        }
        Debug.LogError("Prefab " + name + " not found!");
        return -1;
    }

    public enum Prefabs
    {
        carrot,
        arrow,
        cannon
    }

    [Serializable]
    public class GameObjects
    {
        public Prefabs name;
        public GameObject prefab;
    }
}
