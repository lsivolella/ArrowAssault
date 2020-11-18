using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text uiWaveCounterText;
    [SerializeField] GameObject victoryPanel;

    // Cached References
    private bool isPaused = false;
    private EnemySpawner enemySpawner;
    private MusicPlayer musicPlayer;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        SetDefaultValues();
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Cursor.visible = true;
                isPaused = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;

            }
            else
            {
                Cursor.visible = false;
                isPaused = false;
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void RestartGame()
    {
        SetDefaultValues();
        SceneManager.LoadScene(sceneName);
    }

    public void StartGame()
    {
        SetDefaultValues();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        SetDefaultValues();
        Application.Quit();
        /*
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
        */
    }

    public void AccessMenu()
    {
        SetDefaultValues();
        SceneManager.LoadScene("MenuScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void GameOver()
    {
        if (musicPlayer)
        {
            musicPlayer.PlayGameOverClip();
        }
        uiWaveCounterText.GetComponent<Text>().text = enemySpawner.GetCurrentWaveIndex().ToString();
        Cursor.visible = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Victory()
    {
        if (musicPlayer)
        {
            musicPlayer.PlayVictoryClip();
        }
        Cursor.visible = true;
        victoryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void SetDefaultValues()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
}
