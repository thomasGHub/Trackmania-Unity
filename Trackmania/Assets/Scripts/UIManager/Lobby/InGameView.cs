using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using System.Linq;
using UnityEngine.InputSystem;

public class InGameView : View
{

    public static InGameView instance;

    public Transform lbParent;
    //public List<GameObject> lbs = new List<GameObject>();
    public GameObject lbPrefab;


    public Dictionary<int, Temps> dicTemps = new Dictionary<int, Temps>();
    public Dictionary<int, string> dicNames = new Dictionary<int, string>();
    public Dictionary<int, int> dicInt = new Dictionary<int, int>();

    public Trackmania inputActions;


    private void Awake()
    {
        instance = this;
    }



    public override void Initialize()
    {
        lbParent.gameObject.GetComponent<Image>().enabled = false;

        inputActions = new Trackmania();
        inputActions.UI.Escape.performed += Escaping;
    }


    public void OnEnable()
    {
        inputActions.UI.Escape.Enable();
    }

    public void OnDisable()
    {
        inputActions.UI.Escape.Disable();
    }


    public void DoLeadarboardInGameView()
    {
        DestroyLeaderboard();
        for (int i = 0; i < dicTemps.Count; i++)
        {
            dicInt[i] = Temps.TempsToInt(dicTemps[i]);
        }

        foreach (KeyValuePair<int, int> dic in dicInt.OrderBy(key => key.Value))
        {
            if (dic.Value != -1)
            {
                //Debug.LogWarning("Clé: " + dic.Key + " Valeur: " + dic.Value);

                GameObject lbActuel = Instantiate(lbPrefab, lbParent);
                lbActuel.GetComponent<UILeaderboardEntry>().nameText.text = dicNames[dic.Key];
                lbActuel.GetComponent<UILeaderboardEntry>().placeText.text = dic.Key.ToString();
                lbActuel.GetComponent<UILeaderboardEntry>().valueText.text = Temps.TempsToString(dicTemps[dic.Key]);
            }
        }
    }

    




    public void DestroyLeaderboard()
    {
        lbParent.gameObject.GetComponent<Image>().enabled = true;

        foreach (Transform item in lbParent)
        {
            Destroy(item.gameObject);
        }
    }


    public void Escaping(InputAction.CallbackContext context)
    {
        ViewManager.Show<EchapMenuView>();

    }

}
