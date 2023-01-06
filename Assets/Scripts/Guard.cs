using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public GameObject player;
    public Collider2D playerCollider;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == playerCollider)
        {
            Debug.Log("Player entered range");
        }
    }
}
