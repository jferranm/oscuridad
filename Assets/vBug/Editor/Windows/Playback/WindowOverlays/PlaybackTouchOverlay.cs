using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug.Editor {
    public class PlaybackTouchOverlay : BasePlaybackOverlay {
        private const float quadBaseScale = 0.1f;

        public PlaybackTouchOverlay() {
            this.doDrawMainContainer = false;
            this.doDrawSubContainer = false;
            this.doDrawQuads = true;
        }

        public override void DrawQuads(Transform parent, Vector3 localScale, VerticalActivitySlice slice) {
            //return null;
            if (slice == null || slice.humanInput == null || slice.humanInput.providersData == null) {
                DestroyAllQuads();
                return;
            }

            bool succes = false;
            foreach (HumanInputProviderData provider in slice.humanInput.providersData) {
                if (provider == null)
                    continue ;

                if (provider.providerType == "TouchInputProvider") {
                    if (provider.basicInput != null) {
                        DrawFingerQuads(provider, parent, slice.header.screenWidth, slice.header.screenHeight);
                        succes = true;
                    }
                    break;
                }
            }

            if (!succes)
                DestroyAllQuads();
            return;
        }

        private void DrawFingerQuads(HumanInputProviderData provider, Transform parent, float screenWidth, float screenHeight) {
            int iMax = provider.basicInput.Length;

            DestroyAllQuads();
            if (iMax > 0) {
                for (int i = 0; i < iMax; i++) {
                    int ID = int.Parse(provider.basicInput[i].id);
                    AddFingerQuad((EditorIcon)Mathf.Min(ID + 1, 8));
                }

                for (int i = 0; i < iMax; i++) {
                    BasicInputCommand command = provider.basicInput[i];
                    Color color = Color.white;
                    switch (command.state) {
                        case HumanInputState.down: color = Color.green; break;
                        case HumanInputState.downHold: color = Color.gray; break;
                        case HumanInputState.downMove: color = Color.cyan; break;
                        case HumanInputState.up: color = Color.magenta; break;
                    }

                    Vector2 pos = command.position;
                    float offset = quadBaseScale * 0.225f;
                    float maxValue = Mathf.Max(screenWidth, screenHeight);
                    pos.x = (pos.x - (screenWidth / 2)) / maxValue;
                    pos.y = ((screenHeight / 2) - pos.y) / maxValue;

                    SetQuadColor(allQuads[i], color);
                    Transform transform = allQuads[i].transform;
                    transform.parent = parent;
                    transform.localRotation = Quaternion.identity;
                    transform.localPosition = new Vector3(pos.x, pos.y - offset);
                }
            }
        }

        private GameObject AddFingerQuad(EditorIcon icon) {
            GameObject quad = CreateQuad();
            VisualResources.SetIconUV(icon, -1, quad);
            quad.transform.localScale = new Vector3(quadBaseScale, quadBaseScale, quadBaseScale);
            return quad;
        }

    }
}