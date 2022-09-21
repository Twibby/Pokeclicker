using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Toggle SingleDateToggle;     // for init purpose only
    [SerializeField] private GameObject LoaderPanel;

    // Start is called before the first frame update
    void Start()
    {
        SingleDateToggle.isOn = false;
        SingleDateToggle.isOn = true;

        LoaderPanel.SetActive(false);
        LoaderPanel.transform.SetAsLastSibling();
    }

    public void LoadingActivation(bool isActive)
    {
        LoaderPanel.SetActive(isActive);
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
