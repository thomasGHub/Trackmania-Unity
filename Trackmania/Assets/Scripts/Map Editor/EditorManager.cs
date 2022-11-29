using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering;
using UnityEngine.UI;
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
    private bool _canBeSelected = true;
    private int _height = 0;
    [SerializeField] private Image _editorSelected;
    [SerializeField] private Image _deleteSelected;

    [SerializeField] private GameObject _map;

    private string _json;

    public class jsonData
    {
        public string id;
        public Vector3 position;
    }
    private void Start()
    {
        _plane = new UnityEngine.Plane(UnityEngine.Vector3.up, new Vector3(0, 0, 0));
        _deleteSelected.enabled = false;
    }
    private void Update()
    {
        if (Keyboard.current[Key.T].wasPressedThisFrame)
        {
            print("debug");
        }
        if (!_freePos && _selectedBlock != null && _editMode)
        {
            if (!_preview)
            {
                _preview = true;
                if (_goPreview == null)
                {
                    _goPreview = Instantiate(_selectedBlock);
                }
            }
            if (_preview)
            {
                _goPreview.transform.position = GetPos();
                if (Keyboard.current[Key.R].wasPressedThisFrame)
                {
                    _goPreview.transform.Rotate(0, 45f, 0);
                }
                else if (Keyboard.current[Key.Q].wasPressedThisFrame && _height > 0)
                {
                    _height--;
                }
                else if (Keyboard.current[Key.E].wasPressedThisFrame)
                {
                    _height++;
                }
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
                        _goPreview = null;
                        StartCoroutine(TimerCanBeSelected());
                    }
                }
            }
        }
        if (_selectedBlock == null && Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_deleteMode)
            {
                Destroy(getObjectInEditor());
            }
            else if (_editMode && _canBeSelected)
            {
                _goPreview = getObjectInEditor();
                _selectedBlock = getObjectInEditor();
                StartCoroutine(TimerCanBeSelected());
            }
        }
        if (Mouse.current.middleButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
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
            //_selectedBlock.GetComponent<Highlight>().Outline();
        }
    }

    private UnityEngine.Vector3 GetPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        float enter = 0.0f;

        if (_plane.Raycast(ray, out enter))
        {
            UnityEngine.Vector3 hitPoint = ray.GetPoint(enter);
            if (Keyboard.current[Key.T].wasPressedThisFrame)
            {
                print(hitPoint + "convertit en : " + NearPos(hitPoint));
            }
            return NearPos(hitPoint);
        }
        else return new UnityEngine.Vector3(0, 0, 0);
    }

    private Vector3 NearPos(Vector3 pos)
    {
        pos.x = Mathf.FloorToInt(pos.x / 10) * 10;
        pos.z = Mathf.FloorToInt(pos.z / 10) * 10;
        pos.y = _height;

        return pos;
    }

    public void editMode()
    {
        print("EditMode");
        _editMode = true;
        _editorSelected.enabled = true;
        _deleteMode = false;
        _deleteSelected.enabled = false;
    }
    public void deleteMode()
    {
        print("DeleteMode");
        _editMode = false;
        _editorSelected.enabled = false;
        _deleteMode = true;
        _deleteSelected.enabled = true;
    }

    private IEnumerator TimerCanBeSelected()
    {
        _canBeSelected = false;

        yield return new WaitForSeconds(.1f);

        _canBeSelected = true;
    }

    public void save()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<RoadCurve>() != null)
            {
                jsonData saveObject = new jsonData();
                saveObject.id = go.GetComponent<RoadCurve>()._id;
                saveObject.position = go.transform.position;
                _json = JsonUtility.ToJson(go);
            }
            else if (go.GetComponent<RoadRampCurve>() != null)
            {
                jsonData saveObject = new jsonData();
                saveObject.id = go.GetComponent<RoadRampCurve>()._id;
                saveObject.position = go.transform.position;
                _json = JsonUtility.ToJson(go);
            }
            else if (go.GetComponent<RoadRampStraight>() != null)
            {
                jsonData saveObject = new jsonData();
                saveObject.id = go.GetComponent<RoadRampStraight>()._id;
                saveObject.position = go.transform.position;
                _json = JsonUtility.ToJson(go);
            }
            else if (go.GetComponent<RoadRampTurn>() != null)
            {
                jsonData saveObject = new jsonData();
                saveObject.id = go.GetComponent<RoadRampTurn>()._id;
                saveObject.position = go.transform.position;
                _json = JsonUtility.ToJson(go);
            }
            else if (go.GetComponent<RoadStraight>() != null)
            {
                jsonData saveObject = new jsonData();
                saveObject.id = go.GetComponent<RoadStraight>()._id;
                saveObject.position = go.transform.position;
                _json = JsonUtility.ToJson(go);
            }
            else if (go.GetComponent<RoadTurn>() != null)
            {
                jsonData saveObject = new jsonData();
                saveObject.id = go.GetComponent<RoadTurn>()._id;
                saveObject.position = go.transform.position;
                _json = JsonUtility.ToJson(go);
            }
        }
        print(_json);

                
    }
}
