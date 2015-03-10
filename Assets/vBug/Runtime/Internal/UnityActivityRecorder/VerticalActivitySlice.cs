using UnityEngine;
using System.Collections;
using System;

namespace Frankfort.VBug.Internal
{
    [Serializable]
    public class VerticalActivitySlice : IAbortableWorkObject
    {
        public int streamPriority = 0; //Not serialized and deserialized!
        public VerticalSliceHeaderData header;
        public SystemInfoSnapshot systemInfo;
        public DebugLogSnapshot debugLogs;
        public HumanInputSnapshot humanInput;
        public MeshDataSnapshot meshData;
        public ParticleDataSnapshot particleData;
        public MaterialDataSnapshot materialData;
        public GameObjectsSnapshot gameObjectsSnapshot;
        public ScreenCaptureSnapshot screenCapture;
        //public byte[] screenCapture;


        public VerticalActivitySlice() //Default constructor
        { }
        public VerticalActivitySlice(int frameNumber)
        {
            header = new VerticalSliceHeaderData(frameNumber);
        }
        
        public bool isAborted
        {
            get;
            set;
        }

        public static byte[] Serialize(VerticalActivitySlice input)
        { 
            ComposedByteStream stream = ComposedByteStream.FetchStream();


            //--------------- Header --------------------
            if (input.header != null)
                stream.AddStream(VerticalSliceHeaderData.Serialize(input.header));
            else
                stream.AddEmptyStream(); 
            //--------------- Header --------------------

            //--------------- SystemInfo --------------------
            if (input.systemInfo != null)
                stream.AddStream(SystemInfoSnapshot.Serialize(input.systemInfo));
            else
                stream.AddEmptyStream(); 
            //--------------- SystemInfo --------------------


            //--------------- DebugLogs --------------------
            if (input.debugLogs != null)
                stream.AddStream(DebugLogSnapshot.Serialize(input.debugLogs));
            else
                stream.AddEmptyStream(); 
            //--------------- DebugLogs --------------------


            //--------------- HumanInput --------------------
            if (input.humanInput != null)
                stream.AddStream(HumanInputSnapshot.Serialize(input.humanInput));
            else
                stream.AddEmptyStream(); 
            //--------------- HumanInput --------------------


            //--------------- MeshData --------------------
            if (input.meshData != null)
                stream.AddStream(MeshDataSnapshot.Serialize(input.meshData));
            else
                stream.AddEmptyStream(); 
            //--------------- MeshData --------------------


            //--------------- ParticleData --------------------
            if (input.particleData != null)
                stream.AddStream(ParticleDataSnapshot.Serialize(input.particleData));
            else
                stream.AddEmptyStream();
            //--------------- ParticleData --------------------

            //--------------- MaterialData --------------------
            if (input.materialData != null)
                stream.AddStream(MaterialDataSnapshot.Serialize(input.materialData));
            else
                stream.AddEmptyStream();
            //--------------- MaterialData --------------------

            //--------------- GameObjectReflection --------------------
            if (input.gameObjectsSnapshot != null)
                stream.AddStream(GameObjectsSnapshot.Serialize(input.gameObjectsSnapshot));
            else
                stream.AddEmptyStream();
            //--------------- GameObjectReflection --------------------


            //--------------- ScreenCapture --------------------
            if (input.screenCapture != null)
                stream.AddStream(ScreenCaptureSnapshot.Serialize(input.screenCapture));
            else
                stream.AddEmptyStream(); 
            //--------------- ScreenCapture --------------------
			

            return stream.Compose();
        }


        public static VerticalActivitySlice Deserialize(byte[] input)
        {
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            VerticalActivitySlice result = new VerticalActivitySlice();

            result.header = VerticalSliceHeaderData.Deserialize(stream.ReadNextStream<byte>());
            result.systemInfo = SystemInfoSnapshot.Deserialize(stream.ReadNextStream<byte>());
            result.debugLogs = DebugLogSnapshot.Deserialize(stream.ReadNextStream<byte>());
            result.humanInput = HumanInputSnapshot.Deserialize(stream.ReadNextStream<byte>());
            result.meshData = MeshDataSnapshot.Deserialize(stream.ReadNextStream<byte>());
            result.particleData = ParticleDataSnapshot.Deserialize(stream.ReadNextStream<byte>());
            result.materialData = MaterialDataSnapshot.Deserialize(stream.ReadNextStream<byte>());
            result.gameObjectsSnapshot = GameObjectsSnapshot.Deserialize(stream.ReadNextStream<byte>());
            result.screenCapture = ScreenCaptureSnapshot.Deserialize( stream.ReadNextStream<byte>());

            return result;
        }

        //Todo:Add material data and make all dispose calls recursive.
        public void Dispose()
        {
            if (header != null)                 header.Dispose();
            if (systemInfo != null)             systemInfo.Dispose();
            if (debugLogs != null)              debugLogs.Dispose();
            if (humanInput != null)             humanInput.Dispose();
            if (meshData != null)               meshData.Dispose();
            if (particleData != null)           particleData.Dispose();
            if (materialData != null)           materialData.Dispose();
            if (gameObjectsSnapshot != null)    gameObjectsSnapshot.Dispose();

            header = null;
            systemInfo = null;
            debugLogs = null;
            humanInput = null;
            meshData = null;
            particleData = null;
            materialData = null;
            gameObjectsSnapshot = null;
            screenCapture = null;
        }
    }






    [Serializable]
    public class VerticalSliceHeaderData
    {
        public string userUID;
        public string userNiceID;
        public string levelName;
        
        public int screenWidth;
        public int screenHeight;
        public int frameNumber;
        public int sessionNumber;
        public float birthTimeStamp;
        public double dateTimeStamp;
        
        public VerticalSliceHeaderData() //default constructor
        { }
        public VerticalSliceHeaderData(int frameNumber)
        {
            this.userUID = vBugEnvironment.GetUserUID();
            this.userNiceID = vBugEnvironment.GetNiceUserID();
            this.levelName = Application.loadedLevelName;

            this.screenWidth = Screen.width;
            this.screenHeight = Screen.height;
            this.frameNumber = frameNumber;
            this.sessionNumber = vBugEnvironment.GetCurrentSessionNumber();
            this.birthTimeStamp = Time.realtimeSinceStartup;
            this.dateTimeStamp = DateTimeHelper.ToSeconds();
            
        }


        public static byte[] Serialize(VerticalSliceHeaderData input)
        {
            ComposedPrimitives prims = new ComposedPrimitives();
            prims.AddValue(input.screenWidth);
            prims.AddValue(input.screenHeight);
            prims.AddValue(input.frameNumber);
            prims.AddValue(input.sessionNumber);
            prims.AddValue(input.birthTimeStamp);
            prims.AddValue(input.dateTimeStamp);
            byte[] data = prims.Compose();

            ComposedByteStream stream = ComposedByteStream.FetchStream();
            stream.AddStream(input.userUID);
            stream.AddStream(input.userNiceID);
            stream.AddStream(input.levelName);
            stream.AddStream(data);
            return stream.Compose();
        }



        public static VerticalSliceHeaderData Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0)
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            VerticalSliceHeaderData header = new VerticalSliceHeaderData();
            header.userUID = stream.ReadNextStream();
            header.userNiceID = stream.ReadNextStream();
            header.levelName = stream.ReadNextStream();

            ComposedPrimitives prims = ComposedPrimitives.FromByteArray(stream.ReadNextStream<byte>());
            if (prims != null)
            {
                header.screenWidth = prims.ReadNextValue<int>();
                header.screenHeight = prims.ReadNextValue<int>();
                header.frameNumber = prims.ReadNextValue<int>();
                header.sessionNumber = prims.ReadNextValue<int>();
                header.birthTimeStamp = prims.ReadNextValue<float>();
                header.dateTimeStamp = prims.ReadNextValue<double>();
            }
            stream.Dispose();
            return header;
        }

        public void Dispose(){
            userUID = null;
            userNiceID = null;
            levelName = null;
        }
    }

}