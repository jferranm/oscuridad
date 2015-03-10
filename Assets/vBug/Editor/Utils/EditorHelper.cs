using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;





namespace Frankfort.VBug.Editor
{
    [AddComponentMenu(""), ExecuteInEditMode, RequireComponent(typeof(Camera))]
    public class PostRenderBubble : MonoBehaviour{
        public delegate void Bubble();
        public Bubble callback;
        
        private void OnPostRender() {
            if (callback != null)
                callback();
        }
    }

    public struct SingleLine {
        public Vector3 a;
        public Vector3 b;
        public Color c;
        public float w;

        public SingleLine(Vector3 a, Vector3 b, Color c, float width) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.w = width;
        }
    }


    public static class EditorHelper
    {
        private static bool isInitialized = false;
        private static List<SingleLine> debugLines = new List<SingleLine>();
        private static int sceneViewsRendered;
        private static Dictionary<int, PostRenderBubble> postRenderBubbler = new Dictionary<int,PostRenderBubble>();


		//--------------------------------------- PRO SKIN STYLES --------------------------------------
		//--------------------------------------- PRO SKIN STYLES --------------------------------------
        #region PRO SKIN STYLES

        private static GUIStyle CreateProStyle(string origional, Color color, TextAnchor alignment, int fontsize = 12) {
            GUIStyle result = new GUIStyle(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene).GetStyle(origional));
            result.normal.textColor = color;
            result.alignment = alignment;
            result.fontSize = fontsize;
            return result;
        }


        //--------------- Customs --------------------
        private static GUIStyle _styleLabelNA;
        public static GUIStyle styleLabelNA {
            get {
                if (_styleLabelNA == null)
                    _styleLabelNA = CreateProStyle("label", Color.gray, TextAnchor.MiddleCenter, 20);

                return _styleLabelNA;
            }
        }

        private static GUIStyle _styleLabelLightGray10;
        public static GUIStyle styleLabelLightGray10 {
            get {
                if (_styleLabelLightGray10 == null)
                    _styleLabelLightGray10 = CreateProStyle("label", new Color(0.666f, 0.666f, 0.666f), TextAnchor.UpperLeft, 10);

                return _styleLabelLightGray10;
            }
        }

        private static GUIStyle _styleLabelLightGray12;
        public static GUIStyle styleLabelLightGray12 {
            get {
                if (_styleLabelLightGray12 == null)
                    _styleLabelLightGray12 = CreateProStyle("label", new Color(0.666f, 0.666f, 0.666f), TextAnchor.UpperLeft, 12);

                return _styleLabelLightGray12;
            }
        }

        private static GUIStyle _styleLabelSceneVisuals;
        public static GUIStyle styleLabelSceneVisuals {
            get {
                if (_styleLabelSceneVisuals == null)
                    _styleLabelSceneVisuals = CreateProStyle("label", Color.white, TextAnchor.MiddleCenter, 9);

                return _styleLabelSceneVisuals;
            }
        }

        private static GUIStyle _styleLabelKeyboard;
        public static GUIStyle styleLabelKeyboard {
            get {
                if (_styleLabelKeyboard == null)
                    _styleLabelKeyboard = CreateProStyle("label", Color.white, TextAnchor.MiddleCenter, 9);

                return _styleLabelKeyboard;
            }
        }

        private static GUIStyle _styleLabelCoreSmall;
        public static GUIStyle styleLabelCoreSmall {
            get {
                if (_styleLabelCoreSmall == null)
                    _styleLabelCoreSmall = CreateProStyle("label", Color.white, TextAnchor.MiddleLeft, 9);

                return _styleLabelCoreSmall;
            }
        }

        private static GUIStyle _styleLabelCoreBig;
        public static GUIStyle styleLabelCoreBig {
            get {
                if (_styleLabelCoreBig == null) {
                    _styleLabelCoreBig = CreateProStyle("label", Color.white, TextAnchor.MiddleCenter, 16);
                    _styleLabelCoreBig.fontStyle = FontStyle.Bold;
                }

                return _styleLabelCoreBig;
            }
        }

        private static GUIStyle _styleLabelCoreTooltip;
        public static GUIStyle styleLabelCoreTooltip {
            get {
                if (_styleLabelCoreTooltip == null) {
                    _styleLabelCoreTooltip = CreateProStyle("label", Color.white, TextAnchor.MiddleCenter, 14);
                    _styleLabelCoreTooltip.fontStyle = FontStyle.Bold;
                }

                return _styleLabelCoreTooltip;
            }
        }

        private static GUIStyle _styleSelectableLabel;
        public static GUIStyle styleSelectableLabel {
            get {
                if (_styleSelectableLabel == null)
                    _styleSelectableLabel = CreateProStyle("label", Color.gray, TextAnchor.UpperLeft);

                return _styleSelectableLabel;
            }
        }


        private static GUIStyle _styleHierarchyFoldout;
        public static GUIStyle styleHierarchyFoldout {
            get {
                if (_styleHierarchyFoldout == null) {
                    _styleHierarchyFoldout = CreateProStyle("foldout", Color.gray, TextAnchor.UpperLeft);
                    _styleHierarchyFoldout.onHover.textColor = _styleHierarchyFoldout.onFocused.textColor = _styleHierarchyFoldout.focused.textColor = Color.white;
                    _styleHierarchyFoldout.active.background = _styleHierarchyFoldout.focused.background = VisualResources.hierarchySelectionBlue;
                }

                return _styleHierarchyFoldout;
            }
        }

        private static GUIStyle _styleHierarchyLabel;
        public static GUIStyle styleHierarchyLabel {
            get {
                if (_styleHierarchyLabel == null) {
                    _styleHierarchyLabel = CreateProStyle("button", Color.gray, TextAnchor.UpperLeft);
                    _styleHierarchyLabel.onHover.textColor = _styleHierarchyLabel.onFocused.textColor = _styleHierarchyLabel.focused.textColor = Color.white;
                    _styleHierarchyLabel.normal.background = _styleHierarchyLabel.active.background = _styleHierarchyLabel.hover.background = null;
                    _styleHierarchyLabel.active.background = _styleHierarchyLabel.focused.background = VisualResources.hierarchySelectionBlue;
                    _styleHierarchyLabel.alignment = TextAnchor.UpperLeft;
                    _styleHierarchyLabel.border = _styleHierarchyLabel.margin = _styleHierarchyLabel.padding = new RectOffset(0, 0, 0, 0);

                }

                return _styleHierarchyLabel;
            }
        }
        //--------------- Customs --------------------



        //--------------- Basics --------------------
        private static GUIStyle _styleLabel;
        public static GUIStyle styleLabel {
            get {
                if (_styleLabel == null)
                    _styleLabel = CreateProStyle("label", Color.white, TextAnchor.UpperLeft);

                return _styleLabel;
            }
        }

        private static GUIStyle _styleBox;
        public static GUIStyle styleBox {
            get {
                if (_styleBox == null)
                    _styleBox = CreateProStyle("box", Color.gray, TextAnchor.UpperLeft);

                return _styleBox;
            }
        }

        private static GUIStyle _styleButton;
        public static GUIStyle styleButton {
            get {
                if (_styleButton == null)
                    _styleButton = CreateProStyle("button", new Color(0.666f, 0.666f, 0.666f), TextAnchor.MiddleCenter);

                return _styleButton;
            }
        }

        private static GUIStyle _styleToggle;
        public static GUIStyle styleToggle {
            get {
                if (_styleToggle == null)
                    _styleToggle = CreateProStyle("toggle", new Color(0.666f, 0.666f, 0.666f), TextAnchor.MiddleLeft);

                return _styleToggle;
            }
        }

        private static GUIStyle _styleToggleLeft;
        public static GUIStyle styleToggleLeft {
            get {
                if (_styleToggleLeft == null)
                    _styleToggleLeft = CreateProStyle("toggleLeft", new Color(0.666f, 0.666f, 0.666f), TextAnchor.MiddleLeft);

                return _styleToggleLeft;
            }
        }

        private static GUIStyle _stylePopup;
        public static GUIStyle stylePopup {
            get {
                if (_stylePopup == null)
                    _stylePopup = CreateProStyle("popup", new Color(0.666f, 0.666f, 0.666f), TextAnchor.MiddleLeft, 10);

                return _stylePopup;
            }
        }
        //--------------- Basics --------------------
			


		#endregion
		//--------------------------------------- PRO SKIN STYLES --------------------------------------
		//--------------------------------------- PRO SKIN STYLES --------------------------------------














        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------
        #region GUI


        public static UnityEngine.Object GetFromSelectionByID(int instanceID) {
            if (Selection.objects == null || Selection.objects.Length == 0)
                return null;

            List<UnityEngine.Object> selectedRecursive = new List<UnityEngine.Object>();
            foreach (UnityEngine.Object obj in Selection.objects)
                AddObjectRecursively(obj, ref selectedRecursive);

            if (selectedRecursive.Count > 0) {
                int i = selectedRecursive.Count;
                while (--i > -1) {
                    if (instanceID == selectedRecursive[i].GetInstanceID())
                        return selectedRecursive[i];
                }
            }

            return null;
        }

        private static void AddObjectRecursively(UnityEngine.Object obj, ref List<UnityEngine.Object> storeTo) {
            storeTo.Add(obj);
            if (obj is GameObject) {
                Transform trans = (obj as GameObject).transform;
                foreach (Transform child in trans)
                    AddObjectRecursively(child.gameObject, ref storeTo);

                Component[] components = (obj as GameObject).GetComponents<Component>();
                foreach (Component component in components)
                    AddObjectRecursively(component, ref storeTo);
            }
        }




        public static void DrawDragBar(EditorWindow window, ref Rect rect, ref bool resize, ref float currentWindowHeight, int height = 6) {
            rect.Set(0, rect.y, window.position.width, height);
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeVertical);

            Vector2 mousePos = Event.current.mousePosition;
            if (Event.current.type == EventType.mouseDown && rect.Contains(mousePos))
                resize = true;

            if (resize)
                window.position.Contains(mousePos);

            if (Event.current.type == EventType.MouseUp)
                resize = false;

            if (resize) {
                rect.y = Event.current.mousePosition.y;
                window.Repaint();
            } else {
                float winHeight = window.position.height;
                if (winHeight != currentWindowHeight && currentWindowHeight != 0f) 
                    rect.y = (rect.y / currentWindowHeight) * winHeight;
                
                currentWindowHeight = winHeight;
            }

            rect.y = Mathf.Clamp(rect.y, 40, currentWindowHeight - 40);
            VisualResources.DrawTexture(rect, VisualResources.dragBarGray);
        }



        public static void DrawNotInitializedLabel(Rect windowPos) {
            EditorHelper.DrawNA(new Rect(0,0, windowPos.width, windowPos.height), "Please load a recorded session...\n(You can do so by opening up the vBug-Repository window, then select the root folder.)");
        }

        public static void DrawNA(Rect rect, string subMessage = null) {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            rect.y -= 20;
            GUI.Label(rect, "N/A", styleLabelNA);
            if (!string.IsNullOrEmpty(subMessage)) {
                rect.y += 40;
                styleLabelNA.fontSize = 12;
                GUI.Label(rect, subMessage, styleLabelNA);
            }
            styleLabelNA.fontSize = 20;
        }


        #endregion
        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------
			











        //--------------------------------------- Draw bounds etc --------------------------------------
        //--------------------------------------- Draw bounds etc --------------------------------------
        #region Draw bounds etc

        public static void DrawBounds(Bounds bounds, Color color, float width = 1f) {
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;

            Vector3[] boundPoint = new Vector3[8];
            boundPoint[0] = center + extents; extents.x = -extents.x;
            boundPoint[1] = center + extents; extents.y = -extents.y;
            boundPoint[2] = center + extents; extents.x = -extents.x;
            boundPoint[3] = center + extents; extents.y = -extents.y;
            extents.z = -extents.z;
            boundPoint[4] = center + extents; extents.x = -extents.x;
            boundPoint[5] = center + extents; extents.y = -extents.y;
            boundPoint[6] = center + extents; extents.x = -extents.x;
            boundPoint[7] = center + extents; extents.y = -extents.y;

            SceneviewDrawLine(boundPoint[0], boundPoint[1], color, width);
            SceneviewDrawLine(boundPoint[1], boundPoint[2], color, width);
            SceneviewDrawLine(boundPoint[2], boundPoint[3], color, width);
            SceneviewDrawLine(boundPoint[3], boundPoint[0], color, width);

            SceneviewDrawLine(boundPoint[4], boundPoint[5], color, width);
            SceneviewDrawLine(boundPoint[5], boundPoint[6], color, width);
            SceneviewDrawLine(boundPoint[6], boundPoint[7], color, width);
            SceneviewDrawLine(boundPoint[7], boundPoint[4], color, width);

            SceneviewDrawLine(boundPoint[0], boundPoint[4], color, width);
            SceneviewDrawLine(boundPoint[1], boundPoint[5], color, width);
            SceneviewDrawLine(boundPoint[2], boundPoint[6], color, width);
            SceneviewDrawLine(boundPoint[3], boundPoint[7], color, width);
        } 
        #endregion
        //--------------------------------------- Draw bounds etc --------------------------------------
        //--------------------------------------- Draw bounds etc --------------------------------------
			








        //--------------------------------------- SCENE VIEW LINE RENDERING --------------------------------------
        //--------------------------------------- SCENE VIEW LINE RENDERING --------------------------------------
        #region SCENE VIEW LINE RENDERING

        public static void Initialize() {
            if (isInitialized)
                return;

            SceneView.onSceneGUIDelegate += OnSceneViewRender;
            isInitialized = true;
        }

        public static void Dispose() {
            if (!isInitialized)
                return;

            SceneView.onSceneGUIDelegate -= OnSceneViewRender;
            if (postRenderBubbler != null && postRenderBubbler.Count > 0) {
                foreach (KeyValuePair<int, PostRenderBubble> pair in postRenderBubbler)
                    UnityEngine.Object.DestroyImmediate(pair.Value);

                postRenderBubbler.Clear();
            }

            debugLines.Clear();
            isInitialized = false;
        }

        private static void OnSceneViewRender(SceneView view) {
            if (postRenderBubbler != null && !postRenderBubbler.ContainsKey(view.camera.GetInstanceID())){
                PostRenderBubble bubbler = view.camera.gameObject.AddComponent<PostRenderBubble>();
                bubbler.callback = DrawLinesPostRender;
                postRenderBubbler.Add(view.camera.GetInstanceID(), bubbler);
            }
        }


        public static void SceneviewDrawLine(Vector3 from, Vector3 to, Color color, float width = 1f) {
            Initialize();
            debugLines.Add(new SingleLine(from, to, color, width));
        }


        private static void DrawLinesPostRender(){
            if (debugLines.Count > 0) {
                int i = debugLines.Count;
                while (--i > -1) {
                    SingleLine line = debugLines[i];
                    Handles.color = line.c;
                    Handles.DrawAAPolyLine(line.w, line.a, line.b);
                }
            }
            Handles.color = Color.white;
            
            sceneViewsRendered++;
            if (sceneViewsRendered >= postRenderBubbler.Count) {
                sceneViewsRendered = 0;
                debugLines.Clear();
            }
        }
        #endregion
        //--------------------------------------- SCENE VIEW LINE RENDERING --------------------------------------
        //--------------------------------------- SCENE VIEW LINE RENDERING --------------------------------------

    }
}
