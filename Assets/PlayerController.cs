using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public GameObject sprite;
    public float bucketCapacity = 5f;
    public GameObject waterParticle;
    public Transform bucketAnchor;
    public float emptyingRate = 3f;
    public Slider slider;
    public AudioClip pickupSfx;
    public AudioClip pourSfx;
    
    private Animator animator;
    private float bucketWater;
    private float emptyingTimeout;
    private Rigidbody2D rigidbody;
    private AudioSource audioSource;

    private void Start()
    {
        animator = sprite.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        animator.SetBool("Walks", Mathf.Abs(horizontal * speed) > 0);

        rigidbody.velocity = (Vector2.right * horizontal * speed);

        if (horizontal > 0)
        {
            sprite.transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("Pours", false);
        } else if (horizontal < 0)
        {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }

        slider.maxValue = bucketCapacity;
        slider.value = bucketWater;
    }

    public bool AddWater(float amount)
    {
        if (bucketWater >= bucketCapacity)
        {
            return false;
        }

        audioSource.PlayOneShot(pickupSfx);
        bucketWater += amount;
        return true;
    }

    public void Pour(Vector3 origin)
    {
        emptyingTimeout -= Time.deltaTime;

        if (bucketWater > 0 && emptyingTimeout <= 0)
        {
            GameObject particle = Instantiate(waterParticle, origin, Quaternion.identity);
            particle.GetComponent<Rigidbody2D>().gravityScale *= Random.Range(0.8f, 1.2f);
            particle.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Random.Range(20f, 150f));

            bucketWater -= 1f;
            emptyingTimeout = 1f / emptyingRate;
            
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(pourSfx);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PouroutTrigger")
        {
            // only if player is faced correctly
            if (sprite.transform.localScale.x > 0)
            {
                Pour(collision.transform.position);
                animator.SetBool("Pours", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PouroutTrigger")
        {
            animator.SetBool("Pours", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterDrop")
        {
            if (AddWater(1))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
