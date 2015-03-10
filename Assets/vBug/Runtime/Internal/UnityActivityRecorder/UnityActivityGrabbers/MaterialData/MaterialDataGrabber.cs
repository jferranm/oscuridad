using System;
using UnityEngine;
using System.Collections.Generic;

namespace Frankfort.VBug.Internal
{
    public class MaterialDataGrabber : BaseActivityGrabber<MaterialDataSnapshot>{


        //--------------------------------------- STATIC REGISTER --------------------------------------
        //--------------------------------------- STATIC REGISTER --------------------------------------
        #region STATIC REGISTER

        private static List<Material> registeredMaterials = new List<Material>();
        private static List<int> registeredIDs = new List<int>();




        //--------------- Register Materials --------------------
        public static int[] RegisterMaterials(MeshRenderer renderer) {
            if (renderer == null)
                return null;

            Material[] materials = renderer.sharedMaterials != null ? renderer.sharedMaterials : renderer.materials;
            int[] result = GetInstanceIDsFromMaterials(materials);

            if (CheckRegisteredID(renderer))
                RegisterMaterials(materials);

            return result;
        }

        public static int[] RegisterMaterials(SkinnedMeshRenderer renderer) {
            if (renderer == null)
                return null;

            Material[] materials = renderer.sharedMaterials != null ? renderer.sharedMaterials : renderer.materials;
            int[] result = GetInstanceIDsFromMaterials(materials);
            if (CheckRegisteredID(renderer))
                RegisterMaterials(materials);

            return result;
        }

        public static void RegisterMaterials(Material[] materials) {
            if (materials != null && materials.Length > 0) {
                foreach (Material mat in materials)
                    RegisterMaterial(mat);
            }
        }
        //--------------- Register Materials --------------------
			


        //--------------- Register Material --------------------
        public static int RegisterMaterial(ParticleRenderer renderer) {
            if (renderer == null)
                return -1;

            Material material = renderer.sharedMaterial != null ? renderer.sharedMaterial : renderer.material;
            return RegisterMaterial(material);
        }


        public static int RegisterMaterial(ParticleSystem system) {
            if (system == null)
                return -1;

            Renderer particleRenderer = system.GetComponent<Renderer>();
            if (particleRenderer == null)
                return -1;

            Material material = particleRenderer.sharedMaterial != null ? particleRenderer.sharedMaterial : particleRenderer.material;
            return RegisterMaterial(material);
        }


        public static int RegisterMaterial(Material material) {
            if (material == null)
                return -1;

            if (CheckRegisteredID(material))
                registeredMaterials.Add(material);

            return material.GetInstanceID();
        }
        //--------------- Register Material --------------------
			




        //--------------- Manage --------------------
        private static bool CheckRegisteredID(UnityEngine.Object obj) {
            if (obj == null)
                return false;

            int id = obj.GetInstanceID();
            if (registeredIDs.Contains(id))
                return false;

            registeredIDs.Add(id);
            return true;
        }


        private static int[] GetInstanceIDsFromMaterials(Material[] materials) {
            if (materials == null)
                return null;

            int i = materials.Length;
            int[] result = new int[i];
            while (--i > -1)
                result[i] = materials[i] != null ? materials[i].GetInstanceID() : -1;

            return result;
        }
        //--------------- Manage --------------------
			

        #endregion
        //--------------------------------------- STATIC REGISTER --------------------------------------
        //--------------------------------------- STATIC REGISTER --------------------------------------











        //--------------------------------------- CAPTURE --------------------------------------
        //--------------------------------------- CAPTURE --------------------------------------
        #region CAPTURE

        public override void AbortAndPruneCache() {
            base.AbortAndPruneCache();
            registeredMaterials.Clear();
            registeredIDs.Clear();
        }


        protected override void GrabResultEndOfFrame() {
            base.GrabResultEndOfFrame();
            
            int iMax = registeredMaterials.Count;
            if (iMax > 0) {
                MaterialDataSnapshot result = new MaterialDataSnapshot();
                result.materials = new SMaterial[iMax];

                int m = 0;
                for (int i = 0; i < iMax; i++) {
                    Material mat = registeredMaterials[i];
                    if (mat != null) {
                        result.materials[i] = MaterialSerializeHelper.FromMaterial(mat);
                        m++;
                    }
                }

                if (m != iMax)
                    Array.Resize(ref result.materials, m);

                registeredMaterials.Clear();
                registeredIDs.Clear();

                if (resultReadyCallback != null)
                    resultReadyCallback(currentFrame, result, 0);

            } else {
                if (resultReadyCallback != null)
                    resultReadyCallback(currentFrame, null, 0);
            }
        }

        #endregion
        //--------------------------------------- CAPTURE --------------------------------------
        //--------------------------------------- CAPTURE --------------------------------------
			
    }
}
