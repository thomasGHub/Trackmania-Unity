using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;

public class InGameView : View
{

    public static InGameView instance;

    public Transform lbParent;
    //public List<GameObject> lbs = new List<GameObject>();
    public GameObject lbPrefab;



    private void Awake()
    {
        instance = this;
    }



    public override void Initialize()
    {
        
    }

    public void DoLeadarboardInGameView()
    {
        DestroyLeaderboard();

        List<int> scoresInt = new List<int>();
        for (int i = 0; i < PlayerNetwork.localPlayer.currentMatch.playersScores.Count; i++)
        {
            scoresInt.Add(TempsToInt(PlayerNetwork.localPlayer.currentMatch.playersScores[i]));
        }

        for (int i = 0; i < scoresInt.Count; i++)
        {
            GameObject lbActuel = Instantiate(lbPrefab, lbParent);
            lbActuel.GetComponent<UILeaderboardEntry>().nameText.text = PlayerNetwork.localPlayer.currentMatch.players[i].playerName;
            lbActuel.GetComponent<UILeaderboardEntry>().placeText.text = PlayerNetwork.localPlayer.currentMatch.players[i].playerIndex.ToString();
            lbActuel.GetComponent<UILeaderboardEntry>().valueText.text = TempsToString( PlayerNetwork.localPlayer.currentMatch.playersScores[i] );


            //lbs.Add(lbActuel);
        }
        


    }

    public  int TempsToInt(Temps score)
    {
        int intScore = score._miliseconds;
        intScore += score._seconds * 1000;
        intScore += score._minutes * 100000;

        return intScore;
    }

    public string TempsToString(Temps score)
    {
        return score._seconds +"." + score._minutes +":" + score._miliseconds;
    }


    public void DestroyLeaderboard()
    {
        foreach (Transform item in lbParent)
        {
            Destroy(item.gameObject);
        }
    }




}
