using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BitmapStore
{
    class Program {

        private const int bitSize = 8* sizeof(uint);
        private static List<uint> ids = new List<uint>();
        private static void Init(uint caps= uint.MaxValue){
            for (var i = 0; i < 1 + caps / bitSize; i++)ids.Add(0);
        }

        private const uint constant1 = 1;

        private static void Register_(uint id){
            var idx = (int)(id / bitSize);
            var bit = (int)(id % bitSize);
            ids[idx] |= constant1 << bit;
        }
        private static void Unregist_(uint id){
            var idx = (int)(id / bitSize);
            var bit = (int)(id % bitSize);
            ids[idx] &= ~constant1 << bit;
        }

        private static bool Exist_(uint id){
            var idx = (int)(id / bitSize);
            var bit = (int)(id % bitSize);
            var bitIdx = constant1 << bit;
            return bitIdx == (ids[idx] & bitIdx);
        }

        static void Main(string[] args){
            /// Judge whether 4 billion 200 million account is registered. by QQ:20437023 liaisonme@hotmail.com
            /// MIT License Copyright (c) 2018 zhangxx2015
            /// 
            long bytesLength = GC.GetTotalMemory(true);
            Init();
            bytesLength = GC.GetTotalMemory(true)- bytesLength;
            Console.WriteLine("allow memory {0:N0} bytes", bytesLength);
            var bounds = uint.MaxValue;// 4,294,967,295
            var stage = 100000000;

            var ticks = Environment.TickCount;
            for (uint i = 0; i < bounds; i++){
                if (i>0 && 0 == i % stage) Console.WriteLine("{0:N0}\t{1:f2}%",i, (100d * i)/bounds);
                //Register(i);

                var idx = (int)(i / bitSize);
                var bit = (int)(i % bitSize);
                ids[idx] |= constant1 << bit;
            }
            ticks = Environment.TickCount - ticks;
            Console.WriteLine("fill succeed,elapse:{0:N0}ms",ticks);

            ticks = Environment.TickCount;
            for (uint i = 0; i < bounds; i++){
                if (i > 0 && 0 == i % stage) Console.WriteLine("{0:N0}\t{1:f2}%", i, (100d * i) / bounds);
                //Debug.Assert(Exist(i));

                var idx = (int)(i / bitSize);
                var bit = (int)(i % bitSize);
                var bitIdx = constant1 << bit;
                Debug.Assert(bitIdx == (ids[idx] & bitIdx));
            }
            ticks = Environment.TickCount - ticks;
            Console.WriteLine("assert succeed,elapse:{0:N0}ms", ticks);

            ticks = Environment.TickCount;
            for (uint i = 0; i < bounds; i++){
                if (i > 0 && 0 == i % stage) Console.WriteLine("{0:N0}\t{1:f2}%", i, (100d * i) / bounds);
                //Unregist(i);

                var idx = (int)(i / bitSize);
                var bit = (int)(i % bitSize);
                ids[idx] &= ~constant1 << bit;
            }
            ticks = Environment.TickCount - ticks;
            Console.WriteLine("clear succeed,elapse:{0:N0}ms", ticks);

            ticks = Environment.TickCount;
            for (uint i = 0; i < bounds; i++){
                if (i > 0 && 0 == i % stage) Console.WriteLine("{0:N0}\t{1:f2}%", i, (100d * i) / bounds);
                //Debug.Assert(!Exist(i));

                var idx = (int)(i / bitSize);
                var bit = (int)(i % bitSize);
                var bitIdx = constant1 << bit;
                Debug.Assert(bitIdx != (ids[idx] & bitIdx));
            }
            ticks = Environment.TickCount - ticks;
            Console.WriteLine("assert succeed,elapse:{0:N0}ms", ticks);

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
