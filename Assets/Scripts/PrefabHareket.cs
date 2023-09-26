using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveOptions
{
    up,
    down, 
    left, 
    right
}
public class PrefabHareket : MonoBehaviour
{
    private GameObject seciliObj;
    private Vector2 fareBaslangicPozisyonu;
    private bool objMoving = false;
    public BoardCreator boardCreator;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch dokunus = Input.GetTouch(0);

            if (dokunus.phase == TouchPhase.Began)
            {
                Vector3 pos = dokunus.position;
                pos.z = 10;
                fareBaslangicPozisyonu = Camera.main.ScreenToWorldPoint(pos);

                RaycastHit2D hit = Physics2D.Raycast(fareBaslangicPozisyonu, Vector2.zero);

                if (hit.collider != null)
                {
                    seciliObj = hit.collider.gameObject;
                    objMoving = true;
                }
            }
            else if (dokunus.phase == TouchPhase.Ended)
            {
                if (objMoving)
                {
                    // Hareket y�nlendirmesi hesaplamas� burada yap�labilir
                    Vector3 pos = dokunus.position;
                    pos.z = 10;
                    Vector2 fareSonPozisyonu = Camera.main.ScreenToWorldPoint(pos);

                    Vector2 hareketY�n� = fareSonPozisyonu - fareBaslangicPozisyonu;

                    Debug.Log("Dokunma Bitti. Hareket Y�n�: " + hareketY�n�);

                    if(Mathf.Abs(hareketY�n�.x) < Mathf.Abs(hareketY�n�.y))
                    {
                        //poz y
                        if (hareketY�n�.y < 0)
                            boardCreator.PrefMovingControl(MoveOptions.down, seciliObj);
                        else if(hareketY�n�.y > 0)
                            boardCreator.PrefMovingControl(MoveOptions.up, seciliObj);
                    }
                    else
                    {
                        //poz x
                        if (hareketY�n�.x < 0)
                            boardCreator.PrefMovingControl(MoveOptions.left, seciliObj);
                        else if(hareketY�n�.x > 0)
                            boardCreator.PrefMovingControl(MoveOptions.right, seciliObj);
                    }
                    objMoving = false;
                }
            }
        }
    }
}
