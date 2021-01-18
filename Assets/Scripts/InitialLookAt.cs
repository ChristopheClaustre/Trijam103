using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLookAt : MonoBehaviour
{
    public Vector3 lookAt = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(lookAt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
