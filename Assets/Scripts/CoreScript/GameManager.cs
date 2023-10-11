using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    const string GameManagerKey = "GameManager";
    public static GameManager instance;

    private void Awake()
    {
        print("Awake GameManager");
    }

    private void Start()
    {
        print("Start GameManager");
    }


    //Runtime方法，在场景加载之前，所有Awake之后，左右Start之前
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeGameManager() 
    {
        print("Initialize");
        Addressables.InstantiateAsync(GameManagerKey).Completed += GameManager_Completed; ;
    }

    private static void GameManager_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {       
        DontDestroyOnLoad(obj.Result);
    }
}
