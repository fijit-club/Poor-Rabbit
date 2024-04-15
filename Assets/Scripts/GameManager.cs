using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        for(int i=0;i< weapons.Length; i++)
        {
            availableIndices.Add(i);
        }
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

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator ActivateRandomWeapons()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (availableIndices.Count == 0)
            {
                Debug.Log("No more weapons available.");
                yield break;
            }

            int randomIndex = Random.Range(0, availableIndices.Count);
            int index = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex);

            weapons[index].SetActive(true);
        }
    }
}
