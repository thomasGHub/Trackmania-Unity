using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {

    public class UIPlayer : MonoBehaviour {

        [SerializeField] Text text;
        PlayerNetwork player;

        public void SetPlayer (PlayerNetwork player) {
            this.player = player;
            text.text = "PlayerNetwork " + player.playerIndex.ToString ();
        }

    }
}