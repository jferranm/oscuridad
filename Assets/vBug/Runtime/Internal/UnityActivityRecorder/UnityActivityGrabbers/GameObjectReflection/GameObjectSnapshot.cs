using UnityEngine;
using System.Collections;
using System;


namespace Frankfort.VBug.Internal
{
    [Serializable]
    public class ExposedUnityObjectPointer {
        public string fieldName;
        public int instanceID;
        public string objectName;
        public string typeName;

        //--------------- Serialize --------------------
        public static byte[] SerializeArray(ExposedUnityObjectPointer[] input) {
            if (input == null) 
                return null;
            
            int iMax = input.Length;
            ComposedByteStream stream = ComposedByteStream.FetchStream(iMax);
            for (int i = 0; i < iMax; i++) {
                if (input[i] == null) {
                    stream.AddEmptyStream();
                } else {
                    stream.AddStream(Serialize(input[i]));
                }
            }
            return stream.Compose();
        }
        
        public static byte[] Serialize(ExposedUnityObjectPointer input) {
            ComposedByteStream stream = new ComposedByteStream(3);
            stream.AddStream(input.fieldName);
            stream.AddStream(BitConverter.GetBytes(input.instanceID));
            stream.AddStream(input.objectName);
            stream.AddStream(input.typeName);
            return stream.Compose();
        }

        //--------------- Serialize --------------------



        //--------------- Deserialize --------------------
        public static ExposedUnityObjectPointer[] DeserializeArray(byte[] input) {
            if (input == null) return null;
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            ExposedUnityObjectPointer[] result = new ExposedUnityObjectPointer[iMax];
            for (int i = 0; i < iMax; i++)
                result[i] = Deserialize(stream.ReadNextStream<byte>());

            stream.Dispose();
            return result;
        }


        public static ExposedUnityObjectPointer Deserialize(byte[] input) {
            if (input == null || input.Length == 0)
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null || stream.streamCount < 3)
                return null;

            ExposedUnityObjectPointer result = new ExposedUnityObjectPointer();
            result.fieldName = stream.ReadNextStream();
            result.instanceID = BitConverter.ToInt32(stream.ReadNextStream<byte>(), 0);
            result.objectName = stream.ReadNextStream();
            result.typeName = stream.ReadNextStream();
            stream.Dispose();
            return result;
        } 
        //--------------- Deserialize --------------------
			
    }




    [Serializable]
    public class ExposedGameObject
    {
        public int transformID;
        public int parentID;
        public int layer;
        public bool activeSelf;
        public bool isStatic;
        public bool isHiddenOrDontSave;
        public string name;
        public string tag;
        public SMatrix4x4 matrix;
        public int[] components;

        public ExposedGameObject() //Default constructor.
        { }




        //--------------- SERIALIZE --------------------
        public static byte[] SerializeArray(ExposedGameObject[] input)
        {
            if (input == null) return null;
            ComposedByteStream stream = ComposedByteStream.FetchStream(input.Length);
            for (int i = 0; i < input.Length; i++)
                stream.AddStream(Serialize(input[i]));

            return stream.Compose();
        }

        public static byte[] Serialize(ExposedGameObject input)
        {
            if (input == null) return null;
            //--------------- First primitives --------------------
            ComposedPrimitives prims = new ComposedPrimitives();
            prims.AddValue(input.transformID);
            prims.AddValue(input.parentID);
            prims.AddValue(input.layer);
            prims.AddValue(input.activeSelf);
            prims.AddValue(input.isStatic);
            prims.AddValue(input.isHiddenOrDontSave);
            //--------------- First primitives --------------------


            //--------------- Final stream --------------------
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            stream.AddStream(prims.Compose());
            stream.AddStream(input.name);
            stream.AddStream(input.tag);
            stream.AddStream(input.matrix.GetRawData());
            stream.AddStream(input.components);
            //--------------- Final stream --------------------

            return stream.Compose();
        } 
        //--------------- SERIALIZE --------------------


        //--------------- DESERIALIZE --------------------
        public static ExposedGameObject[] DeserializeArray(byte[] input)
        {
            if (input == null) return null;
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            ExposedGameObject[] result = new ExposedGameObject[iMax];
            for (int i = 0; i < iMax; i++)
                result[i] = Deserialize(stream.ReadNextStream<byte>());

            stream.Dispose();
            return result;
        }

        public static ExposedGameObject Deserialize(byte[] input)
        {
            if (input == null) return null;
            ExposedGameObject result = new ExposedGameObject();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            //--------------- First primitives --------------------
            ComposedPrimitives prims = ComposedPrimitives.FromByteArray(stream.ReadNextStream<byte>());
            result.transformID = prims.ReadNextValue<int>();
            result.parentID = prims.ReadNextValue<int>();
            result.layer = prims.ReadNextValue<int>();
            result.activeSelf = prims.ReadNextValue<bool>();
            result.isStatic = prims.ReadNextValue<bool>();
            result.isHiddenOrDontSave = prims.ReadNextValue<bool>();
            //--------------- First primitives --------------------

            result.name = stream.ReadNextStream();
            result.tag = stream.ReadNextStream();
            result.matrix = new SMatrix4x4(stream.ReadNextStream<float>());
            result.components = stream.ReadNextStream<int>();

            stream.Dispose();
            return result;
        }
        //--------------- DESERIALIZE --------------------


        internal void Dispose() {
            name = null;
            tag = null;
            components = null;
        }
    }




    [Serializable]
    public class ExposedComponent : ExposedObject
    {
        public int instanceID;
        public bool enabled;
        public string name;
        
        public ExposedComponent() //Default constructor
        { }


        //--------------- SERIALIZE --------------------
        public static byte[] SerializeArray(ExposedComponent[] input)
        {
            if (input == null) return null;
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            for (int i = 0; i < input.Length; i++)
                stream.AddStream(Serialize(input[i]));

            return stream.Compose();
        }

        public static byte[] Serialize(ExposedComponent input)
        {
            if (input == null) return null;
            ComposedPrimitives prims = new ComposedPrimitives();
            prims.AddValue(input.instanceID);
            prims.AddValue(input.enabled); 

            ComposedByteStream stream = ComposedByteStream.FetchStream();
            
            stream.AddStream(prims.Compose());
            stream.AddStream(input.name);
            stream.AddStream(SerializeExposedObject(input));
            return stream.Compose();
        } 
        //--------------- SERIALIZE --------------------



        //--------------- DESERIALIZE --------------------
        public static new ExposedComponent[] DeserializeArray(byte[] input) { // Hides ExposedObject.DeserializeArray();
            if (input == null) 
                return null;
            
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            ExposedComponent[] result = new ExposedComponent[iMax];
            for (int i = 0; i < iMax; i++)
                result[i] = Deserialize(stream.ReadNextStream<byte>());

            stream.Dispose();
            return result;
        }


        public static new ExposedComponent Deserialize(byte[] input)
        {
            if (input == null) 
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            ExposedComponent result = new ExposedComponent();
            ComposedPrimitives prims = ComposedPrimitives.FromByteArray(stream.ReadNextStream<byte>());
            result.instanceID = prims.ReadNextValue<int>();
            result.enabled = prims.ReadNextValue<bool>();
            result.name = stream.ReadNextStream();
            
            ComposedByteStream subStream = ComposedByteStream.FromByteArray(stream.ReadNextStream<byte>());
            if (subStream == null)
                return null;

            DeserializeExposedObject(ref subStream, result);
            
            subStream.Dispose();
            stream.Dispose();
            return result;
        }
        //--------------- DESERIALIZE --------------------

        public override void Dispose() {
            base.Dispose();
            name = null;
        }
    }


    [Serializable]
    public class ExposedObject
    {
        public string fieldName;
        public string typeName;
        public bool isStruct;
        public string[] primitiveMembers;
        public ExposedObject[] objectMembers;
        public ExposedCollection[] collectionMembers;
        public ExposedUnityObjectPointer[] unityObjectMembers;

        public ExposedObject() //Default constructor
        { }



        //--------------- SERIALIZE --------------------
        public static byte[] SerializeArray(ExposedObject[] input)
        {
            if (input == null) return null;
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            for (int i = 0; i < input.Length; i++)
                stream.AddStream(Serialize(input[i]));

            return stream.Compose();
        }

        public static byte[] Serialize(ExposedObject input)
        {
            if (input == null) return null;
            return SerializeExposedObject(input);
        }

        protected static byte[] SerializeExposedObject(ExposedObject input)
        {
            if (input == null) 
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream();
            stream.AddStream(input.fieldName);
            stream.AddStream(input.typeName);
            stream.AddStream(new byte[] { (byte)(input.isStruct ? byte.MaxValue : byte.MinValue) });
            stream.AddStream(SerializeSharedHelper.SerializeStrings(input.primitiveMembers));
            stream.AddStream(ExposedObject.SerializeArray(input.objectMembers));
            stream.AddStream(ExposedCollection.SerializeArray(input.collectionMembers));
            stream.AddStream(ExposedUnityObjectPointer.SerializeArray(input.unityObjectMembers));
            return stream.Compose();
        } 
        //--------------- SERIALIZE --------------------




        //--------------- DESERIALIZE --------------------
        public static ExposedObject[] DeserializeArray(byte[] input)
        {
            if (input == null) 
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            ExposedObject[] result = new ExposedObject[iMax];
            for (int i = 0; i < iMax; i++)
                result[i] = Deserialize(stream.ReadNextStream<byte>());

            stream.Dispose();
            return result;
        } 
        public static ExposedObject Deserialize(byte[] input)
        {
            if (input == null) 
                return null;
            
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null || stream.streamCount == 0)
                return null;

            ExposedObject result = new ExposedObject();
            ExposedObject.DeserializeExposedObject(ref stream, result);
            stream.Dispose();
            return result;
        }
        protected static void DeserializeExposedObject(ref ComposedByteStream stream, ExposedObject storeTo)
        {
            storeTo.fieldName = stream.ReadNextStream();
            storeTo.typeName = stream.ReadNextStream();
            storeTo.isStruct = stream.ReadNextStream<byte>()[0] == byte.MaxValue;
            storeTo.primitiveMembers = SerializeSharedHelper.DeserializeStrings(stream.ReadNextStream<byte>());
            storeTo.objectMembers = ExposedObject.DeserializeArray(stream.ReadNextStream<byte>());
            storeTo.collectionMembers = ExposedCollection.Deserialize(stream.ReadNextStream<byte>());
            storeTo.unityObjectMembers = ExposedUnityObjectPointer.DeserializeArray(stream.ReadNextStream<byte>());
        } 
        //--------------- DESERIALIZE --------------------


        //Todo: make more recursive
        public virtual void Dispose() {
            fieldName = null;
            typeName = null;
            primitiveMembers = null;
            objectMembers = null;
            collectionMembers = null;
            unityObjectMembers = null;
        }
    }


    ///TODO: Add nested collections support!
    [Serializable]
    public class ExposedCollection
    {
        public string fieldName;
        public string typeName;
        public string[] primitiveValues;
        public ExposedObject[] objectValues;
        public ExposedUnityObjectPointer[] unityObjectValues;

        public ExposedCollection() //Default constructor
        { } 


        //--------------- SERIALIZE --------------------
        public static byte[] SerializeArray(ExposedCollection[] input)
        {
            if (input == null) return null;
            int iMax = input.Length;
            ComposedByteStream stream = ComposedByteStream.FetchStream(iMax);
            for (int i = 0; i < iMax; i++)
                stream.AddStream(Serialize(input[i]));

            return stream.Compose();
        }

        public static byte[] Serialize(ExposedCollection input)
        {
            if (input == null) return null;
            ComposedByteStream stream = ComposedByteStream.FetchStream();
            stream.AddStream(input.fieldName);
            stream.AddStream(input.typeName);
            stream.AddStream(SerializeSharedHelper.SerializeStrings(input.primitiveValues));
            stream.AddStream(ExposedObject.SerializeArray(input.objectValues));
            stream.AddStream(ExposedUnityObjectPointer.SerializeArray(input.unityObjectValues));
            return stream.Compose();
        } 
        //--------------- SERIALIZE --------------------



        //--------------- DESERIALIZE --------------------
        public static ExposedCollection[] Deserialize(byte[] input)
        {
            if (input == null) 
                return null;
            
            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            ExposedCollection[] result = new ExposedCollection[iMax];

            for (int i = 0; i < iMax; i++)
            {
                ExposedCollection coll = new ExposedCollection();
                byte[] subStreamBytes = stream.ReadNextStream<byte>();
                if (subStreamBytes == null || subStreamBytes.Length == 0)
                    continue;

                ComposedByteStream subStream = ComposedByteStream.FromByteArray(subStreamBytes);
                coll.fieldName = subStream.ReadNextStream();
                coll.typeName = subStream.ReadNextStream();
                coll.primitiveValues = SerializeSharedHelper.DeserializeStrings(subStream.ReadNextStream<byte>());
                coll.objectValues = ExposedObject.DeserializeArray(subStream.ReadNextStream<byte>());
                coll.unityObjectValues = ExposedUnityObjectPointer.DeserializeArray(subStream.ReadNextStream<byte>());
                subStream.Dispose();
                result[i] = coll;
            }

            stream.Dispose();
            return result;
        } 
        //--------------- DESERIALIZE --------------------
			

    }


    [Serializable]
    public class GameObjectsSnapshot
    {
        public ExposedGameObject[] gameObjects;
        public ExposedComponent[] components;

        public GameObjectsSnapshot() //Default Constructor
        { }

        public GameObjectsSnapshot(ExposedGameObject[] gameObjects, ExposedComponent[] components){
            this.gameObjects = gameObjects;
            this.components = components;
        }

        //--------------- SERIALZIE --------------------
        public static byte[] Serialize(GameObjectsSnapshot input)
        {
            if (input == null) 
                return null;

            ComposedByteStream stream = ComposedByteStream.FetchStream(2);
            stream.AddStream(ExposedGameObject.SerializeArray(input.gameObjects));
            stream.AddStream(ExposedComponent.SerializeArray(input.components));
            return CLZF.Compress(stream.Compose());
        } 
        //--------------- SERIALZIE --------------------


        //--------------- DESERIALIZE --------------------
        public static GameObjectsSnapshot Deserialize(byte[] input)
        {
            if (input == null || input.Length == 0) 
                return null;

            GameObjectsSnapshot result = new GameObjectsSnapshot();
            ComposedByteStream stream = ComposedByteStream.FromByteArray(CLZF.Decompress(input));

            result.gameObjects = ExposedGameObject.DeserializeArray(stream.ReadNextStream<byte>());
            result.components = ExposedComponent.DeserializeArray(stream.ReadNextStream<byte>());
            stream.Dispose();

            return result;
        } 
        //--------------- DESERIALIZE --------------------


        public void Dispose(){
            if (gameObjects != null) {
                foreach (ExposedGameObject go in gameObjects) {
                    if (go != null)
                        go.Dispose();
                }
            }
            if (components != null) {
                foreach (ExposedComponent comp in components) {
                    if (comp != null)
                        comp.Dispose();
                }
            }
            gameObjects = null;
            components = null;
        }
    }
}