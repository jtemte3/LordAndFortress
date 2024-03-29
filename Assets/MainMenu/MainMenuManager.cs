using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Renderer bannerRenderer;
    public CustomFactionObject loadedFaction;
    public List<CustomUnitLoader> customUnits;
    // Start is called before the first frame update
    void Start()
    {
        
        loadedFaction = new FileUtils().LoadFactionFromFile();
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
