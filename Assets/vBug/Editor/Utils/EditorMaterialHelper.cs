using System;
using UnityEngine;
using Frankfort.VBug.Internal;
using System.Collections.Generic;

namespace Frankfort.VBug.Editor
{
    public static class EditorMaterialHelper {

        private static Material fallbackMaterial = null;
        

        public static void SetFallbackMaterial(MeshRenderer renderer) {
            if (renderer == null)
                return;

            if (renderer.sharedMaterials == null ||
                renderer.sharedMaterials.Length == 0 ||
                renderer.sharedMaterial == null)
            {
                if (fallbackMaterial == null)
                    fallbackMaterial = new Material(Shader.Find("Diffuse"));

                renderer.sharedMaterial = fallbackMaterial;
            }
        }

        public static Material[] GetClosestAvailableMaterialsByID(int[] instanceIDs, int frameNumber) {
            List<Material> result = new List<Material>();
            
            bool toggle = false;
            for(int i = 0; i < vBugEditorSettings.PlaybackMaterialSearchRange; i ++){
                int offset = (int)Mathf.Ceil(i / 2);
                if (toggle)
                    offset = -offset;
                toggle = !toggle;
                int f = frameNumber + offset;

                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(f);
                if (slice == null || slice.materialData == null || slice.materialData.materials == null)
                    continue;

                bool succes = false;
                foreach (int id in instanceIDs) {
                    SMaterial sMat = GetMaterialByInstanceID(slice.materialData, id);
                    if (sMat != null){
                        bool noErrors;
                        Material mat = MaterialSerializeHelper.ToMaterial(sMat, out noErrors);
                        if (noErrors) {
                            succes = true;
                            result.Add(mat);
                        }
                    }
                }

                if (succes)
                    break;
            }

            return result.ToArray();
        }



        public static Material GetClosestAvailableMaterialByID(int instanceID, int frameNumber) {
            bool toggle = false;
            for (int i = 0; i < vBugEditorSettings.PlaybackMaterialSearchRange; i++) {
                int offset = (int)Mathf.Ceil(i / 2);
                if (toggle)
                    offset = -offset;
                toggle = !toggle;
                int f = frameNumber + offset;

                VerticalActivitySlice slice = VerticalSliceDatabase.GetSlice(f);
                if (slice == null || slice.materialData == null || slice.materialData.materials == null)
                    continue;

                SMaterial sMat = GetMaterialByInstanceID(slice.materialData, instanceID);
                if (sMat != null) {
                    bool succes;
                    return MaterialSerializeHelper.ToMaterial(sMat, out succes);
                }
            }

            return null;
        }

        public static SMaterial GetMaterialByInstanceID(MaterialDataSnapshot materialData, int instanceID) {
            if (materialData == null || materialData.materials == null || materialData.materials.Length == 0)
                return null;

            SMaterial[] materials = materialData.materials;
            int i =materials.Length;
            while(--i > -1){
                if (materials[i] != null && materials[i].instanceID == instanceID)
                    return materials[i];
            }

            return null;
        }


    }

}
