using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MirrorBasics {

    public class UIPlayer : MonoBehaviour {

        [SerializeField] TextMeshProUGUI nameText;
        //PlayerNetwork player;

        
        public void SetPlayer (PlayerNetwork player) {
            
            nameText.text = player.playerName; 
        }


        public IEnumerator SetPlayerLateInfos(PlayerNetwork player)
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.LogError(i + player.playerName);
                nameText.text = player.playerName;  //"Player  " + player.playerIndex.ToString ();
                yield return new WaitForSeconds(1);
            }
        }


        
    }
}