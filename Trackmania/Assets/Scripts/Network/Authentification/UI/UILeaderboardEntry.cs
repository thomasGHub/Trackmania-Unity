using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class UILeaderboardEntry : MonoBehaviour
{
    public TextMeshProUGUI placeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;

    public void SetScore(string _score)
    {
        StringBuilder myString = new StringBuilder();
        if (_score.Length - 8 >= 0 )
        {
            myString.Append(_score[_score.Length - 8]);}   else{myString.Append("0");
        }
        if (_score.Length - 7 >= 0 )
        {
            myString.Append(_score[_score.Length - 7]);}   else{myString.Append("0");
        }
        if (_score.Length - 6 >= 0 )
        {
            myString.Append(_score[_score.Length - 6]);}   else{myString.Append("0");
        }
        
        myString.Append(".");

        if (_score.Length - 5 >= 0 )
        {
            myString.Append(_score[_score.Length - 5]);}   else{myString.Append("0");
        }
        if (_score.Length - 4 >= 0 )
        {
            myString.Append(_score[_score.Length - 4]);}   else{myString.Append("0");
        }
        
        myString.Append(":");

        if (_score.Length - 3 >= 0 )
        {
            myString.Append(_score[_score.Length - 3]);}   else{myString.Append("0");
        }
        if (_score.Length - 2 >= 0 )
        {
            myString.Append(_score[_score.Length - 2]);}   else{myString.Append("0");
        }
        if (_score.Length - 1 >= 0 )
        {
            myString.Append(_score[_score.Length - 1]);}   else{myString.Append("0");
        }

        valueText.text = myString.ToString();
    }

    
}
