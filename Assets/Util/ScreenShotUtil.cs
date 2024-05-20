using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotUtil : MonoBehaviour
{
    public Camera screenShotCamera;

    public void CaptureImage(string fileName)
    {
        RenderTexture activeRenderTexture = new RenderTexture(600, 600, 24, RenderTextureFormat.ARGB32);
        screenShotCamera.targetTexture = activeRenderTexture;

        RenderTexture.active = activeRenderTexture;

        screenShotCamera.Render();

        Texture2D screenShot = new Texture2D(screenShotCamera.targetTexture.width, screenShotCamera.targetTexture.height, TextureFormat.ARGB32, false, true);
        screenShot.ReadPixels(new Rect(0, 0, screenShotCamera.targetTexture.width, screenShotCamera.targetTexture.height), 0, 0);
        screenShot.Apply();

        RenderTexture.active = null;

        new FileUtils().SaveImageToFile(screenShot, fileName);

        Destroy(screenShot);
    }
}
