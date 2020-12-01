using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject sparkle;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            gameManager.CompleteLevel();
            gameObject.SetActive(false);
            Instantiate(sparkle, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            gameManager.completeLevelUI.SetActive(false);
        }
    }
}
