using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Car;
using Newtonsoft.Json;
using System.IO;

namespace MirrorBasics
{

    [RequireComponent(typeof(NetworkMatch))]
    public class PlayerNetwork : NetworkBehaviour
    {

        public static PlayerNetwork localPlayer;
        [SyncVar] public string playerName;
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        NetworkMatch networkMatch;

        [SyncVar] public Match currentMatch;

        [SerializeField] GameObject playerLobbyUI;

        Guid netIDGuid;

        void Awake()
        {
            networkMatch = GetComponent<NetworkMatch>();
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                gameObject.GetComponent<Player>()._timerCount.ShowUI(true);
            }
            else
            {

                Debug.Log($"OnStart __________");

                gameObject.GetComponent<Player>()._timerCount.ShowUI(false);


                gameObject.GetComponent<Player>().SetCamPriorityNotLocalPlayer();
                gameObject.GetComponent<Player>().enabled = false;
                gameObject.GetComponent<Player>().DisableNotLocalPlayerCar();


            }
        }



        public override void OnStartServer()
        {
            netIDGuid = netId.ToString().ToGuid();
            networkMatch.matchId = netIDGuid;
        }

        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                localPlayer = this;
                //GameManager.SetPlayerReference(gameObject.GetComponent<Player>());
                CmdSendName(PlayerPrefs.GetString("UserName"));
                gameObject.GetComponent<Player>().SetCamPriorityLocalPlayer();
                
            }
            else
            {
                Debug.Log($"Spawn other player Prefab");
                playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab(this);

                gameObject.GetComponent<Player>().SetCamPriorityNotLocalPlayer();
                gameObject.GetComponent<Player>().enabled = false;
                gameObject.GetComponent<Player>().DisableNotLocalPlayerCar();

            }
        }

        public override void OnStopClient()
        {
            Debug.Log($"Client Stopped");
            ClientDisconnect();
        }

        public override void OnStopServer()
        {
            Debug.Log($"Client Stopped on Server");
            ServerDisconnect();
        }

        /* 
            HOST MATCH
        */

        public void HostGame(bool publicMatch)
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID, publicMatch);
        }

        [Command]
        void CmdHostGame(string _matchID, bool publicMatch)
        {
            matchID = _matchID;

            if (MatchMaker.instance.HostGame(_matchID, "", this, publicMatch, out playerIndex))
            {
                Debug.Log($"<color=green>Game hosted successfully</color>");
                networkMatch.matchId = _matchID.ToGuid();
                TargetHostGame(true, _matchID, playerIndex);
            }
            else
            {
                Debug.Log($"<color=red>Game hosted failed</color>");
                TargetHostGame(false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string _matchID, int _playerIndex)
        {
            string path = MapSaver.MapDataPath + "/" + "mapToPlay" + ".json";
            string json = File.ReadAllText(path);
            MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(json);
            CmdSendMapID(mapInfo.ID);

            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.instance.HostSuccess(success, _matchID);
        }

        /* 
            JOIN MATCH
        */


        public void JoinGame(string _inputID)
        {
            CmdJoinGame(_inputID);
            UILobby.instance.searchingAllRooms = false;
        }

        [Command]
        void CmdJoinGame(string _matchID)
        {
            matchID = _matchID;
            if (MatchMaker.instance.JoinGame(_matchID, this, out playerIndex))
            {
                Debug.Log($"<color=green>Game Joined successfully</color>");
                networkMatch.matchId = _matchID.ToGuid();

                TargetJoinGame(true, _matchID, playerIndex);

                //Host
                if (isServer && playerLobbyUI != null)
                {
                    playerLobbyUI.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"<color=red>Game Joined failed</color>");
                TargetJoinGame(false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string _matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} === {_matchID}");
            UILobby.instance.JoinSuccess(success, _matchID);

            CmdGetMapId();
        }


        [Command]
        void CmdSendMapID(string _mapID)
        {
            //Debug.Log("Server_________________________________" + _mapID);

            currentMatch.mapId = _mapID;
        }

        [Command]
        public void CmdGetMapId()
        {
            RpcGetMapId(currentMatch.mapId);
        }


        [TargetRpc]
        public void RpcGetMapId(string _mapId)
        {
            currentMatch.mapId = _mapId;
            StartCoroutine(GetMap());
        }


        private IEnumerator GetMap()
        {
            yield return new WaitForSeconds(0.5f); ;

            string mapId = currentMatch.mapId;

            string path = MapSaver.GetMapDirectory(mapId);
            Debug.LogWarning("Start Get Online Map01 " + mapId);

            if(path != null)
            {
                string json = File.ReadAllText(path + "/" + MapSaver.MapInfo);
                MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(json);
                MapSaver.SaveMapToPlay(mapInfo);
            }
            else
            {
                Debug.LogWarning("Start Get Online Map");
                DownloadingData downloadingMapInfo = new DownloadingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo, new FilterID(mapId), new Projection());
                DownloadingData downloadingMapData = new DownloadingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapData, new FilterID(mapId), new MapDataProjection());
                StartCoroutine(RequestManager.DownloadingSingleData(downloadingMapInfo, ReceiveMapInfo));
                StartCoroutine(RequestManager.DownloadingSingleData(downloadingMapData, ReceiveMapData));
            }
        }

        public void ReceiveMapInfo(string data)
        {
            SingleMapInfo myDeserializedClass = JsonConvert.DeserializeObject<SingleMapInfo>(data);
            MapSaver.SaveMapInfo(myDeserializedClass.MapInfo);
            MapSaver.SaveMapToPlay(myDeserializedClass.MapInfo);
        }

        public void ReceiveMapData(string data)
        {
            SingleListJsonData myDeserializedClass = JsonConvert.DeserializeObject<SingleListJsonData>(data);
            MapSaver.SaveMapData(myDeserializedClass.ListJsonData);
        }



        [Command]
        public void CmdGetRooms()
        {
            List<List<string>> matchs = new List<List<string>>();
            matchs = MatchMaker.instance.GetRooms();
            TargetGetRooms(matchs);
        }

        [TargetRpc]
        public void TargetGetRooms(List<List<string>> matchs)
        {
            UILobby.instance.GetRooms(matchs);
        }


        /* 
            DISCONNECT
        */

        public void DisconnectGame()
        {
            CmdDisconnectGame();
        }

        [Command]
        void CmdDisconnectGame()
        {
            ServerDisconnect();
        }

        void ServerDisconnect()
        {
            MatchMaker.instance.PlayerDisconnected(this, matchID);
            RpcDisconnectGame();
            networkMatch.matchId = netIDGuid;
        }

        [ClientRpc]
        void RpcDisconnectGame()
        {
            ClientDisconnect();
        }

        void ClientDisconnect()
        {
            if (playerLobbyUI != null)
            {
                if (!isServer)
                {
                    Destroy(playerLobbyUI);
                }
                else
                {
                    playerLobbyUI.SetActive(false);
                }
            }
        }

        /* 
            SEARCH MATCH
        */

        public void SearchGame()
        {
            CmdSearchGame();
        }

        [Command]
        void CmdSearchGame()
        {
            if (MatchMaker.instance.SearchGame(this, out playerIndex, out matchID))
            {
                Debug.Log($"<color=green>Game Found Successfully</color>");
                networkMatch.matchId = matchID.ToGuid();
                TargetSearchGame(true, matchID, playerIndex);

                //Host
                if (isServer && playerLobbyUI != null)
                {
                    playerLobbyUI.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"<color=red>Game Search Failed</color>");
                TargetSearchGame(false, matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetSearchGame(bool success, string _matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID} | {success}");
            UILobby.instance.SearchGameSuccess(success, _matchID);
        }

        /* 
            MATCH PLAYERS
        */

        [Server]
        public void PlayerCountUpdated(int playerCount)
        {
            TargetPlayerCountUpdated(playerCount);
        }

        [TargetRpc]
        void TargetPlayerCountUpdated(int playerCount)
        {
            if (playerCount > 1)
            {
                UILobby.instance.SetStartButtonActive(true);
            }
            else
            {
                UILobby.instance.SetStartButtonActive(false);
            }
        }

        /* 
            BEGIN MATCH
        */

        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        void CmdBeginGame()
        {
            MatchMaker.instance.BeginGame(matchID);
            Debug.Log($"<color=red>Game Beginning</color>");
        }

        public void StartGame()
        { //Server
            RpcBeginGame();
        }

        [ClientRpc]
        void RpcBeginGame()
        {
            //Debug.Log($"MatchID: {matchID} | Beginning | Index {playerIndex}");

            if (isLocalPlayer)
            {
                ViewManager.Show<InGameView>();
                LobbyPermanentView.ActivateView(false);
                StartCoroutine(LoadMapScence());

            }
            else
            {
            }

            //NetworkManager.singleton.ServerChangeScene("Online");
        }


        private IEnumerator LoadMapScence()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GameMap", LoadSceneMode.Additive);
            yield return asyncOperation.isDone;
            yield return new WaitForSeconds(1);
            GameManager.LanchRace();
        }




        public void OnLeaveNetwork()
        {
            //NetworkManager.singleton.StopHost();
            DisconnectGame();
        }

        [Command]
        void CmdSendName(string name)
        {
            playerName = name;
        }





        //# Send Score




        [Command]
        public void CmdSendScore(int _playerIndex, Temps _score, string _playerName)
        {
            RpcReceiveScore(_playerIndex, _score, _playerName);
        }

        [ClientRpc]
        void RpcReceiveScore(int _playerIndex, Temps _score, string _playerName)
        {
            //Debug.LogWarning("SetScoreLocalPlayerNetwork"+ this.playerIndex);
            //PlayerNetwork.localPlayer.currentMatch.playersScores[playerIndex - 1] = _score;
            InGameView.instance.dicTemps[playerIndex - 1] = _score;
            InGameView.instance.dicNames[playerIndex - 1] = _playerName;
            InGameView.instance.DoLeadarboardInGameView();
            
        }

    }
}