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
        // Get all reference to players
        PaddleController[] lPlayers = FindObjectsOfType<PaddleController>();

        // Destroy Player
        foreach (PaddleController _Player in lPlayers)
        {
            Destroy(_Player.gameObject);
        }

        _Conn.Disconnect();
    }

    public override void OnClientConnect(NetworkConnection _Conn)
    {
        // Reset bricks in the scene when client connects
        GameManager.GetGameManager().ResetBricks();
    }

    public override void OnClientDisconnect(NetworkConnection _Conn)
    {
        // Get all reference to players
        PaddleController[] lPlayers = FindObjectsOfType<PaddleController>();
        Brick[] lBricks = FindObjectsOfType<Brick>();

        // Destroy Player
        foreach (PaddleController _Player in lPlayers)
        {
            Destroy(_Player.gameObject);
        }

        // Destroy Player
        foreach (Brick _Brick in lBricks)
        {
            Destroy(_Brick.gameObject);
        }

        _Conn.Disconnect();
    }
}
