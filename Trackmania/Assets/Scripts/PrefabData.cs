using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PrefabData", menuName = "PrefabData", order = 1)]
public class PrefabData : ScriptableObject
{
    public Dictionary<int, GameObject> RoadsPrefab;
}
