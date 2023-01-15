﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Mirror;
using UnityEngine;

namespace MirrorBasics
{

    [System.Serializable]
    public class Match
    {
        public string matchID;
        public bool publicMatch;
        public bool inMatch;
        public bool matchFull;
        public string mapId;
        public List<PlayerNetwork> players = new List<PlayerNetwork>();
        //public List<Temps> playersScores = new List<Temps>();
        public GameMode gameMode;
        public bool isRoundEnding;
        public int nbRound;

        public Match(string matchID, string mapId, PlayerNetwork player, bool publicMatch, GameMode _gameMode)
        {
            matchFull = false;
            inMatch = false;
            this.matchID = matchID;
            this.mapId = mapId;
            this.publicMatch = publicMatch;
            players.Add(player);
            //Temps temps = new Temps(0,0,0);
            //playersScores.Add(temps);
            gameMode = _gameMode;
            nbRound = 1;
        }

        public Match() { }
    }

    public class MatchMaker : NetworkBehaviour
    {

        public static MatchMaker instance;

        public readonly SyncList<Match> matches = new SyncList<Match>();
        public readonly SyncList<String> matchIDs = new SyncList<String>();
        [SerializeField] int maxMatchPlayers = 12;

        void Start()
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        public bool HostGame(string _matchID, string _mapId, PlayerNetwork _player, bool publicMatch, out int playerIndex)
        {
            playerIndex = -1;

            if (!matchIDs.Contains(_matchID))
            {
                matchIDs.Add(_matchID);

                Match match = new Match(_matchID, _mapId, _player, publicMatch, new GameMode());
                matches.Add(match);
                //Debug.Log ($"Match generated");
                _player.currentMatch = match;
                playerIndex = 1;
                return true;
            }
            else
            {
                Debug.Log($"Match ID already exists");
                return false;
            }
        }

        public bool JoinGame(string _matchID, PlayerNetwork _player, out int playerIndex)
        {
            playerIndex = -1;

            if (matchIDs.Contains(_matchID))
            {

                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].matchID == _matchID)
                    {
                        if (!matches[i].inMatch && !matches[i].matchFull)
                        {
                            matches[i].players.Add(_player);
                            //matches[i].playersScores.Add (null);
                            _player.currentMatch = matches[i];
                            playerIndex = matches[i].players.Count;

                            matches[i].players[0].PlayerCountUpdated(matches[i].players.Count);

                            if (matches[i].players.Count == maxMatchPlayers)
                            {
                                matches[i].matchFull = true;
                            }

                            break;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                Debug.Log($"Match joined");
                return true;
            }
            else
            {
                Debug.Log($"Match ID does not exist");
                return false;
            }
        }

        public bool SearchGame(PlayerNetwork _player, out int playerIndex, out string matchID)
        {
            playerIndex = -1;
            matchID = "";

            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log($"Checking match {matches[i].matchID} | inMatch {matches[i].inMatch} | matchFull {matches[i].matchFull} | publicMatch {matches[i].publicMatch}");
                if (!matches[i].inMatch && !matches[i].matchFull && matches[i].publicMatch)
                {
                    if (JoinGame(matches[i].matchID, _player, out playerIndex))
                    {
                        matchID = matches[i].matchID;
                        return true;
                    }
                }
            }

            return false;
        }

        public List<List<string>> GetRooms()
        {
            List<List<string>> matchs = new List<List<string>>();
            for (int i = 0; i < matches.Count; i++)
            {
                //Debug.Log($"Get match {matches[i].matchID} | inMatch {matches[i].inMatch} | matchFull {matches[i].matchFull} | publicMatch {matches[i].publicMatch}");
                if (!matches[i].matchFull && matches[i].publicMatch)
                {
                    List<string> sousmatch = new List<string>();
                    sousmatch.Add(matches[i].players[0].playerName);
                    sousmatch.Add(matches[i].players.Count.ToString());
                    sousmatch.Add(matches[i].matchID);
                    matchs.Add(sousmatch);
                }
            }
            return matchs;
        }



        public void BeginGame(string _matchID)
        {   //Server
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == _matchID)
                {
                    matches[i].inMatch = true;
                    foreach (var player in matches[i].players)
                    {
                        player.StartGame();
                        //Debug.Log("Start " + player.playerIndex);
                    }
                    break;
                }
            }
        }

        public static string GetRandomMatchID()
        {
            string _id = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);
                if (random < 26)
                {
                    _id += (char)(random + 65);
                }
                else
                {
                    _id += (random - 26).ToString();
                }
            }
            Debug.Log($"Random Match ID: {_id}");
            return _id;
        }

        public void PlayerDisconnected(PlayerNetwork player, string _matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == _matchID)
                {
                    int playerIndex = matches[i].players.IndexOf(player);
                    if (matches[i].players.Count > playerIndex)
                    {
                        matches[i].players.RemoveAt(playerIndex);
                        //matches[i].playersScores.RemoveAt(playerIndex);
                    }
                    Debug.Log($"PlayerNetwork disconnected from match {_matchID} | {matches[i].players.Count} players remaining");

                    if (matches[i].players.Count == 0)
                    {
                        Debug.Log($"No more players in Match. Terminating {_matchID}");
                        matches.RemoveAt(i);
                        matchIDs.Remove(_matchID);
                    }
                    else
                    {
                        matches[i].players[0].PlayerCountUpdated(matches[i].players.Count);
                    }
                    break;
                }
            }
        }

    }

    public static class MatchExtensions
    {
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            return new Guid(hashBytes);
        }
    }

}