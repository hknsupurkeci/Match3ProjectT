using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme : MonoBehaviour
{
    public GameObject[] taþPrefablar; // Mavi, kýrmýzý, yeþil taþ prefablarý
    public GameObject hücrePrefab; // Hücre prefab'ý
    public int alanGeniþliði = 8;
    public int alanYüksekliði = 8;
    public float hücreAralýðý = 1.0f;

    private void Start()
    {
        OyunAlanýnýOluþtur();
    }

    private void OyunAlanýnýOluþtur()
    {
        for (int x = 0; x < alanGeniþliði; x++)
        {
            for (int y = 0; y < alanYüksekliði; y++)
            {
                Vector3 hücreKonumu = new Vector3(x * hücreAralýðý, y * hücreAralýðý, 0);
                GameObject yeniHücre = Instantiate(hücrePrefab, hücreKonumu, Quaternion.identity);
                yeniHücre.transform.SetParent(transform); // Oluþturulan hücreleri ebeveyn olarak ayarla

                Vector3 hücreKonumu2 = new Vector3(x * hücreAralýðý, y * hücreAralýðý, 0);
                GameObject rastgeleTaþPrefab = taþPrefablar[Random.Range(0, taþPrefablar.Length)];
                GameObject yeniTaþ = Instantiate(rastgeleTaþPrefab, hücreKonumu2, Quaternion.identity);
                yeniTaþ.transform.SetParent(transform);
            }
        }
    }
}
