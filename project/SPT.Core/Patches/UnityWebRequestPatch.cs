using System.Reflection;
using SPT.Common.Http;
using SPT.Reflection.Patching;
using UnityEngine.Networking;

namespace SPT.Core.Patches;

public class UnityWebRequestPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return typeof(UnityWebRequestTexture).GetMethod(nameof(UnityWebRequestTexture.GetTexture), new[] { typeof(string) });
    }

    [PatchPostfix]
    private static void PatchPostfix(UnityWebRequest __result)
    {
        __result.timeout = 15000;

        // Add HTTP Basic Auth header if the client has credentials configured
        var authHeader = RequestHandler.HttpClient.GetBasicAuthHeaderValue();
        if (authHeader != null)
        {
            __result.SetRequestHeader("Authorization", authHeader);
        }
    }
}
