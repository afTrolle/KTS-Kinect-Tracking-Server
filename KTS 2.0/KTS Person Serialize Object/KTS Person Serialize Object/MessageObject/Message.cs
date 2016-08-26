using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace KTS_Person_Serialize_Object.MessageObject
{

    [ProtoContract]
    public class KTSMessage
    {
        [ProtoMember(1)]
       public int Insturction = 0;

        [ProtoMember(2)]
        public string Extra;

        [ProtoMember(3)]
        public Person[] People;  
    }
}
