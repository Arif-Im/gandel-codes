using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneActivator : MonoBehaviour
{
    public GameObject timeline;
    public GameObject cutsceneCam;
    public GameObject mainCam;
    public bool startPlaying = false;
    public GameObject bossTitle;
    

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            bossTitle.SetActive(true);
            timeline.SetActive(true);
            cutsceneCam.SetActive(true);
            mainCam.SetActive(false);
        }
    }

    public void StartPlaying()
    {
        cutsceneCam.SetActive(false);
        mainCam.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        startPlaying = true;
    }
}
