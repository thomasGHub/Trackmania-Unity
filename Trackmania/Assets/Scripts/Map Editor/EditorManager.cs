using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using Vector3 = UnityEngine.Vector3;

public class EditorManager : MonoBehaviour
{
    private GameObject _selectedBlock;
    private bool _freePos = false;
    private UnityEngine.Plane _plane;
    private bool _preview = false;
    private GameObject _goPreview;
    private bool _editMode = true;
    private bool _deleteMode = false;
    private bool _selectorMode = false;

    private void Start()
    {
        _plane = new UnityEngine.Plane(UnityEngine.Vector3.forward, new Vector3(0, 0, 0));
    }
    private void Update()
    {
        if (!_freePos && _selectedBlock != null && _editMode)
        {
            if (!_preview)
            {
                _preview = true;
                if(_goPreview == null)
                {
                    _goPreview = Instantiate(_selectedBlock);
                }
            }
            if (_preview)
            {
                _goPreview.transform.position = GetPos();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        Destroy(_goPreview);
                        _preview = false;
                        _selectedBlock = null;
                    }
                    else
                    {
                        _selectedBlock = null;
                        _preview = false;
                    }
                }
            }
        }
        if(_selectedBlock == null && Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_deleteMode)
            {
                Destroy(getObjectInEditor());
            }
            else if (_editMode)
            {
                _goPreview = getObjectInEditor();
                _selectedBlock = getObjectInEditor();
            }
        }
        if(Mouse.current.middleButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            _selectedBlock = getObjectInEditor();
            editMode();
        }
    }
    public void Hello()
    {
        print(_selectedBlock);
    }

    private GameObject getObjectInEditor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.name != "Plane")
            {
                print(hit.transform.root.gameObject.name);
                return hit.transform.root.gameObject;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public void SelectOnUI(GameObject gameObject)
    {
        if (_editMode == true)
        {
            _selectedBlock = gameObject;
        }
    }

    private UnityEngine.Vector3 GetPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
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

    public void editMode()
    {
        print("EditMode");
        _editMode = true;
        _deleteMode = false;
        _selectorMode = false;
    }
    public void deleteMode()
    {
        print("DeleteMode");
        _editMode = false;
        _deleteMode = true;
        _selectorMode = false;
    }
}
