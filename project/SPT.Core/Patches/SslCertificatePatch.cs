using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace SPT.Core.Patches;

public class SslCertificatePatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(
            typeof(SslCertPatchClass),
            nameof(SslCertPatchClass.ValidateCertificate),
            new[] { typeof(X509Certificate) }
        );
    }

    [PatchPrefix]
    private static bool PatchPrefix(ref bool __result)
    {
        if (!SPTCorePlugin.DisableSslValidation.Value)
        {
            return true; // Run original validation
        }

        __result = true;
        return false; // Skip original
    }
}
