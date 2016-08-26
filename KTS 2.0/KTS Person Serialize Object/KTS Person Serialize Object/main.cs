using System;
using System.Collections.Generic;
using System.Text;
using KTS_Person_Serialize_Object.MessageObject;
using System.IO;

namespace KTS_Person_Serialize_Object
{
    public class KTSSerializer
    {

        /// <summary>
        /// Generates KTSMessage Object from Memorystream.
        /// </summary>
        /// <param name="memstream"></param>
        /// <returns></returns>
        public static KTSMessage DeSerializeKTSMessage(MemoryStream memorystream)
        {
            try
            {
                return ProtoBuf.Serializer.Deserialize<KTSMessage>(memorystream);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// For Serialization part increases efficeny.
        /// </summary>
        public static void initSerializer()
        {
            try
            {
                ProtoBuf.Serializer.PrepareSerializer<KTSMessage>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Writes  KTSMessage object to memorybuffer using protobuf-net.  
        /// </summary>
        /// <param name="message"></param>
        /// <param name="memorystream"></param>
        public static void SerializeKTSMessage(KTSMessage message, MemoryStream memorystream)
        {
            try
            {
                ProtoBuf.Serializer.Serialize(memorystream, message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

}
