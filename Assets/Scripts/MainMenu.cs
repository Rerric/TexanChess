using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public PlayerControls controls;

    Vector2 choose;

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

        currentSelection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChooseSelection(float dir)
    {
        int _dir = (int) dir;
        currentSelection -= _dir;
        if (currentSelection > 3) currentSelection = 0;
        if (currentSelection < 0) currentSelection = 3;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
