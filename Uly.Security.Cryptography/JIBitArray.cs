using System;
using System.Collections;

namespace System.Collections
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class JIBitArray
	{
		private ArrayList _Bits = new ArrayList();

		#region Constructors
		public JIBitArray()
		{}
		public JIBitArray(byte[] bits)
		{
			string st;
			foreach(byte b in bits)
			{
				st = FixLength(Convert.ToString(b,2),8);
				AddBits(st);
			}
		}

		public JIBitArray(int[] bits)
		{
			string st;
			foreach(int i in bits)
			{
				st = FixLength(Convert.ToString(i,2),32);
				AddBits(st);
			}
		}

		public JIBitArray(long[] bits)
		{
			string st;
			foreach(long i in bits)
			{
				st = FixLength(Convert.ToString(i,2),64);
				AddBits(st);
			}
		}

		public JIBitArray(short[] bits)
		{
			string st;
			foreach(short i in bits)
			{
				st = FixLength(Convert.ToString(i,2),16);
				AddBits(st);
			}
		}

		public JIBitArray(bool[] bits)
		{
			foreach(bool b in bits)
			{
				_Bits.Add(b);
			}
		}

		public JIBitArray(int length)
		{
			AddBlock(length,false);
		}

		public JIBitArray(int length,bool defaultValue)
		{
			AddBlock(length,defaultValue);			
		}

		private void AddBlock(int length,bool Value)
		{
			for(int i=0; i<length; i++)
				_Bits.Add(Value);
		}
		#endregion

	    public int Length { get { return _Bits.Count; }}

	    private string FixLength(string num,int length)
		{
			while(num.Length < length)
				num = num.Insert(0,"0");
			return num;
		}

		private void AddBits(string bits)
		{
			foreach(char ch in bits)
			{
				if(ch == '0')
					_Bits.Add(false);
				else if(ch == '1')
					_Bits.Add(true);
				else
					throw(new ArgumentException("bits Contain none 0 1 character"));
			}
		}

		/// <summary>
		/// Convert current System.Collections.JIBitArray to binary string
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string rt = string.Empty;
			foreach(bool b in _Bits)
			{
				if(b == false)
					rt += '0';
				else
					rt += '1';
			}
			return rt;
		}

		/// <summary>
		/// Insert a bit at the spesific position in System.Collections.JIBitArray
		/// </summary>
		/// <param name="index">The zero-based index of the bit to insert</param>
		/// <param name="Value">The Boolean value to assign to the bit</param>
		public void Insert(int index,bool Value)
		{
			_Bits.Insert(index,Value);
		}

		/// <summary>
		/// Add a bit at the final position in System.Collections.JIBitArray
		/// </summary>
		/// <param name="Value">The Boolean value to assign to the bit</param>
		public void Add(bool Value)
		{
			_Bits.Add(Value);
		}

		/// <summary>
		/// Set the bit at the spesific position in System.Collection.JIBitArray
		/// </summary>
		/// <param name="index">The zero-based index of the bit to set</param>
		/// <param name="Value">The Boolean value to assign to the bit</param>
		public void Set(int index,bool Value)
		{
			_Bits[index] = Value;			
		}

		/// <summary>
		/// Get the value of a bit at the specific position in the System.Collections.JIBitArray
		/// </summary>
		/// <param name="index">The zero-based index of the value to get</param>
		/// <returns></returns>
		public bool Get(int index)
		{
			return (bool)_Bits[index];
		}

		/// <summary>
		/// Get the number of element actually contained in System.Collections.JIBitArray
		/// </summary>
		public int Count
		{
			get
			{
				return _Bits.Count; 
			}
		}

		/// <summary>
		/// Set all bits in System.Collections.JIBitArray to the specific value
		/// </summary>
		/// <param name="Value">The Boolean value to assign to all bits</param>
		public void SetAll(bool Value)
		{
			for(int i=0; i<_Bits.Count; i++)
			{
				_Bits[i] = Value;
			}
		}

		/// <summary>
		/// Inverts all the bits values in the current System.Collections.JIBitArray, so
		/// that elements set to true are changed to false, and elements set to false 
		/// are changed to true
		/// </summary>
		/// <returns></returns>
		public JIBitArray Not()
		{
			JIBitArray RArray = new JIBitArray(_Bits.Count);
			for(int i=0; i<_Bits.Count; i++)
			{
				if((bool)_Bits[i] == true)
					RArray.Set(i,false);
				else
					RArray.Set(i,true);
			}
			return RArray;
		}
		
		/// <summary>
		/// Retrives a SubJIBitArray from this instance. The SubJIBitArray start at the 
		/// specified bit position and has specified length
		/// </summary>
		/// <param name="index">The index of the start of SubJIBitArray</param>
		/// <param name="length">The number of bits in SubJIBitArray</param>
		/// <returns></returns>
		public JIBitArray SubJIBitArray(int index,int length)
		{
			JIBitArray RArray = new JIBitArray(length);
			int c=0;
			for(int i=index; i<index+length; i++)
				RArray.Set(c++,(bool)_Bits[i]);
			return RArray;
		}

		/// <summary>
		/// Performs the bitwise OR operation on the elements in the current System.Collections.JIBitArray 
		/// against the corresponding elements in the specified System.Collections.JIBitArray
		/// </summary>
		/// <param name="Value">The System.Collections.JIBitArray with which to perform the bitwise OR operation</param>
		/// <returns></returns>
		public JIBitArray Or(JIBitArray Value)
		{
			JIBitArray RArray;
			int Max = _Bits.Count > Value._Bits.Count ? _Bits.Count : Value._Bits.Count;
						
			RArray = new JIBitArray(Max);
			RArray._Bits = this._Bits;

			if(Max == RArray._Bits.Count)
				FixLength(Value,Max);
			else
				FixLength(RArray,Max);
		
			for(int i=0; i<Max; i++)
				RArray.Set(i,(bool)Value._Bits[i] | (bool)RArray._Bits[i]);
			
			return RArray;
		}

		/// <summary>
		/// Perform the bitwise AND operation on the elements in the current System.Collections.JIBitArray 
		/// against the corresponding elements in the specified System.Collections.JIBitArray
		/// </summary>
		/// <param name="Value">The System.Collections.JIBitArray with which to perform the bitwise OR operation</param>
		/// <returns></returns>
		public JIBitArray And(JIBitArray Value)
		{
			JIBitArray RArray;
			int Max = _Bits.Count > Value._Bits.Count ? _Bits.Count : Value._Bits.Count;

			RArray = new JIBitArray(Max);
			RArray._Bits = this._Bits;

			if(Max == RArray._Bits.Count)
				FixLength(Value,Max);
			else
				FixLength(RArray,Max);

			for(int i=0; i<Max; i++)
				RArray.Set(i,(bool)Value._Bits[i] & (bool)RArray._Bits[i]);

			return RArray;
		}

		/// <summary>
		/// Perform the bitwise eXclusive OR operation on the elements in the current System.Collections.JIBitArray 
		/// against the corresponding elements in the specified System.Collections.JIBitArray
		/// </summary>
		/// <param name="Value">The System.Collections.JIBitArray with which to perform the bitwise eXclusive OR operation</param>
		/// <returns></returns>
		public JIBitArray Xor(JIBitArray Value)
		{
			JIBitArray RArray;
			int Max = _Bits.Count > Value._Bits.Count ? _Bits.Count : Value._Bits.Count;

			RArray = new JIBitArray(Max);
			RArray._Bits = this._Bits;

			if(Max == RArray._Bits.Count)
				FixLength(Value,Max);
			else
				FixLength(RArray,Max);

			for(int i=0; i<Max; i++)
				RArray.Set(i,(bool)Value._Bits[i] ^ (bool)RArray._Bits[i]);

			return RArray;
		}

		#region Array Convertors
		/// <summary>
		/// Convert current System.Collections.JIBitArray to a long array
		/// </summary>
		/// <returns></returns>
		public long[] GetLong()
		{
			int ArrayBound = (int)Math.Ceiling((double)this._Bits.Count/64);
			long[] Bits = new long[ArrayBound];
			JIBitArray Temp = new JIBitArray();
			Temp._Bits = this._Bits;
			Temp = FixLength(Temp,ArrayBound * 64);
			for(int i=0; i< Temp._Bits.Count; i += 64)
			{
				Bits[i/64] = Convert.ToInt64(Temp.SubJIBitArray(i,64).ToString(),2);
			}
			return Bits;
        }

		/// <summary>
		/// Convert current System.Collections.JIBitArray to a int array
		/// </summary>
		/// <returns></returns>
		public int[] GetInt()
		{
			int ArrayBound = (int)Math.Ceiling((double)this._Bits.Count/32);
			int[] Bits = new int[ArrayBound];
			JIBitArray Temp = new JIBitArray();
			Temp._Bits = this._Bits;
			Temp = FixLength(Temp,ArrayBound * 32);
			
			for(int i=0; i< Temp._Bits.Count; i += 32)
			{
				Bits[i/32] = Convert.ToInt32(Temp.SubJIBitArray(i,32).ToString(),2);
			}
			return Bits;
		}

		/// <summary>
		/// Convert current System.Collections.JIBitArray to a short array
		/// </summary>
		/// <returns></returns>
		public short[] GetShorts()
		{
			int ArrayBound = (int)Math.Ceiling((double)this._Bits.Count/16);
			short[] Bits = new short[ArrayBound];
			JIBitArray Temp = new JIBitArray();
			Temp._Bits = this._Bits;
			Temp = FixLength(Temp,ArrayBound * 16);
			
			for(int i=0; i< Temp._Bits.Count; i += 16)
			{
				Bits[i/16] = Convert.ToInt16(Temp.SubJIBitArray(i,16).ToString(),2);
			}
			return Bits;
		}

		/// <summary>
		/// Convert current System.Collections.JIBitArray to a byte array
		/// </summary>
		/// <returns></returns>
		public byte[] GetBytes()
		{
			int ArrayBound = (int)Math.Ceiling((double)this._Bits.Count/8);
			byte[] Bits = new byte[ArrayBound];
			JIBitArray Temp = new JIBitArray();
			Temp._Bits = this._Bits;
			Temp = FixLength(Temp,ArrayBound * 8);
			
			for(int i=0; i< Temp._Bits.Count; i += 8)
			{
				Bits[i/8] = Convert.ToByte(Temp.SubJIBitArray(i,8).ToString(),2);
			}
			return Bits;
		}
		#endregion

		/// <summary>
		/// Shift the bits of current System.Collections.JIBitArray as specified number to 
		/// left
		/// </summary>
		/// <param name="count">Specific number to shift left</param>
		/// <returns></returns>
		public JIBitArray ShiftLeft(int count)
		{
			JIBitArray RArray = new JIBitArray();
			RArray._Bits = this._Bits;
			for(int i=0; i<count; i++)
			{
				RArray._Bits.RemoveAt(0);
				RArray._Bits.Add(false);
			}
			return RArray;
		}

		/// <summary>
		/// Shift the bits of current System.Collections.JIBitArray as specified number to 
		/// right
		/// </summary>
		/// <param name="count">Specific number to shift right</param>
		/// <returns></returns>
		public JIBitArray ShiftRight(int count)
		{
			JIBitArray RArray = new JIBitArray();
			RArray._Bits = this._Bits;
			for(int i=0; i<count; i++)
			{
				RArray._Bits.RemoveAt(RArray._Bits.Count-1);
				RArray._Bits.Insert(0,false);
			}
			return RArray;
		}

		/// <summary>
		/// Remove zero's of begining of current System.Collections.JIBitArray
		/// </summary>
		/// <returns></returns>
		public JIBitArray RemoveBeginingZeros()
		{
			JIBitArray RArray = new JIBitArray();
			RArray._Bits = this._Bits;
			while(RArray._Bits.Count != 0 && (bool)RArray._Bits[0] == false)
				RArray._Bits.RemoveAt(0);
			return RArray;
		}

		#region Static
		/// <summary>
		/// Insert enough zero at the begining of the specified System.Collections.JIBitArray to 
		/// make it's lenght to specified length
		/// </summary>
		/// <param name="Value">The System.Collections.JIBitArray with wich to insert zero to begining</param>
		/// <param name="length">The number of bits of Value after inserting</param>
		/// <returns></returns>
		public static JIBitArray FixLength(JIBitArray Value,int length)
		{
			if(length < Value._Bits.Count)
				throw(new ArgumentException("length must be equal or greater than Bits.Length"));
			while(Value._Bits.Count < length)
				Value._Bits.Insert(0,false);
			return Value;
		}
		#endregion

		#region Operators
		public static JIBitArray operator &(JIBitArray Bits1, JIBitArray Bits2)
		{
			return Bits1.And(Bits2);
		}

		public static JIBitArray operator |(JIBitArray Bits1, JIBitArray Bits2)
		{
			return Bits1.Or(Bits2);
		}

		public static JIBitArray operator ^(JIBitArray Bits1, JIBitArray Bits2)
		{
			return Bits1.Xor(Bits2);
		}

		public static JIBitArray operator >>(JIBitArray Bits, int count)
		{
			return Bits.ShiftRight(count);
		}

		public static JIBitArray operator <<(JIBitArray Bits, int count)
		{
			return Bits.ShiftLeft(count);
		}
		#endregion
	}
}