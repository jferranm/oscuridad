using System;
using UnityEditor;
using UnityEngine;

namespace Frankfort.VBug.Editor {
    
    public class vBugToolbarNavigation {

        //--------------- ABOUT --------------------

         



        //--------------- WINDOWS --------------------
        [MenuItem("Tools/vBug/Repository #&r", priority = 1)]
        public static void OpenWindow_Repository() {
            EditorWindow.GetWindow<vBugRepositoryWindow>();
        }

        [MenuItem("Tools/vBug/Timeline #&t", priority = 2)]
        public static void OpenWindow_TimeLine() {
            EditorWindow.GetWindow<vBugTimelineWindow>();
        }

        [MenuItem("Tools/vBug/Console #&c", priority = 3)]
        public static void OpenWindow_Console() {
            EditorWindow.GetWindow<vBugConsoleWindow>();
        }

        [MenuItem("Tools/vBug/Playback #&p", priority = 4)]
        static void OpenWindow_Playback() {
            EditorWindow.GetWindow<vBugPlaybackWindow>();
        }

        [MenuItem("Tools/vBug/Hierarchy #&h", priority = 5)]
        public static void OpenWindow_Hierarchy() {
            EditorWindow.GetWindow<vBugHierarchyWindow>(); 
        }
        //--------------- WINDOWS --------------------


        //--------------- ABOUT --------------------
        [MenuItem("Tools/vBug/About/Facebook", priority = 6)]
        static void Goto_Facebook() {
            Application.OpenURL("http://www.facebook.com/vbug4unity");
        }
          
        [MenuItem("Tools/vBug/About/Youtube channel", priority = 7)]
        static void Goto_Youtube() {
            Application.OpenURL("http://www.youtube.com/channel/UCZQHLbjzAACK5y3dXth4Dig");
        }

        [MenuItem("Tools/vBug/About/User-feedback + Bugreport", priority = 8)]
        static void Goto_UserEcho() {
            Application.OpenURL("http://vbug4unity.userecho.com");
        }

        [MenuItem("Tools/vBug/About/Forum", priority = 9)]
        static void Goto_Forum() {
            Application.OpenURL("http://forum.unity3d.com/threads/introducing-vbug-visual-debugging-playability-testing-usability-testing.281172/");
        }

        [MenuItem("Tools/vBug/About/Email support", priority = 10)]
        static void Mailto_Support() {
            Application.OpenURL("mailto:vbug4unity@gmail.com");
        }




        //--------------- Settings --------------------
        [MenuItem("Tools/vBug/Settings/Update indexed shader properties", priority = 11)]
        public static void Settings_ForceUpdateShaders() {
            ShaderPropertyIndexer.UpdateShaderPropertiesXML(true);
        }

        [MenuItem("Tools/vBug/Settings/Set editor vBug-root folder", priority = 12)]
        public static void Settings_SetEditorRootFolder() {

            string path = EditorUtility.OpenFolderPanel("Select the vBug save-data root folder", Frankfort.VBug.Internal.vBugEnvironment.GetSaveRootFolder(), null);

            if (string.IsNullOrEmpty(path) || !System.IO.Directory.Exists(path)) {
                Debug.LogError("Something went wrong! " + path);
                return;
            }

            Frankfort.VBug.Internal.vBugEnvironment.SetSaveRootFolder(path);
        }
        //--------------- Settings --------------------

    }
}