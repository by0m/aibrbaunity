using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitByCar : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "car")
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 60, 0), ForceMode.Impulse);
        }
    }
}
