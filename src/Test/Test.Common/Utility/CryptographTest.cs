using Assets.Utility;
using Assets.Utility.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Test.Common.Utility {
  [TestClass]
  public class CryptographTest {
    #region ctor
    private readonly Cryptograph _cryptograph;

    public CryptographTest() {
      _cryptograph = ServiceLocator.Current.GetInstance<Cryptograph>();
    }
    #endregion

    [TestMethod, TestCategory("Cryptograph"), TestCategory("AesRoundTrip")]
    public void AesRoundTrip() {

    }

    [TestMethod, TestCategory("Cryptograph"), TestCategory("RijndaelRoundTrip")]
    public void RijndaelRoundTrip() {

    }

    [TestMethod, TestCategory("Cryptograph"), TestCategory("StringToBytesAndBack")]
    public void StringToBytesAndBack() {
      var plainText = "plainText";
      var bytes = Encoding.UTF8.GetBytes(plainText);
      var strs = Encoding.UTF8.GetString(bytes);
      Assert.AreEqual(plainText, strs);
    }

    [TestMethod, TestCategory("Cryptograph"), TestCategory("SymmetricAlgorithm")]
    public void SymmetricAlgorithm() {
      using(var aesAlg = Aes.Create()) {
        //aesAlg.BlockSize = 124;
        var asd = new byte[1];
        aesAlg.Key = asd;
        //aesAlg.IV = Convert.FromBase64String(b64);

        Debug.WriteLine(aesAlg.Key);
        Debug.WriteLine(aesAlg.IV);
      }
    }

    [TestMethod, TestCategory("Cryptograph"), TestCategory("RijndaelExample")]
    public void RijndaelExample() {
      string original = "Here is some data to encrypt!";
      // Create a new instance of the RijndaelManaged
      // class.  This generates a new key and initialization 
      // vector (IV).
      using(var myRijndael = new RijndaelManaged()) {
        myRijndael.GenerateKey();
        myRijndael.GenerateIV();

        // Encrypt the string to an array of bytes.
        byte[] encrypted;
        var encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);

        // Create the streams used for encryption.
        using(var msEncrypt = new MemoryStream()) {
          using(var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
            using(var swEncrypt = new StreamWriter(csEncrypt)) {

              //Write all data to the stream.
              swEncrypt.Write(original);
            }
            encrypted = msEncrypt.ToArray();
          }
        }

        // Decrypt the bytes to a string.
        string plaintext = null;
        var decryptor = myRijndael.CreateDecryptor(myRijndael.Key, myRijndael.IV);

        // Create the streams used for decryption.
        using(var msDecrypt = new MemoryStream(encrypted)) {
          using(var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
            using(var srDecrypt = new StreamReader(csDecrypt)) {
              // Read the decrypted bytes from the decrypting stream
              // and place them in a string.
              plaintext = srDecrypt.ReadToEnd();
            }
          }
        }

        //Display the original data and the decrypted data.
        Assert.AreEqual(plaintext, original);
      }
    }

    [TestMethod, TestCategory("Cryptograph"), TestCategory("GenerateKey")]
    public void GenerateKey() {
      using(var aesAlg = Aes.Create()) {
        byte[] salt = Encoding.ASCII.GetBytes("aZ2XdZk1qVbPtGY6zAVriA");
        var key = new Rfc2898DeriveBytes("h64jHmvj7f0L9BmVo2W6zA", salt);
        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
        aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
      }
    }
  }
}
