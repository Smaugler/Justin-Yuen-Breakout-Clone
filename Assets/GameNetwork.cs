using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameNetwork : NetworkManager
{
    public override void OnStartServer()
    {
        
    }

    public override void OnServerDisconnect(NetworkConnection _Conn)
    {

    }

    public override void OnClientConnect(NetworkConnection _Conn)
    {
        // Reset bricks in the scene when client connects
        GameManager.GetGameManager().ResetBricks();
    }

    public override void OnClientDisconnect(NetworkConnection _Conn)
    {

    }
}
