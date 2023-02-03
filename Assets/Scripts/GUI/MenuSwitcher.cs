using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField]
    public List<Dialog> menus;
    public int defaultMenuIndex;

    private int currentMenu;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (i != defaultMenuIndex)
                menus[i].Hide();
            else
                menus[i].Show();
        }
    }

    public void SwitchToMenu(int menuIndex)
    {
        if (menuIndex != currentMenu)
        {
            menus[currentMenu].Hide();
            menus[menuIndex].Show();

            currentMenu = menuIndex;
        }
    }
}
