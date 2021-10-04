using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0,4);
        Material mat = FindObjectOfType<AshBehavior>().materials[index];
        GetComponent<Renderer>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
