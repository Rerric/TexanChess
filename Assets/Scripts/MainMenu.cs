using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public PlayerControls controls;

    Vector2 choose;

    public GameObject[] buttons; //0 = Start; 1 = How-To; 2 = Options; 3 = Credits;

    public int currentSelection;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();

        controls.Paused.Enable();
        Cursor.lockState = CursorLockMode.None;

        //Menu Controls
        controls.Paused.Choose.performed += ctx => ChooseSelection(ctx.ReadValue<Vector2>().y);
        controls.Paused.Choose.canceled += ctx => choose = Vector2.zero;
        controls.Paused.Quit.performed += ctx => Application.Quit();
        controls.Paused.Select.performed += ctx => Select();

        currentSelection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonUI();
    }

    void ChooseSelection(float dir)
    {
        int _dir = (int) dir;
        currentSelection -= _dir;
        if (currentSelection > 3) currentSelection = 0;
        if (currentSelection < 0) currentSelection = 3;
        Debug.Log(currentSelection);
    }

    void Select()
    {
        var button = buttons[currentSelection].GetComponent<Button>();
        button.UseButton();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void UpdateButtonUI()
    {
        for (var i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i].GetComponent<Button>();
            var buttonImage = button.GetComponent<Image>();
            
            if (button.id == currentSelection)
            {
                buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
            }
            else buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.5f);
        }
    }
}
