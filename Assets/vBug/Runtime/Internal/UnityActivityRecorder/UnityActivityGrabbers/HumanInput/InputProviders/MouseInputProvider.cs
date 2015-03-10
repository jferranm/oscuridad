using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public class MouseInputProvider : IHumanInputProvider
    {
        private HumanInputProviderData currentResult;
        private Vector2 oldMousePos = Vector3.zero;
        private float holdTimeStamp = -1f;
        private List<BasicInputCommand> caughtCommands = new List<BasicInputCommand>();

        //--------------- IHumanInputProvider --------------------
        public void Init()
        {
        }
        public void Reset()
        {
            caughtCommands.Clear();
            oldMousePos = Vector3.zero;
            holdTimeStamp = -1;
        }

        public void InitCurrentFrame() {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.BlackBerryPlayer ||
                Application.platform == RuntimePlatform.WP8Player)
                return;

            currentResult = new HumanInputProviderData();
            currentResult.providerType = this.GetType().Name;
            currentResult.basicInput = GetMouseCommands();
        }

        public HumanInputProviderData GetResultEOF()
        {
            return currentResult;
        }

        public void Dispose()
        {
            currentResult = new HumanInputProviderData();
        } 
        //--------------- IHumanInputProvider --------------------
			



        private BasicInputCommand[] GetMouseCommands()
        {
            int btnMax = 3;//3 == left, middle, right, 4 == navigate left? 5 == navigate right?
            caughtCommands.Clear();
            Vector2 screenPos = Input.mousePosition;
            screenPos.y = Screen.height - screenPos.y;

            //--------------- Detect movement --------------------
            bool moved = false;

            if (oldMousePos != Vector2.zero)
            {
                float distThreshold = vBug.settings.recording.humanInputRecording.mouse.moveDistanceThreshold;

                if (distThreshold <= 0f)
                {
                    moved = oldMousePos != screenPos;
                    oldMousePos = screenPos;
                }
                else
                {
                    float radiusDelta = distThreshold * Time.deltaTime;
                    float radiusSqr = radiusDelta * radiusDelta;

                    if ((oldMousePos - screenPos).sqrMagnitude >= radiusSqr)
                    {
                        holdTimeStamp = Time.realtimeSinceStartup;

                        moved = true;
                        oldMousePos = screenPos;
                    }
                    else
                    {
                        if (holdTimeStamp == -1 || (Time.realtimeSinceStartup - holdTimeStamp) > vBug.settings.recording.humanInputRecording.mouse.holdDurationThreshold)
                        {
                            holdTimeStamp = -1;
                        }
                        else
                        {
                            moved = true;
                        }
                    }
                }
            }
            else
            {
                oldMousePos = screenPos;
            }
            //--------------- Detect movement --------------------


            //--------------- Detect states --------------------
            for(int i = 0; i < btnMax; i ++)
            {
                string id = i.ToString();
                if (Input.GetMouseButtonDown(i))
                {
                    //Debug.Log("down: " + i);
                    caughtCommands.Add(new BasicInputCommand(id, HumanInputState.down, screenPos));
                }
                else if (Input.GetMouseButtonUp(i))
                {
                   // Debug.Log("up: " + i);
                    caughtCommands.Add(new BasicInputCommand(id, HumanInputState.up, screenPos));
                }
                else if (Input.GetMouseButton(i))
                {
                    // Debug.Log("hold: " + i);
                    caughtCommands.Add(new BasicInputCommand(id, moved ? HumanInputState.downMove : HumanInputState.downHold, screenPos));
                }
                else
                {
                    caughtCommands.Add(new BasicInputCommand(id, moved ? HumanInputState.move : HumanInputState.hold, screenPos));
                }
            } 
            //--------------- Detect states --------------------
			

            Vector2 deltaScroll = new Vector2(0f, Input.GetAxis("Mouse ScrollWheel") );
            if (deltaScroll.x != 0f || deltaScroll.y != 0f)
                caughtCommands.Add(new BasicInputCommand("Mouse ScrollWheel", HumanInputState.move, deltaScroll));

            return caughtCommands.ToArray();
        }
    }
}
