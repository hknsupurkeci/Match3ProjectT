using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme : MonoBehaviour
{
    public GameObject[] ta�Prefablar; // Mavi, k�rm�z�, ye�il ta� prefablar�
    public GameObject h�crePrefab; // H�cre prefab'�
    public int alanGeni�li�i = 8;
    public int alanY�ksekli�i = 8;
    public float h�creAral��� = 1.0f;

    private void Start()
    {
        OyunAlan�n�Olu�tur();
    }

    private void OyunAlan�n�Olu�tur()
    {
        for (int x = 0; x < alanGeni�li�i; x++)
        {
            for (int y = 0; y < alanY�ksekli�i; y++)
            {
                Vector3 h�creKonumu = new Vector3(x * h�creAral���, y * h�creAral���, 0);
                GameObject yeniH�cre = Instantiate(h�crePrefab, h�creKonumu, Quaternion.identity);
                yeniH�cre.transform.SetParent(transform); // Olu�turulan h�creleri ebeveyn olarak ayarla

                Vector3 h�creKonumu2 = new Vector3(x * h�creAral���, y * h�creAral���, 0);
                GameObject rastgeleTa�Prefab = ta�Prefablar[Random.Range(0, ta�Prefablar.Length)];
                GameObject yeniTa� = Instantiate(rastgeleTa�Prefab, h�creKonumu2, Quaternion.identity);
                yeniTa�.transform.SetParent(transform);
            }
        }
    }
}
