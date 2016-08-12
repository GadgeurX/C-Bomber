using UnityEngine;
using System.Collections;
using System;

public class Utils
{

    public static byte[] copyByteArray(byte[] dst, byte[] src, int size)
    {
        var outputBytes = new byte[dst.Length + size];
        Buffer.BlockCopy(dst, 0, outputBytes, 0, dst.Length);
        Buffer.BlockCopy(src, 0, outputBytes, dst.Length, size);
        return outputBytes;
    }

    public static UInt32 getUInt32(byte[] data, int startindex)
    {
        UInt32 l_Result = 0;
        l_Result = (UInt32)BitConverter.ToInt32(data, startindex);
        if (BitConverter.IsLittleEndian)
        {
            byte[] l_data = BitConverter.GetBytes(l_Result);
            Array.Reverse(l_data);
            l_Result = (UInt32)BitConverter.ToInt32(l_data, 0);
        }
        return l_Result;
    }

    public static byte getByte(byte[] data, int startindex)
    {
        byte l_Result = 0;
        l_Result = data[startindex];
        return l_Result;
    }
}
