using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static EditorManager;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using Quaternion = UnityEngine.Quaternion;
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
    [SerializeField] private List<GameObject> _blockList = new List<GameObject>();

    private string _json;
    private string _pathMapToLoad = "/test.json";

    private Dictionary<int, GameObject> _idToPrefab;

    [Serializable]
    public class ListBlock
    {
        public List<jsonData> blocks = new List<jsonData>();
    }

    [Serializable]
    public class jsonData
    {
        public int id;
        public Vector3 position;
        public Quaternion rotation;
    }
    private void Start()
    {
        _plane = new UnityEngine.Plane(UnityEngine.Vector3.up, new Vector3(0, 0, 0));
        _deleteSelected.enabled = false;

        _idToPrefab = new Dictionary<int, GameObject>()
        {
            { 1, _blockList[0]},
            { 2, _blockList[1]},
            { 3, _blockList[2]},
            { 4, _blockList[3]},
            { 5, _blockList[4]},
            { 6, _blockList[5]}
        };
    }
    private void Update()
    {
        if (Keyboard.current[Key.T].wasPressedThisFrame)
        {
            loadFile(_pathMapToLoad);
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
                if(BlockOnMap(_goPreview) == true)
                {
                    _goPreview.GetComponent<Road>().redBlock.SetActive(true);
               
                }
                if(BlockOnMap(_goPreview)==false)
                {                   
                    _goPreview.GetComponent<Road>().redBlock.SetActive(false);
                }
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

    /*private bool CanBePlaced()
    {

    }*/

    private bool BlockOnMap(GameObject previewObj)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.name != "Plane" && hit.transform.root != previewObj.transform.root)
            {
                print(hit.transform.root + " compar� a : " + previewObj.transform.root);
                return true;
            }else return false;
        }
        else return false;
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

    public void saveMap()
    {
        ListBlock listOfBlock = new ListBlock();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<Road>() != null)
            {
                jsonData saveObject = new jsonData();
                saveObject.id = go.GetComponent<Road>().id;
                saveObject.position = go.transform.position;
                saveObject.rotation = go.transform.rotation;
                listOfBlock.blocks.Add(saveObject);
            }
        }
        _json += JsonUtility.ToJson(listOfBlock);
        print(_json);
        File.WriteAllText(Application.persistentDataPath + "/test.json", _json);
    }
    public void loadFile(string _pathMapToLoad)
    {
        string path = Application.persistentDataPath + _pathMapToLoad;
        string jsonStr = File.ReadAllText(path);
        ListBlock mySampleFile = JsonUtility.FromJson<ListBlock>(jsonStr);
        print(mySampleFile.blocks.Count);
        foreach (jsonData jsonData in mySampleFile.blocks)
        {         
            Instantiate(_idToPrefab[jsonData.id], jsonData.position, jsonData.rotation);
        }
    }
}