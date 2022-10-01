using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Button))]
public class UI_MenuTabButton : MonoBehaviour
{
    public GameObject panel;
    public UI_MenuTab tabManager;

    public GameObject objectOnselected;

    public void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if(tabManager)
            tabManager.OnClick(this);
    }

    public void SetSelected(bool value)
    {
        if(objectOnselected)
            objectOnselected.SetActive(value);
    }
}
