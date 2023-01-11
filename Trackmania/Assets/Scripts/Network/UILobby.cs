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
        [HideInInspector] public bool searchingAllRooms = true;

        [Header ("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText;
        [SerializeField] GameObject beginGameButton;
        [SerializeField] GameObject PermanentView;

        GameObject localPlayerLobbyUI;

        [Space(10)]
        public Button HostPublicButton;
        public Button HostPrivateButton;
        public Button SearchButton;
        public Button JoinButton;
        public Button BeginGameButton;
        public Button CancelMatchButton;
        public Button CancelSearchButton;
        public GameObject RoomI;
        public Transform ListRoomContener;
        public List<GameObject> ListAllRooms = new List<GameObject>();

        public Button LeaveButton;


        void Start () {
            instance = this;
            ListenerUI();
            StartCoroutine(SearchingRoom());
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
            LeaveButton.onClick.AddListener(() => PlayerNetwork.localPlayer.OnLeaveNetwork());

        }


        public void SetStartButtonActive (bool active) {
            beginGameButton.SetActive (active);
        }

        public void HostPublic () {
            lobbySelectables.ForEach (x => x.interactable = false);

            PlayerNetwork.localPlayer.HostGame (true);
        }

        public void HostPrivate () {
            lobbySelectables.ForEach (x => x.interactable = false);

            PlayerNetwork.localPlayer.HostGame (false);
        }

        public void HostSuccess (bool success, string matchID) {
            if (success) {
                //lobbyCanvas.SetActive(true);
                ViewManager.Show<LobbyMenuView>();

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (PlayerNetwork.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void Join () {
            lobbySelectables.ForEach (x => x.interactable = false);

            PlayerNetwork.localPlayer.JoinGame (joinMatchInput.text.ToUpper ());
        }

        public void JoinSuccess (bool success, string matchID) {
            if (success) {
                lobbyCanvas.SetActive(true);
                ViewManager.Show<LobbyMenuView>();

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (PlayerNetwork.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void DisconnectGame () {
            if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
            PlayerNetwork.localPlayer.DisconnectGame ();

            ViewManager.Show<ConnectMenuView>();
            //lobbyCanvas.SetActive(false);
            lobbySelectables.ForEach (x => x.interactable = true);
        }

        public GameObject SpawnPlayerUIPrefab (PlayerNetwork player) {
            GameObject newUIPlayer = Instantiate (UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
            newUIPlayer.transform.SetSiblingIndex (player.playerIndex - 1);

            return newUIPlayer;
        }

        public void BeginGame () {
            PlayerNetwork.localPlayer.BeginGame ();
            PermanentView.SetActive(false);
            
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
                    PlayerNetwork.localPlayer.SearchGame ();
                }
                yield return null;
            }
            //searchCanvas.SetActive(false);
            //ViewManager.Show<LobbyMenuView>();

        }


        IEnumerator SearchingRoom()
        {
            searchingAllRooms = true;

            float searchInterval = 1;
            float currentTime = 1;

            while (searchingAllRooms)
            {
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    currentTime = searchInterval;
                    List<string> matchs = new List<string>();
                    PlayerNetwork.localPlayer.CmdGetRooms();
                    for (int i = 0; i < matchs.Count; i++)
                    {
                        Debug.Log(matchs[i]);
                    }
                }
                yield return null;
            }
            //searchCanvas.SetActive(false);
            //ViewManager.Show<LobbyMenuView>();

        }

        public void GetRooms(List<List<string>> matchs)
        {
            for (int i = 0; i < ListAllRooms.Count; i++)
            {
                Destroy(ListAllRooms[i].gameObject);
            }
            ListAllRooms.Clear();

            for (int i = 0; i < matchs.Count; i++)
            {
                GameObject roomI = Instantiate(RoomI, ListRoomContener);
                ListAllRooms.Add(roomI);
                roomI.GetComponent<RoomX>().SetRoomUI(matchs[i]); //.roomName.text = $"{matchs[i][0]}  {matchs[i][1]}";
                //roomI.GetComponent<RoomX>().roomID = matchs[i][2];

            }
        }


    }
}