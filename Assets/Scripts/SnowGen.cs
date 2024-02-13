using System.Collections;
using UnityEngine;

public class SnowGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] snowflakes;

    [SerializeField]
    public GameObject endPoint1;

    Vector3 startPos;

    private bool generateSnowflake = false;

    public Transform snowflakeParent;

    private Coroutine snowflakeGenerationCoroutine;


    void Start()
    {
        startPos = transform.position;
        if (generateSnowflake)
        {
            StartGeneratingSnowflakes();
        }
    }

    void SpawnSnowflake(Vector3 spawnPos)
    {
        int randomIndex = UnityEngine.Random.Range(0, snowflakes.Length);
        GameObject snow = Instantiate(snowflakes[randomIndex], spawnPos, Quaternion.identity, snowflakeParent);

        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        snow.transform.localScale = new Vector2(scale, scale);

        float speed = UnityEngine.Random.Range(1f, 2.5f);

        SnowMove snowMoveComponent = snow.GetComponent<SnowMove>();
        if (snowMoveComponent != null)
        {
            // Ustaw punkt koñcowy na sam¹ ziemiê (pozycja X bez zmian)
            snowMoveComponent.StartFloating(speed, endPoint1.transform.position.y);
        }
        else
        {
            Debug.LogError("Snowflake prefab is missing SnowMove component!");
        }
    }

    public void StopGeneratingSnowflakes()
    {
        generateSnowflake = false;
        if (snowflakeGenerationCoroutine != null)
        {
            StopCoroutine(snowflakeGenerationCoroutine);
            snowflakeGenerationCoroutine = null;
        }
    }

    public void StartGeneratingSnowflakes()
    {
        generateSnowflake = true;
        if (snowflakeGenerationCoroutine == null)
        {
            Prewarm();
            snowflakeGenerationCoroutine = StartCoroutine(GenerateSnowflakesCoroutine());
        }
    }

    IEnumerator GenerateSnowflakesCoroutine()
    {
        while (generateSnowflake)
        {
            float randomX = UnityEngine.Random.Range(startPos.x+1f, startPos.x+14f);

            Vector3 spawnPos = new Vector3(randomX, startPos.y, startPos.z);

            SpawnSnowflake(spawnPos);
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
            Vector3 spawnPos = new Vector3(randomX, startPos.y-i*odstep, startPos.z); ; 
            SpawnSnowflake(spawnPos);
        }
    }
}