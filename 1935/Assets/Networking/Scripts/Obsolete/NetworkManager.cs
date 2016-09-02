using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public Text serverJoiningInformation;
    public Text serverPlayerCount;
    const string typeName = "1935";
    const string gameName = "DefaultRoom";
    private HostData[] hostList;
    
    void Start()
    {
        MasterServer.ipAddress = "127.0.0.1"; //Sets master server to local host.
    }

    void Update()
    {
        
    }


    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
            {
                StartServer();
            }
            
            if (GUI.Button(new Rect(100, 250,250,100),"Refresh Hosts"))
            {
                RefreshHostList();
            }

            if (hostList != null)
            {
                //If a host list is not empty, it creates a button for each host that calls JoinServer using that host's data.
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                    {
                        JoinServer(hostList[i]);
                    }
                }
            }
        }
    }

    void StartServer()
    {
        
        Network.InitializeServer(4, 2500, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
        serverJoiningInformation.text = "Server Initialized";
        Debug.Log("Server Initialized");
        NetworkServer.SpawnObjects();



    }

    void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName); //Requests active rooms.
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived) //After requesting the active rooms, this checks if we have gotten a response.
        {
            hostList = MasterServer.PollHostList(); //If we got a response, save the received host list inside hostList variable.
        }
    }

    void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
        
    }

    void OnConnectedToServer()
    {
        serverJoiningInformation.text = "Joined Server";
        Debug.Log("Joined Server");
    }
}
