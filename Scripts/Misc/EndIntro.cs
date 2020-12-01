using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndIntro : MonoBehaviour
{
    GameObject music;

    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        music = GameObject.FindGameObjectWithTag("Music");
        music.GetComponent<AudioSource>().clip = music.GetComponent<Music>().songs[1];
        music.GetComponent<AudioSource>().Play();
    }

    public void ContinueStory()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        music = GameObject.FindGameObjectWithTag("Music");
        music.GetComponent<AudioSource>().clip = music.GetComponent<Music>().songs[0];
        music.GetComponent<AudioSource>().Play();
    }
}
