using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class EditorManager : MonoBehaviour
{
    private GameObject _selectedBlock;
    private bool _freePos = false;
    private UnityEngine.Plane _plane;
    private bool _preview = false;
    private GameObject _goPreview;



    private void Start()
    {
        _plane = new UnityEngine.Plane(UnityEngine.Vector3.forward, new Vector3(0, 0, 0));
    }
    private void Update()
    {
        if (!_freePos && _selectedBlock != null)
        {
            if (!_preview)
            {
                _preview = true;
                _goPreview = Instantiate(_selectedBlock);
            }
            if (_preview)
            {
                _goPreview.transform.position = GetPos();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Instantiate(_goPreview);
                    _selectedBlock = null;
                    _preview = false;
                }
            }
        }
    }
    public void Hello()
    {
        print(_selectedBlock);
    }

    public void SelectOnUI(GameObject gameObject)
    {
        _selectedBlock = gameObject;
        print(_selectedBlock);
    }

    private UnityEngine.Vector3 GetPos()
    {
        Ray ray = Camera.main.ScreenPointToRay((Mouse.current.position.ReadValue()));
        float enter = 0.0f;

        if (_plane.Raycast(ray, out enter))
        {
            UnityEngine.Vector3 hitPoint = ray.GetPoint(enter);
            return NearPos(hitPoint);
        }
        else return new UnityEngine.Vector3(0, 0, 0);
    }

    private Vector3 NearPos(Vector3 pos)
    {
        pos.x = pos.x / 10;
        pos.x = Mathf.FloorToInt(pos.x);
        pos.x = pos.x * 10;

        pos.z = pos.y / 10;
        pos.z = Mathf.FloorToInt(pos.z);
        pos.z = pos.z * 10;

        pos.y = 0;

        return pos;
    }
}
