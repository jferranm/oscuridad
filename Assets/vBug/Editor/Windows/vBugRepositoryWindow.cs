using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Frankfort.VBug.Internal;


namespace Frankfort.VBug.Editor
{
    public class vBugRepositoryWindow : vBugBaseWindow
    {
        private string rootFolder;
        private int userFolderIdx = 0;
        private int dateFolderIdx = 0; 
        private int sessionFolderIdx = 0;
        private string[] userFolders;
        private string[] dateFolders;
        private string[] sessionFolders;
        private string currentVerticalSliceFolder;
        private bool doUpdate = false;

        protected override void OnEnable() {
            base.OnEnable();
            this.title = "vBug Repository";

            string path = vBugEnvironment.GetSaveRootFolder();
            if(Directory.Exists(path)){
                rootFolder = path;
                InitRootFolder();
            }
        }
                
        protected override void OnPlayStateChanged() {
            base.OnPlayStateChanged();
            if (!Application.isPlaying) {
                doUpdate = true;
                Repaint();
            }
        }

        protected override void DisposeAll() {
 	        base.DisposeAll();
            rootFolder = string.Empty;
            userFolderIdx = 0;
            dateFolderIdx = 0;
            sessionFolderIdx = 0;
            userFolders = null;
            dateFolders = null;
            sessionFolders = null;
        }


        void OnGUI() {
            try {
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                    return;

                VisualResources.DrawWindowBGWaterMark(this.position);
                GUILayout.BeginVertical();
                DrawGUIFileSelector();
                GUILayout.EndHorizontal();
            } catch (Exception e) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }





        //--------------------------------------- FILE SELECTION --------------------------------------
        //--------------------------------------- FILE SELECTION --------------------------------------
        #region FILE SELECTION


        private void DrawGUIFileSelector()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(string.IsNullOrEmpty(rootFolder) ? "Open vBug savedata root folder" : "Root: " + IOEditorHelper.AbreviatePaths(rootFolder, 30), EditorHelper.styleButton)) {
                userFolderIdx = 0;
                OpenBrowseFolderWindow();
                doUpdate = true;
            }

            bool isWindows = Application.platform == RuntimePlatform.WindowsEditor;
            GUI.enabled = !string.IsNullOrEmpty(rootFolder) && isWindows;
            if (GUILayout.Button("Open in " + (isWindows ? "Explorer" : "Finder"), EditorHelper.styleButton, GUILayout.Width(125)))
                IOEditorHelper.OpenInExplorer(rootFolder);
            
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            //--------------- User folder --------------------
            if (doUpdate || (userFolders != null && userFolders.Length > 0)) {
                string[] folders = IOEditorHelper.GetDirectoryNames(userFolders);
                if (folders != null) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Users:", EditorHelper.styleLabel, GUILayout.Width(100));
                    userFolderIdx = Mathf.Max(0, EditorGUILayout.Popup("", userFolderIdx, folders, EditorHelper.stylePopup)); //Select latest
                    GUILayout.EndHorizontal();
                    doUpdate = doUpdate || GUI.changed;
                }
            } else {
                userFolderIdx = 0;
                GUILayout.Box("please select a rootfolder.", EditorHelper.styleBox, GUILayout.Width(this.position.width - 8));
            }
            //--------------- User folder --------------------


            //--------------- Dates --------------------
            if (userFolders != null && userFolders.Length > 0) {
                if (doUpdate) {
                    dateFolderIdx = 0;
                    dateFolders = IOEditorHelper.GetUserDateDirs(userFolders[userFolderIdx]);
                }

                if (dateFolders != null && dateFolders.Length > 0) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Date:", EditorHelper.styleLabel, GUILayout.Width(100));
                    dateFolderIdx = EditorGUILayout.Popup("", dateFolderIdx, IOEditorHelper.GetDirectoryNames(dateFolders), EditorHelper.stylePopup);
                    GUILayout.EndHorizontal();
                    doUpdate = doUpdate || GUI.changed;
                } else {
                    dateFolderIdx = 0;
                    GUILayout.Box("selected user-folder is empy.", EditorHelper.styleBox, GUILayout.Width(this.position.width - 8));
                }
            } else {
                dateFolderIdx = 0;
                GUILayout.Box("please select an user-folder.", EditorHelper.styleBox, GUILayout.Width(this.position.width - 8));
            }
            //--------------- Dates --------------------



            //--------------- Sessions --------------------
            if (dateFolders != null && dateFolders.Length > 0) {
                if (doUpdate) {
                    sessionFolderIdx = 0;
                    sessionFolders = IOEditorHelper.GetDateSessionDirs(dateFolders[dateFolderIdx]);
                }

                if (sessionFolders != null && sessionFolders.Length > 0) {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Session:", EditorHelper.styleLabel, GUILayout.Width(100));
                    sessionFolderIdx = EditorGUILayout.Popup("", sessionFolderIdx, IOEditorHelper.GetDirectoryNames(sessionFolders), EditorHelper.stylePopup);
                    GUILayout.EndHorizontal();
                    string slicePath = sessionFolders[sessionFolderIdx];
                    LoadVerticalSlices(slicePath);
                } else {
                    sessionFolderIdx = 0;
                    GUILayout.Box("selected date-folder is empty.", EditorHelper.styleBox, GUILayout.Width(this.position.width - 8));
                }
            } else {
                sessionFolderIdx = 0;
                GUILayout.Box("please select a date-folder.", EditorHelper.styleBox, GUILayout.Width(this.position.width - 8));
            }
            //--------------- Sessions --------------------

            GUILayout.EndVertical();
            doUpdate = false;
        }




        private void OpenBrowseFolderWindow() {
            string path = vBugEnvironment.GetSaveRootFolder();
            if(!Directory.Exists(path))
                path = null;

            rootFolder = IOSharedHelper.EscapePath(EditorUtility.OpenFolderPanel("Select the vBug save-data root folder", path, null));
            //if (rootFolder != path)
            //    vBugEnvironment.SetSaveRootFolder(rootFolder);

            InitRootFolder();
        }

        private void InitRootFolder() {
            if (!string.IsNullOrEmpty(rootFolder) && Directory.Exists(rootFolder)) {
                userFolders = IOEditorHelper.GetUserDirs(rootFolder);

                userFolderIdx = 0;
                dateFolderIdx = 0;
                sessionFolderIdx = 0;

                if (userFolders != null && userFolders.Length > 0)
                    dateFolders = IOEditorHelper.GetDateSessionDirs(userFolders[0]);

                if (dateFolders != null && dateFolders.Length > 0)
                    sessionFolders = IOEditorHelper.GetSessionsMinutesDirs(dateFolders[0]);

                doUpdate = true;
                Repaint();
            }
        }

        #endregion
        //--------------------------------------- FILE SELECTION --------------------------------------
        //--------------------------------------- FILE SELECTION --------------------------------------










        //--------------------------------------- VERTICAL SLICE --------------------------------------
        //--------------------------------------- VERTICAL SLICE --------------------------------------
        #region VERTICAL SLICE


        private void LoadVerticalSlices(string path) {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return;

            if (path != currentVerticalSliceFolder || !VerticalSliceDatabase.isInitialized) {
                currentVerticalSliceFolder = path;
                VerticalSliceDatabase.SetSliceLocations(IOEditorHelper.GetVBugSliceLocations(IOEditorHelper.GetSessionsMinutesDirs(path)));
            }
        }


        #endregion
        //--------------------------------------- VERTICAL SLICE --------------------------------------
        //--------------------------------------- VERTICAL SLICE --------------------------------------

    }
}
