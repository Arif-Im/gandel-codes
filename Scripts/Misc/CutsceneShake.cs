using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneShake : MonoBehaviour
{
    public float magnitude;
    public AudioSource roar;

    Vector3 originalPos;

    public void StartShakeCamera()
    {
        roar.Play();
        StartCoroutine(ShakeCamera());
    }

    public IEnumerator ShakeCamera()
    {
        originalPos = transform.localPosition;

        while (true)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            yield return null;
        }
    }
}
