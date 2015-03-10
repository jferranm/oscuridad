using System;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;


namespace Frankfort.VBug.Internal
{
    public static class SerializeRuntimeHelper
    {
        private static BinaryFormatter binaryFormatter;



        //--------------------------------------- DIRECT HANDY HELPER CALLS --------------------------------------
        //--------------------------------------- DIRECT HANDY HELPER CALLS --------------------------------------
        #region DIRECT HANDY HELPER CALLS


        public static void SerializeAndSaveXML<T>(T data, string path, Type[] includedTypes = null)
        {
            if (data == null)
                return;
             
            IORuntimeHelper.PrepairDestinationPath(path, false);
            string xmlPlane = SerializeXML(data, includedTypes);
            IORuntimeHelper.WriteString(xmlPlane, path);
        }

        public static T LoadAndDeserializeXML<T>(string path, Type[] includedTypes = null) {
            return DeserializeXML<T>(IORuntimeHelper.LoadString(path), includedTypes);
        }


        #endregion
        //--------------------------------------- DIRECT HANDY HELPER CALLS --------------------------------------
        //--------------------------------------- DIRECT HANDY HELPER CALLS --------------------------------------
			










        //--------------------------------------- XML --------------------------------------
        //--------------------------------------- XML --------------------------------------
        #region XML



        public static byte[] SerializeXML(object dataObject, bool compressLZF, Type[] includedTypes = null)
        {
            string resultString = SerializeXML(dataObject, includedTypes);

            if (string.IsNullOrEmpty(resultString) || resultString.Length == 0)
                return null;

            byte[] result = Encoding.UTF8.GetBytes(resultString);
            if (compressLZF)
                result = CLZF.Compress(result);

            return result;
        }

        public static string SerializeXML(object dataObject, Type[] includedTypes = null) {

#if !UNITY_WEBPLAYER && !UNITY_FLASH
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            bool succes = false;
            try
            {
                XmlSerializer serializer = includedTypes != null ? new XmlSerializer(dataObject.GetType(), includedTypes) : new XmlSerializer(dataObject.GetType());
                serializer.Serialize(stringWriter, dataObject);
                succes = true;
            } catch (Exception e){
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            } finally {
                stringWriter.Close();
            }

            if (!succes)
                return null;

            return stringWriter.ToString();
#else
            Debug.LogWarning("IO-operations (read/write to disk) not allowed on this platform");
            return null;
#endif
        }



        public static T DeserializeXML<T>(byte[] dataBuffer, bool decompressLZF, Type[] includedTypes = null)
        {
            if (dataBuffer == null || dataBuffer.Length == 0)
                return default(T);

            if (decompressLZF)
                dataBuffer = CLZF.Decompress(dataBuffer);

            string utf8String = Encoding.UTF8.GetString(dataBuffer);
            return DeserializeXML<T>(utf8String, includedTypes);
        }


        public static T DeserializeXML<T>(string dataString, Type[] includedTypes = null)
        {
            if (string.IsNullOrEmpty(dataString) || dataString.Length == 0)
                return default(T);
            
#if !UNITY_WEBPLAYER && !UNITY_FLASH
            System.IO.StringReader stringReader = new System.IO.StringReader(dataString);
            T result = default(T);
            try{
                XmlSerializer serializer = includedTypes != null ? new XmlSerializer(typeof(T), includedTypes) : new XmlSerializer(typeof(T));
                result = (T)serializer.Deserialize(stringReader);
            }catch (Exception e){
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            } finally{
                stringReader.Close();
            }
            return result;
#else
            Debug.LogWarning("IO-operations (read/write to disk) not allowed on this platform");
            return default(T);
#endif
        }

        #endregion
        //--------------------------------------- XML --------------------------------------
        //--------------------------------------- XML --------------------------------------
			









        //--------------------------------------- BINARY --------------------------------------
        //--------------------------------------- BINARY --------------------------------------
        #region BINARY


        public static byte[] SerializeBinary(object data, bool compressLZF)
        {
            if (data == null)
                return null;
            
#if !UNITY_WEBPLAYER && !UNITY_FLASH
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            if (binaryFormatter == null)
                binaryFormatter = new BinaryFormatter();

            bool succes = false;
            try {
                binaryFormatter.Serialize(memoryStream, data);
                succes = true;
            } catch (SerializationException e) {
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            } finally {
                memoryStream.Close();
            }

            if (succes) {
                byte[] result = memoryStream.GetBuffer();
                if (compressLZF)
                    result = CLZF.Compress(result);

                return result;
            } else {
                return null;
            }
#else
            Debug.LogWarning("IO-operations (read/write to disk) not allowed on this platform");
            return null;
#endif
        }


        public static T DeserializeBinary<T>(byte[] byteArray, bool decompressLZF)
        {
            if (byteArray == null || byteArray.Length == 0)
                return default(T);

            if (byteArray != null && decompressLZF)
                byteArray = CLZF.Decompress(byteArray);
            
#if !UNITY_WEBPLAYER && !UNITY_FLASH
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(byteArray);
            if (binaryFormatter == null)
                binaryFormatter = new BinaryFormatter();

            T result = default(T);
            try {
                result = (T)binaryFormatter.Deserialize(memoryStream);
            } catch (SerializationException e) {
                if (vBug.settings.general.debugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            } finally {
                memoryStream.Close();
            }

            return result;
#else
            Debug.LogWarning("IO-operations (read/write to disk) not allowed on this platform");
            return default(T);
#endif
        }


        #endregion
        //--------------------------------------- BINARY --------------------------------------
        //--------------------------------------- BINARY --------------------------------------
			

    }
}