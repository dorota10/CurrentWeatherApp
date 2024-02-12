using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] snowflakes;

    [SerializeField]
    public GameObject endPoint1; // Punkt koñcowy na ziemi

    Vector3 startPos;

    private bool generateSnowflake = false;
    private float speed = 2;

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

    void SpawnSnowflake(Vector3 spawnPos, float speed)
    {
        int randomIndex = UnityEngine.Random.Range(0, snowflakes.Length);
        GameObject snow = Instantiate(snowflakes[randomIndex], spawnPos, Quaternion.identity, snowflakeParent);

        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        snow.transform.localScale = new Vector2(scale, scale);

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
            SpawnSnowflake(startPos, speed);
            yield return new WaitForSeconds(1);
        }
    }

    void Prewarm()
    {
        // Ustal szerokoœæ ekranu
        float screenWidth = Screen.width;

        // Oblicz szerokoœæ pojedynczej œnie¿ki
        float snowflakeWidth = snowflakes[0].GetComponent<Renderer>().bounds.size.x; // Za³ó¿my, ¿e wszystkie œnie¿ki maj¹ tak¹ sam¹ szerokoœæ

        // Oblicz odstêp miêdzy œnie¿kami
        float snowflakeSpacing = snowflakeWidth;

        // Iteruj przez ka¿dy punkt pocz¹tkowy
        for (int i = 0; i < Mathf.FloorToInt(screenWidth / snowflakeSpacing); i++)
        {
            // Oblicz pozycjê pocz¹tkow¹ œnie¿ynki na ca³ej szerokoœci ekranu
            Vector3 spawnPos = new Vector3(i * snowflakeSpacing, startPos.y, startPos.z);

            // U¿yj wyliczonej pozycji pocz¹tkowej do generowania œnie¿ynki
            SpawnSnowflake(spawnPos, speed);
        }
    }
}
    


 
    