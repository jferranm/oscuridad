using System;
using UnityEditor;
using UnityEngine;
using Frankfort.VBug.Internal;
using System.Collections.Generic;

namespace Frankfort.VBug.Editor
{
    public class PlaybackMouseOverlay : BasePlaybackOverlay
    {
        private const float quadBaseScale = 0.15f;
        
        private GameObject mouseNormal;
        private GameObject mouseDown;
        private GameObject mouseBtn0;
        private GameObject mouseBtn1;
        private GameObject mouseBtn2;
        private GameObject[] mouseBtns;


        public PlaybackMouseOverlay()
        {
            this.doDrawMainContainer = false;
            this.doDrawSubContainer = false;
            this.doDrawQuads = true;
        }



        private void InitQuads()
        {
            mouseNormal = AddMouseQuad(EditorIcon.mouseNormal, -1);
            mouseDown = AddMouseQuad(EditorIcon.mouseDown, -1);
            mouseBtn0 = AddMouseQuad(EditorIcon.mouseButtons, 1, 0.5f);
            mouseBtn1 = AddMouseQuad(EditorIcon.mouseButtons, 3, 0.5f);
            mouseBtn2 = AddMouseQuad(EditorIcon.mouseButtons, 0, 0.5f);
            mouseBtns = new GameObject[]
            {
                mouseBtn0,
                mouseBtn1,
                mouseBtn2
            };
        }

        protected override void DestroyAllQuads() {
            base.DestroyAllQuads();

            mouseBtn0 = null;
            mouseBtn1 = null;
            mouseBtn2 = null;
            mouseNormal = null;
            mouseDown = null;
            mouseBtns = null;
        }

        private GameObject AddMouseQuad(EditorIcon icon, int subIdx, float scale = 1f)
        {
            scale *= quadBaseScale;
            GameObject quad = CreateQuad();
            VisualResources.SetIconUV(icon, subIdx, quad);
            quad.transform.localScale = new Vector3(scale, scale, scale);
            return quad;
        }



        public override void DrawQuads(Transform parent, Vector3 localScale, VerticalActivitySlice slice)
        {
            if (slice == null || slice.humanInput == null) {
                DestroyAllQuads();
                return;
            }

            if (mouseNormal == null)
                InitQuads();

            foreach (HumanInputProviderData provider in slice.humanInput.providersData){
                if (provider == null)
                    continue;

                if (provider.providerType == "MouseInputProvider") {
                    if (provider.basicInput != null && provider.basicInput.Length > 0)
                        DrawMouseQuads(provider, parent, slice.header.screenWidth, slice.header.screenHeight);

                    break;
                }
            }
            return;
        }




        private void DrawMouseQuads(HumanInputProviderData provider, Transform parent, float screenWidth, float screenHeight)
        {
            if (provider.basicInput.Length > 0)
            {
                //--------------- Check if any has mouse down / up --------------------
                bool wasDown = false;
                for (int i = 0; i < provider.basicInput.Length; i++)
                {
                    HumanInputState state = provider.basicInput[i].state;
                    if (state == HumanInputState.down || state == HumanInputState.downHold || state == HumanInputState.downMove || state == HumanInputState.up)
                    {
                        wasDown = true;
                        break;
                    }
                }

                //VisualResources.DrawIcon(wasDown ? EditorIcon.mouseDown : EditorIcon.mouseNormal, pos, scale);
                mouseNormal.SetActive(!wasDown);
                mouseDown.SetActive(wasDown);
                //--------------- Check if any has mouse down / up --------------------

                Vector2 pos = provider.basicInput[0].position;
                float maxValue = Mathf.Max(screenWidth, screenHeight);
                pos.x = (pos.x - (screenWidth / 2)) / maxValue;
                pos.y = ((screenHeight / 2) - pos.y) / maxValue;


                for (int i = 0; i < provider.basicInput.Length; i++)
                {
                    BasicInputCommand command = provider.basicInput[i];
                    bool doDraw = false;
                    Color color = Color.white;

                    switch (command.state) {
                        case HumanInputState.down: doDraw = true; color = Color.white; break;
                        case HumanInputState.downMove: doDraw = true; color = new Color(0.666f, 0.666f, 0.666f); break;
                        case HumanInputState.downHold: doDraw = true; color = new Color(0.333f, 0.333f, 0.333f); break;
                        case HumanInputState.up: doDraw = true; color = Color.white; break;
                    }

                    if (mouseBtns.Length > i) {
                        mouseBtns[i].SetActive(doDraw);
                        if (doDraw)
                            SetQuadColor(mouseBtns[i], color);
                    }
                }

                for (int i = 0; i < allQuads.Count; i++)
                {
                    Transform transform = allQuads[i].transform;
                    transform.parent = parent;
                    transform.localRotation = Quaternion.identity;
                    transform.localPosition = new Vector3(pos.x, pos.y);//, i * -0.001f);
                }
            } else {
                DestroyAllQuads();
            }
        }
    }
}