using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.Sleep();// если объект сущесвует то применить  метод Sleep что б физика на стену не действовала.
    }

    // Update is called once per frame
    
}
