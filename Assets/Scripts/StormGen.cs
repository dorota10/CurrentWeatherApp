using System.Collections;
using UnityEngine;

public class StormGen : MonoBehaviour
{
    [SerializeField]
    public GameObject[] thunders;

    [SerializeField]
    public GameObject endPoint;

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
        // Generuj tylko dwa pioruny
        for (int i = 0; i < 2; i++)
        {
            float randomX = Random.Range(startPos.x + 1f, startPos.x + 14f);
            Vector3 spawnPos = new Vector3(randomX, startPos.y, startPos.z);

            // Twórz piorun w wylosowanej pozycji
            GameObject thunderPrefab = thunders[0]; // Wybierz pierwszy obiekt z tablicy thunders
            GameObject thunder = Instantiate(thunderPrefab, spawnPos, Quaternion.identity, thunderParent);



            // Poczekaj chwilê przed wygenerowaniem kolejnego pioruna
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
}
