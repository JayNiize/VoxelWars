using Unity.Netcode;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class Bootstrapper {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() {
        MatchmakingService.ResetStatics();
        Addressables.InstantiateAsync("CanvasUtilities");
    }
}