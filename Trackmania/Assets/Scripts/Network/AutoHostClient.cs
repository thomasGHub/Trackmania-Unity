using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {
    public class AutoHostClient : MonoBehaviour {

        public string serverIP;

        [SerializeField] NetworkManager networkManager;

        public Button SoloButton;
        public Button OnlineButton;
        public Button LanButton;

        [Space(10)]
        public Button SoloPlay;
        public Button OnlinePlay;
        public Button LanPlay;


        void Start () {
            if (!Application.isBatchMode) { //Headless build
                //Debug.Log ($"=== Client Build ===");
                //networkManager.StartClient ();
            } else {
                //Debug.Log ($"=== Server Build ===");
            }

            ListenerUI();

            //Debug.Log(GetLocalIPv4());
        }

        public void ListenerUI()
        {
            OnlineButton.onClick.AddListener(() => {
                PlayerPrefs.SetInt("Multi", 1);
                networkManager.networkAddress = serverIP;
            }) ;
            LanButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("Multi", 1);
                networkManager.networkAddress = GetLocalIPv4();
            });

            SoloButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("Multi", -1);
            });

            OnlinePlay.onClick.AddListener(() => networkManager.StartClient());
            LanPlay.onClick.AddListener(() => StartConnect());

        }

        


        public void StartConnect()
        {
            try
            {
                networkManager.StartHost();
            }
            catch (System.Exception)
            {
                networkManager.StartClient();
            }
        }



        public string GetLocalIPv4()
        {
            //Debug.Log(Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString());
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.First(
                    f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToString();
        }

    }
}