using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] fogclouds;

    //[SerializeField]
    //private float spawnInterval = 4f;

    [SerializeField]
    public GameObject endPoint;

    Vector3 startPos;

    private bool generateFog = false;
    private float speed = 1;

    public Transform fogParent;

    private Coroutine FogGenerationCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        if (generateFog)
        {
            StartGeneratingFog();
        }
    }

    void SpawnFogClouds(Vector3 spawnPos, float speed)
    {
        int randomIndex = UnityEngine.Random.Range(0, fogclouds.Length);
        GameObject fog = Instantiate(fogclouds[randomIndex], fogParent.position, Quaternion.identity, fogParent);

        float startY = UnityEngine.Random.Range(spawnPos.y - 0.5f, spawnPos.y + 1.5f);

        fog.transform.position = new Vector3(spawnPos.x, startY, spawnPos.z);

        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        fog.transform.localScale = new Vector2(scale, scale);

        fog.GetComponent<FogMove>().StartFloating(speed, endPoint.transform.position.x);
    }

    public void StopGeneratingFog()
    {
        generateFog = false;
        if (FogGenerationCoroutine != null)
        {
            StopCoroutine(FogGenerationCoroutine);
            FogGenerationCoroutine = null; // Ustaw referencjê coroutine na null po zatrzymaniu
        }
    }
    public void StartGeneratingFog()
    {
        generateFog = true;
        if (FogGenerationCoroutine == null) // Uruchom tylko jeœli coroutine nie zosta³a jeszcze uruchomiona
        {
            Prewarm();
            FogGenerationCoroutine = StartCoroutine(GenerateFogCoroutine());
        }
    }

    IEnumerator GenerateFogCoroutine()
    {
        while (generateFog)
        {
            SpawnFogClouds(startPos, speed);
            yield return new WaitForSeconds(1);
        }
    }

    void Prewarm()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 spawnPos = startPos + Vector3.right * (i * 1); //poszerzanie odstêpu
            SpawnFogClouds(spawnPos, speed);
        }
    }

}
