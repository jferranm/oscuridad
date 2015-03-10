using System;
using Frankfort.VBug.Internal;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


namespace Frankfort.VBug.Editor {
    public class vBugHierarchyWindow : vBugBaseWindow {

        private Vector2 hierarchyPosition;
        private Vector2 inspectorPosition;
        private Dictionary<int, bool> hierarchyFoldoutStates = new Dictionary<int, bool>();

        private Dictionary<string, bool> inspectorFoldoutStates = new Dictionary<string, bool>();
        private Dictionary<long, bool> eolStates = new Dictionary<long, bool>();
        private Dictionary<long, bool> birthStates = new Dictionary<long, bool>();

        private Dictionary<int, ExposedGameObject[]> rootGOs = new Dictionary<int, ExposedGameObject[]>();
        private Dictionary<long, ExposedGameObject[]> childrenPerGO = new Dictionary<long, ExposedGameObject[]>();
        private Dictionary<long, ExposedComponent[]> componentsPerGO = new Dictionary<long, ExposedComponent[]>();

        private VerticalActivitySlice currentSlice;
        private int currentFrameNumber;



        private Rect dragBarRect;
        private float dragBarCurrentWinHeight;
        private bool dragBarResize;

        private bool mouseWasUp;
        private int currentSelectedID = -1;
        private int nextSelectedID = -1;
        private ExposedGameObject currentSelection;
        private string currentFocusControlName;
        private bool forceFocusControll = false;

        private float depthSpacing = 20;
        private double downTimeStamp;
        private List<string> controllIndexing = new List<string>();


        protected override void OnEnable() {
            base.OnEnable();
            this.title = "vBug Hierarchy";

            string dragKey = this.GetType().Name + "_dragBarY";
            if (EditorPrefs.HasKey(dragKey)) {
                dragBarRect.y = EditorPrefs.GetFloat(dragKey);
            } else {
                dragBarRect.y = this.position.height * 0.8f;
            }

            if (VerticalSliceDatabase.isInitialized)
                SetCurrentSlice(vBugWindowMediator.currentFrameNumber);

        }


        protected override void OnDisable() {
            base.OnDisable();
            ResetFocus();
            EditorPrefs.SetFloat(this.GetType().Name + "_dragBarY", dragBarRect.y);
        }

        protected override void ReinitSceneVisuals() {
            base.ReinitSceneVisuals();
            ResetFocus(); //needed to fix the blue-bar-controll-focus issue
            ClearDictionaries();
        }

        protected override void DisposeAll() {
            base.DisposeAll();
            ClearDictionaries();
        }

        private void ClearDictionaries() {
            hierarchyFoldoutStates.Clear();
            inspectorFoldoutStates.Clear();
            eolStates.Clear();
            birthStates.Clear();

            rootGOs.Clear();
            childrenPerGO.Clear();
            componentsPerGO.Clear();
        }


        private void ResetFocus() {
            currentFocusControlName = null;
            currentSelectedID = -1;
            currentSelection = null;
        }



        //--------------- Get frame --------------------
        public override void NotifyTimelineChange(int newFrame, int senderID) {
            base.NotifyTimelineChange(newFrame, senderID);
            SetCurrentSlice(newFrame);
        }


        public override void NotifySessionChange(int startFrame, int senderID) {
            base.NotifySessionChange(startFrame, senderID);
            ClearDictionaries();
            SetCurrentSlice(startFrame);
        }


        public override void NotifyVerticalSliceBundleLoaded(VerticalActivitySlice[] slices) {
            base.NotifyVerticalSliceBundleLoaded(slices);
            ClearDictionaries();
            SetCurrentSlice(currentFrameNumber);
        } 


        private void SetCurrentSlice(int frameNumber) {
            currentSlice = null;
            int iMin = Mathf.Max(VerticalSliceDatabase.minRange, frameNumber - vBugEditorSettings.PlaybackHierarchySearchRange);

            for (int i = frameNumber; i >= iMin; i--) {
                currentSlice = VerticalSliceDatabase.GetSlice(i);

                if (currentSlice != null && currentSlice.gameObjectsSnapshot != null && currentSlice.gameObjectsSnapshot.gameObjects != null) {
                    currentFrameNumber = i;
                    currentSelection = null;
                    break;
                }
            }
        }

        private void SortReflectedObjectsByName(ExposedGameObject[] exposedGameObjects) {
            Array.Sort(exposedGameObjects, (a, b) => a.name.CompareTo(b.name) + (a.isHiddenOrDontSave.CompareTo(b.isHiddenOrDontSave) * 100000));
        }
        //--------------- Get frame --------------------

















        //--------------------------------------- NAVIGATION --------------------------------------
        //--------------------------------------- NAVIGATION --------------------------------------
        #region NAVIGATION


        public void NavigateToID(int gameObjectID) {
            if (currentSlice == null || currentSlice.gameObjectsSnapshot == null || currentSlice.gameObjectsSnapshot.gameObjects == null || currentSlice.gameObjectsSnapshot.gameObjects.Length == 0)
                return;

            ExposedGameObject selectedGO = null;
            hierarchyFoldoutStates.Clear(); //Collapse all

            int scrollPos = FindScrollPosByID(currentSlice.gameObjectsSnapshot.gameObjects, currentSlice, gameObjectID, 0, ref selectedGO);
            if (scrollPos != -1)
                this.hierarchyPosition.y = scrollPos;

            SetNextSelection(gameObjectID);
        }


        private int FindScrollPosByID(ExposedGameObject[] gameObjects, VerticalActivitySlice slice, int instanceID, int currentScrollPos, ref ExposedGameObject selectedGO) {
            if (gameObjects == null || instanceID == -1) {
                selectedGO = null;
                return -1;
            }

            //--------------- Scan children second --------------------
            int offset = 0;
            int increment = 16;
            foreach (ExposedGameObject obj in gameObjects) {
                if (obj.transformID == instanceID) {
                    selectedGO = obj;
                    return currentScrollPos + offset;
                } else {
                    ExposedGameObject[] children = GetChildren(obj.transformID, slice);
                    if (children != null && children.Length > 0) {
                        increment = 18;
                        int result = FindScrollPosByID(children, slice, instanceID, currentScrollPos + offset + increment, ref selectedGO);
                        if (result != -1) {
                            if (hierarchyFoldoutStates.ContainsKey(obj.transformID)) {
                                hierarchyFoldoutStates[obj.transformID] = true;
                            } else {
                                hierarchyFoldoutStates.Add(obj.transformID, true);
                            }
                            return result;
                        }
                    } else {
                        increment = 16;
                    }
                }
                offset += increment;
            }
            //--------------- Scan children second --------------------
            return -1;
        }



        private void SetNextSelection(int instanceID) {
            string nextName = "hierarchy_" + instanceID;
            if (currentFocusControlName == nextName)
                return;

            nextSelectedID = instanceID;
            currentSelection = null;
            currentFocusControlName = nextName;
            forceFocusControll = true;
            GUI.FocusControl(currentFocusControlName);
        }
        #endregion
        //--------------------------------------- NAVIGATION --------------------------------------
        //--------------------------------------- NAVIGATION --------------------------------------









        //--------------------------------------- ON GUI HIERARCHY --------------------------------------
        //--------------------------------------- ON GUI HIERARCHY --------------------------------------
        #region ON GUI HIERARCHY



        protected override void Update() {
            base.Update();

            if (hasFocus)
                Repaint();
        }

        void OnGUI() {
            try {
                VisualResources.DrawWindowBGWaterMark(this.position);
                UpdateKeyInput();
                DrawGUI();
            } catch (Exception e) {
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            }
        }

        private void UpdateKeyInput() {
            if (!hasFocus)
                return;

            EventType eventType = Event.current.type;
            if (eventType == EventType.keyDown && downTimeStamp == -1d) {
                downTimeStamp = vBugEnvironment.GetUnixTimestamp();
            } else if (eventType == EventType.keyUp || (downTimeStamp != -1 && vBugEnvironment.GetUnixTimestamp() - downTimeStamp > 0.3f)) {
                if (eventType == EventType.keyUp)
                    downTimeStamp = -1d;

                //--------------- Get Key-direction --------------------
                int direction = 0;
                KeyCode keyCode = Event.current.keyCode;
                if (keyCode == KeyCode.UpArrow) {
                    direction = -1;
                } else if (keyCode == KeyCode.DownArrow) {
                    direction = +1;
                }
                //--------------- Get Key-direction --------------------

                //--------------- Shift focus --------------------
                if (direction != 0 && nextSelectedID != -1) {
                    int idx = controllIndexing.IndexOf("hierarchy_" + currentSelectedID);
                    if (idx != -1) {
                        string nextControll = controllIndexing[Mathf.Clamp(idx + direction, 0, controllIndexing.Count - 1)];

                        SetNextSelection(int.Parse(nextControll.Split('_')[1]));
                    }
                    hierarchyPosition.y += direction * 16;
                }
                //--------------- Shift focus --------------------

            }
        }


        private void DrawGUI() {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (!VerticalSliceDatabase.isInitialized) {
                EditorHelper.DrawNotInitializedLabel(this.position);

            } else if (currentSlice == null || currentSlice.gameObjectsSnapshot == null || currentSlice.gameObjectsSnapshot.gameObjects == null || currentSlice.gameObjectsSnapshot.gameObjects.Length == 0) {
                EditorHelper.DrawNA(new Rect(0, 0, this.position.width, this.position.height));

            } else {
                EventType eventType = Event.current.type;
                if (eventType == EventType.Layout && currentSelectedID != nextSelectedID || currentSelection == null) { //prevents gui-popping issues.
                    currentSelectedID = nextSelectedID;
                    currentFocusControlName = "hierarchy_" + nextSelectedID;
                    currentSelection = FindObjectByID(currentSlice.gameObjectsSnapshot.gameObjects, nextSelectedID);
                }

                Rect rect = new Rect(0, 0, this.position.width, dragBarRect.y);
                GUILayout.BeginArea(rect);

                hierarchyPosition = GUILayout.BeginScrollView(hierarchyPosition);

                GUILayout.BeginHorizontal();
                GUILayout.Space(32);
                GUILayout.BeginVertical();

                //int bFont = GUI.skin.label.fontSize;
                //GUI.skin.label.fontSize = 8;
                controllIndexing.Clear();

                DrawGameObjectsRecursive(currentSlice, GetRootObjects(currentSlice), true, true);
                if (forceFocusControll) {
                    GUI.FocusControl(currentFocusControlName);
                    forceFocusControll = false;
                }

                //GUI.skin.label.fontSize = bFont;

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndScrollView();
                GUILayout.EndArea();

                EditorHelper.DrawDragBar(this, ref dragBarRect, ref dragBarResize, ref dragBarCurrentWinHeight);
                DrawSelectedGOComponents();
            }
        }




        private void DrawGameObjectsRecursive(VerticalActivitySlice slice, ExposedGameObject[] gameObjects, bool parentActive, bool allowMouseClick) {
            EventType eventType = Event.current.type;
            GUILayout.BeginVertical();

            Rect lastRect = default(Rect);

            foreach (ExposedGameObject go in gameObjects) {
                ExposedGameObject[] children = GetChildren(go.transformID, slice);

                bool isFoldout = children != null && children.Length > 0;

                //--------------- Draw recursive --------------------
                if (!hierarchyFoldoutStates.ContainsKey(go.transformID))
                    hierarchyFoldoutStates.Add(go.transformID, false);

                bool localActive = parentActive && go.activeSelf;
                string label = go.name;
                GUI.color = localActive ? Color.white : new Color(0.6f, 0.6f, 0.6f);

                if (go.isHiddenOrDontSave && go.transformID != nextSelectedID) {
                    GUI.color = new Color(0.1f, 0.1f, 0.1f);
                    label = "[hidden]" + label;
                } else if (go.isStatic) {
                    GUI.color = new Color(0.8f, 0.8f, 0.8f);
                    label = "[static] " + label;
                } else {
                    GUI.color = Color.white;
                }

                string currentControlName = "hierarchy_" + go.transformID;
                GUI.SetNextControlName(currentControlName);
                controllIndexing.Add(currentControlName);

                bool drawInspIcon = eventType == EventType.repaint && go.components != null && go.components.Length > 0;
                bool drawEOLIcon = TestEOL(go.transformID, slice.header.frameNumber);
                bool drawBirthIcon = TestBirth(go.transformID, slice.header.frameNumber);
                int offset = 0;

                if (isFoldout) {
                    bool newState = EditorGUILayout.Foldout(hierarchyFoldoutStates[go.transformID], label, EditorHelper.styleHierarchyFoldout);
                    lastRect = GUILayoutUtility.GetLastRect();

                    if (newState != hierarchyFoldoutStates[go.transformID]) {
                        SetNextSelection(go.transformID);
                        if (newState)
                            SortReflectedObjectsByName(children);
                    }

                    hierarchyFoldoutStates[go.transformID] = newState;
                    if (newState) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(16);
                        DrawGameObjectsRecursive(slice, children, localActive, allowMouseClick);
                        GUILayout.EndHorizontal();
                    }

                    if (hasFocus && eventType == EventType.mouseUp && GUI.GetNameOfFocusedControl() == currentControlName)
                        SetNextSelection(go.transformID);


                } else {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(18);
                    offset = 14;

                    if (GUILayout.Button(label, EditorHelper.styleHierarchyLabel, GUILayout.Height(16)))
                        SetNextSelection(go.transformID);

                    lastRect = GUILayoutUtility.GetLastRect();
                    GUILayout.EndHorizontal();
                }

                if (drawInspIcon)
                    VisualResources.DrawIcon(EditorIcon.hierarchyAndInspector, 1, new Vector2(lastRect.x - 12 - offset, lastRect.y + 2), 1f, false);

                if (drawEOLIcon)
                    VisualResources.DrawIcon(EditorIcon.hierarchyAndInspector, 3, new Vector2(lastRect.x - 24 - offset, lastRect.y + 2), 1f, false);

                if (drawBirthIcon)
                    VisualResources.DrawIcon(EditorIcon.hierarchyAndInspector, 0, new Vector2(lastRect.x - 36 - offset, lastRect.y + 2), 1f, false);
                //--------------- Draw recursive --------------------

            }

            GUILayout.EndVertical();
            GUI.color = GUI.contentColor = GUI.backgroundColor = Color.white;
        }



        #endregion
        //--------------------------------------- ON GUI HIERARCHY --------------------------------------
        //--------------------------------------- ON GUI HIERARCHY --------------------------------------















        //--------------------------------------- Draw Components & Fields --------------------------------------
        //--------------------------------------- Draw Components & Fields --------------------------------------
        #region Draw Components & Fields


        private void DrawSelectedGOComponents() {
            if (currentSelectedID == -1 || currentSlice == null || currentSlice.gameObjectsSnapshot == null || currentSlice.gameObjectsSnapshot.gameObjects == null)
                return;

            Rect rect = new Rect(0, dragBarRect.yMax, this.position.width, this.position.height - dragBarRect.yMax);

            if (currentSelection == null) {
                EditorHelper.DrawNA(rect);

            } else {
                EventType eventType = Event.current.type;
                Rect topRect = new Rect(rect.x, rect.y, rect.width, 60);
                VisualResources.DrawTexture(topRect, VisualResources.proConsoleRowLight);

                GUILayout.BeginArea(topRect);
                GUILayout.BeginVertical();

                //TODO: Make difference between activeInHierarchy & selfActive

                //--------------- Name, active & static --------------------
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                Rect iconRect = GUILayoutUtility.GetRect(32, 32);
                VisualResources.DrawIcon(EditorIcon.playbackOptions, 2, new Vector2(iconRect.x, iconRect.y), 1f, false);
                EditorGUILayout.ToggleLeft(currentSelection.name, currentSelection.activeSelf, EditorHelper.styleToggleLeft);
                GUILayout.FlexibleSpace();
                EditorGUILayout.ToggleLeft("Static", currentSelection.isStatic, EditorHelper.styleToggleLeft, GUILayout.Width(65));
                GUILayout.EndHorizontal();
                //--------------- Name, active & static - --------------------

                //--------------- Layer & Tag --------------------
                GUILayout.BeginHorizontal();
                GUILayout.Space(36);
                GUILayout.Label("Tag: " + currentSelection.tag, EditorHelper.styleLabelLightGray10);
                GUILayout.FlexibleSpace();
                GUILayout.Label("Layer: " + LayerMask.LayerToName(currentSelection.layer), EditorHelper.styleLabelLightGray10);
                GUILayout.Space(16);
                GUILayout.EndHorizontal();
                //--------------- Layer & Tag --------------------

                GUILayout.EndVertical();
                GUILayout.EndArea();


                //--------------- Transform --------------------
                Rect transRect = new Rect(topRect.x, topRect.yMax, topRect.width, 85);
                if (eventType == EventType.repaint) {
                    VisualResources.DrawTexture(new Rect(topRect.x, topRect.yMax, topRect.width, 2), VisualResources.proConsoleLineDark);
                    VisualResources.DrawIcon(EditorIcon.hierarchyAndInspector, 2, new Vector2(transRect.x, transRect.y), 1f, false);
                    GUI.Label(new Rect(transRect.x + 32, transRect.y + 5, transRect.width - 32, 32), "Transform", EditorHelper.styleLabelLightGray10);

                    Matrix4x4 m = currentSelection.matrix;
                    GUI.Label(new Rect(transRect.x + 5, transRect.y + 25, transRect.width - 64, 20), "Position", EditorHelper.styleLabelLightGray10);
                    EditorGUI.Vector3Field(new Rect(transRect.x + 64, transRect.y + 25, transRect.width - 64, 20), "", m.GetPosition());
                    GUI.Label(new Rect(transRect.x + 5, transRect.y + 45, transRect.width - 64, 20), "Rotation", EditorHelper.styleLabelLightGray10);
                    EditorGUI.Vector3Field(new Rect(transRect.x + 64, transRect.y + 45, transRect.width - 64, 20), "", m.GetRotation().eulerAngles);
                    GUI.Label(new Rect(transRect.x + 5, transRect.y + 65, transRect.width - 64, 20), "Scale", EditorHelper.styleLabelLightGray10);
                    EditorGUI.Vector3Field(new Rect(transRect.x + 64, transRect.y + 65, transRect.width - 64, 20), "", m.GetScale());
                }
                //--------------- Transform --------------------


                //--------------- Components --------------------
                Rect compSpace = new Rect(transRect.x, transRect.yMax, transRect.width, this.position.height - transRect.yMax);
                ExposedComponent[] components = GetComponents(currentSelection, currentSlice);

                if (components != null) {// && currentSelection.components.Length > 0) {
                    GUILayout.BeginArea(compSpace);
                    GUILayout.BeginVertical();
                    inspectorPosition = GUILayout.BeginScrollView(inspectorPosition);

                    for (int i = 0; i < components.Length; i++) {
                        ExposedComponent comp = components[i];

                        if (!hierarchyFoldoutStates.ContainsKey(comp.instanceID))
                            hierarchyFoldoutStates.Add(comp.instanceID, true);

                        int depth = 1;
                        hierarchyFoldoutStates[comp.instanceID] = EditorGUILayout.Foldout(hierarchyFoldoutStates[comp.instanceID], "    " + comp.typeName, EditorHelper.styleHierarchyFoldout);
                        Rect folTopRect = GUILayoutUtility.GetLastRect();
                        EditorGUI.Toggle(new Rect(folTopRect.x + 12, folTopRect.y, 100, 20), comp.enabled, EditorHelper.styleToggle);

                        if (hierarchyFoldoutStates[comp.instanceID]) {
                            string id = comp.instanceID.ToString();
                            DrawPrimitivesCollumn(comp.primitiveMembers, depth);
                            DrawObjects(id, comp.objectMembers, depth);
                            DrawUnityObjectPointer(comp.unityObjectMembers, depth);
                            DrawCollections(id, comp.collectionMembers, depth);
                        }

                        if (eventType == EventType.repaint) {
                            Rect foldContentRect = GUILayoutUtility.GetLastRect();
                            VisualResources.DrawTexture(new Rect(foldContentRect.x, foldContentRect.yMax + 2, foldContentRect.width, 2), VisualResources.proConsoleLineDark);
                        }
                        GUILayout.Space(4);
                    }

                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    GUILayout.EndArea();
                } else {
                    VisualResources.DrawTexture(new Rect(topRect.x, topRect.yMax, topRect.width, 2), VisualResources.proConsoleLineDark);
                    EditorHelper.DrawNA(compSpace, "Please add the 'vBugGameObjectReflectable'\n component to this GameObject.");
                }

                //--------------- Components --------------------	
            }
        }


        #endregion
        //--------------------------------------- Draw Components & Fields --------------------------------------
        //--------------------------------------- Draw Components & Fields --------------------------------------













        //--------------------------------------- RECURSIVE COMPONENT DATA --------------------------------------
        //--------------------------------------- RECURSIVE COMPONENT DATA --------------------------------------
        #region RECURSIVE COMPONENT DATA

        private void DrawPrimitivesGrid(string[] input, int depth, int rowLength = 4) {
            if (input == null || input.Length == 0)
                return;

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Space(depth * depthSpacing);

            for (int i = 0; i < input.Length; i++) {
                string prim = input[i];
                if (string.IsNullOrEmpty(prim))
                    continue;

                int firstIDX = prim.IndexOf('|');
                if (firstIDX != -1) {
                    GUILayout.Label(prim.Substring(0, firstIDX), EditorHelper.styleLabelLightGray10, GUILayout.MinWidth(35));
                    GUILayout.Label(prim.Substring(firstIDX + 1, prim.Length - firstIDX - 1), EditorHelper.styleLabelLightGray10, GUILayout.Height(16), GUILayout.MinWidth(75));
                } else {
                    GUILayout.Label(prim, EditorHelper.styleLabelLightGray10, GUILayout.Height(16), GUILayout.MinWidth(35));
                }

                if (i > 0 && i % rowLength == 0) {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(depth * depthSpacing);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }


        private void DrawPrimitivesCollumn(string[] input, int depth, bool drawIdx = false) {
            if (input == null || input.Length == 0)
                return;

            GUILayout.BeginVertical();
            for (int i = 0; i < input.Length; i++) {
                string prim = input[i];
                if (string.IsNullOrEmpty(prim))
                    continue;

                GUILayout.BeginHorizontal();
                GUILayout.Space(depth * depthSpacing);
                if (drawIdx)
                    GUILayout.Label("Element " + i, EditorHelper.styleLabelLightGray10, GUILayout.Width(75));

                int firstIDX = prim.IndexOf('|');
                if (firstIDX != -1) {
                    GUILayout.Label(prim.Substring(0, firstIDX), EditorHelper.styleLabelLightGray10, GUILayout.Width(100));
                    GUILayout.Label(prim.Substring(firstIDX + 1, prim.Length - firstIDX - 1), EditorHelper.styleLabelLightGray10, GUILayout.Height(16));
                } else {
                    GUILayout.Label(prim, EditorHelper.styleLabelLightGray10, GUILayout.Height(16));
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }


        private void DrawCollections(string id, ExposedCollection[] collections, int depth) {
            if (collections == null || collections.Length == 0)
                return;

            id += "_coll_";
            for (int i = 0; i < collections.Length; i++) {
                ExposedCollection coll = collections[i];
                if (coll == null)
                    continue;

                id += i;
                if (!inspectorFoldoutStates.ContainsKey(id))
                    inspectorFoldoutStates.Add(id, false);

                GUILayout.BeginHorizontal();
                GUILayout.Space(depth * depthSpacing);
                GUILayout.BeginVertical();

                if (inspectorFoldoutStates[id] = EditorGUILayout.Foldout(inspectorFoldoutStates[id], coll.fieldName, EditorHelper.styleHierarchyFoldout)) {
                    if (coll.primitiveValues != null && coll.primitiveValues.Length > 0) {
                        DrawSizes(coll.primitiveValues.Length, 1);
                        DrawPrimitivesCollumn(coll.primitiveValues, 1, true);
                    } else if (coll.objectValues != null && coll.objectValues.Length > 0) {
                        DrawSizes(coll.objectValues.Length, 1);
                        DrawObjects(id, coll.objectValues, 1, "Element " + i);
                    } else if (coll.unityObjectValues != null && coll.unityObjectValues.Length > 0) {
                        DrawSizes(coll.unityObjectValues.Length, 1);
                        DrawUnityObjectPointer(coll.unityObjectValues, 1, true);
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }


        private void DrawSizes(int length, int depth) {
            GUILayout.BeginHorizontal();
            GUILayout.Space(depth * depthSpacing);
            GUILayout.Label("Size " + length, EditorHelper.styleLabelLightGray10, GUILayout.Width(125));
            GUILayout.EndHorizontal();
        }



        private void DrawObjects(string id, ExposedObject[] objects, int depth, string customName = null) {
            if (objects == null || objects.Length == 0)
                return;

            GUILayout.BeginHorizontal();
            GUILayout.Space(depth * depthSpacing);
            GUILayout.BeginVertical();

            id += "_obj_";
            for (int i = 0; i < objects.Length; i++) {
                ExposedObject obj = objects[i];
                if (obj == null)
                    continue;

                id += i;
                if (!inspectorFoldoutStates.ContainsKey(id))
                    inspectorFoldoutStates.Add(id, false);

                if (inspectorFoldoutStates[id] = EditorGUILayout.Foldout(inspectorFoldoutStates[id], customName != null ? customName : obj.fieldName, EditorHelper.styleHierarchyFoldout)) {
                    if (obj.primitiveMembers != null && obj.primitiveMembers.Length > 0) {
                        if (obj.isStruct) {
                            DrawPrimitivesGrid(obj.primitiveMembers, 1);
                        } else {
                            DrawPrimitivesCollumn(obj.primitiveMembers, 1);
                        }
                    }

                    if (obj.collectionMembers != null && obj.collectionMembers.Length > 0) {
                        DrawCollections(id, obj.collectionMembers, 1);
                    }

                    if (obj.objectMembers != null && obj.objectMembers.Length > 0) {
                        DrawObjects(id, obj.objectMembers, 1);
                    }

                    if (obj.unityObjectMembers != null && obj.unityObjectMembers.Length > 0) {
                        DrawUnityObjectPointer(obj.unityObjectMembers, 1);
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }


        private void DrawUnityObjectPointer(ExposedUnityObjectPointer[] objects, int depth, bool drawIdx = false) {
            if (objects == null || objects.Length == 0)
                return;

            for (int i = 0; i < objects.Length; i++) {
                ExposedUnityObjectPointer obj = objects[i];
                if (obj == null)
                    continue;

                GUILayout.BeginHorizontal();
                GUILayout.Space(depth * depthSpacing);
                if (drawIdx)
                    GUILayout.Label("Element " + i, EditorHelper.styleLabelLightGray10, GUILayout.Width(100));

                if (!string.IsNullOrEmpty(obj.fieldName))
                    GUILayout.Label(obj.fieldName, EditorHelper.styleLabelLightGray10, GUILayout.Width(100));

                GUILayout.Label(obj.objectName, EditorHelper.styleLabelLightGray10, GUILayout.Height(16));
                GUILayout.EndHorizontal();
            }
        }
        #endregion
        //--------------------------------------- RECURSIVE COMPONENT DATA --------------------------------------
        //--------------------------------------- RECURSIVE COMPONENT DATA --------------------------------------














        //--------------------------------------- FIND STUFF --------------------------------------
        //--------------------------------------- FIND STUFF --------------------------------------
        #region FIND STUFF


        private bool TestEOL(int instanceID, int currentFrameNumber) {
            long key = MathHelper.CombineInts(instanceID, currentFrameNumber);

            if (eolStates.ContainsKey(key)) {
                return eolStates[key];

            } else {
                bool foundValidSlice = false; //eol for as long as nothing is found
                int iMax = Mathf.Min(VerticalSliceDatabase.maxRange, currentFrameNumber + vBugEditorSettings.PlaybackHierarchyEOLBirthSearchRange);

                for (int i = currentFrameNumber + 1; i < iMax; i++) {
                    VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(i);
                    if (slice == null || slice.gameObjectsSnapshot == null || slice.gameObjectsSnapshot.gameObjects == null)
                        continue;

                    foundValidSlice = true;
                    if (FindObjectByID(slice.gameObjectsSnapshot.gameObjects, instanceID) != null) {
                        eolStates.Add(key, false);
                        return false;
                    }
                }

                eolStates.Add(key, foundValidSlice);
                return foundValidSlice;
            }
        }




        private bool TestBirth(int instanceID, int currentFrameNumber) {
            long key = MathHelper.CombineInts(instanceID, currentFrameNumber);

            if (birthStates.ContainsKey(key)) {
                return birthStates[key];

            } else {
                bool foundValidSlice = false; //eol for as long as nothing is found
                int iMin = Mathf.Max(VerticalSliceDatabase.minRange, currentFrameNumber - vBugEditorSettings.PlaybackHierarchyEOLBirthSearchRange);

                for (int i = currentFrameNumber - 1; i >= iMin; i--) {
                    VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(i);
                    if (slice == null || slice.gameObjectsSnapshot == null || slice.gameObjectsSnapshot.gameObjects == null)
                        continue;

                    foundValidSlice = true;
                    if (FindObjectByID(slice.gameObjectsSnapshot.gameObjects, instanceID) != null) {
                        birthStates.Add(key, false);
                        return true;
                    }
                }

                birthStates.Add(key, foundValidSlice);
                return foundValidSlice;
            }
        }



        private ExposedGameObject FindObjectByID(ExposedGameObject[] objects, int instanceID) {
            if (objects == null || instanceID == -1)
                return null;

            int i = objects.Length;
            while (--i > -1) {
                if (objects[i].transformID == instanceID)
                    return objects[i];
            }
            return null;
        }


        public ExposedGameObject[] GetRootObjects(VerticalActivitySlice slice) {
            if (slice == null || slice.gameObjectsSnapshot == null || slice.gameObjectsSnapshot.gameObjects == null)
                return null;

            if (rootGOs.ContainsKey(slice.header.frameNumber)) {
                return rootGOs[slice.header.frameNumber];
            } else {
                ExposedGameObject[] allGameObjects = slice.gameObjectsSnapshot.gameObjects;
                List<ExposedGameObject> result = new List<ExposedGameObject>();
                int i = allGameObjects.Length;

                while (--i > -1) {
                    if (allGameObjects[i].parentID == -1)
                        result.Add(allGameObjects[i]);
                }

                ExposedGameObject[] resultArray = result.ToArray();
                SortReflectedObjectsByName(resultArray);
                rootGOs.Add(slice.header.frameNumber, resultArray);
                return resultArray;
            }
        }


        public ExposedGameObject[] GetChildren(int instanceID, VerticalActivitySlice slice) {
            if (slice == null || slice.gameObjectsSnapshot == null || slice.gameObjectsSnapshot.gameObjects == null)
                return null;

            long key = MathHelper.CombineInts(instanceID, slice.header.frameNumber);
            if (childrenPerGO.ContainsKey(key)) {
                return childrenPerGO[key];
            } else {
                List<ExposedGameObject> result = new List<ExposedGameObject>();
                ExposedGameObject[] allGameObjects = slice.gameObjectsSnapshot.gameObjects;
                int i = allGameObjects.Length;
                while (--i > -1) {
                    if (allGameObjects[i].parentID == instanceID)
                        result.Add(allGameObjects[i]);
                }

                ExposedGameObject[] resultArray = result.ToArray();
                childrenPerGO.Add(key, resultArray);
                return resultArray;
            }
        }



        public ExposedComponent[] GetComponents(ExposedGameObject go, VerticalActivitySlice slice) {
            if (go == null || go.components == null)
                return null;

            if (slice == null || slice.gameObjectsSnapshot == null || slice.gameObjectsSnapshot.components == null)
                return null;

            long key = MathHelper.CombineInts(go.transformID, slice.header.frameNumber);
            if (componentsPerGO.ContainsKey(key)) {
                return componentsPerGO[key];
            } else {
                int i = go.components.Length;
                ExposedComponent[] allComponents = slice.gameObjectsSnapshot.components;
                ExposedComponent[] result = new ExposedComponent[i];
                while (--i > -1)
                    result[i] = allComponents[go.components[i]];

                componentsPerGO.Add(key, result);
                return result;
            }
        }
        #endregion
        //--------------------------------------- FIND STUFF --------------------------------------
        //--------------------------------------- FIND STUFF --------------------------------------



    }
}
