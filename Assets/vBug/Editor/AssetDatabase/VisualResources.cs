
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Frankfort.VBug.Editor
{

    public enum EditorIcon
    {
        none = 0,
        touch1 = 1,
        touch2 = 2,
        touch3 = 3,
        touch4 = 4,
        touch5 = 5,
        touch6 = 6,
        touch7 = 7,
        touch8 = 8,

        consoleLogs = 9,
        graphArrows = 10,
        playAndSettings = 11,
        playbackOptions = 12,
        mouseNormal = 13,
        mouseDown = 14,
        mouseButtons = 15,
        playbackToggles = 16,
        vBugLogoHollow = 17,
        vBugLogoSolid = 18,
        hierarchyAndInspector = 19,
        timelineIndicators = 20,
        coreNavPanel1 = 21,
        coreNavPanel2 = 22,
        coreNavPanel3 = 23,
        navbarToggleBGs = 24
    }


    public static class VisualResources
    {
        private static Material guiMaterial;
        private static Texture2D iconSpreadSheat;
        private static Texture2D bgWaterMark;

        private static Dictionary<Color, Texture2D> colorTextures = new Dictionary<Color, Texture2D>();
        private static Dictionary<ushort, Rect> sourceLocations = new Dictionary<ushort, Rect>();
        private static Dictionary<ushort, float> sourceSizes = new Dictionary<ushort, float>();
			
      

        //--------------------------------------- DRAW TEXTURES --------------------------------------
        //--------------------------------------- DRAW TEXTURES --------------------------------------
        #region DRAW TEXTURES

        public static void DrawTexture(Rect position, Texture2D image) {
            if (Event.current != null && Event.current.type == EventType.repaint)
                GUI.DrawTexture(position, image);
        }

        public static void DrawTextureWithTexCoords(Rect position, Texture2D image, Rect texCoords) {
            if (Event.current != null && Event.current.type == EventType.repaint)
                GUI.DrawTextureWithTexCoords(position, image, texCoords);
        }
        #endregion
        //--------------------------------------- DRAW TEXTURES --------------------------------------
        //--------------------------------------- DRAW TEXTURES --------------------------------------
			








        //--------------------------------------- PRUNE --------------------------------------
        //--------------------------------------- PRUNE --------------------------------------
        #region PRUNE
        public static void Prune()
        {
            if (iconSpreadSheat != null)
                Resources.UnloadAsset(iconSpreadSheat);
            if (bgWaterMark != null)
                Resources.UnloadAsset(bgWaterMark);

            if(colorTextures.Count > 0)
            {
                foreach (KeyValuePair<Color, Texture2D> color in colorTextures)
                    UnityEngine.Object.DestroyImmediate(color.Value);

                if (vBugEditorSettings.DebugMode)
                    Debug.Log("Cleared: " + colorTextures.Count);
            }

            if (guiMaterial != null)
                UnityEngine.Object.DestroyImmediate(guiMaterial);

            iconSpreadSheat = null;
            bgWaterMark = null;
            colorTextures.Clear(); 
            guiMaterial = null;
        }

        #endregion
        //--------------------------------------- PRUNE --------------------------------------
        //--------------------------------------- PRUNE --------------------------------------











        //--------------------------------------- Material --------------------------------------
        //--------------------------------------- Material --------------------------------------
        #region Material

        public static Material GetGUIMaterial()
        {
            if (guiMaterial == null)
            {
                guiMaterial = new Material(GetGUIShader());
                guiMaterial.hideFlags = HideFlags.HideAndDontSave;
                guiMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
                guiMaterial.name = "vBugMaterial";
            }

            return guiMaterial;
        }

        private static string GetGUIShader()
        {
            return "Shader \"vBug/Unlit\" {\n" +
                    "   Properties {\n" +
                    "       _Color (\"Main Color\", Color) = (1,1,1,1)\n" +
                    "       _MainTex (\"Base (RGB)\", 2D) = \"white\" {}\n" +
                    "       _Offset (\"Offset\", Float) = 0\n" +
                    "   }\n" +
                    "   Category {\n" +
                    "       Lighting Off\n" +
                    "       ZWrite Off\n" +
                    "       ZTest Always\n" +
                    "       Cull Off\n" +
                    "       Blend SrcAlpha OneMinusSrcAlpha\n" +
                    "       Offset [_Offset],[_Offset]\n" +
                    "       Tags {\"Queue\"=\"Transparent\" \"IgnoreProjector\"=\"True\" \"RenderType\"=\"Transparent\"}\n" +
                    "       SubShader {\n" +
                    "           Pass {\n" +
                    "               SetTexture [_MainTex] {\n" +
                    "                   constantColor [_Color]\n" +
                    "                   Combine texture * constant, texture * constant\n" +
                    "               }\n" +
                    "           }\n" +
                    "       }\n" +
                    "   }\n" +
                    "}\n";
        }


        public static Material GetParticleMaterial()
        {
            //return new Material(Shader.Find("Specular"))
            return GetGUIMaterial();
        }

        #endregion
        //--------------------------------------- Material --------------------------------------
        //--------------------------------------- Material --------------------------------------
			











        //--------------------------------------- COLOR TEXTURES --------------------------------------
        //--------------------------------------- COLOR TEXTURES --------------------------------------
        #region COLOR TEXTURES

        public static Texture2D white{
            get { return GetColorTexture(Color.white); }
        }
        public static Texture2D gray {
            get { return GetColorTexture(Color.gray); }
        }
        public static Texture2D black {
            get { return GetColorTexture(Color.black); }
        }


        public static Texture2D proBGGray
        {
            get { return GetColorTexture(new Color(0.1f, 0.1f, 0.1f)); }
        }
        public static Texture2D proConsoleRowLight
        {
            get { return GetColorTexture(new Color32(64, 64, 64, 255)); }
        }
        public static Texture2D proConsoleRowDark
        {
            get { return GetColorTexture(new Color32(50, 50, 50, 255)); }
        }
        public static Texture2D proConsoleLineDark
        {
            get { return GetColorTexture(new Color32(26, 26, 26, 255)); }
        }
        public static Texture2D proWindowForground
        {
            get { return GetColorTexture(new Color32(56, 56, 56, 255)); }
        }
        public static Texture2D proWindowBackground
        {
            get { return GetColorTexture(new Color32(41, 41, 41, 255)); }
        }
        public static Texture2D proSelection
        {
            get { return GetColorTexture(new Color32(62, 95, 150, 255)); }
        }
        public static Texture2D timelineOrange
        {
            get { return GetColorTexture(new Color(1f, 0.5f, 0f)); }
        }



        public static Texture2D keyboardNormal
        {
            get { return GetColorTexture(new Color(0.35f, 0.35f, 0.35f)); }
        }
        public static Texture2D keyboardDown
        {
            get { return GetColorTexture(new Color(0.1f, 0.1f, 0.2f)); }
        }
        public static Texture2D keyboardHold
        {
            get { return GetColorTexture(new Color(0.175f, 0.175f, 0.275f)); }
        }
        public static Texture2D keyboardUp
        {
            get { return GetColorTexture(new Color(0.45f, 0.45f, 0.55f)); }
        }


        public static Texture2D hierarchySelectionBlue {
            get { return GetColorTexture(new Color32(62,95,150, 255)); }
        }
        public static Texture2D dragBarGray {
            get { return GetColorTexture(new Color32(41,41,41, 255)); }
        }

        public static Texture2D coreNavBG {
            get { return GetColorTexture(new Color32(35, 35, 35, 255)); }
        }


        public static Texture2D GetColorTexture(Color color)
        {
            Texture2D result = null;
            if(colorTextures.ContainsKey(color))
                result = colorTextures[color];
            
            if (result != null)
                return result;

            result = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            result.name = "vBugTexture";
            result.hideFlags = HideFlags.HideAndDontSave;
            result.SetPixel(1, 1, color);
            result.Apply(false, true);

            if (colorTextures.ContainsKey(color)) {
                colorTextures[color] = result;
            } else {
                colorTextures.Add(color, result);
            }
            return result;
        }

        #endregion
        //--------------------------------------- COLOR TEXTURES --------------------------------------
        //--------------------------------------- COLOR TEXTURES --------------------------------------
			









        //--------------------------------------- SPRITE SHEET --------------------------------------
        //--------------------------------------- SPRITE SHEET --------------------------------------
        #region SPRITE SHEET

        public static Rect DrawIcon(EditorIcon icon, Vector2 pos, float scale = 1f, bool center = true)
        {
            return DrawIcon(icon, -1, pos, scale, center);
        }

        public static Rect DrawIcon(EditorIcon icon, int subIdx, Vector2 pos, float scale = 1f, bool center = true)
        {
            if (icon == EditorIcon.none)
                return new Rect();

            InitSpriteSheet();

            if (iconSpreadSheat == null)
                return new Rect();

            Rect sourceRect;
            float size = 1f;
            GetIconRect(icon, subIdx, out sourceRect, out size);

            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            size *= scale;
            if (center) {
                float half = (size / 2);
                pos.x -= half;
                pos.y -= half;
            }

            Rect destRect = new Rect(pos.x, pos.y, size, size);
            VisualResources.DrawTextureWithTexCoords(destRect, iconSpreadSheat, sourceRect);
            return destRect;
        }


        private static void GetIconRect(EditorIcon icon, int subIdx, out Rect sourceRect, out float size) {

            ushort safeSubIdx = subIdx < 0 ? (ushort)255 : (ushort)subIdx;
            ushort key = (ushort)((int)icon << 8 | safeSubIdx);
            size = 1f;

            if (sourceLocations.ContainsKey(key)) {
                sourceRect = sourceLocations[key];
                size = sourceSizes[key];
            } else {
                int iconIdx = (int)icon - 1;
                float collums = 4;
                float idY = Mathf.Floor(iconIdx / collums);
                float idX = iconIdx - (idY * collums);

                float rato = (float)iconSpreadSheat.width / (float)iconSpreadSheat.height;
                float localSizeX = 1f / collums;
                float localSizeY = localSizeX * rato;


                float sourceX = idX / collums;
                float sourceY = (1f - (idY / collums) * rato) - localSizeY;

                size = (float)iconSpreadSheat.width / (float)collums;
                if (subIdx != -1) {
                    size /= 2;
                    localSizeX /= 2;
                    localSizeY /= 2;
                    float subIdxX = Mathf.Floor(subIdx / 2);
                    float subIdxY = subIdx - (subIdxX * 2);
                    sourceX += subIdxX * localSizeX;
                    sourceY += subIdxY * localSizeY;
                }

                sourceRect = new Rect(sourceX, sourceY, localSizeX, localSizeY);
                sourceLocations.Add(key, sourceRect);
                sourceSizes.Add(key, size);
            }
        }


        private static void InitSpriteSheet()
        {
            if (iconSpreadSheat == null)
                iconSpreadSheat = (Texture2D)Resources.LoadAssetAtPath(IOEditorHelper.FindFileByName(Application.dataPath, "vBugSpriteSheet"), typeof(Texture2D));
        } 
        #endregion
        //--------------------------------------- SPRITE SHEET --------------------------------------
        //--------------------------------------- SPRITE SHEET --------------------------------------














        //--------------------------------------- TILES BG WATERMARK --------------------------------------
        //--------------------------------------- TILES BG WATERMARK --------------------------------------
        #region TILES BG WATERMARK

        public static void DrawWindowBGWaterMark(Rect windowRect, float intensity = 0.235f) {
            if (Application.isPlaying)
                return;

            InitBGWaterMark();
            if (bgWaterMark == null)
                return;

            Rect position = new Rect(0, 0, windowRect.width, windowRect.height);

            GUI.DrawTexture(position, proWindowForground);
            GUI.color = new Color(intensity, intensity, intensity);
            Rect texCoords = new Rect(
                windowRect.x / bgWaterMark.width,
                -(windowRect.y / bgWaterMark.height),
                windowRect.width / bgWaterMark.width,
                windowRect.height / bgWaterMark.height);

            DrawTextureWithTexCoords(position, bgWaterMark, texCoords);
            GUI.color = Color.white;
        }

        private static void InitBGWaterMark() {
            if (bgWaterMark == null)
                bgWaterMark = (Texture2D)Resources.LoadAssetAtPath(IOEditorHelper.FindFileByName(Application.dataPath, "vBugBGWatermark"), typeof(Texture2D));
        }
        #endregion
        //--------------------------------------- TILES BG WATERMARK --------------------------------------
        //--------------------------------------- TILES BG WATERMARK --------------------------------------
			











        //--------------------------------------- QUAD UVS --------------------------------------
        //--------------------------------------- QUAD UVS --------------------------------------
        #region QUAD UVS

        internal static void SetIconUV(EditorIcon icon, int subIdx, GameObject quad)
        {
            InitSpriteSheet();

            Rect sourceRect;
            float size = 1;
            GetIconRect(icon, subIdx, out sourceRect, out size);
            
            if (quad != null && quad.renderer != null && quad.renderer.sharedMaterial != null)
            {
                quad.renderer.sharedMaterial.SetTexture("_MainTex", iconSpreadSheat);
                quad.renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(sourceRect.x, sourceRect.y));
                quad.renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(sourceRect.width, sourceRect.height));
            }
        } 
        #endregion
        //--------------------------------------- QUAD UVS --------------------------------------
        //--------------------------------------- QUAD UVS --------------------------------------


    }
}