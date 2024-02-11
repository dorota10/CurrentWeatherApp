using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] clouds;

    [SerializeField]
    public GameObject endPoint;

    Vector3 startPos;

    private bool generateClouds = false;
    private float speed = 2;

    public Transform cloudParent;

    private Coroutine cloudGenerationCoroutine; // Dodano referencjê do Coroutine

    void Start()
    {
        startPos = transform.position;
        if (generateClouds)
        {
            StartGeneratingClouds();
        }
    }

    void SpawnCloud(Vector3 spawnPos, float speed)
    {
        int randomIndex = UnityEngine.Random.Range(0, clouds.Length);
        GameObject cloud = Instantiate(clouds[randomIndex], cloudParent.position, Quaternion.identity, cloudParent);

        float startY = UnityEngine.Random.Range(spawnPos.y - 0.5f, spawnPos.y + 1.5f);

        cloud.transform.position = new Vector3(spawnPos.x, startY, spawnPos.z);

        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        cloud.transform.localScale = new Vector2(scale, scale);

        cloud.GetComponent<CloudsMove>().StartFloating(speed, endPoint.transform.position.x);
    }

    public void StopGeneratingClouds()
    {
        generateClouds = false;
        if (cloudGenerationCoroutine != null)
        {
            StopCoroutine(cloudGenerationCoroutine);
            cloudGenerationCoroutine = null; // Ustaw referencjê coroutine na null po zatrzymaniu
        }
    }

    public void StartGeneratingClouds()
    {
        generateClouds = true;
        if (cloudGenerationCoroutine == null) // Uruchom tylko jeœli coroutine nie zosta³a jeszcze uruchomiona
        {
            Prewarm();
            cloudGenerationCoroutine = StartCoroutine(GenerateCloudsCoroutine());
        }
    }

    IEnumerator GenerateCloudsCoroutine()
    {
        while (generateClouds)
        {
            SpawnCloud(startPos, speed);
            yield return new WaitForSeconds(1);
        }
    }

    void Prewarm()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 spawnPos = startPos + Vector3.right * (i * 2); //poszerzanie odstêpu
            SpawnCloud(spawnPos, speed);
        }
    }
}