using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccineBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VaccineManager.Instance.AddVaccine(1);
            Destroy(gameObject);
        }
    }
}

