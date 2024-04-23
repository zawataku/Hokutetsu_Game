using System.Collections;
using UnityEngine;

public class TrainsDropper : MonoBehaviour
{
    [SerializeField] private RandomTrainsSelector randomTrainsSelector;
    private bool hasSpawned = false;

    [SerializeField] private float coolTime = 2f;
    private Trains trainsInstance;

    private void Start()
    {
        StartCoroutine(HandleTrains(coolTime));
    }

    private IEnumerator HandleTrains(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (!hasSpawned)
            {
                SpawnTrain();
                hasSpawned = true;
            }
        }
    }

    private void SpawnTrain()
    {
        var trainsPrefab = randomTrainsSelector.Pop();
        trainsInstance = Instantiate(trainsPrefab, transform.position, Quaternion.identity);
        trainsInstance.transform.SetParent(transform);
        trainsInstance.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void Update()
    {
        MoveDropper();

        if (Input.GetMouseButtonDown(0) && hasSpawned && trainsInstance != null)
        {
            StartCoroutine(DropTrain());
        }

        if (Input.GetMouseButtonUp(0))
        {
            hasSpawned = false;
        }
    }

    private void MoveDropper()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Clamp(mousePos.x, -2.5f, 2.5f);
        mousePos.y = 3f;

        transform.position = mousePos;
    }

    private IEnumerator DropTrain()
    {
        trainsInstance.GetComponent<Rigidbody2D>().isKinematic = false;
        trainsInstance.transform.SetParent(null);
        trainsInstance = null;
        yield return new WaitForSeconds(0.1f);
    }
}