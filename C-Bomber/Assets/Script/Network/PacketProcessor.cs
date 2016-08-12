using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PacketProcessor
{
    public delegate void ProcessFunction(Packet p_Packet);
    static Dictionary<byte, ProcessFunction> m_Processor;

    public static void execute(Packet p_Packet)
    {
        m_Processor[p_Packet.Opcode](p_Packet);
    }

    public static void addHandler(byte p_Opcode, ProcessFunction p_Func)
    {
        m_Processor[p_Opcode] = p_Func;
    }
}
