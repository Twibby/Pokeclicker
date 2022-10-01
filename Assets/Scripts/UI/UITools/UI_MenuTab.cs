using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_MenuTab : MonoBehaviour
{
    public UI_MenuTabButton openedTab;
    public bool clickAgainToClose;
    public bool openFirstPanelAtStart;
    private UI_MenuTabButton[] tabButtons;
    
    public void Initialize()
    {
        tabButtons = this.GetComponentsInChildren<UI_MenuTabButton>();

        if (tabButtons.Length == 0)
            return;

        for (int i = 0; i < tabButtons.Length; i++)
        {
            Close(tabButtons[i]);
        }

        if (openFirstPanelAtStart)
            Open(tabButtons[0]);
    }

    public void OnClick(UI_MenuTabButton newTab)
    {
        if (openedTab == null)
        {
            Open(newTab);
        }
        else if (openedTab == newTab)
        {
            if (clickAgainToClose)
                Close();

            return;
        }
        else
        {
            Close();
            Open(newTab);
        }
    }

    public void Open(UI_MenuTabButton newTab)
    {
        newTab.SetSelected(true);

        openedTab = newTab;
        if (newTab.panel)
            openedTab.panel.SetActive(true);
    }

    public void Close()
    {
        if (!openedTab)
            return;

        Close(openedTab);
    }

    public void Close(UI_MenuTabButton tabButton)
    {
        if (tabButton.panel)
            tabButton.panel.SetActive(false);

        tabButton.SetSelected(false);

        openedTab = null;
    }
}
