using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UI_MenuTab MainPanelMenuTab;
    [SerializeField] private LoadingScreenManager LoaderPanel;

    // Start is called before the first frame update
    void Start()
    {
        MainPanelMenuTab.Initialize();

        LoadingActivation(false);
    }

    public void LoadingActivation(bool value)
    {
        if (value)
        {
            LoaderPanel.gameObject.SetActive(true);
            LoaderPanel.StartLoading();
        }
        else
            LoaderPanel.StopLoading();
    }



    /// <summary>
    /// The instance to call to get the UIManager
    /// </summary>
    private static UIManager _instance = null;

    /// <summary>
    /// Public accessor for this manager's instance
    /// </summary>
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                    /*throw new System.Exception*/
                    Debug.LogError("[UIManager] There is no UIManager in the scene, please add one before restarting the game. or instantiate it?");
            }
            return _instance;
        }
    }

}
