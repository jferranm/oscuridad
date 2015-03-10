using System;
using System.Collections.Generic;
using UnityEngine;
using Frankfort.VBug.Internal;
using UnityEditor;

namespace Frankfort.VBug.Editor
{

    public class vBugBaseWindow : EditorWindow
    {
        //--------------- Managed open & closed --------------------
        private static List<vBugBaseWindow> isOpen = new List<vBugBaseWindow>();

        internal static bool CheckIfOpen(Type type) {
            return isOpen.FindIndex(item => item.GetType() == type) != -1;
        }

        public static vBugBaseWindow[] GetAllWindows() {
            return isOpen.ToArray();
        }
        //--------------- Managed open & closed --------------------
			




        protected bool hasFocus { get; private set; }
        protected bool repaintOnFocus;
        private string editorCurrentSceneName;

        protected virtual void OnFocus()
        {
            hasFocus = true;
            Repaint();
        }
        protected virtual void OnFocusLost()
        {
            hasFocus = false;
        }

        private void RebuildOpenList() {
            isOpen = new List<vBugBaseWindow>(Resources.FindObjectsOfTypeAll<vBugBaseWindow>());
        }


        protected virtual void OnEnable() {
            RebuildOpenList(); 
            editorCurrentSceneName = EditorApplication.currentScene;
            EditorApplication.playmodeStateChanged += OnPlayStateChanged;
            EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
            vBugCoreScenePanel.OnEnable();
            EditorHelper.Initialize();
        }


        protected virtual void OnDisable() {
            RebuildOpenList();
            isOpen.Remove(this);

            EditorApplication.playmodeStateChanged -= OnPlayStateChanged;
            EditorApplication.hierarchyWindowChanged -= OnHierarchyWindowChanged;

            if (isOpen.Count == 0) {
                VisualResources.Prune();
                VerticalSliceDatabase.Reset(); 
                TexturePool.PruneCache();
                MaterialSerializeHelper.PruneCache();
                EditorHelper.Dispose();
                vBugCoreScenePanel.Dispose();
            }

            DisposeAll();
        }
         
        
        protected virtual void OnHierarchyWindowChanged() {
            if (EditorApplication.currentScene != editorCurrentSceneName) {
                VisualResources.Prune();
                VerticalSliceDatabase.Reset();
                TexturePool.PruneCache();
                MaterialSerializeHelper.PruneCache();
                vBugCoreScenePanel.ResetSceneVisuals();
                ReinitSceneVisuals();
                editorCurrentSceneName = EditorApplication.currentScene;
            }
        }


        protected virtual void OnPlayStateChanged()
        {
            VisualResources.Prune();
            VerticalSliceDatabase.Reset();
            TexturePool.PruneCache();
            MaterialSerializeHelper.PruneCache();
			ShaderPropertyIndexer.UpdateShaderPropertiesXML(false);

            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode) {
                DisposeSceneVisuals();
            } else if (!EditorApplication.isPlaying) {
                ReinitSceneVisuals();
            }
        }


        
        protected virtual void Update() {
            if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (repaintOnFocus)
                Repaint();

            if (CheckPrimaryVBugWindow())
                VerticalSliceDatabase.Update();
        }



        //--------------- Check other windows --------------------
        protected bool CheckPrimaryVBugWindow() {
            return isOpen.Count > 0 && isOpen[0] == this;
        }
        //--------------- Check other windows --------------------
			

        //--------------- Dispose --------------------
        protected virtual void DisposeAll()
        { }
        //--------------- Dispose --------------------


        //--------------- Toggle playmode --------------------
        protected virtual void ReinitSceneVisuals()
        { }

        protected virtual void DisposeSceneVisuals()
        { }
        //--------------- Toggle playmode --------------------
			



        //--------------- Notifications --------------------
        public virtual void NotifyTimelineChange(int newFrame, int senderID) {
            if (CheckPrimaryVBugWindow())
                vBugCoreScenePanel.NotifyTimelineChange(newFrame, senderID);
        }

        public virtual void NotifySessionChange(int startFrame, int senderID) {
            if (CheckPrimaryVBugWindow())
                vBugCoreScenePanel.NotifySessionChange(startFrame);
        }
        public virtual void NotifyRecordingStart(int senderID)
        { }

        public virtual void NotifyRecordingStop(int senderID)
        { }

        public virtual void NotifyDeviceConnected(int senderID)
        { }

        public virtual void NotifyDeviceDisconnected(int senderID)
        { }


        public virtual void NotifyVerticalSliceBundleLoaded(VerticalActivitySlice[] slices) {
            if (CheckPrimaryVBugWindow())
                vBugCoreScenePanel.NotifyVerticalSliceBundleLoaded();
        }

        //--------------- Notifications --------------------


    }
}