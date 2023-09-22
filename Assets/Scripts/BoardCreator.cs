using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public GameObject backgroundObject;
    public GameObject inGameObject;
    public GameObject[] tilePrefabs;
    public GameObject cellPrefab;
    public int boardWidth = 8;
    public int boardHeight = 8;
    public float boardDepth = 1.0f;
    //private GameObject[,] board;
    public GameObject destroyEffect;
    public GameObject matchEffect;

    private Board board;

    private void Start()
    {
        board = new Board(boardWidth, boardHeight);
        CreateBoard();
    }

    void Update()
    {
        CheckMatches();
    }

    private void CheckMatches()
    {
        // Yatayda ve dikeyde taþ eþleþmelerini kontrol et
        for (int x = 0; x < boardHeight; x++)
        {
            for (int y = 0; y < boardWidth; y++)
            {
                GameObject currentObj = board.boardArray[x, y];

                if (currentObj != null)
                {
                    // Yatay kontrol
                    if (y < boardWidth - 2)
                    {
                        GameObject obj1 = board.boardArray[x, y + 1];
                        GameObject obj2 = board.boardArray[x, y + 2];

                        if (obj1 != null && obj2 != null)
                        {
                            if (currentObj.tag == obj1.tag && currentObj.tag == obj2.tag)
                            {
                                Debug.Log("Yatay eþleþme bulundu: " + currentObj.tag);
                                // Eþleþme bulunduðunda yapýlacak iþlemleri burada gerçekleþtirin
                                StartCoroutine(FillBoardCoroutine(currentObj, obj1, obj2));
                            }
                        }
                    }

                    // Dikey kontrol
                    if (x < boardHeight - 2)
                    {
                        GameObject obj1 = board.boardArray[x + 1, y];
                        GameObject obj2 = board.boardArray[x + 2, y];

                        if (obj1 != null && obj2 != null)
                        {
                            if (currentObj.tag == obj1.tag && currentObj.tag == obj2.tag)
                            {
                                Debug.Log("Dikey eþleþme bulundu: " + currentObj.tag);
                                // Eþleþme bulunduðunda yapýlacak iþlemleri burada gerçekleþtirin
                                StartCoroutine(FillBoardCoroutine(currentObj, obj1, obj2));
                            }
                        }
                    }
                }
            }
        }
    }
    private IEnumerator FillBoardCoroutine(GameObject currentObj, GameObject obj1, GameObject obj2)
    {
        yield return new WaitForSeconds(0.4f);
        //destroy and instantiate
        Instantiate(destroyEffect, currentObj.transform.position, Quaternion.identity);
        Instantiate(destroyEffect, obj1.transform.position, Quaternion.identity);
        Instantiate(destroyEffect, obj2.transform.position, Quaternion.identity);
        Destroy(currentObj);
        Destroy(obj1);
        Destroy(obj2);
        yield return new WaitForSeconds(0.5f); // Biraz bekleme süresi ekleyebilirsin

        int destroyCount = 0;
        float xValue = 0;
        float yValue = 0;
        for (int x = 0; x < boardHeight; x++)
        {
            for (int y = 0; y < boardWidth; y++)
            {
                if (board.boardArray[x, y] == null)
                {
                    //Prefabs
                    Vector3 hücreKonumu2 = new Vector3(xValue, yValue, -1);
                    GameObject randomPref = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                    GameObject newPref = Instantiate(randomPref, hücreKonumu2, Quaternion.identity);
                    Instantiate(matchEffect, newPref.transform.position, Quaternion.identity);

                    newPref.transform.SetParent(inGameObject.transform);
                    board.boardArray[x, y] = newPref;
                    destroyCount++;
                }
                xValue += boardDepth;
            }
            xValue = 0;
            yValue -= boardDepth;
        }
        Debug.Log(destroyCount);
    }

    private void CreateBoard()
    {
        float xValue = 0;
        float yValue = 0;
        for (int x = 0; x < boardHeight; x++)
        {
            for (int y = 0; y < boardWidth; y++)
            {
                // Board
                Vector3 cellPosition = new Vector3(xValue, yValue, 0);
                GameObject newCell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                newCell.transform.SetParent(backgroundObject.transform);

                // Prefabs
                Vector3 randomStartPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(10f, 20f), -1); // Örnek rastgele baþlangýç pozisyonlarý
                GameObject tilePrefab = GetValidTile(x, y);
                GameObject newTile = Instantiate(tilePrefab, randomStartPosition, Quaternion.identity);
                newTile.transform.SetParent(inGameObject.transform);
                board.boardArray[x, y] = newTile;

                // Lerp hareketi için coroutine baþlat
                StartCoroutine(MoveTileToPosition(newTile, cellPosition));

                xValue += boardDepth;
            }
            xValue = 0;
            yValue -= boardDepth;
        }
    }


    IEnumerator MoveTileToPosition(GameObject tile, Vector3 targetPosition)
    {
        float lerpTime = 2f; // Bu süreyi ayarlayarak hareketin ne kadar süre alacaðýný belirleyebilirsiniz.
        float startTime = Time.time;
        Vector3 startPosition = tile.transform.position;

        while (Time.time - startTime < lerpTime)
        {
            float t = (Time.time - startTime) / lerpTime;
            //tile.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        tile.transform.position = targetPosition;
    }


    private GameObject GetValidTile(int x, int y)
    {
        List<GameObject> possibleTiles = new List<GameObject>(tilePrefabs);
        while (possibleTiles.Count > 0)
        {
            GameObject randomTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
            if (IsValidPlacement(x, y, randomTile))
            {
                return randomTile;
            }
            else
            {
                possibleTiles.Remove(randomTile);
            }
        }
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)]; // Fallback, bu durumun olmamasý gerekir.
    }

    private bool IsValidPlacement(int x, int y, GameObject tile)
    {
        if (x >= 2)
        {
            if (board.boardArray[x - 1, y].tag == tile.tag && board.boardArray[x - 2, y].tag == tile.tag)
            {
                return false;
            }
        }
        if (y >= 2)
        {
            if (board.boardArray[x, y - 1].tag == tile.tag && board.boardArray[x, y - 2].tag == tile.tag)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator LerpPosition(Vector3 startPos, Vector3 targetPos, GameObject obj)
    {
        float lerpDuration = 0.3f; // Hareket süresi (saniye)
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            obj.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Ýþlem sonrasý pozisyonu kesinleþtirin
        obj.transform.position = new Vector3(targetPos.x, targetPos.y, -1);
    }

    public void PrefMovingControl(MoveOptions moveOptions, GameObject selectedObj)
    {
        bool isCompleted = false;
        for (int x = 0; x < boardHeight; x++)
        {
            for (int y = 0; y < boardWidth; y++)
            {
                if (board.boardArray[x, y] == selectedObj)
                {
                    switch (moveOptions)
                    {
                        case MoveOptions.up:
                            if (x != 0)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board.boardArray[x - 1, y].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board.boardArray[x - 1, y].transform.position, targetPozSecondElement, board.boardArray[x - 1, y]));
                                //index
                                board.boardArray[x, y] = board.boardArray[x - 1, y];
                                board.boardArray[x - 1, y] = selectedObj;
                            }
                            break;
                        case MoveOptions.down:
                            if (x != boardHeight - 1)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board.boardArray[x + 1, y].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board.boardArray[x + 1, y].transform.position, targetPozSecondElement, board.boardArray[x + 1, y]));
                                //index
                                board.boardArray[x, y] = board.boardArray[x + 1, y];
                                board.boardArray[x + 1, y] = selectedObj;
                            }
                            break;
                        case MoveOptions.left:
                            if (y != 0)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board.boardArray[x, y - 1].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board.boardArray[x, y - 1].transform.position, targetPozSecondElement, board.boardArray[x, y - 1]));
                                //index
                                board.boardArray[x, y] = board.boardArray[x, y - 1];
                                board.boardArray[x, y - 1] = selectedObj;
                            }
                            break;
                        case MoveOptions.right:
                            if (y != boardWidth - 1)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board.boardArray[x, y + 1].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board.boardArray[x, y + 1].transform.position, targetPozSecondElement, board.boardArray[x, y + 1]));
                                //index
                                board.boardArray[x, y] = board.boardArray[x, y + 1];
                                board.boardArray[x, y + 1] = selectedObj;
                            }
                            break;
                        default:
                            break;
                    }
                    isCompleted = true;
                    break;
                }
            }
            if (isCompleted)
                break;
        }
    }
}
