using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabre_SKL_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter file name");
            SKLFile skl = new SKLFile(Console.ReadLine());
            if(skl.Version == 1 || skl.Version == 2)
            {
                ParseVersion12(skl);
            }
            else if(skl.Version == 0)
            {
                ParseVersion0(skl);
            }
        }
        public static void ParseVersion12(SKLFile s)
        {
            Console.WriteLine("> Magic : " + s.Magic);
            Console.WriteLine("> Version : " + s.Version);
            Console.WriteLine("> ID : " + s.h.ID);
            Console.WriteLine("> Bone Count : " + s.h.BoneCount);
            Console.WriteLine();
            foreach(var b in s.Bones)
            {
                Console.WriteLine("> Name : " + b.Name);
                Console.WriteLine("> Parent ID : " + b.ParentID);
                Console.WriteLine("> Scale : " + b.Scale);
                Console.WriteLine("> Matrix : " + b.Matrix[0, 0] + ", " + b.Matrix[0, 1] + ", " + b.Matrix[0, 2] + ", " + b.Matrix[0, 3]);
                Console.WriteLine("           " + b.Matrix[1, 0] + ", " + b.Matrix[1, 1] + ", " + b.Matrix[1, 2] + ", " + b.Matrix[1, 3]);
                Console.WriteLine("           " + b.Matrix[2, 0] + ", " + b.Matrix[2, 1] + ", " + b.Matrix[2, 2] + ", " + b.Matrix[2, 3]);
                Console.WriteLine();
            }
            if(s.Version == 2)
            {
                Console.WriteLine("> Name : " + s.ReorderedCount);
                Console.WriteLine();
                foreach(UInt32 i in s.ReorderedIDs)
                {
                    Console.WriteLine("> Reordered ID : " + i);
                }
            }
            Console.ReadLine();
        }
        public static void ParseVersion0(SKLFile s)
        {
            Console.WriteLine("> Magic : " + s.Magic);
            Console.WriteLine("> Version : " + s.Version);
            Console.WriteLine("> Zero : " + s.rh.zero);
            Console.WriteLine("> Bone Count : " + s.rh.BoneCount);
            Console.WriteLine("> Bone ID COunt : " + s.rh.BoneIDCount);
            Console.WriteLine("> Offset Bone Data : " + s.rh.offsetBoneData);
            Console.WriteLine("> Offset Bone ID Map : " + s.rh.offsetBoneIDMap);
            Console.WriteLine("> Offset Indices : " + s.rh.offsetIndices);
            Console.WriteLine("> Offset UInt32 1 : " + s.rh.offsetToUInt32);
            Console.WriteLine("> Offset UInt32 2 : " + s.rh.offsetToUInt32_);
            Console.WriteLine("> Offset Strings : " + s.rh.offsetToStrings);
            Console.WriteLine();
            foreach(var b in s.RawBones)
            {
                Console.WriteLine("> Name : " + b.Name);
                Console.WriteLine("> Zero : " + b.Zero);
                Console.WriteLine("> ID : " + b.ID);
                Console.WriteLine("> Parent ID : " + b.ParentID);
                Console.WriteLine("> Unknown : " + b.Unknown);
                Console.WriteLine("> Hash : " + b.Hash);
                Console.WriteLine("> 2,1 : " + b.TwoPointOne);
                Console.WriteLine("> Position : " + b.Position[0] + ", " + b.Position[1] + ", " + b.Position[2]);
                Console.WriteLine("> Scaling : " + b.Scaling[0] + ", " + b.Scaling[1] + ", " + b.Scaling[2]);
                Console.WriteLine("> Orientation : " + b.Orientaion[0] + ", " + b.Orientaion[1] + ", " + b.Orientaion[2] + ", " + b.Orientaion[3]);
                Console.WriteLine("> CT : " + b.CT[0] + ", " + b.CT[1] + ", " + b.CT[2]);
                Console.WriteLine("> Extra : " + b.Extra[0] + ", " + b.Extra[1] + ", " + b.Extra[2] + ", " + b.Extra[3] + ", " + b.Extra[4] + ", " + b.Extra[5] + ", " + b.Extra[6] + ", " + b.Extra[7]);
                Console.WriteLine();
            }
            foreach(var i in s.RawBoneIDs)
            {
                Console.WriteLine("> Bone ID : " + i.ID + ", " + i .Hash);
            }
            Console.WriteLine();
            foreach(UInt16 i in s.Indices)
            {
                Console.WriteLine("> Indice : " + i);
            }
            Console.WriteLine("> UInt32 after Indices : " + s.zeroAfterIndices);
            Console.WriteLine("> Second UInt32 after Indices : " + s.zeroAfterIndices2);
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
