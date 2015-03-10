using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frankfort.VBug.Internal
{

    public class ScreenCaptureSnapshot
    {
        public string[] camNames;
        public byte[][] camRenders;
        public Rect[] camRects;
        public SVector3 mainCamLocalRotation;
        public SVector3 mainCamWorldRotation;
        public SVector3 inputAcceleration;



        public ScreenCaptureSnapshot() //Default constructor
        { }

        public ScreenCaptureSnapshot(string[] camNames, byte[][] camRenders, Rect[] camRects, Camera mainCam)
        {
            this.camNames = camNames;
            this.camRenders = camRenders;
            this.camRects = camRects;

            if(mainCam != null)
            {
                this.mainCamLocalRotation = mainCam.transform.localEulerAngles;
                this.mainCamWorldRotation = mainCam.transform.eulerAngles;
            }
            this.inputAcceleration = Input.acceleration;
        }


        public static byte[] Serialize(ScreenCaptureSnapshot input)
        {
            ComposedByteStream stream = ComposedByteStream.FetchStream();

            //--------------- Primitives --------------------
            ComposedPrimitives prims = new ComposedPrimitives();
            prims.AddValue(input.camNames != null ? input.camNames.Length : 0);
            prims.AddValue(input.camRenders != null ? input.camRenders.Length : 0);
            prims.AddValue(input.camRects != null ? input.camRects.Length : 0);

            prims.AddValue(input.mainCamLocalRotation.x);
            prims.AddValue(input.mainCamLocalRotation.y);
            prims.AddValue(input.mainCamLocalRotation.z);
            prims.AddValue(input.mainCamWorldRotation.x);
            prims.AddValue(input.mainCamWorldRotation.y);
            prims.AddValue(input.mainCamWorldRotation.z);
            prims.AddValue(input.inputAcceleration.x);
            prims.AddValue(input.inputAcceleration.y);
            prims.AddValue(input.inputAcceleration.z);

            stream.AddStream(prims.Compose());
            //--------------- Primitives --------------------
			
            if(input.camNames != null || input.camNames.Length != 0)
            {
                foreach(string name in input.camNames)
                    stream.AddStream(name);
            }

            if (input.camRenders != null && input.camRenders.Length != 0)
            {
                foreach (byte[] render in input.camRenders)
                    stream.AddStream(render);
            }

            if (input.camRects != null && input.camRects.Length != 0)
            {
                foreach (Rect rect in input.camRects)
                    stream.AddStream(new float[] { rect.x, rect.y, rect.width, rect.height });
            }
            return stream.Compose();
        }


        public static ScreenCaptureSnapshot Deserialize(byte[] input)
        {
            if (input == null)
                return null;

            ScreenCaptureSnapshot result = new ScreenCaptureSnapshot();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream != null)
            {
                ComposedPrimitives prims = ComposedPrimitives.FromByteArray(stream.ReadNextStream<byte>());
                int camNamesCount = prims.ReadNextValue<int>();
                int camRendersCount = prims.ReadNextValue<int>();
                int camRectCount = prims.ReadNextValue<int>();

                //Debug.Log("camNamesCount: " + camNamesCount + "\ncamRendersCount: " + camRendersCount + "\ncamRectCount: " + camRectCount);

                result.mainCamLocalRotation = new SVector3(prims.ReadNextValue<float>(), prims.ReadNextValue<float>(), prims.ReadNextValue<float>());
                result.mainCamWorldRotation = new SVector3(prims.ReadNextValue<float>(), prims.ReadNextValue<float>(), prims.ReadNextValue<float>());
                result.inputAcceleration = new SVector3(prims.ReadNextValue<float>(), prims.ReadNextValue<float>(), prims.ReadNextValue<float>());

                result.camNames = new string[camNamesCount];
                for (int i = 0; i < camNamesCount; i++)
                    result.camNames[i] = stream.ReadNextStream();

                result.camRenders = new byte[camRendersCount][];
                for (int i = 0; i < camRendersCount; i++)
                    result.camRenders[i] = stream.ReadNextStream<byte>();

                result.camRects = new Rect[camRectCount];
                for (int i = 0; i < camRectCount; i++)
                {
                    float[] rectFloats = stream.ReadNextStream<float>();
                    if (rectFloats != null && rectFloats.Length == 4)
                        result.camRects[i] = new Rect(rectFloats[0], rectFloats[1], rectFloats[2], rectFloats[3]);
                }
            }
            return result;
        }
    }
}

