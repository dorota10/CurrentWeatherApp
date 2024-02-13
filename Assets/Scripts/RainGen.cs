using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] raindrops;

    [SerializeField]
    public GameObject endPoint1;

    Vector3 startPos;

    private bool generateRaindrop = false;

    public Transform raindropParent;

    private Coroutine raindropGenerationCoroutine;


    void Start()
    {
        startPos = transform.position;
        if (generateRaindrop)
        {
            StartGeneratingRaindrops();
        }
    }

    void SpawnRaindrop(Vector3 spawnPos)
    {
        int randomIndex = UnityEngine.Random.Range(0, raindrops.Length);
        GameObject rain = Instantiate(raindrops[randomIndex], spawnPos, Quaternion.identity, raindropParent);

        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        rain.transform.localScale = new Vector2(scale, scale);

        float speed = UnityEngine.Random.Range(1f, 2.5f);

        RainMove rainMoveComponent = rain.GetComponent<RainMove>();
        if (rainMoveComponent != null)
        {
            // Ustaw punkt koñcowy na sam¹ ziemiê (pozycja X bez zmian)
            rainMoveComponent.StartFloating(speed, endPoint1.transform.position.y);
        }
        else
        {
            Debug.LogError("Raindrop prefab is missing RainMove component!");
        }
    }

    public void StopGeneratingRaindrops()
    {
        generateRaindrop = false;
        if (raindropGenerationCoroutine != null)
        {
            StopCoroutine(raindropGenerationCoroutine);
            raindropGenerationCoroutine = null;
        }
    }

    public void StartGeneratingRaindrops()
    {
        generateRaindrop = true;
        if (raindropGenerationCoroutine == null)
        {
            Prewarm();
            raindropGenerationCoroutine = StartCoroutine(GenerateRaindropsCoroutine());
        }
    }

    IEnumerator GenerateRaindropsCoroutine()
    {
        while (generateRaindrop)
        {
            float randomX = UnityEngine.Random.Range(startPos.x + 1f, startPos.x + 14f);

            Vector3 spawnPos = new Vector3(randomX, startPos.y, startPos.z);

            SpawnRaindrop(spawnPos);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Prewarm()
    {
        //Tu coœ nie chce dzia³aæ ten podgl¹d
        float odstep = 0.5f; //poszerzanie odstêpu

        for (int i = 0; i < 30; i++)
        {
            float randomX = UnityEngine.Random.Range(startPos.x + 1f, startPos.x + 14f);
            Vector3 spawnPos = new Vector3(randomX, startPos.y - i * odstep, startPos.z); ;
            SpawnRaindrop(spawnPos);
        }
    }
}