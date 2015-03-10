using System;
using UnityEngine;
using System.Collections.Generic;
using Frankfort.VBug.Editor;

namespace Frankfort.VBug.Internal
{
    public class MaterialSerializeHelper
    {
        private static Dictionary<int, Texture> recordedIDTextureTable = new Dictionary<int, Texture>();

        public static void PruneCache() {
            if (recordedIDTextureTable != null)
                recordedIDTextureTable.Clear();
        }


        private static Texture GetTextureByInstanceIDOrName(int instanceID, string textureName) {
            //--------------- Get from cache --------------------
            if (recordedIDTextureTable.ContainsKey(instanceID)) {
                Texture tex = recordedIDTextureTable[instanceID];
                if (tex != null)
                    return tex;
            }
            //--------------- Get from cache --------------------

            //--------------- Third try: Most exepnsive to search by name, but it needs to be done at this point --------------------
            Texture[] loadedTextures = Resources.FindObjectsOfTypeAll<Texture>();
            foreach (Texture texture in loadedTextures) {
                if (texture != null && texture.name == textureName && !recordedIDTextureTable.ContainsKey(instanceID)) {
                    recordedIDTextureTable.Add(instanceID, texture);
                    return texture;
                }
            }
            //--------------- Third try: Most exepnsive to search by name, but it needs to be done at this point --------------------

            return null;
        }



        public static SMaterial FromMaterial(Material mat)
        {
            if (mat == null)
                return null;

            Shader shader = mat.shader;
            if (shader == null)
                return null;

            SMaterial result = new SMaterial();
            result.instanceID = mat.GetInstanceID();
            result.materialName = mat.name;
            result.shaderName = shader.name;

            ShaderProperties.Info info = ShaderPropertyHelper.GetShaderInfo(shader.name);

            if (info != null){
                ComposedByteStream rawData = new ComposedByteStream();

                int iMax = info.propertyNames.Length;
                for (int i = 0; i < iMax; i++)
                {
                    string propName = info.propertyNames[i];
                    switch (info.propertyTypes[i])
                    {
                        case ShaderProperties.PropType.Color:
                            Color color = mat.GetColor(propName);
                            rawData.AddStream(new float[] { color.r, color.g, color.b, color.a });
                            break;

                        case ShaderProperties.PropType.Range:
                        case ShaderProperties.PropType.Float:
                            rawData.AddStream(new float[] { mat.GetFloat(propName) });
                            break;

                        case ShaderProperties.PropType.TexEnv:
                            Texture texture = mat.GetTexture(propName);
                            Vector2 offset = mat.GetTextureOffset(propName);
                            Vector2 scale = mat.GetTextureScale(propName);

                            rawData.AddStream(new int[] { texture != null ? texture.GetInstanceID() : -1 });
                            rawData.AddStream(texture != null ? texture.name : "" );
                            rawData.AddStream(new float[] { offset.x, offset.y });
                            rawData.AddStream(new float[] { scale.x, scale.y });
                            break;

                        case ShaderProperties.PropType.Vector:
                            Vector4 vector = mat.GetVector(propName);
                            rawData.AddStream(new float[] { vector.x, vector.y, vector.z, vector.w });
                            break;
                    }
                }
                result.rawData = rawData.Compose();
                return result;
            } else {
                if (vBug.settings.general.debugMode)
                    Debug.LogError("No shader-info found @" + shader.name);
                return null;
            }

        }









        public static Material ToMaterial(SMaterial source, out bool noErrors) {
            noErrors = true;
            Material result = null;

            if (source == null || source.rawData == null || source.rawData.Length == 0) {
                //Debug.Log("ToMaterial fail source null");
                return null;
            }

            ComposedByteStream rawData = ComposedByteStream.FromByteArray(source.rawData);
            ShaderProperties.Info info = ShaderPropertyHelper.GetShaderInfo(source.shaderName);
            
            if(rawData == null){
                //Debug.LogError("GetShaderInfo rawData-stream == null @" + source.shaderName + ": " + source.rawData.Length);
                noErrors = false;
                return null;
            }else if(info == null || info.propertyNames == null){
                //Debug.LogError("GetShaderInfo info == null @" + source.shaderName);
                noErrors = false;
                return null;
            } else {

                //--------------- If not found, create new material --------------------
                if (result == null || result.shader.name != source.shaderName) {
                    Shader shader = Shader.Find(source.shaderName);
                    if (shader == null) {
                        //Debug.LogError("shader not found");
                        noErrors = false;
                        return null;
                    }

                    if (result == null) {
                        result = new Material(shader);
                    } else {
                        result.shader = shader;
                    }
                }
                result.name = "vBugMaterial_" + source.materialName;
                result.hideFlags = HideFlags.HideAndDontSave;
                //--------------- If not found, create new material --------------------

                int iMax = info.propertyNames.Length;
                for (int i = 0; i < iMax; i++) {
                    
                    try {
                        string propName = info.propertyNames[i];
                        ShaderProperties.TextDim textDim = ShaderProperties.TextDim.TexDim2D;
                        if (info.textDimensions != null && i < info.textDimensions.Length)
                            textDim = info.textDimensions[i];

                        switch (info.propertyTypes[i])
                        {
                            case ShaderProperties.PropType.Color:
                                float[] color = rawData.ReadNextStream<float>();
                                result.SetColor(propName, new Color(color[0], color[1], color[2], color[3]));
                                break;

                            case ShaderProperties.PropType.Range:
                            case ShaderProperties.PropType.Float:
                                result.SetFloat(propName, rawData.ReadNextStream<float>()[0]);
                                break;

                            case ShaderProperties.PropType.TexEnv:
                                int[] ids = rawData.ReadNextStream<int>();
                                int textureID = (ids != null && ids.Length > 0) ? ids[0] : -1;

                                string textureName = rawData.ReadNextStream();
                                float[] offset = rawData.ReadNextStream<float>();
                                float[] scale = rawData.ReadNextStream<float>();

                                //--------------- Search texture & set it to material --------------------
                                Texture target = GetTextureByInstanceIDOrName(textureID, textureName);

                                if (textDim == ShaderProperties.TextDim.TexDimCUBE && !(target is Cubemap))
                                    continue;

                                if (textDim == ShaderProperties.TextDim.TexDim3D && !(target is Texture3D))
                                    continue;

                                if (target != null)
                                    result.SetTexture(propName, target);

                                result.SetTextureOffset(propName, new Vector2(offset[0], offset[1]));
                                result.SetTextureScale(propName, new Vector2(scale[0], scale[1]));
                                //--------------- Search texture & set it to material --------------------
                            break;


                            case ShaderProperties.PropType.Vector:
                                float[] vector = rawData.ReadNextStream<float>();
                                result.SetVector(propName, new Vector4(vector[0], vector[1], vector[2], vector[3]));

                                break;
                        }
                    } catch (Exception e) {
                        if(vBugEditorSettings.DebugMode)
                            Debug.LogError(e.Message + e.StackTrace);
                    }
                }
            }

            return result;
        }


    }
}
