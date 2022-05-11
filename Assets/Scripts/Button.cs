using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public int id;

    private MainMenu menuScript;

    public bool isHovered;

    // Start is called before the first frame update
    void Start()
    {
        menuScript = GameObject.Find("MainMenuManager").GetComponent<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseButton()
    {
        if (id == 0) //Start
        {
            menuScript.StartGame();
        }

        if (id == 1) //Display How-To and Controls
        {

        }

        if (id == 2) //Display Settings
        {

        }

        if (id == 3) //Display Credits
        {

        }
    }
}
