using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitializeStart : MonoBehaviour
{
    const string GameManagerKey = "GameManager";

    //Runtime�������ڳ�������֮ǰ������Awake֮������Start֮ǰ
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
