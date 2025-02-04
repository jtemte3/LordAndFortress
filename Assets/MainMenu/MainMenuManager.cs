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
        SetupFlagMaterial();

        loadedFaction = new FileUtils().LoadFactionFromFile();
        bannerRenderer.material.color = loadedFaction.color;
        
        for (int i = 0; i < customUnits.Count; i++)
        {
            customUnits[i].LoadUnit(loadedFaction.customUnits[i], loadedFaction.color);
        }
    }

    private void SetupFlagMaterial()
    {
        bannerRenderer.material = new Material(bannerRenderer.material);
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
