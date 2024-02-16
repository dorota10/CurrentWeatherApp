using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrizzleGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] smallraindrops;

    [SerializeField]
    public GameObject endPoint;

    Vector3 startPos;

    private bool generateSmallRaindrop = false;

    public Transform smallraindropParent;

    private Coroutine smallraindropGenerationCoroutine;


    void Start()
    {
        startPos = transform.position;
        if (generateSmallRaindrop)
        {
            StartGeneratingSmallRaindrops();
        }
    }

    void SpawnRaindrop(Vector3 spawnPos)
    {
        int randomIndex = UnityEngine.Random.Range(0, smallraindrops.Length);
        GameObject drizzle = Instantiate(smallraindrops[randomIndex], spawnPos, Quaternion.identity, smallraindropParent);

        float scale = UnityEngine.Random.Range(1.5f, 1.6f);
        drizzle.transform.localScale = new Vector2(scale, scale);

        float speed = UnityEngine.Random.Range(3f, 5f);

        DrizzleMove drizzleMoveComponent = drizzle.GetComponent<DrizzleMove>();
        if (drizzleMoveComponent != null)
        {
            // Ustaw punkt koñcowy na sam¹ ziemiê (pozycja X bez zmian)
            drizzleMoveComponent.StartFloating(speed, endPoint.transform.position.y);
        }
        else
        {
            Debug.LogError("Raindrop prefab is missing DrizzleMove component!");
        }
    }

    public void StopGeneratingSmallRaindrops()
    {
        generateSmallRaindrop = false;
        if (smallraindropGenerationCoroutine != null)
        {
            StopCoroutine(smallraindropGenerationCoroutine);
            smallraindropGenerationCoroutine = null;
        }
    }

    public void StartGeneratingSmallRaindrops()
    {
        generateSmallRaindrop = true;
        if (smallraindropGenerationCoroutine == null)
        {
            Prewarm();
            smallraindropGenerationCoroutine = StartCoroutine(GenerateSmallRaindropsCoroutine());
        }
    }

    IEnumerator GenerateSmallRaindropsCoroutine()
    {
        while (generateSmallRaindrop)
        {
            float randomX = UnityEngine.Random.Range(startPos.x + 1f, startPos.x + 14f);

            Vector3 spawnPos = new Vector3(randomX, startPos.y, startPos.z);
            // Generowanie wiêkszej liczby kropli deszczu w ka¿dej iteracji
            for (int i = 0; i < 10; i++)
            {
                SpawnRaindrop(spawnPos);
            }

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