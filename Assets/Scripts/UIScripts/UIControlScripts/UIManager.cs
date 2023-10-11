using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;

public class UIManager:MonoBehaviour
{
    VisualElement rootVisualElement;
    [SerializeField]
    PartyData partyData;
    [SerializeField]
    FontStyle pixelStyle;

    private void Awake()
    {
        
    }

    private void Start()
    {
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        ScreenUI.Singleton.InitStart(rootVisualElement,partyData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&rootVisualElement!=null) 
        {
            rootVisualElement.visible = false;
        }
    }
}
