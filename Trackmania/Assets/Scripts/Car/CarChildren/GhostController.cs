using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PlayerLoop;

public class GhostController : MonoBehaviour
{
    private List<GhostData> _ghosts = new List<GhostData>();
    private Rigidbody _GhostRb;
    Ghost ghost = new Ghost();
    private void Awake()
    {
        _GhostRb = GetComponent<Rigidbody>();
        _ghosts = ghost.loadGhost(ghost._pathMapToLoad);
    }
    private void Start()
    {
        StartCoroutine(_raceGhost(ghost));
    }

    IEnumerator _raceGhost(Ghost ghost)
    {
        foreach(GhostData ghostData in _ghosts)
        {
            _GhostRb.DOMove(ghostData.position, ghost._timeBetweenGetData);
            _GhostRb.DORotate(ghostData.rotation.eulerAngles, ghost._timeBetweenGetData);
            yield return new WaitForSeconds(ghost._timeBetweenGetData);
        }
    }


}
