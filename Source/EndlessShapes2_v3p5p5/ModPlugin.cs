//Reference : Assembly-CSharp-firstpass.dll, Core.dll, Modding.dll, Newtonsoft.Json.dll, Steamworks.dll, Ui.dll

using BrilliantSkies.Core.Constants;
using BrilliantSkies.Core.Timing;
using BrilliantSkies.Modding;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Displayer.Types;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using Steamworks;
using System;
using System.IO;
using System.Reflection;

namespace EndlessShapes2
{
    public class ModPlugin : GamePlugin
    {
        //Mod name
        public static string ModName { get; } = "EndlessShapes2";

        //Mod version
        public static System.Version ModVersion { get; } = new System.Version(0, 0, 2);

        //Steam workshop ID
        public static ulong WorkshopID { get; } = 0;



        private static string MyModDirPathMemory;

        public static string MyModDirPath
        {
            get
            {
                if (string.IsNullOrEmpty(MyModDirPathMemory))
                {
                    MyModDirPathMemory = Path.Combine(Get.ProfilePaths.RootModDir().ToString(), ModName);
                }

                return MyModDirPathMemory;
            }
        }



        private int RequestCount;

        private CallResult<SteamUGCRequestUGCDetailsResult_t> SteamCall;

        public string name { get; } = ModName;

        public System.Version version { get; } = ModVersion;

        public void OnLoad()
        {
            UpdateJSON(Path.Combine(MyModDirPath, "plugin.json"));
            ModProblemOverwrite($"{ModName}  v{ModVersion}  Active!", MyModDirPath, string.Empty, false);

            GameEvents.StartEvent.RegWithEvent(OnStart);
            GameEvents.Twice_Second.RegWithEvent(SteamUGCRequest);
        }

        public void OnStart()
        {
            GameEvents.StartEvent.UnregWithEvent(OnStart);

            Harmony harmony = new Harmony("EndlessShapes2Patch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnSave()
        {
        }

        private void UpdateJSON(string pluginPath)
        {
            if (File.Exists(pluginPath))
            {
                JObject jObject = JObject.Parse(File.ReadAllText(pluginPath));

                string ModVersionText = ModVersion.ToString();

                bool F0 = jObject["name"].ToString() != ModName;
                bool F1 = jObject["version"].ToString() != ModVersionText;

                if (F0 || F1)
                {
                    if (F0) jObject["name"] = ModName;
                    if (F1) jObject["version"] = ModVersionText;

                    File.WriteAllText(pluginPath, jObject.ToString());
                }
            }
        }

        public void ModProblemOverwrite(string InitModName, string InitModPath, string InitDescription, bool InitIsError)
        {
            ModProblems.AllModProblems.Remove(InitModPath);
            ModProblems.AddModProblem(InitModName, InitModPath, InitDescription, InitIsError);

            foreach (IGui_GuiSystem guiSystem in GuiDisplayer.GetSingleton().ActiveGuis)
            {
                guiSystem.OnActivateGui();
            }
        }

        private void SteamUGCRequest(ITimeStep t)
        {
            if (WorkshopID != 0 && ++RequestCount <= 5)
            {
                Console.WriteLine("SteamUGCRequest : " + RequestCount);

                SteamAPICall_t UGCDetails = SteamUGC.RequestUGCDetails(new PublishedFileId_t(WorkshopID), 0);
                SteamCall = new CallResult<SteamUGCRequestUGCDetailsResult_t>(Callback);
                SteamCall.Set(UGCDetails);
            }
            else
            {
                GameEvents.Twice_Second.UnregWithEvent(SteamUGCRequest);
            }
        }

        public void Callback(SteamUGCRequestUGCDetailsResult_t param, bool bIOFailure)
        {
            GameEvents.Twice_Second.UnregWithEvent(SteamUGCRequest);

            string Description = param.m_details.m_rgchDescription;

            if (!string.IsNullOrEmpty(Description))
            {
                using (StringReader Reader = new StringReader(Description))
                {
                    string InputLine;
                    System.Version LatestVersion = null;

                    while ((InputLine = Reader.ReadLine()) != null)
                    {
                        if (InputLine.StartsWith("Mod latest version "))
                        {
                            LatestVersion = System.Version.Parse(InputLine.Remove(0, 18));
                            break;
                        }
                    }

                    if (LatestVersion != null && ModVersion.CompareTo(LatestVersion) == -1)
                    {
                        ModProblemOverwrite(ModName, MyModDirPath + "UpdateText", "New version released! v" + LatestVersion, false);
                    }
                }
            }
        }
    }
}