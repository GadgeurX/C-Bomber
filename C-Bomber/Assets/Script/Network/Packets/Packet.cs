using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class Packet
{
    private byte[] m_Data;
    private byte m_Opcode;

    public byte[] Data
    {
        get
        {
            return m_Data;
        }

        set
        {
            m_Data = value;
        }
    }

    public byte Opcode
    {
        get
        {
            return m_Opcode;
        }

        set
        {
            m_Opcode = value;
        }
    }

    public static void SendAsync(Packet paquet, Socket p_Socket)
    {
        NetworkStream stream = new NetworkStream(p_Socket);
        stream.Write(paquet.Data, 0, paquet.Data.Length);
        stream.Flush();
    }

    public static Packet Receive(Socket p_Socket)
    {
        NetworkStream stream = new NetworkStream(p_Socket);

        Packet p = null;
        byte[] l_Buffer = new byte[512];
        byte[] l_PacketData = new byte[0];
        UInt32 l_PacketSize = 0;

        do
        {
            int size = stream.Read(l_Buffer, 0, l_Buffer.Length);
            l_PacketData = Utils.copyByteArray(l_PacketData, l_Buffer, size);
            if (l_PacketSize == 0 && l_PacketData.Length >= 4)
            {
                l_PacketSize = Utils.getUInt32(l_PacketData, 0);
            }
        } while (l_PacketData.Length != l_PacketSize);
        p.Opcode = Utils.getByte(l_PacketData, 4);
        return p;
    }
}
