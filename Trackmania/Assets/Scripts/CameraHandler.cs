using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationOffSet;
    [Space(20)]
    [SerializeField] private Vector3 _positionOffSet;

    [SerializeField] private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        _rotationOffSet = transform.eulerAngles;
        _positionOffSet = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Lerp(transform.position, _target.position + _positionOffSet, 10f * Time.deltaTime));
        transform.position = Vector3.Lerp(transform.position, _target.position + _positionOffSet, 0.2f * Time.deltaTime);

        //transform.eulerAngles = _target.eulerAngles + _rotationOffSet;//new Vector3(transform.eulerAngles.x, _target.eulerAngles.y, transform.eulerAngles.z); 
    }
}
