using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Tile[] tiles;

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

    [SerializeField]
    private Player[] players;

    [SerializeField] private GameObject xPiecePrefab;
    [SerializeField] private GameObject oPiecePrefab;
    [SerializeField] private GameObject[] gameoverPanels;
    [SerializeField] private UITurnText UIText;


    private Player currentActivePlayer;
    private int turnNumber = 1;
    private bool isGameOver;

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
    }

    private void RandomlyChooseStartingPlayer()
    {
        if (UnityEngine.Random.Range(0, 100) % 2 == 0)
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

        UIText.SetTurnText(currentActivePlayer);
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
                    tile.SetTilePiece(currentActivePlayer.state);
                    PlacePieceOnTile(tile);
                    turnNumber++;

                    if (CheckGameOver())
                    {
                        int winningPlayer = (currentActivePlayer == players[0] ? 0 : 1);
                        gameoverPanels[winningPlayer].SetActive(true);
                    }
                    SwitchTurns();
                }
            }

        }
    }

    private void PlacePieceOnTile(Tile tile)
    {
        if (currentActivePlayer.state == State.X)
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
                    return false;
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
        winningCombinationOfTiles[6] = new Tile[] { tiles[0], tiles[4], tiles[7] };
        winningCombinationOfTiles[7] = new Tile[] { tiles[2], tiles[4], tiles[6] };
    }


}