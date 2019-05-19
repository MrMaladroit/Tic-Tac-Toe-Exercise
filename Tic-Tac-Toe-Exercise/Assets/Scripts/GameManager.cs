using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Tile[] tiles;
    [SerializeField] private Player[] players;
    [SerializeField] private GameObject[] gameoverPanels;
    [SerializeField] private GameObject xPiecePrefab;
    [SerializeField] private GameObject oPiecePrefab;
    [SerializeField] private UITurnText UIText;

    [Header("Ranges for Randomizing Starting Player")]
    [SerializeField] private int MinRange = 0;
    [SerializeField] private int MaxRange = 100;

    private Player currentActivePlayer;
    private int turnNumber = 1;
    private bool isGameOver;

    private Tile[][] winningCombinationOfTiles = new Tile[8][] {
        new Tile[3],
        new Tile[3],
        new Tile[3],
        new Tile[3],
        new Tile[3],
        new Tile[3],
        new Tile[3],
        new Tile[3]
    };
    private int sceneIndex;

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }

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

        SetupWinningCombinations();
        RandomlyChooseStartingPlayer();
        SetupUIPlayerText();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void SetupUIPlayerText()
    {
        UIText.SetTurnText(currentActivePlayer);
        UIText.SetPlayerPieceText(players);
    }

    private void RandomlyChooseStartingPlayer()
    {
        if (UnityEngine.Random.Range(MinRange, MaxRange) % 2 == 0)
        {
            currentActivePlayer = players[0];
            players[0].SetPlayerPiece(State.X);
            players[1].SetPlayerPiece(State.O);
        }
        else
        {
            currentActivePlayer = players[1];
            players[1].SetPlayerPiece(State.X);
            players[0].SetPlayerPiece(State.O);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && isGameOver != true)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.GetComponent<Tile>() != null)
                {
                    var tile = raycastHit.collider.GetComponent<Tile>();
                    tile.SetTilePiece(currentActivePlayer.Piece);
                    PlacePieceOnTile(tile);
                    turnNumber++;

                    if (CheckGameOver())
                    {
                        int winningPlayer = (currentActivePlayer == players[0] ? 0 : 1);                        
                        gameoverPanels[winningPlayer].SetActive(true);
                        return;
                    }
                    SwitchTurns();
                }
            }

        }
    }

    private void PlacePieceOnTile(Tile tile)
    {
        if (currentActivePlayer.Piece == State.X)
        {
            Instantiate(xPiecePrefab, tile.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(oPiecePrefab, tile.transform.position, Quaternion.identity);
        }
    }

    private void SwitchTurns()
    {
        currentActivePlayer = (currentActivePlayer == players[0]) ? players[1] : players[0];
        UIText.SetTurnText(currentActivePlayer);
    }

    private bool CheckGameOver()
    {
        for (int i = 0; i <= winningCombinationOfTiles.Length - 1; i++)
        {
            if (winningCombinationOfTiles[i][0].CurrentState == winningCombinationOfTiles[i][1].CurrentState &&
                winningCombinationOfTiles[i][0].CurrentState == winningCombinationOfTiles[i][2].CurrentState)
            {
                if (winningCombinationOfTiles[i][0].CurrentState == State.Undecided ||
                    winningCombinationOfTiles[i][1].CurrentState == State.Undecided ||
                    winningCombinationOfTiles[i][2].CurrentState == State.Undecided)
                {
                    continue;
                }

                return isGameOver = true;
            }
        }

        if (turnNumber > 9)
        {
            gameoverPanels[2].gameObject.SetActive(true);
            isGameOver = true;
            return false;
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
        winningCombinationOfTiles[6] = new Tile[] { tiles[0], tiles[4], tiles[8] };
        winningCombinationOfTiles[7] = new Tile[] { tiles[2], tiles[4], tiles[6] };
    }


}