using System;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using Frankfort.VBug.Internal;

namespace Frankfort.VBug.Editor
{
    public static class SerializeEditorHelper
    {

        //--------------------------------------- DIRECT HANDY HELPER CALLS --------------------------------------
        //--------------------------------------- DIRECT HANDY HELPER CALLS --------------------------------------
        #region DIRECT HANDY HELPER CALLS


        public static void SerializeAndSaveXML<T>(T data, string path, Type[] includedTypes = null)
        {
            if (data == null)
                return;
             
            IOEditorHelper.PrepairDestinationPath(path, false);
            IOEditorHelper.WriteString(SerializeXML(data, includedTypes), path);
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

        public static string SerializeXML(object dataObject, Type[] includedTypes = null)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            bool succes = false;
            try
            {
                XmlSerializer serializer = includedTypes != null ? new XmlSerializer(dataObject.GetType(), includedTypes) : new XmlSerializer(dataObject.GetType());
                serializer.Serialize(stringWriter, dataObject);
                succes = true;
            } catch (Exception e){
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            } finally {
                stringWriter.Close();
            }

            if (!succes)
                return null;
            
            return stringWriter.ToString();
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

            System.IO.StringReader stringReader = new System.IO.StringReader(dataString);
            T result = default(T);
            try{
                XmlSerializer serializer = includedTypes != null ? new XmlSerializer(typeof(T), includedTypes) : new XmlSerializer(typeof(T));
                result = (T)serializer.Deserialize(stringReader);
            }catch (Exception e){
                if (vBugEditorSettings.DebugMode)
                    Debug.LogError(e.Message + e.StackTrace);
            } finally{
                stringReader.Close();
            }
            return result;
        }

        #endregion
        //--------------------------------------- XML --------------------------------------
        //--------------------------------------- XML --------------------------------------

    }
}