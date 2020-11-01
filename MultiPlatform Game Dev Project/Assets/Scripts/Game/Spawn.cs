using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

////This code is moved to gamecontroller
//public class Spawn : MonoBehaviour
//{
    //// Start is called before the first frame update
    //[SerializeField]
    //private GameObject key;
    //
    //private float x;
    //private float y;
    //private float z;
    //
    //
    //void Start()
    //{               
    //    spawnKey();
    //}
    //
    //private void spawnKey()
    //{
    //    bool keySpawned = false;
    //    int index = UnityEngine.Random.Range(0, 6);
    //    List<Spawn> coord = new List<Spawn>();
    //    Vector3 keyPosition = new Vector3(0, 0, 0);
    //
    //    coord.Add(new Spawn(-18f, 5.391f, 33f));
    //    coord.Add(new Spawn(32f, 5.391f, 36f));
    //    coord.Add(new Spawn(37f, 1f, 1.23f));
    //    coord.Add(new Spawn(-16.18f, 1f, 30.6f));
    //    coord.Add(new Spawn(53.39f, 1f, 53f));
    //    coord.Add(new Spawn(14f, 3.742f, 6.5f));
    //    coord.Add(new Spawn(26f, 1f, 36f));
    //
    //    Spawn temp = coord[index];
    //    keyPosition = new Vector3(temp.x, temp.y, temp.z);
    //    Instantiate(key, keyPosition, Quaternion.identity);
    //    Debug.Log(index);
    //    keySpawned = true;
    //    
    //}
    //
    //private Spawn(float x, float y, float z)
    //{
    //    this.x = x;
    //    this.y = y;
    //    this.z = z;
    //
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //    
    //}
//}
