using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Button : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public PlayerControls controls;
    public int id;

    private MainMenu menuScript;

    public Image[] images;

    public bool isSelected;

    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        menuScript = GameObject.Find("MainMenuManager").GetComponent<MainMenu>();

        controls = new PlayerControls();
        controls.Paused.Enable();

        controls.Paused.Choose.performed += ctx => CycleButton(ctx.ReadValue<Vector2>().x);
    }

    void Update()
    {
        UpdateValue();
        if (isSelected)
        {
            for (var i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1f);
            }
        }
        else
        {
            for (var i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 0.4f);
            }
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
            menuScript.OptionsScreen();
        }

        if (id == 3) //Display Credits
        {

        }
    }

    public void CycleButton(float dir)
    {
        if (id > 3 && isSelected && menuScript.currentScreen == 2)
        {
            int _dir = (int)dir;
            if (id == 4) //sensitivity
            {
                GlobalSettings.sensitivity += _dir * 0.1f;
            }

            if (id == 5) //ads
            {
                GlobalSettings.adsSensitivity += _dir * 0.1f;
            }

            if (id == 6) //volume
            {
                AudioManager.GlobalVolume += _dir * 0.1f;
                int value = Mathf.RoundToInt(AudioManager.GlobalVolume);
                Debug.Log(value);
            }

            if (id == 7) //bullet bounce
            {
                GlobalSettings.bulletBounces += _dir;
            }

            if (id == 8) //explosions
            {

            }
        }
    }

    public void UpdateValue()
    {
        if (id > 3)
        {
            if (id == 4) //sensitivity
            {
                textMesh.text = GlobalSettings.sensitivity.ToString();
            }

            if (id == 5) //ads
            {
                textMesh.text = GlobalSettings.adsSensitivity.ToString();
            }

            if (id == 6) //volume
            {
                textMesh.text = AudioManager.GlobalVolume.ToString();
            }

            if (id == 7) //bullet bounce
            {
                textMesh.text = GlobalSettings.bulletBounces.ToString();
            }

            if (id == 8) //explosions
            {

            }
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
