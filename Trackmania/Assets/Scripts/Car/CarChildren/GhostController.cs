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

    private bool _isRunning = false;
    private bool _isFinish = false;
    private int _index = 0;
    private float _lerpTime;
    private float _startTime;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;

    private void Awake()
    {
        _GhostRb = GetComponent<Rigidbody>();
        _lerpTime = new Ghost()._timeBetweenGetData;
    }

    public void Init(List<GhostData> ghostDatas)
    {
        _ghosts = ghostDatas;
    }

    public void Update()
    {
        if(_isRunning && !_isFinish)
        {

            if ((Time.time - _startTime) < _lerpTime)
            {
                _GhostRb.transform.position = Vector3.Lerp(_startPosition, _targetPosition, (Time.time - _startTime) / _lerpTime);
                _GhostRb.transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, (Time.time - _startTime) / _lerpTime);
            }
            else
            {
                _index++;

                if(_index == _ghosts.Count)
                {
                    _isFinish = true;
                    return;
                }

                ChangeLerpValue();
            }
        }
    }

    private void ChangeLerpValue()
    {
        _startPosition = _GhostRb.transform.position;
        _startRotation = _GhostRb.transform.rotation;

        _targetPosition = _ghosts[_index].position;
        _targetRotation = _ghosts[_index].rotation;

        _startTime = Time.time;
    }

    public void Restart(Transform destination)
    {
        _GhostRb.transform.position = destination.position;
        _GhostRb.transform.rotation = destination.rotation;

        _isFinish = false;
        _isRunning = false;
    }

    public void StartRace()
    {
        _isRunning = true;
        _index = 0;
        ChangeLerpValue();
    }
}
