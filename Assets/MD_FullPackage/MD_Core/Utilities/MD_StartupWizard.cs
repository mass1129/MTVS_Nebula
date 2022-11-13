using UnityEngine;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using MD_Plugin;
using UnityEditor.SceneManagement;
#endif

namespace MD_PluginEditor
{
#if UNITY_EDITOR
    public class MD_StartupWizard : EditorWindow
    {
        public Texture2D Logo;

        public Texture2D Home;
        public Texture2D Web;
        public Texture2D Doc;
        public Texture2D Discord;

        private GUIStyle style;

        [MenuItem("Window/MD_Package/Startup Window")]
        public static void Init()
        {
            MD_StartupWizard md = (MD_StartupWizard)GetWindow(typeof(MD_StartupWizard));
            md.maxSize = new Vector2(400, 600);
            md.minSize = new Vector2(399, 599);
            md.titleContent = new GUIContent("MD Startup");
            md.Show();
        }

        private void OnGUI()
        {
            style = new GUIStyle(GUI.skin.button);
            style.richText = true;
            style.normal.textColor = Color.white;
            style.wordWrap = false;

            GUILayout.Label(Logo);
            style.fontSize = 13;
            style.wordWrap = true;
            style.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginVertical("Box");
            GUILayout.Label("Welcome to the Mesh Deformation [MD] Full Collection!\nPlease read the latest change-log below...", style);
            GUILayout.EndVertical();

            style.alignment = TextAnchor.UpperLeft;
            GUILayout.Space(5);
            style.fontSize = 12;

            GUILayout.BeginVertical("Box");
            GUILayout.Label("<size=14><color=#ffa84a>MD Package version <b>" + MD_Debug.VERSION + "</b> [" + MD_Debug.DATE + " <size=11>dd/mm/yyyy</size>]</color></size>\n- Added MDM_MeshCut modifier\n- Added MDM_SoundReact modifier\n- EasyMeshTracker renamed to LiteMeshTracker\n- Added Editor-Undo feature in almost all possible cases\n- Added new alternative of 'Normals Recalculation'\n- Creating a new mesh reference will keep multiple materials\n- Major code refactor\n- Major cleanup of all features in all modifiers\n- Official MD Package website created\n- Major documentation update\n- Major editor icons & images upgrade\n...and many more in the official roadmap below.", style);
            if(GUILayout.Button("Official Roadmap - Trello"))
                Application.OpenURL("https://trello.com/b/MFqllEZE/matej-vanco-unity-extension");
            GUILayout.EndVertical();
            GUILayout.Space(5);
            style.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("No idea where to start? Open documentation to learn more!", style);
            GUILayout.Space(5);
            style.alignment = TextAnchor.UpperLeft;
            style = new GUIStyle(GUI.skin.button);
            style.imagePosition = ImagePosition.ImageAbove;

            GUILayout.BeginHorizontal("Box");
            if (GUILayout.Button(new GUIContent("Take Me To Intro", Home), style))
            {
                GenerateScenesToBuild();
                string scene = GetScenePath("MDExample_Introduction");
                if (!string.IsNullOrEmpty(scene))
                    EditorSceneManager.OpenScene(scene);
                else
                    Debug.LogError("Scene is not in Build Settings! Required path: [" + Application.dataPath + "/MD_FullPackage/Examples/Scenes/]");
            }
            if (GUILayout.Button(new GUIContent("Official Website", Web), style))
                Application.OpenURL("https://www.matejvanco.com/md-package");

            if (GUILayout.Button(new GUIContent("Open Documentation", Doc), style))
            {
                try
                {
                    System.Diagnostics.Process.Start(Application.dataPath + "/MD_FullPackage/MD_Package_Documentation.pdf");
                }
                catch
                {
                    Debug.LogError("Documentation could not be found! [Documentation name: MD_Package_Documentation]");
                }
            }

            GUILayout.EndHorizontal();
            style.alignment = TextAnchor.MiddleCenter;
            if (GUILayout.Button(new GUIContent(Discord), style))
                Application.OpenURL("https://discord.gg/WdcYHBtCfr");
        }

        public static void GenerateScenesToBuild()
        {
            try
            {
                EditorBuildSettings.scenes = new EditorBuildSettingsScene[0];
                List<EditorBuildSettingsScene> sceneAr = new List<EditorBuildSettingsScene>();

                int cat = 0;
                while (cat < 6)
                {
                    string[] tempPaths;
                    if (cat == 0)       tempPaths = Directory.GetFiles(Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/", "*.unity");
                    else if (cat == 1)  tempPaths = Directory.GetFiles(Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/AdvancedShapes/", "*.unity");
                    else if (cat == 2)  tempPaths = Directory.GetFiles(Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/MeshEditor/", "*.unity");
                    else if (cat == 3)  tempPaths = Directory.GetFiles(Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/Modifiers/", "*.unity");
                    else if (cat == 4)  tempPaths = Directory.GetFiles(Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/Mobile/", "*.unity");
                    else                tempPaths = Directory.GetFiles(Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/Shaders/", "*.unity");

                    for (int i = 0; i < tempPaths.Length; i++)
                    {
                        string path = tempPaths[i].Substring(Application.dataPath.Length - "Assets".Length);
                        path = path.Replace('\\', '/');

                        sceneAr.Add(new EditorBuildSettingsScene(path, true));
                    }
                    cat++;
                }
                EditorBuildSettings.scenes = sceneAr.ToArray();
            }
            catch (IOException e)
            {
                Debug.Log("Can't load example scenes! Try to play again. Otherwise you can find all example scenes in MD_Examples_Scenes.\nException: " + e.Message);
            }

        }

        private static string GetScenePath(string sceneName)
        {
            try
            {
                if (File.Exists(Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/" + sceneName + ".unity"))
                    return Application.dataPath + "/MD_FullPackage/MD_Examples/MD_Examples_Scenes/" + sceneName + ".unity";
                else
                    return "";
            }
            catch (IOException e)
            {
                Debug.Log("Can't load example scenes! Go to /MD_FullPackage/MD_Examples/MD_Examples_Scenes/.\nException: " + e.Message);
            }
            return "";
        }
    }
#endif
}

namespace MD_Plugin
{
    public class MD_Debug
    {
        public const string ORGANISATION = "Matej Vanco";
        public const string PACKAGENAME = "/MD Package/";
        public const short VERSION = 16;
        //--yyyy/mm/dd
        public static string DATE = "23/06/2022";

        public enum DebugType { Error, Warning, Information };
        public static void Debug(MonoBehaviour Sender, string Message, DebugType DType = DebugType.Information)
        {
            string senderName = !Sender ? "(Unknown sender)" : Sender.GetType().Name;
            string senderObjName = !Sender ? "(Unknown sender)" : Sender.gameObject.name;
            switch (DType)
            {
                case DebugType.Information: UnityEngine.Debug.Log(senderName + " [" + senderObjName + "]: " + Message);        break;
                case DebugType.Warning:     UnityEngine.Debug.LogWarning(senderName + " [" + senderObjName + "]: " + Message); break;
                case DebugType.Error:       UnityEngine.Debug.LogError(senderName + " [" + senderObjName + "]: " + Message);   break;
            }
        }
    }
}