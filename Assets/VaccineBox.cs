using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccineBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int before = VaccineManager.Instance.currentVaccine;
            VaccineManager.Instance.AddVaccine(1);
            int after = VaccineManager.Instance.currentVaccine;
            
            Debug.Log("BEFORE : " + before);
            Debug.Log("AFTER : " + after);
            if (after > before) // 백신이 실제로 증가한 경우에만 제거
            {
                Destroy(gameObject);
            }
        }
    }
}

