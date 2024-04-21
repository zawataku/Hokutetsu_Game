using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTrainsSelector : MonoBehaviour
{
    [SerializeField] private Trains[] TrainsPrefabs;

    private Trains reservedTrains;
    public Trains ReservedTrains
    {
        get { return reservedTrains; }
    }

    private void Start()
    {
        Pop();
    }

    public Trains Pop()
    {
        Trains ret = reservedTrains;

        int index = Random.Range(0, TrainsPrefabs.Length);
        reservedTrains = TrainsPrefabs[index];

        return ret;
    }
}