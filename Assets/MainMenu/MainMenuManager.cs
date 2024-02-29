using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private string factionJsonPath;
    public Renderer bannerRenderer;
    public CustomFactionObject loadedFaction;
    public List<CustomUnitLoader> customUnits;
    // Start is called before the first frame update
    void Start()
    {
        factionJsonPath = Application.streamingAssetsPath + "/FactionCustomizer/CurrentFaction.json";
        Debug.Log("MainMenuManager json path:" + factionJsonPath);

        StreamReader reader = new(factionJsonPath);
        string configJson = reader.ReadToEnd();
        reader.Close();

        loadedFaction = JsonUtility.FromJson<CustomFactionObject>(configJson);
        bannerRenderer.material.color = loadedFaction.color;
        
        foreach(CustomUnitLoader unit in customUnits)
        {
            unit.LoadUnit(loadedFaction.customUnits[0], loadedFaction.color);
        }
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
