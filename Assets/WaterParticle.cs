using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Evaporate e = collision.gameObject.GetComponent<Evaporate>();
        if (e != null)
        {
            e.AddWater(1f);
            Destroy(gameObject);
        }
    }
}
