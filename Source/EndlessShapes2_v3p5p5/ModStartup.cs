using HarmonyLib;
using System.Reflection;

namespace ModManagement
{
    public static class ModStartup
    {
        /*
        public static void OnLoad()
        {
        }
        */

        public static void OnStart()
        {
            ModInformation.VersionConfirmation();

            Harmony harmony = new Harmony("EndlessShapes2Patch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        /*
        public static void OnSave()
        {
        }
        */
    }
}
