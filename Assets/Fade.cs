using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float duration = 5f;

    private Text text;
    private float timeout;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        timeout = duration;
    }

    // Update is called once per frame
    void Update()
    {
        timeout -= Time.deltaTime;

        float alpha = Mathf.Lerp(0, 1, timeout / duration);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        if (alpha <= 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
