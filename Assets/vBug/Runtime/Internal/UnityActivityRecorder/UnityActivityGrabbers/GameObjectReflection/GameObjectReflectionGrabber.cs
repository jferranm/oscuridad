using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;



namespace Frankfort.VBug.Internal {


    public class GameObjectReflectionGrabber : BaseActivityGrabber<GameObjectsSnapshot> {
        private bool allowStaticScanning;
        private int maxCollectionMemberSize;
        private int maxMemberScanDepth;

        private Dictionary<int, bool> flaggedAsCapturable = new Dictionary<int, bool>();
        private Dictionary<int, bool> flaggedNativeUnityComponents = new Dictionary<int, bool>();

        public override void AbortAndPruneCache() {
            base.AbortAndPruneCache();
            ExposedObjectDescriptor.Dispose();
            flaggedAsCapturable.Clear();
            flaggedNativeUnityComponents.Clear();
        }


        protected override void GrabResultEndOfFrame() {
            base.GrabResultEndOfFrame();

            if (resultReadyCallback != null) {
                allowStaticScanning = vBug.settings.recording.gameObjectReflection.allowStaticGameObjectScanning;
                maxCollectionMemberSize = vBug.settings.recording.gameObjectReflection.collecionMemberMaxSize;
                maxMemberScanDepth = vBug.settings.recording.gameObjectReflection.objectMemberScanDepth;

                ExposedGameObject[] objects;
                ExposedComponent[] components;
                ScanHierarchyRecursively(out objects, out components);
                resultReadyCallback(currentFrame, new GameObjectsSnapshot(objects, components), 0);
            }
        }


        public void ScanHierarchyRecursively(out ExposedGameObject[] reflectedObjects, out ExposedComponent[] reflectedComponents) {
            //--------------- GET REFLECTABLE OBJECTS --------------------
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
            int i = gameObjects.Length;

            List<ExposedGameObject> gameObjectStorage = new List<ExposedGameObject>(i);
            List<ExposedComponent> componentStorage = new List<ExposedComponent>();

            while (--i > -1)
                ScanGameObjectRecursively(gameObjects[i], 0, ref gameObjectStorage, ref componentStorage);
            //--------------- GET REFLECTABLE OBJECTS --------------------

            reflectedObjects = gameObjectStorage.ToArray();
            reflectedComponents = componentStorage.ToArray();
        }





        //--------------------------------------- RECURSIVE REFLECTION SCANNING --------------------------------------
        //--------------------------------------- RECURSIVE REFLECTION SCANNING --------------------------------------
        #region RECURSIVE REFLECTION SCANNING


        private void ScanGameObjectRecursively(GameObject current, int currentObjectScanDepth, ref List<ExposedGameObject> gameObjectStorage, ref List<ExposedComponent> componentStorage) {
            if (current == null)
                return;

            if (!allowStaticScanning && current.isStatic)
                return;

            currentObjectScanDepth++;
            ExposedGameObject result = new ExposedGameObject();

            //--------------- Basics --------------------
            Transform currentTrans = current.transform;
            result.name = current.name;
            result.transformID = currentTrans.GetInstanceID();
            result.tag = current.tag;
            result.layer = current.layer;
            result.activeSelf = current.activeSelf;
            result.isStatic = current.isStatic;
            result.isHiddenOrDontSave = current.hideFlags != HideFlags.None;
            result.matrix = current.transform.worldToLocalMatrix;

            Transform parent = currentTrans.parent;
            result.parentID = parent != null ? parent.GetInstanceID() : -1;
            //--------------- Basics --------------------


            //--------------- Local reflection --------------------
            bool localReflect = false;
            bool allowNativeUnityComponents = false;
            if (flaggedAsCapturable.ContainsKey(result.transformID)) {
                localReflect = flaggedAsCapturable[result.transformID];
                allowNativeUnityComponents = flaggedNativeUnityComponents[result.transformID];
            } else {
                vBugGameObjectReflectable info = GameObjectUtility.GetComponentInParent<vBugGameObjectReflectable>(current);
                if (info != null) {
                    localReflect = !result.isStatic;
                    allowNativeUnityComponents = info.allowNativeUnityComponents;
                }
                flaggedAsCapturable.Add(result.transformID, localReflect);
                flaggedNativeUnityComponents.Add(result.transformID, allowNativeUnityComponents);
            }
            //--------------- Local reflection --------------------


            //--------------- Components --------------------
            if (localReflect) {
                Behaviour[] components = (Behaviour[])current.GetComponents<Behaviour>();
                List<int> componentIndexes = new List<int>();

                int i = components.Length;
                result.components = new int[i];

                while (--i > -1) {
                    Behaviour comp = components[i];
                    if (comp == null)
                        continue;

                    if (!vBugEnvironment.CheckIsVBugClass(comp.GetType())) {
                        componentStorage.Add(ScanComponentRecursively(comp, currentObjectScanDepth, allowNativeUnityComponents));
                        componentIndexes.Add(componentStorage.Count - 1);
                    }
                }

                result.components = componentIndexes.ToArray();
            }
            //--------------- Components --------------------

            gameObjectStorage.Add(result);
        }


        private ExposedUnityObjectPointer ScanUnityObjectPointer(UnityEngine.Object uObj, string fieldName = null) {
            if (uObj == null)
                return null;

            ExposedUnityObjectPointer result = new ExposedUnityObjectPointer();
            result.instanceID = uObj.GetInstanceID();
            result.fieldName = fieldName;
            result.objectName = uObj.name;
            result.typeName = uObj.GetType().Name;
            return result;
        }


        private ExposedComponent ScanComponentRecursively(Behaviour obj, int currentObjectScanDepth, bool allowNativeUnityComponents) {
            ExposedComponent result = new ExposedComponent();
            result.enabled = obj.enabled;
            result.instanceID = obj.GetInstanceID();
            result.name = obj.name;

            ExposedObject content = ScanObjectRecursively(obj, currentObjectScanDepth, allowNativeUnityComponents);
            result.typeName = content.typeName;
            result.primitiveMembers = content.primitiveMembers;
            result.objectMembers = content.objectMembers;
            result.collectionMembers = content.collectionMembers;
            result.unityObjectMembers = content.unityObjectMembers;
            return result;
        }


        private ExposedObject ScanObjectRecursively(object obj, int currentObjectScanDepth, bool allowNativeUnityComponents, string fieldName = null) {
            if (obj == null)
                return null;

            bool continueObjectScanning = currentObjectScanDepth < maxMemberScanDepth;
            currentObjectScanDepth++;
            ExposedObject result = new ExposedObject();
            Type objType = obj.GetType();

            ExposedObjectDescriptor descriptor = ExposedObjectDescriptor.GetObjectDescriptor(objType, allowNativeUnityComponents);

            int iMax = descriptor.members.Length;
            int uoCount = 0;
            int pCount = 0;
            int cCount = 0;
            int oCount = 0;

            result.fieldName = fieldName;
            result.typeName = objType.FullName;
            result.isStruct = descriptor.isStruct;
            result.primitiveMembers = new string[descriptor.primCount];
            result.collectionMembers = new ExposedCollection[descriptor.collCount];
            result.objectMembers = new ExposedObject[descriptor.objCount];
            result.unityObjectMembers = new ExposedUnityObjectPointer[descriptor.unityObjCount];

            for (int i = 0; i < iMax; i++) {
                ICustomAttributeProvider member = descriptor.members[i];
                TypeClassification memberType = descriptor.memberTypes[i];

                if (memberType == TypeClassification.unknown)
                    continue;

                object fieldValue = null;
                try {
                    if (member is FieldInfo) {
                        fieldValue = (member as FieldInfo).GetValue(obj);
                    } else if (member is PropertyInfo) {
                        try {
                            fieldValue = (member as PropertyInfo).GetValue(obj, null);
                        } catch {
                            descriptor.memberTypes[i] = TypeClassification.unknown;
                            continue;
                        }
                    } else {
                        continue;
                    }
                } catch (Exception e) {
                    if (vBug.settings.general.debugMode)
                        Debug.LogError(e.Message + e.StackTrace);

                    continue;
                }

                switch (descriptor.memberTypes[i]) {
                    case TypeClassification.primitive:
                        result.primitiveMembers[pCount++] = descriptor.memberNames[i] + "|" + (fieldValue != null ? fieldValue.ToString() : "null");
                        break;

                    case TypeClassification.unityObj:
                        if (continueObjectScanning)
                            result.unityObjectMembers[uoCount++] = ScanUnityObjectPointer(fieldValue as UnityEngine.Object, descriptor.memberNames[i]);
                        break;

                    case TypeClassification.collection:
                        if (continueObjectScanning)
                            result.collectionMembers[cCount++] = ScanCollectionRecursive(fieldValue as Array, currentObjectScanDepth, allowNativeUnityComponents, descriptor.memberNames[i]);
                        break;

                    case TypeClassification.obj:
                        if (continueObjectScanning)
                            result.objectMembers[oCount++] = ScanObjectRecursively(fieldValue, currentObjectScanDepth, allowNativeUnityComponents, descriptor.memberNames[i]);
                        break;
                }
            }
            return result;
        }



        private ExposedCollection ScanCollectionRecursive(ICollection collection, int currentObjectScanDepth, bool allowNativeUnityComponents, string fieldName = null) {
            if (collection == null)
                return null;

            currentObjectScanDepth++;
            ExposedCollection result = new ExposedCollection();
            result.fieldName = fieldName;

            if (collection.Count == 0)
                return result;

            int iMax = Mathf.Min(collection.Count, maxCollectionMemberSize);
            bool continueObjectScanning = currentObjectScanDepth < maxMemberScanDepth;
            currentObjectScanDepth++;

            Type listType = collection.GetType();
            Type elementType = listType.GetElementType();
            result.typeName = elementType.Name;

            TypeClassification objectType = ExposedObjectDescriptor.GetObjectTypeClassification(elementType);
            int i = 0;
            switch (objectType) {
                case TypeClassification.unityObj:
                    if (continueObjectScanning) {
                        result.unityObjectValues = new ExposedUnityObjectPointer[iMax];
                        foreach (object obj in collection)
                            result.unityObjectValues[i++] = ScanUnityObjectPointer(obj as UnityEngine.Object);
                    }
                    break;

                case TypeClassification.primitive:
                    result.primitiveValues = new string[iMax];
                    foreach (object obj in collection)
                        result.primitiveValues[i++] = obj.ToString();
                    break;

                case TypeClassification.obj:
                    if (continueObjectScanning) {
                        result.objectValues = new ExposedObject[iMax];
                        foreach (object obj in collection)
                            result.objectValues[i++] = ScanObjectRecursively(obj, currentObjectScanDepth, allowNativeUnityComponents);
                    }
                    break;
            }
            return result;
        }

        #endregion
        //--------------------------------------- RECURSIVE REFLECTION SCANNING --------------------------------------
        //--------------------------------------- RECURSIVE REFLECTION SCANNING --------------------------------------

    }
}