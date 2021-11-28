using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
	private void Awake()
	{
		// Ignore all collisions between balls in Ball layer
		Physics.IgnoreLayerCollision(3, 3);
	}

    private void OnCollisionExit(Collision _Collision)
    {
		// Check if object is a brick
		if (_Collision.transform.tag == "Brick")
		{
			// Reset all player balls if this is the last brick
			if (GameManager.GetGameManager().GetBrickCount() - 1 <= 0)
			{
				// Get all reference to players
				PaddleController[] lPlayers = FindObjectsOfType<PaddleController>();

				// Reset ball
				foreach (PaddleController _Player in lPlayers)
				{
					_Player.ResetBall();
				}
			}

			// Destroy brick and gain score
			GameManager.GetGameManager().DestroyedBrick(_Collision.gameObject);
		}
	}
}
