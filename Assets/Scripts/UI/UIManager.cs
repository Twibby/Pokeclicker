using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UI_MenuTab MainPanelMenuTab;
    [SerializeField] private LoadingScreenManager LoaderPanel;
    [SerializeField] private LoadingScreenManager MessagePanel;
    [SerializeField] private TMPro.TMP_Text MessageLabel;

    // Start is called before the first frame update
    void Start()
    {
        MessageLabel.text = "";
        MessagePanel.gameObject.SetActive(false);

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

    public enum MessageLevel { NOTICE, WARNING, ERROR, VALIDATE }

    public void SetMessage(string text, MessageLevel level = MessageLevel.ERROR)
    {
        string message = "";
        if (level == MessageLevel.WARNING || level == MessageLevel.ERROR)
        {
            message += level + ": ";
        }
        message += text;

        MessageLabel.text = message;

        switch (level)
        {
            case MessageLevel.NOTICE:
                MessageLabel.color = Color.black;
                Debug.Log(text); break;
            case MessageLevel.WARNING:
                MessageLabel.color = Color.yellow;
                Debug.LogWarning(text); break;
            case MessageLevel.ERROR:
                MessageLabel.color = Color.red;
                Debug.LogError(text); break;
            case MessageLevel.VALIDATE:
                MessageLabel.color = Color.green;
                Debug.Log(text); break;
        }

        MessagePanel.gameObject.SetActive(true);
        MessagePanel.StartLoading();
        //MessagePanel.CloseAfterDelay(5f);
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
