using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Tile[] tiles;
    [SerializeField] private Player[] players;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject xPiecePrefab;
    [SerializeField] private GameObject oPiecePrefab;
    [SerializeField] private UITurnText UIText;
    [SerializeField] private UserTextInput userTextInput;


    [Header("Ranges for Randomizing Starting Player")]
    [SerializeField] private int MinRange = 0;
    [SerializeField] private int MaxRange = 100;

    [Header("Debug Options")]
    [SerializeField] private bool autoPlay = false;
    [SerializeField] private Player firstAIPlayer;
    [SerializeField] private Player secondAIPlayer;
    [SerializeField] private float durationBetweenMoves = 5f;
    [SerializeField] private float waitTimeBetweenGamesInSeconds = 2f;
    private float currentTime = 1f;

    private Player currentActivePlayer;
    private int turnNumber = 1;
    private bool isGameOver = true;
    private int sceneIndex;
    private bool isDraw;
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

        if(autoPlay == false)
            userTextInput.SetUpInputPanel(players);

        SetupWinningCombinations();
        RandomlyChooseStartingPlayer();       
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void SetupUIPlayerText()
    {
        if(autoPlay == false)
        {
            userTextInput.SetInputToPlayerName(players);
            userTextInput.AmendPlayerName(players);
            UIText.SetTurnText(currentActivePlayer);
            UIText.SetPlayerPieceText(players);
            isGameOver = false;
        }
    }
    private void RandomlyChooseStartingPlayer()
    {
        if(autoPlay)
        {
            currentActivePlayer = firstAIPlayer;
            firstAIPlayer.SetPlayerPiece(State.X);
            secondAIPlayer.SetPlayerPiece(State.O);
            return;
        }
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
        if(autoPlay)
        {
            HandleAutoPlay();
            return;
        }

        if (Input.GetMouseButtonDown(0) && isGameOver != true)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.GetComponent<Tile>() != null)
                {
                    var tile = raycastHit.collider.GetComponent<Tile>();
                    bool isTileUnOccupied = tile.CurrentState == State.Undecided;

                    if (isTileUnOccupied)
                    {                        
                        PlacePieceOnTile(tile);
                        turnNumber++;
                    }
                    else
                    {
                        return;
                    }

                    if(CheckGameOver())
                    {                       
                        gameOverPanel.GetComponentInChildren<Text>().text = "GAME OVER\n\n" + currentActivePlayer.Name + " Wins!";
                        gameOverPanel.SetActive(true);
                    }

                    SwitchTurns();
                }
            }

        }
    }
    private void HandleAutoPlay()
    {
        currentTime += currentTime * Time.deltaTime;

        if (currentTime >= durationBetweenMoves)
        {
            int randomTileIndex = UnityEngine.Random.Range(0, tiles.Length);
            if (tiles[randomTileIndex].CurrentState == State.Undecided)
            {
                PlacePieceOnTile(tiles[randomTileIndex]);
                turnNumber++;
                if(CheckGameOver())
                {
                    StartCoroutine(WaitForTwoSeconds());
                    currentTime = 1f;
                    currentActivePlayer = firstAIPlayer;
                    return;
                }
            }
            else
            {
                return;
            }

            currentActivePlayer = currentActivePlayer == firstAIPlayer ? secondAIPlayer : firstAIPlayer;
            currentTime = 1f;
        }
    }

    private IEnumerator WaitForTwoSeconds()
    {
        yield return new WaitForSeconds(waitTimeBetweenGamesInSeconds);
        ResetBoard();
    }

    private void ResetBoard()
    {
        foreach (Tile tile in tiles)
        {
            tile.ClearTile();
        }
    }
    private void PlacePieceOnTile(Tile tile)
    {

        tile.SetTilePiece(currentActivePlayer.Piece);

        if (currentActivePlayer.Piece == State.X)
        {
            var piece = Instantiate(xPiecePrefab, tile.transform.position, Quaternion.identity);
            piece.transform.SetParent(tile.transform);
        }
        else
        {
            var piece = Instantiate(oPiecePrefab, tile.transform.position, Quaternion.identity);
            piece.transform.SetParent(tile.transform);
        }
    }
    public bool CheckGameOver()
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
                turnNumber = 0;
                return isGameOver = true;
            }
        }

        if (turnNumber > 9)
        {
            isDraw = true;
            isGameOver = true;
            turnNumber = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SwitchTurns()
    {
        currentActivePlayer = (currentActivePlayer == players[0]) ? players[1] : players[0];
        UIText.SetTurnText(currentActivePlayer);
    } 

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void Quit()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
        Debug.Log("Application Quit");
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
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