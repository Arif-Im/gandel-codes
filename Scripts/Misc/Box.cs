using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class Box : MonoBehaviour
{
    EnemyHealth eh;
    public GameObject end;

    // Start is called before the first frame update
    void Start()
    {
        eh = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (eh.health <= 0)
        {
            Instantiate(end, gameObject.transform.position, Quaternion.identity);
        }
    }
}
