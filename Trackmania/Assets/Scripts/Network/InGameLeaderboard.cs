using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLeaderboard : MonoBehaviour
{
    public static InGameLeaderboard instance;

    private void Awake()
    {
        instance = this;
    }

    public static int TempsToInt(Temps score)
    {
        int intScore = score._miliseconds;
        intScore += score._seconds * 1000;
        intScore += score._minutes * 100000;

        return intScore;
    }
    









}
