﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Tile[] tiles;

    private Tile[][] winningCombinationOfTiles = new Tile[][] { };

    [SerializeField]
    private Player[] players;

    private Player currentActivePlayer;
    private int turnNumber = 0;

    private void Awake()
    {
        if (instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        tiles = GetComponents<Tile>();
        SetupWinningCombinations();

        RandomlyChooseStartingPlayer();
    }

    private void RandomlyChooseStartingPlayer()
    {
        if (UnityEngine.Random.Range(0,100) % 2 == 0)
        {
            currentActivePlayer = players[0];
        }
        else
        {
            currentActivePlayer = players[1];
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {

        }
    }

    private void SwitchTurns()
    {
        currentActivePlayer = (currentActivePlayer == players[0]) ? players[1] : players[0];
    }

    private bool CheckGameOver()
    {
        for (int i = 0; i <= 8; i++)
        {
            if (winningCombinationOfTiles[i][0].CurrentPiece == winningCombinationOfTiles[i][1].CurrentPiece &&
                winningCombinationOfTiles[i][0].CurrentPiece == winningCombinationOfTiles[i][2].CurrentPiece)
            {
                return true;
            }
        }

        if (turnNumber <= 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    private void SetupWinningCombinations()
    {
        winningCombinationOfTiles[0] = new Tile[] { tiles[0], tiles[1], tiles[2] };
        winningCombinationOfTiles[1] = new Tile[] { tiles[3], tiles[4], tiles[5] };
        winningCombinationOfTiles[2] = new Tile[] { tiles[6], tiles[7], tiles[8] };
        winningCombinationOfTiles[3] = new Tile[] { tiles[0], tiles[3], tiles[6] };
        winningCombinationOfTiles[4] = new Tile[] { tiles[1], tiles[4], tiles[7] };
        winningCombinationOfTiles[5] = new Tile[] { tiles[2], tiles[5], tiles[8] };
        winningCombinationOfTiles[6] = new Tile[] { tiles[0], tiles[4], tiles[7] };
        winningCombinationOfTiles[7] = new Tile[] { tiles[2], tiles[4], tiles[6] };
    }


}