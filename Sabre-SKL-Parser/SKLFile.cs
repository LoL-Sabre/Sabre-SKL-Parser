using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sabre_SKL_Parser
{
    class SKLFile
    {
        public string Magic;
        public UInt32 Version;
        public UInt32 ReorderedCount;
        public UInt32 zeroAfterIndices;
        public UInt32 zeroAfterIndices2;
        public BinaryReader br;
        public Header h;
        public RawHeader rh;
        public List<Bone> Bones = new List<Bone>();
        public List<RawBone> RawBones = new List<RawBone>();
        public List<RawBoneID> RawBoneIDs = new List<RawBoneID>();
        public List<string> BoneNames = new List<string>();
        public List<UInt16> Indices = new List<UInt16>();
        public List<UInt32> ReorderedIDs = new List<UInt32>();
        public SKLFile(string fileLocation)
        { 
            br = new BinaryReader(File.Open(fileLocation, FileMode.Open));
            Magic = Encoding.ASCII.GetString(br.ReadBytes(8));
            Version = br.ReadUInt32();
            if(Version == 1 || Version == 2)
            {
                h = new Header(br);
                for (int i = 0; i < h.BoneCount; i++)
                {
                    Bones.Add(new Bone(br));
                }
                if(Version == 2)
                {
                    ReorderedCount = br.ReadUInt32();
                    for(int i = 0; i < ReorderedCount; i++)
                    {
                        ReorderedIDs.Add(br.ReadUInt32());
                    }
                }
            }
            else if(Version == 0)
            {
                rh = new RawHeader(br);
                br.BaseStream.Seek(rh.offsetBoneData, SeekOrigin.Begin);
                for(int i = 0; i < rh.BoneCount; i++)
                {
                    RawBones.Add(new RawBone(br));
                }
                br.BaseStream.Seek(rh.offsetBoneIDMap, SeekOrigin.Begin);
                for(int i = 0;i < rh.BoneCount; i++)
                {
                    RawBoneIDs.Add(new RawBoneID(br));
                }
                for (int i = 0; i < rh.BoneIDCount; i++)
                {
                    Indices.Add(br.ReadUInt16());
                }
                br.BaseStream.Seek(rh.offsetToUInt32, SeekOrigin.Begin);
                zeroAfterIndices = br.ReadUInt32();
                br.BaseStream.Seek(rh.offsetToUInt32_, SeekOrigin.Begin);
                zeroAfterIndices2 = br.ReadUInt32();
                for(int i = 0; i < rh.BoneCount; i++)
                {
                    BoneNames.Add(GetBoneName(br));
                    RawBones[i].Name = BoneNames[i];
                }
            }
        }
        public class Header
        {   
            public UInt32 ID;
            public UInt32 BoneCount;
            public Header(BinaryReader br)
            {
                ID = br.ReadUInt32();
                BoneCount = br.ReadUInt32();
            }
        }
        public class Bone
        {
            public string Name;
            public Int32 ParentID;
            public float Scale;
            public float[,] Matrix = new float[3, 4];
            public Bone(BinaryReader br)
            {
                Name = Encoding.ASCII.GetString(br.ReadBytes(32));
                ParentID = br.ReadInt32();
                Scale = br.ReadSingle();
                for(int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Matrix[i, j] = br.ReadSingle();
                    }
                }
            }
        }
        public class RawHeader
        {
            public UInt16 zero;
            public UInt16 BoneCount;
            public UInt32 BoneIDCount;
            public UInt32 offsetBoneData;
            public UInt32 offsetBoneIDMap;
            public UInt32 offsetIndices;
            public UInt32 offsetToUInt32;
            public UInt32 offsetToUInt32_;      
            public UInt32 offsetToStrings;
            public RawHeader(BinaryReader br)
            {
                zero = br.ReadUInt16();
                BoneCount = br.ReadUInt16();
                BoneIDCount = br.ReadUInt32();
                offsetBoneData = br.ReadUInt32();
                offsetBoneIDMap = br.ReadUInt32();
                offsetIndices = br.ReadUInt32();
                offsetToUInt32 = br.ReadUInt32();
                offsetToUInt32_ = br.ReadUInt32();
                offsetToStrings = br.ReadUInt32();
            }
        }
        public class RawBone //100
        {
            public string Name;
            public UInt16 Zero;
            public Int16 ID;
            public Int16 ParentID;
            public UInt16 Unknown;
            public UInt32 Hash;
            public float TwoPointOne;
            public float[] Position = new float[3];
            public float[] Scaling = new float[3];
            public float[] Orientaion = new float[4];
            public float[] CT = new float[3];
            public float[] Extra = new float[8];
            public RawBone(BinaryReader br)
            {
                Zero = br.ReadUInt16();
                ID = br.ReadInt16();
                ParentID = br.ReadInt16();
                Unknown = br.ReadUInt16();
                Hash = br.ReadUInt32();
                TwoPointOne = br.ReadSingle();
                for(int i = 0; i < 3; i++)
                {
                    Position[i] = br.ReadSingle();
                }
                for (int i = 0; i < 3; i++)
                {
                    Scaling[i] = br.ReadSingle();
                }
                for (int i = 0; i < 4; i++)
                {
                    Orientaion[i] = br.ReadSingle();
                }
                for (int i = 0; i < 3; i++)
                {
                    CT[i] = br.ReadSingle();
                }
                for (int i = 0; i < 8; i++)
                {
                    Extra[i] = br.ReadSingle();
                }
            }
        }
        public class RawBoneID
        {
            public UInt32 ID;
            public UInt32 Hash;
            public RawBoneID(BinaryReader br)
            {
                ID = br.ReadUInt32();
                Hash = br.ReadUInt32();
            }
        }
        public static string GetBoneName(BinaryReader br)
        {
            string name = "";
            do
            {
                name += br.ReadChar().ToString();
                name += br.ReadChar().ToString();
                name += br.ReadChar().ToString();
                name += br.ReadChar().ToString();
            } while (name.IndexOf('\u0000') == -1);
            name = name.Replace("\0", string.Empty);
            return name;
        }
    }
}
