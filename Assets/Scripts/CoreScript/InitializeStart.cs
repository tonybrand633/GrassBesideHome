using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitializeStart : MonoBehaviour
{
    const string GameManagerKey = "GameManager";

    //Runtime方法，在场景加载之前，所有Awake之后，左右Start之前
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeGameManager()
    {
        Debug.Log("<color=yellow>GameManager Initialize!!!!!!</color>");
        Addressables.InstantiateAsync(GameManagerKey).Completed += GameManager_Completed; ;
    }

    private static void GameManager_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        DontDestroyOnLoad(obj.Result);
    }
}
