using UnityEngine;
using UnityEditor;

public static class AutoBuilder
{

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }

    [MenuItem("File/AutoBuilder/Windows")]
    static void PerformWindowsBuild()
    {
        var args = System.Environment.GetCommandLineArgs();
        System.Console.WriteLine(args.ToString());

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
        BuildPipeline.BuildPlayer(GetScenePaths(), "Builds/Windows/TheVileBunch.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    }

}