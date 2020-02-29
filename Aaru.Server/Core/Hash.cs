using System.Security.Cryptography;

namespace Aaru.Server
{
    public static class Hash
    {
        public static string Sha512(byte[] data)
        {
            byte[] hash;

            using(var sha = new SHA512Managed())
            {
                sha.Initialize();
                hash = sha.ComputeHash(data);
            }

            char[] chars = new char[hash.Length * 2];

            int j = 0;

            foreach(byte b in hash)
            {
                int nibble1 = (b & 0xF0) >> 4;
                int nibble2 = b & 0x0F;

                nibble1 += nibble1 > 9 ? 0x57 : 0x30;
                nibble2 += nibble2 > 9 ? 0x57 : 0x30;

                chars[j++] = (char)nibble1;
                chars[j++] = (char)nibble2;
            }

            return new string(chars);
        }
    }
}