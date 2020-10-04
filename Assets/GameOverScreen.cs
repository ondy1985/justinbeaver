using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject highScore;
    public bool isGameOver;

    private void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }
    }

    // Start is called before the first frame update
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NewHighScore(float score)
    {
        if (score > 0)
            highScore.SetActive(true);
    }

    public void Resume()
    {
        if (!isGameOver)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
