using System;
using Frankfort.VBug.Internal;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Frankfort.VBug.Editor
{
    public static class SceneVisualsHelper
    {


        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------
        #region GUI

        public static void DrawBBRect(Rect bbRect) {
            Texture2D white = VisualResources.white;
            VisualResources.DrawTexture(new Rect(bbRect.x, bbRect.y, bbRect.width, 2), white);
            VisualResources.DrawTexture(new Rect(bbRect.x, bbRect.yMax - 2, bbRect.width, 2), white);
            VisualResources.DrawTexture(new Rect(bbRect.x, bbRect.y, 2, bbRect.height), white);
            VisualResources.DrawTexture(new Rect(bbRect.xMax - 2, bbRect.y, 2, bbRect.height), white);
        }


        public static void DrawBBArrow(Rect bbRect) {
            Vector2 arrowPos = new Vector2(bbRect.xMax, bbRect.y + (bbRect.height / 2f) - 8);
            GUI.color = Color.gray;
            VisualResources.DrawIcon(EditorIcon.playbackOptions, 0, arrowPos, 0.5f, false);
            GUI.color = Color.white;
        }


        #endregion
        //--------------------------------------- GUI --------------------------------------
        //--------------------------------------- GUI --------------------------------------










        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------
        #region MISC

        public static SceneVisualState DecrementState(SceneVisualState state) {
            state -= 1;
            if ((int)state < 0)
                state = SceneVisualState.full;
            
            return state;
        }


        public static Rect GetRectFromBounds(Bounds bounds) {
            Vector3 c = bounds.center;
            Vector3 e = bounds.extents;

            Vector3[] corners = new Vector3[]{
                HandleUtility.WorldToGUIPoint(c + new Vector3(e.x, e.y, e.z)),
                HandleUtility.WorldToGUIPoint(c + new Vector3(-e.x, e.y, e.z)),
                HandleUtility.WorldToGUIPoint(c + new Vector3(-e.x, -e.y, e.z)),
                HandleUtility.WorldToGUIPoint(c + new Vector3(e.x, -e.y, e.z)),
                
                HandleUtility.WorldToGUIPoint(c + new Vector3(e.x, e.y, -e.z)),
                HandleUtility.WorldToGUIPoint(c + new Vector3(-e.x, e.y, -e.z)),
                HandleUtility.WorldToGUIPoint(c + new Vector3(-e.x, -e.y, -e.z)),
                HandleUtility.WorldToGUIPoint(c + new Vector3(e.x, -e.y, -e.z))
            };

            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);

            int i = 8;
            while (--i > -1) {
                Vector2 vertex = corners[i];
                min.x = Mathf.Min(min.x, vertex.x); min.y = Mathf.Min(min.y, vertex.y);
                max.x = Mathf.Max(max.x, vertex.x); max.y = Mathf.Max(max.y, vertex.y);
            }
            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }




        public static int GetBoneByID(SBonesArray bones, int ID) {
            int i = bones.count;
            while (--i > -1) {
                if (bones.instanceIDs[i] == ID)
                    return i;
            }
            return -1;
        }


        #endregion
        //--------------------------------------- MISC --------------------------------------
        //--------------------------------------- MISC --------------------------------------









        //--------------------------------------- SEARCH MESH --------------------------------------
        //--------------------------------------- SEARCH MESH --------------------------------------
        #region SEARCH MESH

        public static SMesh GetMeshByFrameNumber(int frameNumber, int meshGoInstanceID) {
            return GetMeshFromSlice(VerticalSliceDatabase.GetSlice(frameNumber), meshGoInstanceID);
        }

        public static SMesh GetMeshFromSlice(VerticalActivitySlice slice, int meshGoInstanceID) {
            if (slice == null || slice.meshData == null)
                return null;

            foreach (SWorldObject wo in slice.meshData.worldObjects) {
                if (wo == null || wo.meshes == null)
                    continue;

                foreach (SMesh mesh in wo.meshes) {
                    if (mesh != null && mesh.header.goInstanceID == meshGoInstanceID)
                        return mesh;
                }
            }
            return null;
        }

        public static bool CheckMeshKeyFrame(SMesh mesh) {
            return
                mesh != null &&
                mesh.vertices != null &&
                mesh.vertices.count > 0 &&
                mesh.triangles != null &&
                mesh.triangles.Length > 0;
        }
        #endregion
        //--------------------------------------- SEARCH MESH --------------------------------------
        //--------------------------------------- SEARCH MESH --------------------------------------
			


    }
}
