using System;
using Uly.Security.Cryptography;

namespace Encryption
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            byte[] k = {0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30};
            byte[] b = {0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31};

            //byte[] b = new byte[] { 0x02, 0x46, 0x8a, 0xce, 0xec, 0xa8, 0x64, 0x20 };
            //byte[] k = new byte[] { 0x0f, 0x15, 0x71, 0xc9, 0x47, 0xd9, 0xe8, 0x59 };

            //byte[] b = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            //byte[] k = new byte[] { 0x13, 0x34, 0x57, 0x79, 0x9B, 0xBC, 0xDF, 0xF1 };

            Console.WriteLine("PLAINTEXT:" + b.PrintInHex());
            Console.WriteLine("KEY      :" + k.PrintInHex());

            //Console.WriteLine(b.PrintInBinary());
            //Console.WriteLine(k.PrintInBinary());
            Console.ReadLine();
            byte[] bs = DES.Encrypt(b, k);

            Console.Write("ENCRYPTED:");

            Console.WriteLine(bs.PrintInHex());

            Console.ReadLine();

            byte[] bo = DES.Decrypt(bs, k);
            Console.Write("DECRYPTED:");
            Console.WriteLine(bo.PrintInHex());
            Console.ReadLine();

            //var dese = DES.Encrypt(Encoding.UTF8.GetBytes("test测试").PrintInHex(), new byte[] {0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38});
            //Console.WriteLine(dese);
            //dese = DES.Decrypt(dese, new byte[] {0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38});
            //Console.WriteLine(Encoding.UTF8.GetString(dese.HexStringToBytes()));

            uint e1, d1, n1;
            e1 = 63;
            d1 = 847;
            n1 = 2773;
            Console.WriteLine("e={0} d={1} n={2}", e1.ToString(), d1.ToString(), n1.ToString());

            uint rsaC = RSA.Encrypt(465, e1, n1);
            Console.WriteLine(rsaC);
            uint rsaM = RSA.Decrypt(rsaC, d1, n1);
            Console.WriteLine(rsaM);
            Console.WriteLine();
            string plainTextRsa = "233333333333333333333333333333333333333333333333333333333333333333333333333333333";

            string e, d, n;
            bool result = RSA.GenerateKeys(out n, out d, out e, radix: 16);
            if (!result)
            {
                Console.WriteLine("Failed to generate keys.");
            }
            else
            {
                Console.WriteLine("[PUBLIC KEY]" + e + " " + n);
                Console.WriteLine("[PRIVATE KEY]" + d + " " + n);
                Console.WriteLine("[PLAINTEXT]" + plainTextRsa);

                string rc = RSA.Encrypt(plainTextRsa, e, n, 16);
                Console.WriteLine("[ENCRYPTED]" + rc);
                string rm = RSA.Decrypt(rc, d, n, 16);
                Console.WriteLine("[DECRYPTED]" + rm);
            }
            Console.ReadLine();
        }
    }
}