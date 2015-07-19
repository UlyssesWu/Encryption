using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Uly.Security.Cryptography
{
    public class DES
    {
        /// <summary>
        ///     置换选择1（PC-1）
        /// </summary>
        /// CHECKED
        private static readonly int[] PC_1 =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        /// <summary>
        ///     置换选择2（PC-2）
        /// </summary>
        /// CHECKED
        private static readonly int[] PC_2 =
        {
            14, 17, 11, 24, 1, 5, 3, 28,
            15, 6, 21, 10, 23, 19, 12, 4,
            26, 8, 16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };

        /// <summary>
        ///     左移次数
        /// </summary>
        private static readonly int[] LeftShiftTime = new int[16]
        {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };

        /// <summary>
        ///     初始置换（IP）
        /// </summary>
        /// CHECKED
        private static readonly int[] IP =
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };


        /// <summary>
        ///     逆初始置换（IP^-1）
        /// </summary>
        /// CHECKED
        private static readonly int[] RIP =
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        /// <summary>
        ///     扩展置换（E）
        /// </summary>
        private static readonly int[] E =
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        /// <summary>
        ///     置换函数P
        /// </summary>
        /// CHECKED
        private static readonly int[] P =
        {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };

        /// <summary>
        ///     S盒
        /// </summary>
        private static readonly int[][][] SB = new int[8][][]
        {
            //S1
            new[]
            {
                new[] {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                new[] {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                new[] {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                new[] {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
            },  
            //S2  
            new[]
            {
                new[] {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                new[] {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                new[] {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                new[] {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
            },  
            //S3  
            new[]
            {
                new[] {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                new[] {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                new[] {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                new[] {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
            },  
            //S4  
            new[]
            {
                new[] {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                new[] {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                new[] {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                new[] {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
            },  
            //S5  
            new[]
            {
                new[] {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                new[] {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                new[] {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                new[] {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
            },  
            //S6  
            new[]
            {
                new[] {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                new[] {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                new[] {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                new[] {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
            },  
            //S7  
            new[]
            {
                new[] {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                new[] {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                new[] {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                new[] {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
            },  
            //S8  
            new[]
            {
                new[] {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                new[] {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                new[] {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                new[] {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            }
        };

        /// <summary>
        ///     初始置换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static BitArray FirstSwap(BitArray input)
        {
            if (input.Length != 64)
            {
                throw new ArgumentException("Input should be 64 bits.");
            }
            BitArray output = new BitArray(64);
            for (int i = 0; i < 64; i++)
            {
                //原来第58号位置的bit移动到1号位置
                //IP的值从1开始
                output[i] = input[IP[i] - 1];
            }
            return output;
        }

        /// <summary>
        ///     逆初始置换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static BitArray ReverseFirstSwap(BitArray input)
        {
            if (input.Length != 64)
            {
                throw new ArgumentException("Input should be 64 bits.");
            }
            BitArray output = new BitArray(64);
            for (int i = 0; i < 64; i++)
            {
                //原来第40号位置的bit移动到1号位置
                //RIP的值从1开始
                output[i] = input[RIP[i] - 1];
            }
            return output;
        }

        /// <summary>
        ///     置换选择1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static BitArray SwapSelect1(BitArray input)
        {
            BitArray output = new BitArray(56);
            for (int i = 0; i < 56; i++)
            {
                //原来第57号位置的bit移动到1号位置
                //PC_1的值从1开始
                output[i] = input[PC_1[i] - 1];
            }
            return output;
        }

        /// <summary>
        ///     置换选择2
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static BitArray SwapSelect2(BitArray input)
        {
            BitArray output = new BitArray(48);
            for (int i = 0; i < 48; i++)
            {
                //原来第14号位置的bit移动到1号位置
                //PC_2的值从1开始
                output[i] = input[PC_2[i] - 1];
            }
            return output;
        }

        /// <summary>
        ///     P置换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static BitArray SwapP(BitArray input)
        {
            BitArray output = new BitArray(32);
            for (int i = 0; i < 32; i++)
            {
                //原来第16号位置的bit移动到1号位置
                //P的值从1开始
                output[i] = input[P[i] - 1];
            }
            return output;
        }

        /// <summary>
        ///     S盒
        /// </summary>
        /// <param name="input">输入（48位）</param>
        /// <returns>输出（32位）</returns>
        private static BitArray SBox(BitArray input)
        {
            BitArray output = new BitArray(32);
            //8个S盒
            for (int i = 0; i < 8; i++)
            {
                int col = 0;
                int row = 0;
                GenerateRowAndColumnNumber(input, i*6, out row, out col);
                //output.SetRange(i*4, 4, SwitchSBox(i)[row][col]);
                output.SetRange(i*4, 4, SB[i][row][col]);
            }

            return output;
        }

        /// <summary>
        ///     生成S盒选择项的行号列号
        /// </summary>
        /// <param name="input">输入（48位）</param>
        /// <param name="firstBit">第一位位置（从此位置向后取6位）</param>
        /// <param name="row">产生的行号</param>
        /// <param name="col">产生的列号</param>
        private static void GenerateRowAndColumnNumber(BitArray input, int firstBit, out int row, out int col)
        {
            int lastBit = firstBit + 5;
            BitArray columnArray = new BitArray(4);
            BitArray rowArray = new BitArray(2);
            rowArray[0] = input[firstBit];
            rowArray[1] = input[lastBit];
            columnArray[0] = input[firstBit + 1];
            columnArray[1] = input[firstBit + 2];
            columnArray[2] = input[firstBit + 3];
            columnArray[3] = input[firstBit + 4];
            row = rowArray.ToInt32();
            col = columnArray.ToInt32();
        }

        /// <summary>
        ///     DES一轮加密
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ki"></param>
        /// <returns></returns>
        private static RoundPackage Round(RoundPackage p, BitArray ki)
        {
            RoundPackage next = new RoundPackage(p.RoundID + 1);
            //扩展/置换
            BitArray exR = new BitArray(48);
            for (int i = 0; i < 48; i++)
            {
                //扩展32位至48位
                //E的值从1开始
                exR[i] = p.R[E[i] - 1];
            }

            //XOR
            var s = exR.Xor(ki);
            //S盒（48->32）
            s = SBox(s);
            //置换
            s = SwapP(s);
            //XOR
            next.R = s.Xor(p.L);
            next.L = p.R;
            return next;
        }

        /// <summary>
        ///     生成子密钥
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        private static BitArray[] GenerateKeys(BitArray key)
        {
            //置换选择1 64位->56位
            key = SwapSelect1(key);
            //Debug.WriteLine(key.PrintInBinary());

            BitArray[] output = new BitArray[16];

            BitArray c = new BitArray(28);
            BitArray d = new BitArray(28);
            c.SetRange(0, 28, key);
            d.SetRange(0, 28, key, 28);

            //Debug.WriteLine(c.PrintInBinary());
            //Debug.WriteLine(d.PrintInBinary());

            for (int i = 0; i < 16; i++)
            {
                //密钥产生
                //循环左移
                c = c.ROL(LeftShiftTime[i]);
                d = d.ROL(LeftShiftTime[i]);
                //Debug.WriteLine("i:{2} C:{0} D:{1}", c.PrintInBinary(), d.PrintInBinary(), i+1);
                //置换选择2
                output[i] = SwapSelect2(c.Append(d));
                //Debug.WriteLine(output[i].PrintInBinary());
                //Debug.WriteLine("C:{0} D:{1} out:{2}",c.PrintInHex(),d.PrintInHex(),output[i].PrintInHex());
            }
            return output;
        }


        /// <summary>
        ///     DES加密
        /// </summary>
        /// <param name="plain">8Byte字节数组明文</param>
        /// <param name="key">8Byte字节数组密文</param>
        /// <returns></returns>
        public static byte[] Encrypt(Byte[] plain, Byte[] key)
        {
            if (plain.Length > 8 || key.Length > 8)
            {
                throw new ArgumentException("Plain text and key should be 8 bytes.");
            }
            //不足8字节，补0
            if (plain.Length < 8)
            {
                plain = plain.Concat(new Byte[8 - plain.Length]).ToArray();
            }
            if (key.Length < 8)
            {
                key = key.Concat(new Byte[8 - key.Length]).ToArray();
            }

            //转为位数组 处理小端->大端
            BitArray input = new BitArray(plain.Reverse().ToArray()).Reverse();
            BitArray inputKey = new BitArray(key.Reverse().ToArray()).Reverse();
            Debug.WriteLine("[PLAIN]" + input.PrintInBinary());
            Debug.WriteLine("[KEY]" + inputKey.PrintInBinary());
            //初始置换
            input = FirstSwap(input);
            //Debug.WriteLine(input.PrintInHex());
            BitArray[] keys = new BitArray[16];
            //设置L R
            RoundPackage rounds = new RoundPackage();
            rounds.L.SetRange(0, 32, input);
            rounds.R.SetRange(0, 32, input, 32);
            //生成16轮用子密钥
            keys = GenerateKeys(inputKey);

            //16轮加密
            for (int i = 0; i < 16; i++)
            {
                rounds = Round(rounds, keys[i]);
                //Debug.WriteLine("i:{3}, L:{0},R:{1},Ki:{2}", rounds.L.PrintInBinary(), rounds.R.PrintInBinary(), keys[i].PrintInBinary(),i+1);
            }
            //Debug.WriteLine("L:{0},R:{1}", rounds.L.PrintInBinary(), rounds.R.PrintInBinary());

            BitArray output = new BitArray(64);
            //拼接：R+L
            output = rounds.R.Append(rounds.L);
            //Debug.WriteLine(output.PrintInBinary());
            //逆初始置换
            output = ReverseFirstSwap(output);
            Debug.WriteLine("[ENCRYPT]" + output.PrintInBinary());
            return output.ToByteArray();
        }

        public static byte[] Decrypt(Byte[] inputBytes, Byte[] key)
        {
            if (inputBytes.Length > 8 || key.Length > 8)
            {
                throw new ArgumentException("Encrypted text and key should be 8 bytes.");
            }
            if (inputBytes.Length < 8)
            {
                inputBytes = inputBytes.Concat(new Byte[8 - inputBytes.Length]).ToArray();
            }
            if (key.Length < 8)
            {
                key = key.Concat(new Byte[8 - key.Length]).ToArray();
            }
            //BitArray input = new BitArray(inputBytes);
            //BitArray inputKey = new BitArray(key);
            //处理小端->大端
            BitArray input = new BitArray(inputBytes.Reverse().ToArray()).Reverse();
            BitArray inputKey = new BitArray(key.Reverse().ToArray()).Reverse();
            Debug.WriteLine("[ENCRYPTED]" + input.PrintInBinary());
            Debug.WriteLine("[KEY]" + inputKey.PrintInBinary());
            input = FirstSwap(input);
            BitArray[] keys = new BitArray[16];
            RoundPackage rounds = new RoundPackage();
            rounds.L.SetRange(0, 32, input);
            rounds.R.SetRange(0, 32, input, 32);

            keys = GenerateKeys(inputKey);
            //Debug.WriteLine("L:{0},R:{1},Ki:{2}", rounds.L.PrintInHex(), rounds.R.PrintInHex(), keys[15].PrintInHex());

            for (int i = 15; i >= 0; i--)
            {
                rounds = Round(rounds, keys[i]);
                //Debug.WriteLine("L:{0},R:{1},Ki:{2}", rounds.L.PrintInHex(), rounds.R.PrintInHex(), keys[i].PrintInHex());
            }
            BitArray output = new BitArray(64);
            output = rounds.R.Append(rounds.L);
            output = ReverseFirstSwap(output);
            Debug.WriteLine("[DECRYPT]" + output.PrintInBinary());
            return output.ToByteArray();
        }

        /// <summary>
        ///     DES加密
        /// </summary>
        /// <param name="plain">字符串表示的16进制明文</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string plain, Byte[] key)
        {
            //密钥不足8byte，补足
            if (key.Length < 8)
            {
                key = key.Concat(new Byte[8 - key.Length]).ToArray();
            }
            List<Byte> pBytes = new List<byte>(plain.HexStringToBytes());
            //明文的长度不是8的倍数，补足
            while (pBytes.Count%8 != 0)
            {
                pBytes.Add(new byte());
            }
            var bytes = pBytes.ToArray();
            byte[] p = new byte[8];
            StringBuilder sb = new StringBuilder();
            //每8Byte进行一次DES加密，结果拼接
            for (int i = 0; i < bytes.Length; i += 8)
            {
                Array.ConstrainedCopy(bytes, i, p, 0, 8);
                sb.Append(DES.Encrypt(p, key).PrintInHex());
            }
            return sb.ToString();
        }

        /// <summary>
        ///     DES解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string input, Byte[] key)
        {
            if (key.Length < 8)
            {
                key = key.Concat(new Byte[8 - key.Length]).ToArray();
            }
            List<Byte> pBytes = new List<byte>(input.HexStringToBytes());
            while (pBytes.Count%8 != 0)
            {
                pBytes.Add(new byte());
            }
            var bytes = pBytes.ToArray();
            byte[] p = new byte[8];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 8)
            {
                Array.ConstrainedCopy(bytes, i, p, 0, 8);
                sb.Append(DES.Decrypt(p, key).PrintInHex());
            }
            return sb.ToString();
        }

        /// <summary>
        ///     存储一轮加密的信息
        /// </summary>
        private class RoundPackage
        {
            public RoundPackage(int round = 0)
            {
                L = new BitArray(32, false);
                R = new BitArray(32, false);

                RoundID = round;
            }

            /// <summary>
            ///     L (32位)
            /// </summary>
            public BitArray L { get; set; }

            /// <summary>
            ///     R (32位)
            /// </summary>
            public BitArray R { get; set; }

            public int RoundID { get; set; }
        }
    }
}