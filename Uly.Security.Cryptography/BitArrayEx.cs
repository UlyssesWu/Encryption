using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Uly.Security.Cryptography
{
    public static class BitArrayEx
    {
        /// <summary>
        ///     循环左移
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static BitArray ROL(this BitArray b1, int time)
        {
            //FIXED:CHECKED
            BitArray b2 = new BitArray(b1);
            for (int i = 0; i < b1.Length; i++)
            {
                b2[i] = b1[CalcIndex(i, time, b1.Length)];
                //Debug.WriteLine("{0}->{1}",CalcIndex(i, time, b1.Length),i);
            }
            return b2;
        }

        /// <summary>
        ///     循环右移
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static BitArray ROR(this BitArray b1, int time)
        {
            BitArray b2 = new BitArray(b1);
            for (int i = 0; i < b1.Length; i++)
            {
                b2[i] = b1[CalcIndex(i, -time, b1.Length)];
            }
            return b2;
        }

        private static int CalcIndex(int index, int shiftTime, int capacity)
        {
            shiftTime = shiftTime%capacity;
            int ni = index + shiftTime;
            if (ni < 0)
            {
                ni = capacity + ni;
            }
            while (ni >= capacity)
            {
                ni = ni - capacity;
            }
            //Debug.WriteLine(ni);
            return ni;
        }

        /// <summary>
        ///     将一个BitArray拼接在当前对象之前
        /// </summary>
        /// <param name="current"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        public static BitArray Prepend(this BitArray current, BitArray before)
        {
            var bools = new bool[current.Length + before.Length];
            before.CopyTo(bools, 0);
            current.CopyTo(bools, before.Length);
            return new BitArray(bools);
        }

        /// <summary>
        ///     将一个BitArray拼接在当前对象之后
        /// </summary>
        /// <param name="current"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Length + after.Length];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Length);
            return new BitArray(bools);
        }

        /// <summary>
        ///     设置BitArray某一范围的数据
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="val"></param>
        public static void SetRange(this BitArray b1, int startIndex, int count, int val)
        {
            BitArray v = new BitArray(new[] {val});
            for (int i = 0; i < count; i++)
            {
                b1[startIndex + i] = v[count - 1 - i];
                //Debug.WriteLine("{0}={1}", startIndex + i, 3 - i);
            }
        }

        public static void SetRange(this BitArray b1, int startIndex, int count, BitArray val, int valStartIndex = 0)
        {
            for (int i = 0; i < count; i++)
            {
                b1[startIndex + i] = val[valStartIndex + i];
            }
        }

        /// <summary>
        ///     转换为16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string PrintInHex(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var b in bytes)
            {
                var s = Convert.ToString(b, 16);
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(this string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length%2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length/2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i*2, 2), 16);
            return returnBytes;
        }

        public static string PrintInBinary(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var b in bytes)
            {
                var s = Convert.ToString(b, 2);
                for (int i = 0; i < 8 - s.Length; i++)
                {
                    sb.Append("0");
                }
                sb.Append(s);
                sb.Insert(sb.Length - 4, " ");
                sb.Append(" ");
            }

            return sb.ToString();
        }

        /// <summary>
        ///     转换为int
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="toLittleEndian">按小端转换</param>
        /// <returns></returns>
        public static int ToInt32(this BitArray binary, bool toLittleEndian = true)
        {
            if (binary == null)
                throw new ArgumentNullException("binary");
            if (binary.Length > 32)
                throw new ArgumentException("must be at most 32 bits long");

            //var result = new int[1];
            //((ICollection)binary).CopyTo(result, 0);
            //return result[0];

            int value = 0;

            for (int i = 0; i < binary.Count; i++)
            {
                if (binary[i])
                    value += Convert.ToInt16(Math.Pow(2, toLittleEndian ? binary.Count - i - 1 : i));
            }

            return value;
        }

        /// <summary>
        ///     转换为字节数组
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="toLittleEndian">按小端转换</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this BitArray binary, bool toLittleEndian = true)
        {
            //CHECKED
            if (binary == null)
                throw new ArgumentNullException("binary");

            var result = binary.Length%8 == 0 ? new Byte[binary.Length/8] : new byte[binary.Length/8 + 1];
            if (!toLittleEndian)
            {
                binary.CopyTo(result, 0);
            }
            else
            {
                binary.Reverse().CopyTo(result, 0);
                result = result.Reverse().ToArray();
            }
            return result;
        }

        public static BitArray ToBitArray(this int numeral)
        {
            return new BitArray(new[] {numeral});
        }

        public static string PrintInHex(this BitArray b, bool toLittleEndian = true)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var ba in b.ToByteArray(toLittleEndian))
            {
                var s = Convert.ToString(ba, 16);
                if (s.Length == 1)
                {
                    s = "0" + s;
                }
                sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     反转BitArray
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BitArray Reverse(this BitArray b)
        {
            BitArray b2 = new BitArray(b.Length);
            for (int i = 0; i < b.Length; i++)
            {
                b2[i] = b[b.Length - 1 - i];
            }
            return b2;
        }

        /// <summary>
        ///     转换为2进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string PrintInBinary(this BitArray b)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < b.Count; i++)
            {
                sb.Append(b[i] ? "1" : "0");
                if (i != 0 && i != b.Count - 1 && (i + 1)%4 == 0)
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }
    }
}