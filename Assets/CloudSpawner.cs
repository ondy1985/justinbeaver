using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterParticle")
        {
            // spawn a new cloud
            Transform t = collision.gameObject.transform;
            Vector2 closestPoint = collision.ClosestPoint(new Vector2(t.position.x, t.position.y));
            GameObject cloud = Instantiate(cloudPrefab, new Vector3(closestPoint.x, closestPoint.y + Random.Range(-0.5f, 0f)), Quaternion.identity);
            cloud.GetComponent<Cloud>().capacity = Random.Range(3, 6);
            Destroy(collision.gameObject);
        }
    }
}
