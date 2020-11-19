using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotate : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        while(true)
        {
            transform.Rotate(transform.up);
        }
    }
}
