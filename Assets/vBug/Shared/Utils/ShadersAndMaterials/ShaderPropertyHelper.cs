using System;
using UnityEngine;
using System.Collections.Generic;

namespace Frankfort.VBug.Internal
{

    [Serializable]
    public class ShaderProperties
    {
        [Serializable]
        public enum PropType
        {
            Color = 0,
            Vector = 1,
            Float = 2,
            Range = 3,
            TexEnv = 4
        }

        [Serializable]
        public enum TextDim {
            TexDimUnknown = -1,
            TexDimNone = 0,
            TexDimDeprecated1D = 1,
            TexDim2D = 2,
            TexDim3D = 3,
            TexDimCUBE = 4,
            TexDimRECT = 5,
            TexDimAny = 5,
        }

        [Serializable]
        public class Info
        {
            public string name;
            public string[] propertyNames;
            public PropType[] propertyTypes;
            public TextDim[] textDimensions;

            public Info() //Default constructor
            { }
            public Info(int count)
            {
                propertyNames = new string[count];
                propertyTypes = new PropType[count];
                textDimensions = new TextDim[count];
            }

            public override string ToString()
            {
                string result = this.GetType().FullName + " [";
                for(int i = 0; i < propertyNames.Length; i ++)
                    result += propertyNames[i] + ": " + propertyTypes[i] + " - ";
                
                return result;
            }
        }

        public Info[] shaders;
        public ShaderProperties() //Default constructor
        { }
        public ShaderProperties(int count)
        {
            shaders = new Info[count];
        }
    }



    public static class ShaderPropertyHelper
    {
        private static ShaderProperties properties;
        private static Dictionary<string, ShaderProperties.Info> propertiesTable = new Dictionary<string, ShaderProperties.Info>();

        private static bool Init()
        {
            if (properties == null){
                TextAsset txt = Resources.Load<TextAsset>("vBug/vBugShaderData");
                if (txt == null || string.IsNullOrEmpty(txt.text))
                    return false;

                properties = SerializeRuntimeHelper.DeserializeXML<ShaderProperties>(txt.text);
                propertiesTable.Clear();
            }
            return properties != null;
        }


        public static ShaderProperties.Info GetShaderInfo(string shaderName)
        {
            if (!Init())
                return null;

            if (propertiesTable.ContainsKey(shaderName))
                return propertiesTable[shaderName];
            
            foreach(ShaderProperties.Info info in properties.shaders){
                if (info.name == shaderName) {
                    propertiesTable.Add(shaderName, info);
                    return info;
                }
            }
            return null;
        }

    }
}