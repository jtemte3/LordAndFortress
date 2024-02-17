using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuScript : MonoBehaviour
{
    public bool showMenu;
    public GameObject menu;
    public LevelManager gameManager;

    private void Start()
    {
        menu.SetActive(showMenu);
        //Set Cursor to the middle of the game window
        Cursor.lockState = CursorLockMode.Locked;
        //Set Cursor to not be visible
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!showMenu)
            {
                menu.SetActive(true);
                showMenu = true;
                gameManager.showCursor = true;
            }
            else
            {
                menu.SetActive(false);
                showMenu = false;
                gameManager.showCursor = false;
            }
        }
        
    }
}
