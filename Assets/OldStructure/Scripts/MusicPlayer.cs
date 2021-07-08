using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    [Header("Music Selection")]
    [SerializeField] AudioClip menuSong;
    [SerializeField] AudioClip[] backgroundSongs;
    [SerializeField] AudioClip gameOverSong;
    [SerializeField] AudioClip victorySong;
    //[SerializeField] [Range(0, 1)] float musicVolume;

    // Cached References

    private AudioSource audioSource;
    private MusicPlayer[] musicPlayer;
    private int randomSongIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        musicPlayer = FindObjectsOfType<MusicPlayer>();
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (musicPlayer.Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckAndChangeMusic();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void CheckAndChangeMusic()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSource.clip = menuSong;
            audioSource.loop = false;
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            return;
        }
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            randomSongIndex = Random.Range(0, backgroundSongs.Length);
            audioSource.clip = backgroundSongs[randomSongIndex];
            audioSource.loop = true;
        }
        audioSource.Play();
    }

    public void PlayGameOverClip()
    {
        audioSource.clip = gameOverSong;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayVictoryClip()
    {
        audioSource.clip = victorySong;
        audioSource.loop = false;
        audioSource.Play();
    }
}
