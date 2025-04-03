using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class Logger
{

    public static void OnAssetLoaded(AsyncOperationHandle asyncOperationHandle, string resourceName)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            Debug.Log($"Loaded {resourceName}");
        else
            Debug.LogWarning($"Couldn't load {resourceName}");
    }

} 