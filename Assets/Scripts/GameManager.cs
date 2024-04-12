using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform[] carrotSpawnPos;
    public GameObject cannonEffect;
    public GameObject[] weapons;
    private List<int> availableIndices = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnCarrots();
        StartCoroutine(ActivateRandomWeapons());
    }

    private void Update()
    {
        
    }

    public void SpawnCarrots()
    {
        CarrotSpawner(ObjectPooler.Prefabs.carrot, carrotSpawnPos[Random.Range(0, carrotSpawnPos.Length)]);
    }

    private void CarrotSpawner(ObjectPooler.Prefabs name, Transform pos)
    {
        GameObject obj = ObjectPooler.instance.GetObject(name);
        obj.SetActive(true);
        obj.transform.position = pos.position;
    }

    public void CannonEffect(Transform pos)
    {
        Destroy(Instantiate(cannonEffect, pos.position-new Vector3(0,0.15f,0),Quaternion.identity,transform), 1f);
    }

    private IEnumerator ActivateRandomWeapons()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, weapons.Length);
            yield return new WaitForSeconds(20);
            if (!availableIndices.Contains(randomIndex))
            {
                availableIndices.Add(randomIndex);
                weapons[randomIndex].SetActive(true);
            }
        }
    }
}
