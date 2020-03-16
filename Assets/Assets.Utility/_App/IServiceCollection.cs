using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Utility {
    public interface ICryptograph {
        string AesEncrypt(string plainText);
        string AesDecrypt(string cipherText);
        byte[] RijndaelEncrypt(string plainText);
        string RijndaelDecrypt(byte[] cipherText);
    }

    public interface ICompressionHandler {
        string Compress(string plainText);
        string Decompress(string compressedText);
        string Decompress<T>(string compressedText);
    }
}
