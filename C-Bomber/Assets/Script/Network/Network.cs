using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading;

public class Network : MonoBehaviour {

    Socket m_Socket;
    Thread netThread;
    ConcurrentQueue<Packet> m_PendingPackets;
    

    // Use this for initialization
    void Start () {
        Object.DontDestroyOnLoad(this);
        m_PendingPackets = new ConcurrentQueue<Packet>();
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint EndPoint = new IPEndPoint(ipAddress, 5555);
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Socket.Connect(EndPoint);
        netThread = new Thread(Netupdate);
        netThread.Start();
    }
	
	// Update is called once per frame
	void Netupdate () {
        while (true)
        {
            if (m_Socket.Poll(-1, SelectMode.SelectRead))
            {
                Packet l_Packet = Packet.Receive(m_Socket);
                m_PendingPackets.Enqueue(l_Packet);
            }
        }
    }

    void Update()
    {
        while (m_PendingPackets.Count != 0)
        {
            Packet l_Packet = m_PendingPackets.Dequeue();
            PacketProcessor.execute(l_Packet);
        }
    }

    public Socket getSocket()
    {
        return m_Socket;
    }

    class ConcurrentQueue<T>
    {
        private readonly object syncLock = new object();
        private Queue<T> queue;

        public int Count
        {
            get
            {
                lock (syncLock)
                {
                    return queue.Count;
                }
            }
        }

        public ConcurrentQueue()
        {
            this.queue = new Queue<T>();
        }

        public T Peek()
        {
            lock (syncLock)
            {
                return queue.Peek();
            }
        }

        public void Enqueue(T obj)
        {
            lock (syncLock)
            {
                queue.Enqueue(obj);
            }
        }

        public T Dequeue()
        {
            lock (syncLock)
            {
                return queue.Dequeue();
            }
        }

        public void Clear()
        {
            lock (syncLock)
            {
                queue.Clear();
            }
        }

        public T[] CopyToArray()
        {
            lock (syncLock)
            {
                if (queue.Count == 0)
                {
                    return new T[0];
                }

                T[] values = new T[queue.Count];
                queue.CopyTo(values, 0);
                return values;
            }
        }

        public static ConcurrentQueue<T> InitFromArray(IEnumerable<T> initValues)
        {
            var queue = new ConcurrentQueue<T>();

            if (initValues == null)
            {
                return queue;
            }

            foreach (T val in initValues)
            {
                queue.Enqueue(val);
            }

            return queue;
        }
    }
}
