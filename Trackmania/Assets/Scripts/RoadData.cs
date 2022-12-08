using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PrefabData", menuName = "PrefabData", order = 1)]
public class RoadData : ScriptableObject
{
    [SerializeField] private GameObject[] _allRoadsPrefab;

    [SerializeField] private Road _checkpoint; 
    [SerializeField] private Road _start; 
    [SerializeField] private Road _goal;

    public Road CheckPoint => _checkpoint;
    public Road Start => _start;
    public Road Goal => _goal;

    public GameObject[] AllRoadsPrefabs => _allRoadsPrefab;
}
