using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float defaultFovValue = 30;
    // Kameranýn bakýþ açýsý deðeri
    public float cameraFOV = 100.0f;
    // Ekran boyutunu ve çözünürlüðünü belirleyecek deðiþkenler
    private float screenWidth;
    private float screenHeight;

    public BoardCreator creator;

    private float maxWidth = 8;
    void Start()
    {
        cameraFOV = creator.boardWidth < maxWidth ? defaultFovValue - ((maxWidth - creator.boardWidth) * 2) : defaultFovValue;
        // Ekran boyutunu ve çözünürlüðünü al
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // Kameranýn bakýþ açýsýný ayarla
        float aspectRatio = screenWidth / screenHeight;
        Camera.main.fieldOfView = cameraFOV / aspectRatio;
    }

    void Update()
    {
       
        // Eðer ekran boyutu deðiþtiyse, kameranýn bakýþ açýsýný yeniden hesapla
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            float aspectRatio = screenWidth / screenHeight;
            Camera.main.fieldOfView = cameraFOV / aspectRatio;
        }
    }
}
