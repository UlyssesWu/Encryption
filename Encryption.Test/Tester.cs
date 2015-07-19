using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uly.Security.Cryptography;

namespace Encryption.Test
{
    [TestClass]
    public class Tester
    {
        private Stopwatch _timer = new Stopwatch();

        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void TestDES()
        {
            var nanoSecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            Console.WriteLine("时间单位：Tick（每Tick为 " + nanoSecPerTick + " 纳秒）");
            Console.WriteLine("1毫秒 = " + 1000000 / nanoSecPerTick + " Tick");

            _timer.Reset();
            byte[] encrypted = new byte[64];
            byte[] decrypted = new byte[64];

            //From 《计算机与网络安全》课程实验
            byte[] key = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };
            byte[] plain = new byte[] { 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31 };
            string encryptedText = "655EA628CF62585F";

            //Run first time to make JIT compile this.
            DES.Encrypt(plain, key);
            DES.Decrypt(plain, key);

            _timer.Start();
            //Encryption
            encrypted = DES.Encrypt(plain, key);
            _timer.Stop();
            Console.WriteLine("Encrypt Time:" + _timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            //Decryption
            decrypted = DES.Decrypt(encrypted, key);
            _timer.Stop();
            Console.WriteLine("Decrypt Time:" + _timer.ElapsedTicks);
            _timer.Reset();

            Console.WriteLine(plain.PrintInHex() + " VS " + decrypted.PrintInHex());
            Assert.AreEqual(encryptedText, encrypted.PrintInHex().ToUpper());
            Assert.AreEqual(plain.PrintInHex(), decrypted.PrintInHex());

            //From Cryptography and Network Security (5th) Page 59
            plain = new byte[] { 0x02, 0x46, 0x8a, 0xce, 0xec, 0xa8, 0x64, 0x20 };
            key = new byte[] { 0x0f, 0x15, 0x71, 0xc9, 0x47, 0xd9, 0xe8, 0x59 };
            encryptedText = "DA02CE3A89ECAC3B";

            _timer.Start();
            //Encryption
            encrypted = DES.Encrypt(plain, key);
            _timer.Stop();
            Console.WriteLine("Encrypt Time:" + _timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            //Decryption
            decrypted = DES.Decrypt(encrypted, key);
            _timer.Stop();
            Console.WriteLine("Decrypt Time:" + _timer.ElapsedTicks);
            _timer.Reset();

            Console.WriteLine(plain.PrintInHex() + " VS " + decrypted.PrintInHex());
            Assert.AreEqual(encryptedText, encrypted.PrintInHex().ToUpper());
            Assert.AreEqual(plain.PrintInHex(), decrypted.PrintInHex());

            //From http://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm
            plain = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            key = new byte[] { 0x13, 0x34, 0x57, 0x79, 0x9B, 0xBC, 0xDF, 0xF1 };
            encryptedText = "85E813540F0AB405";

            _timer.Start();
            //Encryption
            encrypted = DES.Encrypt(plain, key);
            _timer.Stop();
            Console.WriteLine("Encrypt Time:" + _timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            //Decryption
            decrypted = DES.Decrypt(encrypted, key);
            _timer.Stop();
            Console.WriteLine("Decrypt Time:" + _timer.ElapsedTicks);
            _timer.Reset();

            Console.WriteLine(plain.PrintInHex() + " VS " + decrypted.PrintInHex());
            Assert.AreEqual(encryptedText, encrypted.PrintInHex().ToUpper());
            Assert.AreEqual(plain.PrintInHex(), decrypted.PrintInHex());

        }

        [TestMethod]
        public void TestRSA()
        {
            _timer.Reset();
            string publicKey;
            string privateKey;
            string n;

            string plain = "465";
            string encrypted;
            string decrypted;

            _timer.Start();
            //Generate 512 bits keys
            Assert.IsTrue(RSA.GenerateKeys(out n, out privateKey, out publicKey));
            _timer.Stop();
            Console.WriteLine("Generate Key Time(ms):" + _timer.ElapsedMilliseconds);
            _timer.Reset();
            _timer.Start();
            encrypted = RSA.Encrypt(plain, publicKey, n);
            _timer.Stop();
            Console.WriteLine("Encrypt Time(ms):" + _timer.ElapsedMilliseconds);
            _timer.Reset();
            _timer.Start();
            decrypted = RSA.Decrypt(encrypted, privateKey, n);
            _timer.Stop();
            Console.WriteLine("Decrypt Time(ms):" + _timer.ElapsedMilliseconds);
            _timer.Reset();
            Console.WriteLine(plain + " VS " + decrypted);
            Assert.AreEqual(plain,decrypted);

        }
    }
}
