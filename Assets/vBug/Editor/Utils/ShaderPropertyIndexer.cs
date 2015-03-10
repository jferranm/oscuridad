using System;
using UnityEngine;
using Frankfort.VBug.Internal;
using System.Collections.Generic;
using UnityEditor;



namespace Frankfort.VBug.Editor
{
    public class ShaderPropertyIndexer : AssetPostprocessor
    {
        private static double lastUpdateTimeStamp = -1;

        private static string[] buildinShaderNames = new string[] {
            "Bumped Diffuse",
            "Bumped Specular",
            "Decal",
            "Diffuse",
            "Diffuse Detail",
            "Parallax Diffuse",
            "Prallax Specular",
            "Specular",
            "Specular",
            "VertexLit",
            "FX/Flare",
            "GUI/Text Shader",
            
            "Mobile/Bumped Diffuse",
            "Mobile/Bumped Specular",
            "Mobile/Bumped Specular (1 Directional Light)",
            "Mobile/Diffuse",
            "Mobile/Particles/Addative",
            "Mobile/Particles/Alpha Blended",
            "Mobile/Particles/Multiply",
            "Mobile/Particles/VertexLit Blended",
            "Mobile/Skybox",
            "Mobile/Unlit (Supports Lightmap)",
            "Mobile/VertexLit",
            "Mobile/VertexLit (Only Directional Lights)",
            
            "Nature/Terrain/Bumped Specular",
            "Nature/Terrain/Diffuse",
            "Nature/Tree Creator Bark",
            "Nature/Tree Creator Leaves",
            "Nature/Tree Creator Leaves Fast",
            "Nature/Tree Soft Occlusion Bark",
            "Nature/Tree Soft Occlusion Leaves",
            
            "Particles/Addative",
            "Particles/Addative (Soft)",
            "Particles/Alpha Blended",
            "Particles/Alpha Blended Premultiply",
            "Particles/Multiply",
            "Particles/Multiply (Double)",
            "Particles/VertexLit Blended",
            "Particles/~Addative~Multiply",
            
            
            "Reflective/Bumped Diffuse",
            "Reflective/Bumped Specular",
            "Reflective/Bumped Unlit",
            "Reflective/Bumped VertexLit",
            "Reflective/Diffuse",
            "Reflective/Parallax Diffuse",
            "Reflective/Parallax Specular",
            "Reflective/Specular",
            "Reflective/VertexLit",   

            "RenderFX/Skybox",
            "RenderFX/Skybox Cubed",

            "Self-Illumin/",
            "Self-Illumin/Bumped Diffuse",
            "Self-Illumin/Bumped Specular",
            "Self-Illumin/Diffuse",
            "Self-Illumin/Parallax Diffuse",
            "Self-Illumin/Parallax Specular",
            "Self-Illumin/Specular",
            "Self-Illumin/VertexLit",

            "Sprites/Default",
            "Sprites/Diffuse",

            "Transparent/Cutout/Bumped Diffuse",
            "Transparent/Cutout/Bumped Specular",
            "Transparent/Cutout/Diffuse",
            "Transparent/Cutout/Soft Edge Unlit",
            "Transparent/Cutout/Specular",
            "Transparent/Cutout/VertexLit",
            "Transparent/Bumped Diffuse",
            "Transparent/Bumped Specular",
            "Transparent/Parallax Diffuse",
            "Transparent/Parallax Specular",
            "Transparent/Diffuse",
            "Transparent/Specular",
            "Transparent/VertexLit",
            
            "Unlit/Texture",
            "Unlit/Transparent",
            "Unlit/Transparent Cutout"
        };



        public static void OnPostprocessAllAssets ( String[] importedAssets, String[] deletedAssets, String[] movedAssets, String[] movedFromAssetPaths) {
            UpdateShaderPropertiesXML(false);
        }


        public static void UpdateShaderPropertiesXML(bool forceUpdate) {

            string path = IOEditorHelper.FindFileByName(Application.dataPath, "vBugShaderData.xml", true, true, false);
            if (string.IsNullOrEmpty(path))
                path = Application.dataPath + "/" + "Resources/vBug/vBugShaderData.xml";

            if (!forceUpdate && System.IO.File.Exists(path)) {
                double timestamp = vBugEnvironment.GetUnixTimestamp();
                if (timestamp - lastUpdateTimeStamp < 60f) //max every minute 
                    return;

                lastUpdateTimeStamp = timestamp;
            }

			//--------------- Update --------------------
            Shader[] shaders = GetAllShaders();
            
            if (shaders != null && shaders.Length > 0){
                int iMax = shaders.Length;
                ShaderProperties data = new ShaderProperties(iMax);
                for (int i = 0; i < iMax; i++){

                    Shader shader = shaders[i];

                    int propCount = ShaderUtil.GetPropertyCount(shader);
                    ShaderProperties.Info info = data.shaders[i] = new ShaderProperties.Info(propCount);
                    info.name = shader.name;

                    for (int p = 0; p < propCount; p++){
                        info.propertyNames[p] = ShaderUtil.GetPropertyName(shader, p);
                        info.propertyTypes[p] = (ShaderProperties.PropType)ShaderUtil.GetPropertyType(shader, p);
                        info.textDimensions[p] = (ShaderProperties.TextDim)ShaderUtil.GetTexDim(shader, p);
                    }
                }

                try {
                    SerializeEditorHelper.SerializeAndSaveXML<ShaderProperties>(data, path);
                    AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
                    AssetDatabase.Refresh();
                } catch (Exception e) {
                    if (vBugEditorSettings.DebugMode)
                        Debug.LogError("UpdateShaderPropertiesXML - FAIL: " + e.Message + e.StackTrace);
                }
            } else {
                if (vBugEditorSettings.DebugMode)
                    Debug.Log("Shaders null or empty");
            }
        }
        //--------------- Update --------------------





        //--------------- Find Hidden/Unloaded Unity shaders --------------------
        public static Shader[] GetAllShaders() {
            List<Shader> storeTo = new List<Shader>(Resources.FindObjectsOfTypeAll<Shader>());
            foreach (string name in buildinShaderNames) {
                if (storeTo.FindIndex(item => item.name == name) != -1)
                    continue;

                Shader shader = Shader.Find(name);
                if (shader != null)
                    storeTo.Add(shader);
            }
            
            Shader[] loaded = Resources.LoadAll<Shader>(Application.dataPath);
            foreach (Shader shader in loaded) {
                if (storeTo.FindIndex(item => item.name == shader.name) == -1)
                    storeTo.Add(shader);
            }

            Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
            foreach (Material material in materials) {
                if (material.shader != null && storeTo.FindIndex(item => item.name == material.shader.name) == -1)
                    storeTo.Add(material.shader);
            }

            Resources.UnloadUnusedAssets();
            return storeTo.ToArray();
        } 
        //--------------- Find Hidden/Unloaded Unity shaders --------------------
			
    }


}
