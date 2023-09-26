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
                    // Hareket yönlendirmesi hesaplamasý burada yapýlabilir
                    Vector3 pos = dokunus.position;
                    pos.z = 10;
                    Vector2 fareSonPozisyonu = Camera.main.ScreenToWorldPoint(pos);

                    Vector2 hareketYönü = fareSonPozisyonu - fareBaslangicPozisyonu;

                    Debug.Log("Dokunma Bitti. Hareket Yönü: " + hareketYönü);

                    if(Mathf.Abs(hareketYönü.x) < Mathf.Abs(hareketYönü.y))
                    {
                        //poz y
                        if (hareketYönü.y < 0)
                            boardCreator.PrefMovingControl(MoveOptions.down, seciliObj);
                        else if(hareketYönü.y > 0)
                            boardCreator.PrefMovingControl(MoveOptions.up, seciliObj);
                    }
                    else
                    {
                        //poz x
                        if (hareketYönü.x < 0)
                            boardCreator.PrefMovingControl(MoveOptions.left, seciliObj);
                        else if(hareketYönü.x > 0)
                            boardCreator.PrefMovingControl(MoveOptions.right, seciliObj);
                    }
                    objMoving = false;
                }
            }
        }
    }
}
