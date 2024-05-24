using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "./test.text";
        string encryptedFilePath = "zakodowany_plik.enc";
        string decryptedFilePath = "odkodowany_plik.txt";

        RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider(2048);
        AesManaged myAES = new AesManaged();
        myAES.GenerateKey();

        byte[] RSAciphertext = myRSA.Encrypt(myAES.Key, true);

        EncryptFile(filePath, encryptedFilePath, myAES);

        byte[] AESKey = myRSA.Decrypt(RSAciphertext, true);
        AesManaged myAESDecrypted = new AesManaged();
        myAESDecrypted.Key = AESKey;
        myAESDecrypted.IV = myAES.IV;

        DecryptFile(encryptedFilePath, decryptedFilePath, myAESDecrypted);
    }

    static void EncryptFile(string inputFile, string outputFile, AesManaged aes)
    {
        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            using (FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (CryptoStream cs = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write))
                    {
                        int data;
                        while ((data = fsInput.ReadByte()) != -1)
                        {
                            cs.WriteByte((byte)data);
                        }
                    }
                }
            }
        }
    }

    static void DecryptFile(string inputFile, string outputFile, AesManaged aes)
    {
        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
        {
            using (FileStream fsDecrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                    {
                        int data;
                        while ((data = cs.ReadByte()) != -1)
                        {
                            fsDecrypted.WriteByte((byte)data);
                        }
                    }
                }
            }
        }
    }
}

