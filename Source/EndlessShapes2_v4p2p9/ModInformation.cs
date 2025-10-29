using BrilliantSkies.Core.Timing;
using BrilliantSkies.Modding;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Displayer.Types;
using Newtonsoft.Json.Linq;
using Steamworks;
using System;
using System.IO;
using System.Reflection;

namespace ModManagement
{
    public static class ModInformation
    {
        private static string _name;

        private static string _myModDirPath;

        public static string MyModFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(_myModDirPath)) Preparation();

                return _myModDirPath;
            }
        }

        public static string MyModName
        {
            get
            {
                if (string.IsNullOrEmpty(_name)) Preparation();

                return _name;
            }
        }

        private static void Preparation()
        {
            string path1 = Assembly.GetExecutingAssembly().Location;
            string path2 = Path.GetDirectoryName(path1);

            while (Path.GetFileName(path2) != "Mods")
            {
                path1 = path2;
                path2 = Path.GetDirectoryName(path1);
            }

            _myModDirPath = path1;
            _name = Path.GetFileName(path1);
        }



        private static System.Version _modVersion = new System.Version(0, 0, 0);

        private static ulong _workshopID;

        private static int _requestCount;

        private static CallResult<SteamUGCRequestUGCDetailsResult_t> _steamCall;

        public static void VersionConfirmation()
        {
            GameEvents.Twice_Second.RegWithEvent(SteamUGCRequest);

            string pluginPath = Path.Combine(MyModFolderPath, "plugin.json");

            if (File.Exists(pluginPath))
            {
                JObject jObject = JObject.Parse(File.ReadAllText(pluginPath));

                JToken jobj1 = jObject["version"];
                JToken jobj2 = jObject["workshop_id"];

                if (jobj1 != null)
                {
                    _modVersion = System.Version.Parse(jobj1.ToString());
                }

                if (jobj2 != null)
                {
                    _workshopID = ulong.Parse(jobj2.ToString());
                }
            }

            ModProblemOverwrite($"{MyModName}  v{_modVersion}  Active!", MyModFolderPath, string.Empty, false);
        }

        private static void ModProblemOverwrite(string InitModName, string InitModPath, string InitDescription, bool InitIsError)
        {
            ModProblems.AllModProblems.Remove(InitModPath);
            ModProblems.AddModProblem(InitModName, InitModPath, InitDescription, InitIsError);

            foreach (IGui_GuiSystem guiSystem in GuiDisplayer.GetSingleton().ActiveGuis)
            {
                guiSystem.OnActivateGui();
            }
        }

        private static void SteamUGCRequest(ITimeStep t)
        {
            if (_workshopID != 0 && ++_requestCount <= 5)
            {
                Console.WriteLine("SteamUGCRequest : " + _requestCount);

                SteamAPICall_t ugcDetails = SteamUGC.RequestUGCDetails(new PublishedFileId_t(_workshopID), 0);
                _steamCall = new CallResult<SteamUGCRequestUGCDetailsResult_t>(Callback);
                _steamCall.Set(ugcDetails);
            }
            else
            {
                GameEvents.Twice_Second.UnregWithEvent(SteamUGCRequest);
            }
        }

        private static void Callback(SteamUGCRequestUGCDetailsResult_t param, bool bIOFailure)
        {
            GameEvents.Twice_Second.UnregWithEvent(SteamUGCRequest);

            string description = param.m_details.m_rgchDescription;

            if (!string.IsNullOrEmpty(description))
            {
                using (StringReader reader = new StringReader(description))
                {
                    string inputLine;
                    System.Version latestVersion = null;

                    while ((inputLine = reader.ReadLine()) != null)
                    {
                        if (inputLine.StartsWith("Mod latest version "))
                        {
                            latestVersion = System.Version.Parse(inputLine.Remove(0, 18));
                            break;
                        }
                    }

                    if (latestVersion != null && _modVersion.CompareTo(latestVersion) == -1)
                    {
                        ModProblemOverwrite(MyModName, MyModFolderPath + "UpdateText", "New version released! v" + latestVersion, false);
                    }
                }
            }
        }
    }
}
