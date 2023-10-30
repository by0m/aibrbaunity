using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carScript : MonoBehaviour
{
    [SerializeField]
    Transform carGameObject;

    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = carGameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        carGameObject.position = new Vector3(carGameObject.position.x, carGameObject.position.y, carGameObject.position.z - 0.35f);
        if(carGameObject.position.z < -110)
        {
            carGameObject.position = startPosition;
        }
    }
}
