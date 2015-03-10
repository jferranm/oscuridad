using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;



namespace Frankfort.VBug.Internal {
    [Serializable]
    public class ExposedObjectDescriptor {
        
        //--------------- Fields --------------------
        public bool isStruct;
        public string representedType;
        public ICustomAttributeProvider[] members;
        public string[] memberNames;
        public TypeClassification[] memberTypes;

        public int primCount;
        public int collCount;
        public int objCount;
        public int unityObjCount;
        //--------------- Fields --------------------








        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------
        #region STATIC

        public static BindingFlags fieldFlags =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.GetField |
            BindingFlags.Instance |
            BindingFlags.FlattenHierarchy |
            BindingFlags.DeclaredOnly;

        public static BindingFlags propertyFlags =
            BindingFlags.Public |
            BindingFlags.Instance |
            BindingFlags.GetProperty |
            BindingFlags.DeclaredOnly;

        private static Dictionary<Type, TypeClassification> typeClassifications = new Dictionary<Type, TypeClassification>();
        private static Dictionary<Type, ExposedObjectDescriptor> typeDescriptors = new Dictionary<Type, ExposedObjectDescriptor>();


        public static void Dispose() {
            typeClassifications.Clear();
            typeDescriptors.Clear();
        }


        public static ExposedObjectDescriptor GetObjectDescriptor(Type objType, bool allowNativeUnityComponents) {
            if (!typeDescriptors.ContainsKey(objType))
                typeDescriptors.Add(objType, CreateObjectDescriptor(objType, allowNativeUnityComponents));

            return typeDescriptors[objType];
        }


        private static ExposedObjectDescriptor CreateObjectDescriptor(Type objType, bool allowNativeUnityComponents) {
            ExposedObjectDescriptor result = new ExposedObjectDescriptor();
            result.isStruct = objType.IsValueType && !objType.IsClass && !objType.IsPrimitive && !objType.IsEnum;

            if (allowNativeUnityComponents) {
                result.members = CreateTypeMemberEntry(
                    objType,
                    vBug.settings.recording.gameObjectReflection.allowCustomClassPropertyScanning || !objType.IsSubclassOf(typeof(UnityEngine.MonoBehaviour))
                );
            } else {
                result.members = CreateTypeMemberEntry(objType, vBug.settings.recording.gameObjectReflection.allowCustomClassPropertyScanning);
            }
            
            SetTypeMembertypeEntry(result);
            result.memberNames = CreateMemberNamesEntry(result.members);
            result.representedType = objType.FullName;
            return result;
        }


        private static ICustomAttributeProvider[] CreateTypeMemberEntry(Type objType, bool addProperties) {
            List<ICustomAttributeProvider> result = new List<ICustomAttributeProvider>(objType.GetFields(fieldFlags));

            //--------------- Filter fields by 'IsPublic & SerializeField' --------------------
            int i = result.Count;
            while (--i > -1) {
                FieldInfo field = (FieldInfo)result[i];
                if (!field.IsPublic) {
                    object[] attributes = field.GetCustomAttributes(typeof(SerializeField), false);
                    if (attributes.Length == 0)
                        result.RemoveAt(i);
                }
            }
            //--------------- Filter fields by 'IsPublic & SerializeField' --------------------

            //--------------- Add properties --------------------
            if (addProperties) {
                PropertyInfo[] properties = objType.GetProperties(propertyFlags);

                if (properties.Length > 0) {
                    for (int m = 0; m < properties.Length; m++) {
                        Type propType = properties[m].PropertyType;
                        if ((propType.IsPrimitive || propType.IsValueType) && !propType.IsClass)
                            result.Add(properties[m]);
                    }
                }
            }
            //--------------- Add properties --------------------

            //--------------- Filter out all Obsolete members --------------------
            i = result.Count;
            while (--i > -1) {
                MemberInfo member = (MemberInfo)result[i];
                object[] attributes = member.GetCustomAttributes(typeof(ObsoleteAttribute), false);
                if (attributes.Length > 0)
                    result.RemoveAt(i);
            }
            //--------------- Filter out all Obsolete members --------------------

            return result.ToArray();
        }


        private static void SetTypeMembertypeEntry(ExposedObjectDescriptor target) {
            int iMax = target.members.Length;
            target.memberTypes = new TypeClassification[iMax];
            int i = iMax;
            while (--i > -1) {
                Type memberType = null;
                ICustomAttributeProvider member = target.members[i];

                if (member is FieldInfo) {
                    memberType = (member as FieldInfo).FieldType;
                } else if (member is PropertyInfo) {
                    memberType = (member as PropertyInfo).PropertyType;
                } else {
                    if (vBug.settings.general.debugMode)
                        Debug.LogError("WTF? " + member.GetType().Name);
                    target.memberTypes[i] = TypeClassification.unknown;
                    continue;
                }
                GetObjectTypeClassification(memberType, target, i);
            }
        }


        public static TypeClassification GetObjectTypeClassification(Type objType) {
            if (typeClassifications.ContainsKey(objType))
                return typeClassifications[objType];

            TypeClassification result = TypeClassification.unknown;
            Type unityObjectType = typeof(UnityEngine.Object);
            if (objType == unityObjectType || objType.IsAssignableFrom(unityObjectType) || objType.IsSubclassOf(unityObjectType)) {
                result = TypeClassification.unityObj;
            } else if (objType.IsPrimitive || objType.IsEnum || objType == typeof(string)) {
                result = TypeClassification.primitive;
            } else if (objType.IsArray || objType is ICollection) {
                result = TypeClassification.collection;
            } else if (objType.IsClass || objType.IsValueType) {
                result = TypeClassification.obj;
            }

            typeClassifications.Add(objType, result);
            return result;
        }
        

        public static void GetObjectTypeClassification(Type objType, ExposedObjectDescriptor target, int idx) {
            Type unityObjectType = typeof(UnityEngine.Object);
            if (objType == unityObjectType || objType.IsAssignableFrom(unityObjectType) || objType.IsSubclassOf(unityObjectType)) {
                target.unityObjCount++;
                target.memberTypes[idx] = TypeClassification.unityObj;
            } else if (objType.IsPrimitive || objType.IsEnum || objType == typeof(string)) {
                target.primCount ++;
                target.memberTypes[idx] = TypeClassification.primitive;
            } else if (objType.IsArray || objType is ICollection) {
                target.collCount++;
                target.memberTypes[idx] = TypeClassification.collection;
            } else if (objType.IsClass || objType.IsValueType) {
                target.objCount++;
                target.memberTypes[idx] = TypeClassification.obj;
            }
         }


        private static string[] CreateMemberNamesEntry(ICustomAttributeProvider[] members) {
            string[] result = new string[members.Length];
            int i = members.Length;
            while (--i > -1)
                result[i] = (members[i] is MemberInfo ? (members[i] as MemberInfo).Name : "NotAMember");

            return result;
        }

        #endregion
        //--------------------------------------- STATIC --------------------------------------
        //--------------------------------------- STATIC --------------------------------------
			
    }
}
