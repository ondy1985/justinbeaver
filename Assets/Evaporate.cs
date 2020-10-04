using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evaporate : MonoBehaviour
{
    public GameObject particle;
    public float evaporationRate = 1f;
    public float startWaterContent = 100f;
    public Text levelText;
    public Text waterCirculatedText;
    public Text highScoreText;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    private float currentWaterContent;
    private float evaporationTimer;
    private BoxCollider2D boxCollider;
    private float targetY;
    private float startY;
    private float waterCirculated;
    private bool newHighScore;
    
    void Start()
    {
        Time.timeScale = 1;
        currentWaterContent = startWaterContent;
        boxCollider = GetComponent<BoxCollider2D>();
        evaporationTimer = 5f;
        targetY = startY = transform.position.y;
        highScoreText.text = PlayerPrefs.GetFloat("highscore").ToString("High score: #,##0");
        waterCirculatedText.text = waterCirculated.ToString("Score: #,##0");
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaterContent > 0)
        {
            evaporationTimer -= Time.deltaTime;

            if (evaporationTimer <= 0)
            {
                Vector3 pos = transform.position;
                pos.x = Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x);
                pos.y = boxCollider.bounds.max.y;

                Instantiate(particle, pos, Quaternion.identity);
                currentWaterContent -= 1f;
                evaporationTimer = 1f / evaporationRate;
                targetY = startY - (2.5f / startWaterContent) * (startWaterContent - currentWaterContent);
            }

            if (Mathf.Round(targetY * 10) != Mathf.Round(transform.position.y * 10))
            {
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetY, Time.deltaTime));
            }

            float waterPercents = (currentWaterContent / startWaterContent) * 100;
            levelText.text = waterPercents.ToString("#,##0") + " %";

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
            }
        } 
        else if (!gameOverScreen.activeSelf)
        {
            GameOver();
        }
    }

    public void AddWater(float amount)
    {
        if (currentWaterContent < startWaterContent && currentWaterContent > 0)
        {
            currentWaterContent += amount;
            waterCirculated += amount;
            waterCirculatedText.text = waterCirculated.ToString("Score: #,##0");
            if (waterCirculated > PlayerPrefs.GetFloat("highscore"))
            {
                newHighScore = true;
                PlayerPrefs.SetFloat("highscore", waterCirculated);
                highScoreText.text = PlayerPrefs.GetFloat("highscore").ToString("High score: #,##0");
            }
            targetY = startY - (2.5f / startWaterContent) * (startWaterContent - currentWaterContent);
        }
    }

    public float GetWaterCirculated()
    {
        return waterCirculated;
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        if (newHighScore) {
            gameOverScreen.GetComponent<GameOverScreen>().NewHighScore(waterCirculated);
        }
    }
}
