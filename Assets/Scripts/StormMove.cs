using System.Collections;
using UnityEngine;

public class StormMove : MonoBehaviour
{
    private float destroyDelay = 2f; // Op�nienie przed zniszczeniem piorunu

    void Start()
    {
        // Zniszcz piorun po pewnym czasie (destroyDelay)
        StartCoroutine(DestroyAfterDelay());
    }

    private void Update()
    {
        Sounds.StormSounds();
    }
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

}
