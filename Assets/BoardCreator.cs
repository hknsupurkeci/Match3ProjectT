using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public GameObject BackgroundObj;
    public GameObject InGameObj;
    public GameObject[] tasPrefablar; // Mavi, kýrmýzý, yeþil taþ prefablarý
    public GameObject hücrePrefab; // Hücre prefab'ý
    public int areaWidth = 8;
    public int areaHeight = 8;
    public float areaDepth = 1.0f;
    private GameObject[,] board;
    public GameObject destroyEffect;
    public GameObject matchEffect;
    private void Start()
    {
        board = new GameObject[areaHeight, areaWidth];
        CreateBoard();
    }
    void Update()
    {
        CheckMatches(); // Her Update döngüsünde taþ eþleþmelerini kontrol et
    }

    private void CheckMatches()
    {
        // Yatayda ve dikeyde taþ eþleþmelerini kontrol et
        for (int x = 0; x < areaHeight; x++)
        {
            for (int y = 0; y < areaWidth; y++)
            {
                GameObject currentObj = board[x, y];

                if (currentObj != null)
                {
                    // Yatay kontrol
                    if (y < areaWidth - 2)
                    {
                        GameObject obj1 = board[x, y + 1];
                        GameObject obj2 = board[x, y + 2];

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
                    if (x < areaHeight - 2)
                    {
                        GameObject obj1 = board[x + 1, y];
                        GameObject obj2 = board[x + 2, y];

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
        for (int x = 0; x < areaHeight; x++)
        {
            for (int y = 0; y < areaWidth; y++)
            {
                if(board[x,y] == null)
                {
                    //Prefabs
                    Vector3 hücreKonumu2 = new Vector3(xValue, yValue, -1);
                    GameObject randomPref = tasPrefablar[Random.Range(0, tasPrefablar.Length)];
                    GameObject newPref = Instantiate(randomPref, hücreKonumu2, Quaternion.identity);
                    Instantiate(matchEffect, newPref.transform.position, Quaternion.identity);

                    newPref.transform.SetParent(InGameObj.transform);
                    board[x, y] = newPref;
                    destroyCount++;
                }
                xValue += areaDepth;
            }
            xValue = 0;
            yValue -= areaDepth;
        }
        Debug.Log(destroyCount);
    }

    private void CreateBoard()
    {
        float xValue = 0;
        float yValue = 0;
        for (int x = 0; x < areaHeight; x++)
        {
            for (int y = 0; y < areaWidth; y++)
            {
                //Board
                Vector3 hücreKonumu = new Vector3(xValue, yValue, 0);
                GameObject yeniHücre = Instantiate(hücrePrefab, hücreKonumu, Quaternion.identity);
                yeniHücre.transform.SetParent(BackgroundObj.transform); // Oluþturulan hücreleri ebeveyn olarak ayarla
                //Prefabs
                Vector3 hücreKonumu2 = new Vector3(xValue, yValue, -1);
                GameObject randomPref = tasPrefablar[Random.Range(0, tasPrefablar.Length)];
                GameObject newPref = Instantiate(randomPref, hücreKonumu2, Quaternion.identity);
                newPref.transform.SetParent(InGameObj.transform);
                board[x, y] = newPref;
                xValue += areaDepth;
            }
            xValue = 0;
            yValue -= areaDepth;
        }
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
        for (int x = 0; x < areaHeight; x++)
        {
            for (int y = 0; y < areaWidth; y++)
            {
                if (board[x, y] == selectedObj)
                {
                    switch (moveOptions)
                    {
                        case MoveOptions.up:
                            if (x != 0)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board[x - 1, y].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board[x - 1, y].transform.position, targetPozSecondElement, board[x - 1, y]));
                                //index
                                board[x, y] = board[x - 1, y];
                                board[x - 1, y] = selectedObj;
                            }
                            break;
                        case MoveOptions.down:
                            if (x != areaHeight - 1)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board[x + 1, y].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board[x + 1, y].transform.position, targetPozSecondElement, board[x + 1, y]));
                                //index
                                board[x, y] = board[x + 1, y];
                                board[x + 1, y] = selectedObj;
                            }
                            break;
                        case MoveOptions.left:
                            if(y != 0)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board[x, y-1].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board[x, y-1].transform.position, targetPozSecondElement, board[x, y-1]));
                                //index
                                board[x, y] = board[x, y - 1];
                                board[x,y-1] = selectedObj;
                            }
                            break;
                        case MoveOptions.right:
                            if(y != areaWidth - 1)
                            {
                                //position
                                Vector3 targetPosSelectedElement = board[x, y + 1].transform.position;
                                Vector3 targetPozSecondElement = selectedObj.transform.position;
                                StartCoroutine(LerpPosition(selectedObj.transform.position, targetPosSelectedElement, selectedObj));
                                StartCoroutine(LerpPosition(board[x, y + 1].transform.position, targetPozSecondElement, board[x, y + 1]));
                                //index
                                board[x, y] = board[x, y + 1];
                                board[x, y + 1] = selectedObj;
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
