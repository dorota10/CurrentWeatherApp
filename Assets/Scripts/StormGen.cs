using System.Collections;
using UnityEngine;

public class StormGen : MonoBehaviour
{
    [SerializeField] public GameObject[] thunders;
    [SerializeField] public GameObject endPoint;
    Vector3 startPos;

    private bool generateThunder = false;
    public Transform thunderParent;
    private Coroutine thunderGenerationCoroutine;

    void Start()
    {
        if (generateThunder)
        {
            StartGeneratingThunders();
        }
    }

    public void StopGeneratingThunders()
    {
        generateThunder = false;
        if (thunderGenerationCoroutine != null)
        {
            StopCoroutine(thunderGenerationCoroutine);
            thunderGenerationCoroutine = null;
        }
    }

    public void StartGeneratingThunders()
    {
        generateThunder = true;
        if (thunderGenerationCoroutine == null)
        {
            thunderGenerationCoroutine = StartCoroutine(GenerateThundersCoroutine());
        }
    }

    IEnumerator GenerateThundersCoroutine()
    {
        while (generateThunder)
        {
            // Losuj pozycjê generowania pioruna
            float randomX = Random.Range(startPos.x, endPoint.transform.position.x);
            float randomY = Random.Range(startPos.y, endPoint.transform.position.y);
            Vector3 spawnPos = new Vector3(randomX, randomY, startPos.z);

            // Losuj prefab pioruna
            GameObject thunderPrefab = thunders[Random.Range(0, thunders.Length)];

            // Twórz piorun w wylosowanej pozycji
            GameObject thunder = Instantiate(thunderPrefab, spawnPos, Quaternion.identity, thunderParent);

            // Dezaktywuj piorun po 1 sekundzie
            yield return new WaitForSeconds(1f);
            thunder.SetActive(false);

            // Poczekaj a¿ piorun zostanie dezaktywowany
            yield return new WaitUntil(() => !thunder.activeSelf);

            // Zniszcz piorun
            Destroy(thunder);

            // Poczekaj 3 sekundy przed wygenerowaniem kolejnego pioruna
            yield return new WaitForSeconds(2f);
        }
    }
}