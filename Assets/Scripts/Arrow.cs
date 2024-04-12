using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform arrowSpawnPos;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.down * 10);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
            
        }
    }
}
