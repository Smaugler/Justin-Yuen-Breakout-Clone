using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameNetwork : NetworkManager
{
    public override void OnStartServer()
    {

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {

    }

    public override void OnServerReady(NetworkConnection conn)
    {
        GameManager.GetGameManager().ResetBricks();
    }
}
