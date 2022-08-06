using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    

    // On Trigger Enter
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthManager>().TakeDamage();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.tag == "SpinObject")
        {
            StartCoroutine(other.GetComponent<SpinObject>().Spined());
        }    
    }
}
