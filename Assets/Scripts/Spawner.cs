using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] SpawnPos;
    public ObjectPooler.Prefabs prefab;
    public float spawnInterval = 3f;
    public Vector2 direction;
    public float speed = 10;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Spawn()
    {
        GameObject obj = ObjectPooler.instance.GetObject(prefab);
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>().velocity = direction * speed;
        obj.transform.position = SpawnPos[Random.Range(0, SpawnPos.Length)].position;
        if (prefab == ObjectPooler.Prefabs.cannon)
        {
            GameManager.Instance.CannonEffect(SpawnPos[Random.Range(0, SpawnPos.Length)]);
        }
    }
}
