using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitRepo : MonoBehaviour
{
    void Awake()
    {
        MapSaver.CheckMapFolder();
    }
}
