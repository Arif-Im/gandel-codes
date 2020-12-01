using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public GameObject completeLevelUI;
    GameObject music;

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            music = GameObject.FindGameObjectWithTag("Music");
            if (music == null)
                return;
            music.GetComponent<AudioSource>().clip = music.GetComponent<Music>().songs[6];
            music.GetComponent<AudioSource>().Play();
            gameHasEnded = true;
            StartCoroutine("Restart");
        }
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        StartCoroutine("TransitionTime");
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        music.GetComponent<AudioSource>().clip = music.GetComponent<Music>().songs[1];
        music.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator TransitionTime()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
