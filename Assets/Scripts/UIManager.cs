using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas defaultCanvas;

    public GameObject actionWheel;
    public GameObject[] _weapons = new GameObject[6];
    public Image[] weaponIcons = new Image[6];
    public Sprite[] iconSprites;

    public int currentSlot; //which slot is active in the player's inventory / what weapon they have selected

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentSlot = 0;
        animator = actionWheel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprites();
        Cycle();
    }

    void Cycle()
    {
        animator.SetInteger("Slot", currentSlot);
    }

    void UpdateSprites()
    {
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            weaponIcons[i].GetComponent<Sprite>();
            if (_weapons[i] != null)
            {
                if (_weapons[i].name == "Revolver") weaponIcons[i].sprite = iconSprites[0];
                if (_weapons[i].name == "Dynamite") weaponIcons[i].sprite = iconSprites[1];
                if (_weapons[i].name == "Shovel") weaponIcons[i].sprite = iconSprites[2];
            }
            else weaponIcons[i].sprite = null;
        }
    }
}
