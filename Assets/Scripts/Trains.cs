using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRAINS_TYPE
{
    _2200 = 1,
    _3750,
    _6000,
    _7100,
    _7700,
    _03,
    ED20,
}

public class Trains : MonoBehaviour
{
    public TRAINS_TYPE TrainsType;
    private static int Trains_Serial = 0;
    private int My_Serial;
    public bool isDestroyed = false;

    [SerializeField] private Trains nextTrainsPrefab;

    private void Awake()
    {
        My_Serial = Trains_Serial;
        Trains_Serial++;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDestroyed)
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out Trains otherTrains))
        {
            if (otherTrains.TrainsType == TrainsType)
            {
                if (My_Serial < otherTrains.My_Serial)
                {
                    if (nextTrainsPrefab != null)
                    {
                        isDestroyed = true;
                        otherTrains.isDestroyed = true;
                        Destroy(gameObject);
                        Destroy(other.gameObject);

                        Vector3 center = (transform.position + other.transform.position) / 2;
                        Quaternion rotation = Quaternion.Lerp(transform.rotation, other.transform.rotation, 0.5f);
                        Trains next = Instantiate(nextTrainsPrefab, center, rotation);

                        Rigidbody2D nextRb = next.GetComponent<Rigidbody2D>();
                        Vector3 velocity = (GetComponent<Rigidbody2D>().velocity + other.gameObject.GetComponent<Rigidbody2D>().velocity) / 2;
                        nextRb.velocity = velocity;

                        float angularVelocity = (GetComponent<Rigidbody2D>().angularVelocity + other.gameObject.GetComponent<Rigidbody2D>().angularVelocity) / 2;
                        nextRb.angularVelocity = angularVelocity;
                    }
                    // If nextTrainsPrefab is null, do nothing
                }
            }
        }
    }
}
