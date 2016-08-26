using System;
using System.Collections.Generic;
using System.Text;
using Serializer.Protobuf_net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;
using Serializer.Binaryformatter;
using Serializer.Binaryformatter.customFormatter;
using ProtoBuf;
using Serializer.Binaryformatter.protobuff;

namespace Serializer
{

    /// <summary>
    /// Way to run testing different implmentations of serilzation and de deserlization!
    /// 
    /// Test to be performed is with person tracking data serlization and de serilazation.
    /// 
    /// simple Binary formatter
    /// 
    /// custom Binary formatter
    /// 
    /// protobuf-net by marc.gravell 10 times faster than binary formatter
    /// 
    /// write my own with BinaryWriter
    /// 
    /// Expectations 
    /// from simple binary formatter to a custom formatter we should excpet a 10 to 15 times increase in spead 
    /// 
    /// and from binaryformatter  to proto buf  3-5 times better time characteristics than BinaryFormatter, and worse time characteristics than Manual approach.
    /// 
    /// source is this: http://www.codeproject.com/Articles/311944/BinaryFormatter-or-Manual-serializing
    /// http://maxondev.com/serialization-performance-comparison-c-net-formats-frameworks-xmldatacontractserializer-xmlserializer-binaryformatter-json-newtonsoft-servicestack-text/
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            Person per = new Person();

            //test basic binary formatter 
            basicBinaryFormatter();

            customBinaryFormatter();

            protobuffFormatterTest();

            //temp test


            Console.ReadLine();

        }

        private static void modtes(test tes)
        {
            tes.x++;
        }

        public class test
        {
            public int x = 0;
            
        }

        private static void protobuffFormatterTest()
        {

            pPerson per = new pPerson();
            pPerson per2;
            MemoryStream mem = new MemoryStream(10000);
            //    

            ProtoBuf.Serializer.PrepareSerializer<pPerson>();

            Stopwatch st = new Stopwatch();
            Stopwatch st1 = new Stopwatch();
            Stopwatch st2 = new Stopwatch();

            st.Start();

            st1.Start();
            for (int i = 0; i < 1000; i++)
            {
                mem.Position = 0;
                ProtoBuf.Serializer.Serialize(mem, per);

            }
            st1.Stop();


            st2.Start();
            for (int i = 0; i < 1000; i++)
            {
                //seralize 
                //deseralize
                mem.Position = 0;
                per2 =  ProtoBuf.Serializer.Deserialize<pPerson>(mem);

            }
            st2.Stop();
            st.Stop();

            decimal serialzeTime = calcElapsedTime(st1.ElapsedMilliseconds);
            decimal deserialzeTime = calcElapsedTime(st2.ElapsedMilliseconds);
            decimal totserialzeTime = calcElapsedTime(st.ElapsedMilliseconds);
            Console.WriteLine("protobuf serialiazer : {0} ms per iteration", serialzeTime);
            Console.WriteLine("protobuf deserialiazer : {0} ms per iteration", deserialzeTime);
            Console.WriteLine("protobuf rtt serlize time : {0} ms per iteration", totserialzeTime);

            mem.Close();


        }

        private static void customBinaryFormatter()
        {

            cPerson per = new cPerson();
            cPerson per2;

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream mem = new MemoryStream(10000);

            // seralize and deseralize thousand body objects
            Stopwatch st = new Stopwatch();
            Stopwatch st1 = new Stopwatch();
            Stopwatch st2 = new Stopwatch();

            st.Start();

            st1.Start();
            for (int i = 0; i < 1000; i++)
            {
                mem.Position = 0;
                formatter.Serialize(mem, per);
            }
            st1.Stop();


            st2.Start();
            for (int i = 0; i < 1000; i++)
            {
                //seralize 
                //deseralize
                mem.Position = 0;
                per2 = (cPerson)formatter.Deserialize(mem);

            }
            st2.Stop();
            st.Stop();

            decimal serialzeTime = calcElapsedTime(st1.ElapsedMilliseconds);
            decimal deserialzeTime = calcElapsedTime(st2.ElapsedMilliseconds);
            decimal totserialzeTime = calcElapsedTime(st.ElapsedMilliseconds);
            Console.WriteLine("Custom binary formatter serialiazer : {0} ms per iteration", serialzeTime);
            Console.WriteLine("Custom binary formatter deserialiazer : {0} ms per iteration", deserialzeTime);
            Console.WriteLine("Custom binary formatter rtt serlize time : {0} ms per iteration", totserialzeTime);

            mem.Close();
        }


        /// <summary>
        /// Basic binary formamtter takes 0.336 ms per iteration on i7 4660k 4.36 GHz
        /// my computer is on 3.9 ghz 
        /// A57 - 2.1GHz
        /// A53 - 1.5GHz
        /// 
        /// the vr headset is clocked around  1.2 - 1.5 GHz
        /// 
        /// 4.4 Ghz 
        /// 
        /// </summary>
        /// <returns>
        /// output:
        /// Basic binary formatter serialiazer : 0.177 ms per iteration
        /// Basic binary formatter deserialiazer : 0.177 ms per iteration
        /// Basic binary formatter rtt serlize time : 0.354 ms per iteration
        /// </returns>

        private static void basicBinaryFormatter()
        {

            bPerson pers = new bPerson();

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream mem = new MemoryStream(10000);
            bPerson pers2;

            // seralize and deseralize thousand body objects
            Stopwatch st = new Stopwatch();
            Stopwatch st1 = new Stopwatch();
            Stopwatch st2 = new Stopwatch();

            st.Start();

            st1.Start();
            for (int i = 0; i < 1000; i++)
            {
                mem.Position = 0;
                formatter.Serialize(mem, pers);
            }
            st1.Stop();


            st2.Start();
            for (int i = 0; i < 1000; i++)
            {
                //seralize 
                //deseralize
                mem.Position = 0;
                pers2 = (bPerson)formatter.Deserialize(mem);

            }
            st2.Stop();
            st.Stop();

            decimal serialzeTime = calcElapsedTime(st1.ElapsedMilliseconds);
            decimal deserialzeTime = calcElapsedTime(st2.ElapsedMilliseconds);
            decimal totserialzeTime = calcElapsedTime(st.ElapsedMilliseconds);
            Console.WriteLine("Basic binary formatter serialiazer : {0} ms per iteration", serialzeTime);
            Console.WriteLine("Basic binary formatter deserialiazer : {0} ms per iteration", deserialzeTime);
            Console.WriteLine("Basic binary formatter rtt serlize time : {0} ms per iteration", totserialzeTime);

            mem.Close();
        }

        private static decimal calcElapsedTime(long ElapsedMilliseconds)
        {
            return Convert.ToDecimal(ElapsedMilliseconds) / Convert.ToDecimal(1000);
        }

    }
}
