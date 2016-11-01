using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabre_SKL_Parser
{
    class BoneHash
    {
        public static uint GetHash(string s)
        {
            uint hash = 0;
            uint temp = 0;
            uint mask = 4026531840;
            s = s.ToLower();
            for(int i = 0; i < s.Length; i++)
            {
                hash = (hash << 4) + s[i];
                temp = hash & mask;
                if (temp != 0)
                {
                    hash = hash ^ (temp >> 24);
                    hash = hash ^ temp;
                }
            }
            return hash;
        }
    }
}
