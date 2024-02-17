using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private string factionJsonPath;
    public Renderer bannerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        factionJsonPath = Application.streamingAssetsPath + "/FactionCustomizer/CurrentFaction.json";
        Debug.Log("MainMenuManager json path:" + factionJsonPath);

        StreamReader reader = new(factionJsonPath);
        string configJson = reader.ReadToEnd();

        CustomFactionObject loadedFaction = JsonUtility.FromJson<CustomFactionObject>(configJson);
        bannerRenderer.material.color = loadedFaction.color;
        reader.Close();
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void ExitProgram()
    {
        Application.Quit();
    }
}
