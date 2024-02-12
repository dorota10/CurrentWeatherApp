using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] snowflakes;

    [SerializeField]
    public GameObject endPoint1; // Punkt ko�cowy na ziemi

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
            // Ustaw punkt ko�cowy na sam� ziemi� (pozycja X bez zmian)
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
        // Ustal szeroko�� ekranu
        float screenWidth = Screen.width;

        // Oblicz szeroko�� pojedynczej �nie�ki
        float snowflakeWidth = snowflakes[0].GetComponent<Renderer>().bounds.size.x; // Za��my, �e wszystkie �nie�ki maj� tak� sam� szeroko��

        // Oblicz odst�p mi�dzy �nie�kami
        float snowflakeSpacing = snowflakeWidth;

        // Iteruj przez ka�dy punkt pocz�tkowy
        for (int i = 0; i < Mathf.FloorToInt(screenWidth / snowflakeSpacing); i++)
        {
            // Oblicz pozycj� pocz�tkow� �nie�ynki na ca�ej szeroko�ci ekranu
            Vector3 spawnPos = new Vector3(i * snowflakeSpacing, startPos.y, startPos.z);

            // U�yj wyliczonej pozycji pocz�tkowej do generowania �nie�ynki
            SpawnSnowflake(spawnPos, speed);
        }
    }
}
    


 
    