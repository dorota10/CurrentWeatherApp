using System.Collections;
using UnityEngine;

public class StormMove : MonoBehaviour
{
    private float destroyDelay = 2f; // OpóŸnienie przed zniszczeniem piorunu

    private bool isDestroyed = false;

    void Start()
    {
        // Zniszcz piorun po pewnym czasie (destroyDelay)
        StartCoroutine(DestroyAfterDelay());
    }

    private void Update()
    {
        if (!isDestroyed)
        {
            Sounds.StormSounds();
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        isDestroyed = true;
        Destroy(gameObject);
    }
}