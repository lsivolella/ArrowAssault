using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class StartGameButtonScript : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioSource buttonAudioSoruce;
    [SerializeField] LevelManager levelManager;
    [SerializeField] [Range(0, 1)] float buttonSoundVolume = 1f;

    private float clipDuration;

    public void PlayButtonSound()
    {
        clipDuration = buttonSound.length;
        buttonAudioSoruce.PlayOneShot(buttonSound, buttonSoundVolume);
        StartCoroutine(WaitButtonSound(clipDuration));
    }

    IEnumerator WaitButtonSound(float clipDuration)
    {
        yield return new WaitForSeconds(clipDuration);
        levelManager.StartGame();
    }
}
