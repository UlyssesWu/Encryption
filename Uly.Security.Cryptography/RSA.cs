using System;

namespace Uly.Security.Cryptography
{
    public class RSA
    {
        //Random number generator 效率并不高
        private static Random randGen = new Random();

        //C = M^e mod n
        public static string Encrypt(string message, string e, string n, int radix = 10)
        {
            BigInteger bm = new BigInteger(message, radix);
            return bm.ModPow(new BigInteger(e, radix), new BigInteger(n, radix)).ToString(radix);
        }

        //M = C^d mod n
        public static string Decrypt(string message, string d, string n, int radix = 10)
        {
            BigInteger bm = new BigInteger(message, radix);
            return bm.ModPow(new BigInteger(d, radix), new BigInteger(n, radix)).ToString(radix);
        }

        /// <summary>
        ///     生成公钥私钥
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="bitDepth">位数</param>
        /// <param name="radix">表示进制</param>
        /// <param name="confidence">可信度</param>
        /// <returns></returns>
        public static bool GenerateKeys(out string n, out string d, out string e, int bitDepth = 512, int radix = 10,
            int confidence = 5)
        {
            //随机生成素数p,q
            var p = GeneratePrime(bitDepth, confidence);
            var q = GeneratePrime(bitDepth, confidence);
            if (p == 0 || q == 0)
            {
                n = "";
                d = "";
                e = "";
                return false;
            }
            //n = p * q
            var bn = p*q;
            //fai(n) = (p - 1)*(q - 1)
            var fai = (p - 1)*(q - 1);
            //取e与fai(n)互素
            var be = fai.GenCoPrime(bitDepth, randGen);
            //d = e^-1 (mod fai(n))
            var bd = be.ModInverse(fai);
            n = bn.ToString(radix);
            d = bd.ToString(radix);
            e = be.ToString(radix);
            return true;
        }

        /// <summary>
        ///     产生随机质数
        /// </summary>
        /// <param name="bitDepth">位数</param>
        /// <param name="confidence">可信度</param>
        /// <returns></returns>
        private static BigInteger GeneratePrime(int bitDepth = 512, int confidence = 5)
        {
            BigInteger pr = BigInteger.GenPseudoPrime(bitDepth, confidence, randGen);
            return pr;
        }


        //C = M^e mod n
        public static uint Encrypt(uint message, uint e, uint n)
        {
            uint c = ModularPow(message, e, n);
            return c;
        }

        //M - C^d mod n
        public static uint Decrypt(uint message, uint d, uint n)
        {
            uint m = ModularPow(message, d, n);
            return m;
        }

        //From http://en.wikipedia.org/wiki/Modular_exponentiation#Memory-efficient_method
        /// <summary>
        ///     模幂运算
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static uint ModularPow(uint b, uint e, uint m)
        {
            uint c = 1;
            for (int ePrime = 1; ePrime <= e; ePrime++)
                c = (c*b)%m;
            return c;
        }
    }
}