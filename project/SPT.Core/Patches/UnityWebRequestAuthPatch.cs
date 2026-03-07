using System.Reflection;
using SPT.Common.Http;
using SPT.Reflection.Patching;
using UnityEngine.Networking;

namespace SPT.Core.Patches;

/// <summary>
/// Injects HTTP Basic Auth into ALL outgoing UnityWebRequests.
/// This covers the game's own backend HTTP calls (locale, game config, etc.)
/// that don't go through SPT's Client class.
/// </summary>
public class UnityWebRequestAuthPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(UnityWebRequest).GetMethod(nameof(UnityWebRequest.SendWebRequest));
    }

    [PatchPrefix]
    private static void PatchPrefix(UnityWebRequest __instance)
    {
        var authHeader = RequestHandler.HttpClient.GetBasicAuthHeaderValue();
        if (authHeader != null && __instance.GetRequestHeader("Authorization") == null)
        {
            __instance.SetRequestHeader("Authorization", authHeader);
        }
    }
}
