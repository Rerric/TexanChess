using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int id;

    private MainMenu menuScript;

    public Image image;

    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        menuScript = GameObject.Find("MainMenuManager").GetComponent<MainMenu>();
    }

    void Update()
    {

        if (isSelected)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
        }
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

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == this.gameObject)
        {
            isSelected = true;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
            isSelected = false;
    }
}
