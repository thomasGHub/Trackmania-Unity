using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    #region Map
    private UnityEngine.Plane _plane;
    private GameObject _selectedBlock;
    private GameObject _goPreview;
    private GameObject _currentCar;
    private GameObject _roadStart;
    private bool _freePos = false;
    private bool _preview = false;
    private float _height = 0;
    [SerializeField] private List<GameObject> _blockList = new List<GameObject>();
    private Dictionary<int, GameObject> _idToPrefab;
    private MapInfo _currentMapInfo = null;
    #endregion

    #region UI
    private bool _editMode = true;
    private bool _deleteMode = false;
    private bool _canBeSelected = true;

    [SerializeField] private Image _editorSelected;
    [SerializeField] private Image _deleteSelected;
    [SerializeField] private GameObject _UI;
    [SerializeField] private GameObject _car;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_InputField _inputFieldBronzeMedal;
    [SerializeField] private TMP_InputField _inputFieldSilverMedal;
    [SerializeField] private TMP_InputField _inputFieldGoldMedal;
    [SerializeField] private TMP_InputField _inputFieldAuthorMedal;

    #endregion

    #region JSON
    [SerializeField] private RoadData _roadData;
    private string _mapName;
    private int _bronzeMedal;
    private int _silverMedal;
    private int _goldMedal;
    private int _authorMedal;
    private string _mapToLoadFile = "mapToLoad.json";
    #endregion

    private bool _waitingForInfo = false;
    private Regex letterRegex = new Regex(@"[a-zA-Z]");
    private Regex numberRegex = new Regex(@"[1-9]");

    private void Start()
    {
        _plane = new UnityEngine.Plane(UnityEngine.Vector3.up, new Vector3(0, 0, 0));
        _deleteSelected.enabled = false;

        _idToPrefab = _roadData.GenerateDict();

        CheckMapToLoad();
    }

    private void Update()
    {
        if (_waitingForInfo)
        {
            if (Keyboard.current[Key.Enter].wasPressedThisFrame && letterRegex.IsMatch(_inputField.text) 
                && numberRegex.IsMatch(_inputFieldBronzeMedal.text)
                && numberRegex.IsMatch(_inputFieldSilverMedal.text)
                && numberRegex.IsMatch(_inputFieldGoldMedal.text)
                && numberRegex.IsMatch(_inputFieldAuthorMedal.text))

            {

                _mapName = _inputField.text;
                _bronzeMedal = (int)Int64.Parse(_inputFieldBronzeMedal.text);
                _silverMedal = (int)Int64.Parse(_inputFieldSilverMedal.text);
                _goldMedal = (int)Int64.Parse(_inputFieldGoldMedal.text);
                _authorMedal = (int)Int64.Parse(_inputFieldAuthorMedal.text);
                print(_mapName);
                print(_bronzeMedal);
                saveMap();

                _inputField.gameObject.SetActive(false);
                _inputFieldBronzeMedal.gameObject.SetActive(false);
                _inputFieldSilverMedal.gameObject.SetActive(false);
                _inputFieldGoldMedal.gameObject.SetActive(false);
                _inputFieldAuthorMedal.gameObject.SetActive(false);

                SceneManager.LoadScene("EditMap");

            }
        }

        if (!_freePos && _selectedBlock != null && _editMode && !_waitingForInfo)
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
                if (BlockOnMap(_goPreview) == true)
                {
                    _goPreview.GetComponent<Road>().redBlock.SetActive(true);

                }
                if (BlockOnMap(_goPreview) == false)
                {
                    _goPreview.GetComponent<Road>().redBlock.SetActive(false);
                }
                if (Keyboard.current[Key.R].wasPressedThisFrame)
                {
                    _goPreview.transform.Rotate(0, 45f, 0);
                }
                else if (Keyboard.current[Key.Q].wasPressedThisFrame && _height > 0)
                {
                    _height -= 0.5f;
                }
                else if (Keyboard.current[Key.E].wasPressedThisFrame)
                {
                    _height += 0.5f;
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

    private bool BlockOnMap(GameObject previewObj)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.name != "Plane" && hit.transform.root != previewObj.transform.root)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }

    private Vector3 NearPos(Vector3 pos)
    {
        pos.x = Mathf.FloorToInt(pos.x);
        pos.z = Mathf.FloorToInt(pos.z);
        if (pos.x % 5 <= 2f)
        {
            pos.x -= pos.x % 5;
        }
        else pos.x += 5 - (pos.x % 5);
        if (pos.z % 5 <= 2)
        {
            pos.z -= (pos.z % 5);
        }
        else pos.z += 5 - (pos.z % 5);
        pos.y = _height;

        return pos;
    }

    public void editMode()
    {
        _editMode = true;
        _editorSelected.enabled = true;
        _deleteMode = false;
        _deleteSelected.enabled = false;
    }

    public void deleteMode()
    {
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

    public void preSaveMap()
    {
        ShowHideUI();
        ValidMapEditor.LanchRace();
    }

    public void setInfo()
    {
        if (!(_currentMapInfo is null))
        {
            saveMap();
            SceneManager.LoadScene("EditMap");
            return;
        }
        _inputField.gameObject.SetActive(true);
        _inputFieldBronzeMedal.gameObject.SetActive(true);
        _inputFieldSilverMedal.gameObject.SetActive(true);
        _inputFieldGoldMedal.gameObject.SetActive(true);
        _inputFieldAuthorMedal.gameObject.SetActive(true);

        _waitingForInfo = true;
    }

    private void saveMap()

    {
        ListJsonData listOfBlock = new ListJsonData();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<Road>() != null)
            {
                JsonData saveObject = new JsonData(go.GetComponent<Road>().id, go.transform.position, go.transform.rotation);
                listOfBlock.blocks.Add(saveObject);
            }
        }
        if (_currentMapInfo is null)
            MapSaver.CreateNewMap(listOfBlock, _inputField.text, _bronzeMedal, _silverMedal, _goldMedal, _authorMedal);
        else
        {
            listOfBlock.ID = _currentMapInfo.ID;
            MapSaver.SaveMap(listOfBlock, _currentMapInfo, true);
        }

    }

    public void loadFile(string id)
    {
        ListBlockData listBlockData = MapSaver.GetMapBlock(id);

        foreach (BlockData blockData in listBlockData.blocks)
        {
            Instantiate(_idToPrefab[blockData.id], blockData.position, blockData.rotation);
        }
    }

    public void CheckMapToLoad()
    {
        string path = MapSaver.MapDataPath + "/" + _mapToLoadFile;
        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            _currentMapInfo = JsonConvert.DeserializeObject<MapInfo>(content);


            File.Delete(path);
            loadFile(_currentMapInfo.ID);
        }
    }

    private void ShowHideUI()
    {
        if (_UI.activeInHierarchy)
        {
            _UI.SetActive(false);
        }
        else
        {
            _UI.SetActive(true);
        }
    }
}