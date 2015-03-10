using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{
    public class AxisInputProvider : IHumanInputProvider
    {
        private HumanInputProviderData currentResult;
        private List<BasicInputCommand> caughtCommands = new List<BasicInputCommand>();

        private Dictionary<string, float> axisMoveDetectTable = new Dictionary<string, float>();
        
        //--------------- IHumanInputProvider --------------------
        public void Init()
        {
        }
        public void Reset()
        {
            caughtCommands.Clear();
            currentResult = new HumanInputProviderData();
        }

        public void InitCurrentFrame() {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.BlackBerryPlayer ||
                Application.platform == RuntimePlatform.WP8Player)
                return;

            currentResult = new HumanInputProviderData();
            currentResult.providerType = this.GetType().Name;
            currentResult.basicInput = GetAxisInput();
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




        private BasicInputCommand[] GetAxisInput()
        {

            HumanInputSettings.AxisSettings settings = vBug.settings.recording.humanInputRecording.axis;
            if (settings == null || settings.definitions == null)
                return null;

            bool useRaw = settings.useGetAxisRaw;
            
            int i = settings.definitions.Length;
            while (--i > -1)
            {
                HumanInputSettings.AxisSettings.Definition def = settings.definitions[i];
                
                //--------------- Get values and ID's --------------------
                float resultX = 0;
                float resultY = 0;
                string id = null;
                bool hasMoved = false;
                
                if (!string.IsNullOrEmpty(def.x))
                    hasMoved = GetDefinitionValue(def.x, useRaw, settings.detectMotionThreshold, ref id, ref resultX);

                if (!string.IsNullOrEmpty(def.y))
                    hasMoved = GetDefinitionValue(def.y, useRaw, settings.detectMotionThreshold, ref id, ref resultY) || hasMoved;
                //--------------- Get values and ID's --------------------

                if (hasMoved && id != null)
                    caughtCommands.Add(new BasicInputCommand(id, HumanInputState.move, new SVector2(resultX, resultY)));
            }
            return caughtCommands.ToArray();
        }


        private bool GetDefinitionValue(string definition, bool useRaw, float deltaThreshold, ref string id, ref float result)
        {
            try {
                result = useRaw ? Input.GetAxisRaw(definition) * Time.deltaTime : Input.GetAxis(definition);
                id = id == null ? definition : id + "_" + definition;

                if (!axisMoveDetectTable.ContainsKey(definition)) {
                    axisMoveDetectTable.Add(definition, result);
                    return false;
                } else {
                    if (Mathf.Abs(axisMoveDetectTable[definition] - result) >= deltaThreshold) {
                        axisMoveDetectTable[definition] = result;
                        return true;
                    }
                }
                return false;
            } catch {
                //do nothing...
                return false;
            }
        }
    }
}
