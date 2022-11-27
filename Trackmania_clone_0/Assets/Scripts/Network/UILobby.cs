using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {

    public class UILobby : MonoBehaviour {

        public static UILobby instance;

        [Header ("Host Join")]
        [SerializeField] InputField joinMatchInput;
        [SerializeField] List<Selectable> lobbySelectables = new List<Selectable> ();
        [SerializeField] GameObject lobbyCanvas;
        [SerializeField] GameObject searchCanvas;
        bool searching = false;

        [Header ("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText;
        [SerializeField] GameObject beginGameButton;

        GameObject localPlayerLobbyUI;

        [Space(10)]
        public Button HostPublicButton;
        public Button HostPrivateButton;
        public Button SearchButton;
        public Button JoinButton;
        public Button BeginGameButton;
        public Button CancelMatchButton;
        public Button CancelSearchButton;


        void Start () {
            instance = this;
            ListenerUI();
        }


        public void ListenerUI()
        {
            HostPublicButton.onClick.AddListener(() => HostPublic());
            HostPrivateButton.onClick.AddListener(() => HostPrivate());
            SearchButton.onClick.AddListener(() => SearchGame());
            JoinButton.onClick.AddListener(() => Join());
            BeginGameButton.onClick.AddListener(() => BeginGame());
            CancelMatchButton.onClick.AddListener(() => DisconnectGame());
            CancelSearchButton.onClick.AddListener(() => CancelSearchGame());

        }


        public void SetStartButtonActive (bool active) {
            beginGameButton.SetActive (active);
        }

        public void HostPublic () {
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.HostGame (true);
        }

        public void HostPrivate () {
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.HostGame (false);
        }

        public void HostSuccess (bool success, string matchID) {
            if (success) {
                //lobbyCanvas.SetActive(true);
                ViewManager.Show<LobbyMenuView>();

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void Join () {
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.JoinGame (joinMatchInput.text.ToUpper ());
        }

        public void JoinSuccess (bool success, string matchID) {
            if (success) {
                lobbyCanvas.SetActive(true);
                ViewManager.Show<LobbyMenuView>();

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void DisconnectGame () {
            if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
            Player.localPlayer.DisconnectGame ();

            ViewManager.Show<ConnectMenuView>();
            //lobbyCanvas.SetActive(false);
            lobbySelectables.ForEach (x => x.interactable = true);
        }

        public GameObject SpawnPlayerUIPrefab (Player player) {
            GameObject newUIPlayer = Instantiate (UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer> ().SetPlayer (player);
            newUIPlayer.transform.SetSiblingIndex (player.playerIndex - 1);

            return newUIPlayer;
        }

        public void BeginGame () {
            Player.localPlayer.BeginGame ();
            ViewManager.Show<NoUI>();
        }

        public void SearchGame () {
            StartCoroutine (Searching ());
        }

        public void CancelSearchGame () {
            searching = false;
            ViewManager.Show<ConnectMenuView>();

        }

        public void SearchGameSuccess (bool success, string matchID) {
            if (success) {
                //searchCanvas.SetActive(false);

                ViewManager.Show<LobbyMenuView>();

                searching = false;
                JoinSuccess (success, matchID);
            }
        }

        IEnumerator Searching () {
            //searchCanvas.SetActive(true);
            ViewManager.Show<SearchMenuView>();

            searching = true;

            float searchInterval = 1;
            float currentTime = 1;

            while (searching) {
                if (currentTime > 0) {
                    currentTime -= Time.deltaTime;
                } else {
                    currentTime = searchInterval;
                    Player.localPlayer.SearchGame ();
                }
                yield return null;
            }
            //searchCanvas.SetActive(false);
            //ViewManager.Show<LobbyMenuView>();

        }

    }
}