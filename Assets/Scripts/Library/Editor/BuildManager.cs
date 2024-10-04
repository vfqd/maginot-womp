using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Framework;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Library.Editor
{
    [CreateAssetMenu(fileName = "Build Manager", menuName = "Library/Build Manager")]
    public class BuildManager : SerializedScriptableObject
    {
        public enum BuildPlatform
        {
            WindowsSteam,
            OSXSteam,
            LinuxSteam,
            WindowsStandalone,
            OSXStandalone,
            LinuxStandalone
        }
        
        public enum UploadPlatform
        {
            Steam,
            GoG,
            EGS,
            Itch
        }

        private static readonly string buildManagerAssetPath = "Assets/Resources/Settings/Build Manager.asset";
        
        [OnValueChanged(nameof(UpdateBuildVersion)),Title("Version"),PropertySpace(SpaceBefore = 10, SpaceAfter = 40)]
        public string buildVersion;

        [SerializeField,Title("Build Data")]
        private List<SceneAsset> scenesInBuild;

        [SerializeField,PropertySpace(SpaceAfter = 40)]
        private string executableName;

        [SerializeField, TableList(ShowPaging = false, AlwaysExpanded = true),FoldoutGroup("Builds")]
        private List<BuildConfig> buildConfigs;

        [SerializeField,FoldoutGroup("Steam"),GUIColor(.8f,.8f,1f)] private string pathToSteamcmd = "C:/Steamworks/sdk/tools/ContentBuilder/builder/steamcmd.exe";

        public enum Result
        {
            Success,
            Fail,
            Canceled
        }

        public const string STEAM_OUTPUT_PATH = "Builds/Steam/";
        public const string STANDALONE_OUTPUT_PATH = "Builds/Standalone/";
        public const string STANDALONE_ZIP_PATH = "Builds/Standalone/Zips/";

        private static readonly string[] DEFAULT_DEFINES =
        {
            "ODIN_INSPECTOR",
            "ODIN_INSPECTOR_3",
            "ODIN_INSPECTOR_3_1",
            "ODIN_VALIDATOR",
            "ODIN_VALIDATOR_3_1",
            "AMPLIFY_SHADER_EDITOR",
        };
        
        private class PlatformBuildData
        {
            public BuildTarget Target;
            public BuildTargetGroup Group;
            public string FileName;
            public string ZipName;
            public string[] AdditionalDefines;
            public int VSyncOverride;
        }
        
        private struct PlatformUploadData
        {
            public string UploaderPath;
            public string UploaderArguments;
        }

        [Button,FoldoutGroup("Builds")]
        private void BuildSelected()
        {
            foreach (var buildConfig in buildConfigs)
            {
                if (buildConfig.@on)
                {
                    BuildRelease(buildConfig.platform);
                }
            }
        }

        [Button,FoldoutGroup("Builds")]
        private void ReimportPrefabs()
        {
            AssetDatabase.ImportAsset( "Assets/Prefabs/", ImportAssetOptions.ImportRecursive );
        }

        [Button,FoldoutGroup("Steam"),GUIColor(.8f,.8f,1f)]
        private void UploadToSteam()
        {
            Upload(UploadPlatform.Steam);
        }

        [Button,FoldoutGroup("Steam"),GUIColor(.8f,.8f,1f)]
        private void OpenSteamUrl()
        {
            Application.OpenURL("https://partner.steamgames.com/apps/builds/1593030");
        }
        
        [Button,FoldoutGroup("Util")]
        private void ZipStandalone()
        {
            Zip(BuildPlatform.WindowsStandalone, STANDALONE_OUTPUT_PATH + "/" + BuildPlatform.WindowsStandalone);
            Zip(BuildPlatform.OSXStandalone, STANDALONE_OUTPUT_PATH + "/" + BuildPlatform.OSXStandalone);
            Zip(BuildPlatform.LinuxStandalone, STANDALONE_OUTPUT_PATH + "/" + BuildPlatform.LinuxStandalone);
        }

        private void OnValidate()
        {
            if (!Application.isBatchMode)
                UpdateBuildVersion();
        }

        private void UpdateBuildVersion()
        {
            PlayerSettings.bundleVersion = buildVersion;
        }

        private PlatformUploadData GetPlatformData(UploadPlatform platform)
        {
            DateTime now = DateTime.Now;
            string description = PlayerSettings.bundleVersion + " (" + now.Day + "-" + now.Month.ToString("D2") + "-" +
                                 now.Year + " " + now.Hour + ":" + now.Minute.ToString("D2") + ")";

            var rootContentPath = Path.GetDirectoryName(Application.dataPath);
            
            switch (platform)
            {
                case UploadPlatform.Steam:
                    return new PlatformUploadData()
                    {
                        UploaderPath = pathToSteamcmd,
                        UploaderArguments = ""/*$"+login <user> <pass> " +
                                            $"+run_app_build -desc \"{description}\" {rootContentPath}/Steamworks/scripts/terra_nil_app_build_1593030.vdf " /*+
                                            $"+quit "#1#*/
                    };
                default: throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }
        
        private PlatformBuildData GetPlatformData(BuildPlatform platform)
        {
            switch (platform)
            {
                case BuildPlatform.WindowsSteam:
                    return new PlatformBuildData
                    {
                        Target = BuildTarget.StandaloneWindows64,
                        Group = BuildTargetGroup.Standalone,
                        FileName = $"{executableName}.exe",
                        ZipName = $"{executableName} Windows (Steam)",
                        AdditionalDefines = new[] {"STEAMWORKS_NET"},
                        VSyncOverride = -1
                    };
                case BuildPlatform.OSXSteam:
                    return new PlatformBuildData
                    {
                        Target = BuildTarget.StandaloneOSX,
                        Group = BuildTargetGroup.Standalone,
                        FileName = $"{executableName}.app",
                        ZipName = $"{executableName} OSX (Steam)",
                        AdditionalDefines = new[] {"STEAMWORKS_NET"},
                        VSyncOverride = 0
                    };
                case BuildPlatform.LinuxSteam:
                    return new PlatformBuildData
                    {
                        Target = BuildTarget.StandaloneLinux64,
                        Group = BuildTargetGroup.Standalone,
                        FileName = $"{executableName}.x86_64",
                        ZipName = $"{executableName} Linux (Steam)",
                        AdditionalDefines = new[] {"STEAMWORKS_NET"},
                        VSyncOverride = -1
                    };
                case BuildPlatform.WindowsStandalone:
                    return new PlatformBuildData
                    {
                        Target = BuildTarget.StandaloneWindows64,
                        Group = BuildTargetGroup.Standalone,
                        FileName = $"{executableName}.exe",
                        AdditionalDefines = new[] {"DISABLESTEAMWORKS"},
                        ZipName = $"{executableName} Windows {buildVersion}(Standalone)",
                        VSyncOverride = -1
                    };
                case BuildPlatform.OSXStandalone:
                    return new PlatformBuildData
                    {
                        Target = BuildTarget.StandaloneOSX,
                        Group = BuildTargetGroup.Standalone,
                        FileName = $"{executableName}.app",
                        AdditionalDefines = new[] {"DISABLESTEAMWORKS"},
                        ZipName = $"{executableName} OSX {buildVersion}(Standalone)",
                        VSyncOverride = 0
                    };
                case BuildPlatform.LinuxStandalone:
                    return new PlatformBuildData
                    {
                        Target = BuildTarget.StandaloneLinux64,
                        Group = BuildTargetGroup.Standalone,
                        FileName = $"{executableName}.x86_64",
                        AdditionalDefines = new[] {"DISABLESTEAMWORKS"},
                        ZipName = $"{executableName} Linux {buildVersion}(Standalone)",
                        VSyncOverride = -1
                    };
                default: throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }

        public Result BuildRelease(BuildPlatform platform)
        {
            if (platform == BuildPlatform.WindowsSteam ||
                platform == BuildPlatform.OSXSteam ||
                platform == BuildPlatform.LinuxSteam)
            {
                return Build(platform, STEAM_OUTPUT_PATH + "/" + platform);
            }
            else
            {
                return Build(platform, STANDALONE_OUTPUT_PATH + "/" + platform);
            }
        }

        // public static Result ZipRelease(BuildPlatform platform)
        // {
        //     return Zip(platform, STEAM_OUTPUT_PATH + "/" + platform);
        // }

        private Result Build(BuildPlatform platform, string buildPath, bool log = true)
        {
            Result result = Result.Fail;
            PlatformBuildData buildData = GetPlatformData(platform);
            string originalDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildData.Group);

            try
            {
                if (log)
                    Debug.Log("Building: " + platform + "\n" + Application.dataPath.RemoveFromEnd("/Assets") + "/" +
                              buildPath);
                
                if (EditorUtility.DisplayCancelableProgressBar("Building " + platform, "Deleting old directory", 0.1f))
                    return Result.Canceled;

                FileUtil.DeleteFileOrDirectory(buildPath);
                
                // if (EditorUtility.DisplayCancelableProgressBar("Building " + platform, "Reimporting prefabs", 0.2f))
                //     return Result.Canceled;
                //
                // ReimportPrefabs();

                if (EditorUtility.DisplayCancelableProgressBar("Building " + platform, "Switching build target", 0.333f))
                    return Result.Canceled;

                EditorUserBuildSettings.SwitchActiveBuildTarget(buildData.Group, buildData.Target);

                if (EditorUtility.DisplayCancelableProgressBar("Building " + platform, "Building player", 0.5f))
                    return Result.Canceled;

                BuildPlayerOptions options = new BuildPlayerOptions
                {
                    target = buildData.Target,
                    locationPathName = buildPath + "/" + buildData.FileName,
                    scenes = new string[scenesInBuild.Count]
                };

                for (var i = 0; i < scenesInBuild.Count; i++)
                {
                    var scene = scenesInBuild[i];
                    options.scenes[i] = AssetDatabase.GetAssetPath(scene);
                }

                string defines = "";

                for (int i = 0; i < DEFAULT_DEFINES.Length; i++)
                {
                    if (!string.IsNullOrEmpty(defines)) defines += ";";
                    defines += DEFAULT_DEFINES[i];
                }

                if (buildData.AdditionalDefines != null)
                {
                    for (int i = 0; i < buildData.AdditionalDefines.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(defines)) defines += ";";
                        defines += buildData.AdditionalDefines[i];
                    }
                }

                // if (animalBeta)
                // {
                //     if (!string.IsNullOrEmpty(defines)) defines += ";";
                //     defines += "ANIMAL_BETA";
                // }
                
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildData.Group, defines);
                if (buildData.VSyncOverride >= 0) QualitySettings.vSyncCount = buildData.VSyncOverride;
                if (buildData.VSyncOverride == 0) Application.targetFrameRate = 60;
                // GameConfig.Instance.SetBuildInformation(PlayerSettings.bundleVersion, platform.ToString(), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                // EditorUtility.SetDirty(GameConfig.Instance);

                BuildReport report = BuildPipeline.BuildPlayer(options);

                if (report.summary.result == BuildResult.Succeeded)
                {
                    result = Result.Success;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildData.Group, originalDefines);
                AssetDatabase.Refresh();
                EditorUtility.ClearProgressBar();
                if (log) Debug.Log("Build result: " + ColourResult(result) + "\n");
            }

            return result;
        }

        public Result Zip(BuildPlatform platform, string outputDirectoryPath, bool log = true)
        {
            Result result = Result.Fail;
            PlatformBuildData buildData = GetPlatformData(platform);

            try
            {

                if (!string.IsNullOrEmpty(outputDirectoryPath))
                {
                    string date = $"{DateTime.Now.Day:D2}_{DateTime.Now.Month:D2}_{DateTime.Now.Year.ToString().Substring(2)}";
                    
                    // string zipPath = $"{Application.dataPath.RemoveFromEnd("/Assets")}/{STANDALONE_ZIP_PATH}/{buildData.ZipName} {date}.zip";
                    string zipPath = $"{Application.dataPath.RemoveFromEnd("/Assets")}/{STANDALONE_ZIP_PATH}/{buildData.ZipName}.zip";

                    Directory.CreateDirectory(Application.dataPath.RemoveFromEnd("/Assets") + "/" + STANDALONE_ZIP_PATH);
                    
                    if (log) Debug.Log("Zipping: " + platform + "\n" + zipPath);

                    if (EditorUtility.DisplayCancelableProgressBar("Zipping " + platform, "Deleting old zip", 0f))
                        return Result.Canceled;

                    FileUtil.DeleteFileOrDirectory(zipPath);

                    if (EditorUtility.DisplayCancelableProgressBar("Zipping " + platform, "Zipping build", 0.5f))
                        return Result.Canceled;

                    ZipFile.CreateFromDirectory(outputDirectoryPath, zipPath);

                    result = Result.Success;
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                if (log) Debug.Log("Zip result: " + ColourResult(result) + "\n");
            }

            return result;
        }
        //
        // public static Result Upload(BuildPlatform platform, bool log = true)
        // {
        //
        // }

        private Result Upload(UploadPlatform uploadPlatform)
        {
            Result result = Result.Fail;
            var uploadData = GetPlatformData(uploadPlatform);

            if (!string.IsNullOrEmpty(uploadData.UploaderPath))
            {
                try
                {
                    Debug.Log("Starting Uploader: " + uploadPlatform);

                    if (EditorUtility.DisplayCancelableProgressBar("Uploading " + uploadPlatform, "Starting Uploader", 0.5f))
                        return Result.Canceled;

                    // string path = uploadData.UploaderPath;
                    // string fileName = path.Substring(path.LastIndexOf('/') + 1);

                    Process.Start(uploadData.UploaderPath, uploadData.UploaderArguments);
                    // Process.Start(new ProcessStartInfo
                    // {
                    //     FileName = "steamcmd.exe",
                    //     WorkingDirectory = path.Substring(0, path.LastIndexOf('/')),
                    //     UseShellExecute = true,
                    //     CreateNoWindow = false,
                    //     WindowStyle = ProcessWindowStyle.Normal,
                    //     Arguments = "/K " + fileName + " " + uploadData.UploaderArguments
                    // });

                    result = Result.Success;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                    Debug.Log("Uploader was started: " + ColourResult(result) + "\n");
                }
            }

            return result;
        }

        private string ColourResult(Result result)
        {
            string titleized = StringUtils.Titelize(result.ToString());
            switch (result)
            {
                case Result.Success:
                    return $"<color=green>{titleized}</color>";
                case Result.Fail:
                    return $"<color=red>{titleized}</color>";
                case Result.Canceled:
                    return $"<color=yellow>{titleized}</color>";
            }
            return titleized;
        }

        [MenuItem("Library/Build Manager")]
        private static void SelectBuildManager()
        {
            Selection.activeObject=AssetDatabase.LoadMainAssetAtPath(buildManagerAssetPath);
        }
        
        [Serializable]
        private struct BuildConfig
        {
            [VerticalGroup("Platform"),HideLabel]
            public BuildPlatform platform;
            [VerticalGroup("On"),HideLabel]
            public bool on;
        }
    }
}
