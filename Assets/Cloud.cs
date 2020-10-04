using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float capacity = 5f;
    public float speed = 1f;
    public GameObject rainDropPrefab;
    public Sprite[] colorSprites;
    public Sprite[] shrinkSprites;

    private float waterContent = 1f;
    private SpriteRenderer spriteRenderer;
    private float rainTimer;
    private bool isRaining;
    private bool shouldMove;
    private float timeout;
    
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        isRaining = false;
        shouldMove = false;
        timeout = Random.Range(10f, 20f);
    }

    private void Update()
    {
        timeout -= Time.deltaTime;

        if (waterContent >= capacity || timeout <= 0)
        {
            shouldMove = true;            
        }

        if (shouldMove)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (isRaining)
        {
            rainTimer -= Time.deltaTime;
            if (rainTimer <= 0)
            {
                // spawn rain drop
                Instantiate(rainDropPrefab, transform.position, Quaternion.identity);
                waterContent -= 1f;
                // decrease cloud size sprite
                int shrinkIndex = (int) Mathf.Round((waterContent / capacity) * (shrinkSprites.Length - 1));
                if (shrinkIndex > shrinkSprites.Length - 1) shrinkIndex = shrinkSprites.Length - 1;
                spriteRenderer.sprite = shrinkSprites[shrinkIndex];
                rainTimer = Random.Range(1f, 2f);
            }
        }

        if (waterContent <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterParticle")
        {
            if (waterContent < capacity)
            {
                waterContent += 1f;
                int colorIndex = (int)Mathf.Round((waterContent / capacity) * (colorSprites.Length - 1));
                if (colorIndex > colorSprites.Length - 1) colorIndex = colorSprites.Length - 1;
                spriteRenderer.sprite = colorSprites[colorIndex];
                Destroy(collision.gameObject);
            }
        } else if (collision.gameObject.tag == "LandBorder")
        {
            StartRain();
        }
    }

    public void StartRain()
    {
        isRaining = true;
        rainTimer = Random.Range(0, 2f);
    }
}
