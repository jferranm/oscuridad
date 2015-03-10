using System;


namespace Frankfort.VBug.Internal {

    public static class SerializeSharedHelper {


        //--------------------------------------- SERIALIZE STRINGS --------------------------------------
        //--------------------------------------- SERIALIZE STRINGS --------------------------------------
        #region SERIALIZE STRINGS


        public static byte[] SerializeStrings(string[] input) {
            if (input == null)
                return null;

            int iMax = input.Length;
            ComposedByteStream stream = ComposedByteStream.FetchStream(iMax);
            for (int i = 0; i < iMax; i++)
                stream.AddStream(input[i]);

            return stream.Compose();
        }


        public static string[] DeserializeStrings(byte[] input) {
            if (input == null)
                return null;

            ComposedByteStream stream = ComposedByteStream.FromByteArray(input);
            if (stream == null)
                return null;

            int iMax = stream.streamCount;
            string[] result = new string[iMax];
            for (int i = 0; i < iMax; i++)
                result[i] = stream.ReadNextStream();

            stream.Dispose();
            return result;
        }



        #endregion
        //--------------------------------------- SERIALIZE STRINGS --------------------------------------
        //--------------------------------------- SERIALIZE STRINGS --------------------------------------

    }
}