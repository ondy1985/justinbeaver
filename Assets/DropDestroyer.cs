using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterDrop" || collision.gameObject.tag == "Cloud") 
            Destroy(collision.gameObject);
    }
}
