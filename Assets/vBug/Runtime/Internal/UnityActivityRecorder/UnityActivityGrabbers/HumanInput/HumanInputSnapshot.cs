using System;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    [Serializable]
    public enum HumanInputState
    {
        hold,
        move,
        down,
        downMove,
        downHold,
        up
    }


    [Serializable]
    public class BasicInputCommand
    {
        public string id;
        public HumanInputState state;
        public SVector2 position;

        public BasicInputCommand() //Default constructor
        { }
        public BasicInputCommand(string id, HumanInputState state)
        {
            this.id = id;
            this.state = state;
            this.position = SVector2.zero;
        }
        public BasicInputCommand(string id, HumanInputState state, SVector2 position)
        {
            this.id = id;
            this.state = state;
            this.position = position;
        }

        public override string ToString()
        {
            return "id: " + id + ", state: " + state.ToString() + ", pos: " + position.ToString();
        }

        public static void Serialize(BasicInputCommand input, ref ComposedByteStream storeTo)
        {
            float[] data = new float[3];
            data[0] = (int)input.state;
            data[1] = input.position.x;
            data[2] = input.position.y;
            
            storeTo.AddStream(input.id);
            storeTo.AddStream(data);
        }

        public static BasicInputCommand Deserialize(ref ComposedByteStream stream)
        {
            BasicInputCommand result = new BasicInputCommand();
            result.id = stream.ReadNextStream();
            float[] data = stream.ReadNextStream<float>();

            result.state = (HumanInputState)data[0];
            result.position.x = data[1];
            result.position.y = data[2];
            return result;
        }

        internal void Dispose() {
            id = null;
        }
    }






    [Serializable]
    public class AdvancedInputCommand
    {
        public string id;
        public HumanInputState state;
        public SVector3 position;
        public SQuaternion orientation;

        public AdvancedInputCommand() //Default constructor.
        { }
        public AdvancedInputCommand(string id, HumanInputState state, Vector3 position, Quaternion orientation)
        {
            this.id = id;
            this.state = state;
            this.position = position;
            this.orientation = orientation;
        }

        public override string ToString()
        {
            return "id: " + id + ", state: " + state.ToString() + ", pos: " + position.ToString() + ", orientation: " + this.orientation;
        }

        public static void Serialize(AdvancedInputCommand input, ref ComposedByteStream storeTo)
        {
            float[] data = new float[3];
            data[0] = (int)input.state;
            data[1] = input.position.x;
            data[2] = input.position.y;
            data[3] = input.position.z;

            data[4] = input.orientation.x;
            data[5] = input.orientation.y;
            data[6] = input.orientation.z;
            data[7] = input.orientation.w;

            storeTo.AddStream(input.id);
            storeTo.AddStream(data);
        }

        public static AdvancedInputCommand Deserialize(ref ComposedByteStream stream)
        {
            AdvancedInputCommand result = new AdvancedInputCommand();
            result.id = stream.ReadNextStream();
            float[] data = stream.ReadNextStream<float>();

            result.state = (HumanInputState)data[0];
            result.position.x = data[1];
            result.position.y = data[2];
            result.position.z = data[3];

            result.orientation.x = data[4];
            result.orientation.y = data[5];
            result.orientation.z = data[6];
            result.orientation.w = data[7];
            return result;
        }

        internal void Dispose() {
            id = null;
        }
    }



    [Serializable]
    public class HumanInputProviderData
    {
        public string providerType;
        public BasicInputCommand[] basicInput;
        public AdvancedInputCommand[] advancedInput;


        public static byte[] Serialize(HumanInputProviderData input){
            if (input == null)
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream();
            stream.AddStream(input.providerType);
            
            if (input.basicInput != null && input.basicInput.Length > 0){
                ComposedByteStream subStream = ComposedByteStream.FetchStream();
                for (int i = 0; i < input.basicInput.Length; i++)
                    BasicInputCommand.Serialize(input.basicInput[i], ref subStream);

                stream.AddStream(subStream.Compose());
            } else {
                stream.AddEmptyStream();
            }

            if (input.advancedInput != null && input.advancedInput.Length > 0) {
                ComposedByteStream subStream = ComposedByteStream.FetchStream();
                for (int i = 0; i < input.advancedInput.Length; i++)
                    AdvancedInputCommand.Serialize(input.advancedInput[i], ref subStream);

                stream.AddStream(subStream.Compose());
            } else {
                stream.AddEmptyStream();
            }

            return stream.Compose();
        }


        public static HumanInputProviderData Deserialize(byte[] input) {
            if (input == null || input.Length == 0)
                return null;

            HumanInputProviderData result = new HumanInputProviderData();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            result.providerType = stream.ReadNextStream();
            
            ComposedByteStream basic = ComposedByteStream.FromByteArray(stream.ReadNextStream<byte>());
            if (basic != null && basic.streamCount > 0) {
                int iMax = basic.streamCount / 2;
                result.basicInput = new BasicInputCommand[iMax];
                for (int i = 0; i < iMax; i++)
                    result.basicInput[i] = BasicInputCommand.Deserialize(ref basic);
                
                basic.Dispose();
            }

            ComposedByteStream advanced = ComposedByteStream.FromByteArray(stream.ReadNextStream<byte>());
            if (advanced != null && advanced.streamCount > 0) {
                int iMax = advanced.streamCount / 2;
                result.advancedInput = new AdvancedInputCommand[iMax];
                for (int i = 0; i < iMax; i++)
                    result.advancedInput[i] = AdvancedInputCommand.Deserialize(ref advanced);
                
                advanced.Dispose();
            }

            stream.Dispose();
            return result;
        }


        internal void Dispose() {
            if(basicInput != null){
                foreach (BasicInputCommand command in basicInput) {
                    if (command != null)
                        command.Dispose();
                }
            }
            if (advancedInput != null) {
                foreach (AdvancedInputCommand command in advancedInput) {
                    if(command != null)
                        command.Dispose();
                }
            }
            providerType = null;
            basicInput = null;
            advancedInput = null;
        }
    }



    [Serializable]
    public class HumanInputSnapshot
    {
        public HumanInputProviderData[] providersData;

        public HumanInputSnapshot() //Default constructor.
        { }
        public HumanInputSnapshot(HumanInputProviderData[] providersData)
        {
            this.providersData = providersData;
        }


        public static byte[] Serialize(HumanInputSnapshot input) {
            if (input == null)
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream();
            foreach(HumanInputProviderData data in input.providersData)
                stream.AddStream(HumanInputProviderData.Serialize(data));
            
            return stream.Compose();
        }

        public static HumanInputSnapshot Deserialize(byte[] input){
            if (input == null || input.Length == 0)
                return null;

            HumanInputSnapshot result = new HumanInputSnapshot();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            result.providersData = new HumanInputProviderData[iMax];
            for (int i = 0; i < iMax; i++)
                result.providersData[i] = HumanInputProviderData.Deserialize(stream.ReadNextStream<byte>());

            stream.Dispose();
            return result;
        }

        public void Dispose()
        {
            if (providersData != null) {
                foreach (HumanInputProviderData data in providersData){
                    if (data != null)
                        data.Dispose();
                }
            }
            providersData = null;
        }
    }
}