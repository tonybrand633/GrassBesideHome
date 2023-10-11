using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    [SerializeField]
    UIDocument uiDoc;
    VisualElement root;
    List<Button>buttonGroup;

    // Start is called before the first frame update
    void Start()
    {
        uiDoc = GetComponent<UIDocument>();
        root = uiDoc.GetComponent<UIDocument>().rootVisualElement;

        //绑定所有按钮
        buttonGroup = root.Query<Button>().ToList();

        buttonGroup[0].clicked += StartGame;
        buttonGroup[1].clicked += LoadGame;
        buttonGroup[2].clicked += OpenSettings;
        buttonGroup[3].clicked += ExitGame;
    }

    void StartGame() 
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("<color=green>!!!!!!!Game Start!!!!!</color>");
    }

    void LoadGame() 
    {
        Debug.Log("<color=green>!!!!!!!Game Load!!!!!</color>");
    }

    void OpenSettings() 
    {
        Debug.Log("<color=green>!!!!!!!OpenSettings!!!!!</color>");
    }


    void ExitGame() 
    {
        Debug.Log("<color=green>!!!!!!!ExitGame!!!!!</color>");
    }

   
}
