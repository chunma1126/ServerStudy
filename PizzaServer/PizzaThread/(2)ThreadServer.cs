using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Threading;

namespace _2_Pizza_Thread
{
    internal class Handler
    {
        private const int BUFFER_SIZE = 1024;
        private readonly TcpClient client;

        public Handler(TcpClient client)//clinet 셋팅하기    
        {
            this.client = client;
        }

        public void Run()//돌아가는 부분
        {
            IPEndPoint clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            Console.WriteLine($"Connected to {clientEndPoint}");

            NetworkStream stream = client.GetStream();//스트림 받아와서
            byte[] buffer = new byte[BUFFER_SIZE];

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, BUFFER_SIZE);//버퍼가 그거 읽고
                    if (bytesRead == 0) break;

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);//인간이 알아 먹을수 있게 변환하고
                    string response;

                    //주문한 피자만큼 출력
                    if (int.TryParse(received, out int order))
                    {
                        response = $"Thank you for ordering {order} pizzas!\n";
                    }
                    else
                    {
                        response = "Wrong number of pizzas, please try again\n";
                    }

                    Console.WriteLine($"Sending message to {clientEndPoint}");
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseData, 0, responseData.Length);
                }
            }
            finally
            {
                Console.WriteLine($"Connection with {clientEndPoint} has been closed");
                client.Close();
            }
        }
    }

    internal class ThreadServer
    {
        private const int PORT = 12345;
        private readonly TcpListener server;

        public ThreadServer() 
        {
            try
            {
                IPEndPoint localAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT);
                Console.WriteLine($"Starting up at: {localAddress}");
                server = new TcpListener(localAddress);
                server.Start();
            }
            catch (SocketException)
            {
                server?.Stop();
                Console.WriteLine("\nServer stopped.");
            }
        }

        public void Start()
        {
            Console.WriteLine("Server listening for incoming connections");

            try
            {
                while (true)    
                {   
                    TcpClient client = server.AcceptTcpClient();//클라이언트의 연결을 수락하고
                    Console.WriteLine($"Client connection request from {client.Client.RemoteEndPoint}");

                    Handler handler = new Handler(client);//클라이언트 별로 Handler만듦.
                    Thread thread = new Thread(new ThreadStart(handler.Run));//Handler마다 또 Tread드를 만들어줌.
                    thread.Start();//Thread스타트
                } 
            }
            finally
            {
                server.Stop();
                Console.WriteLine("\nServer stopped.");
            }
        }
        static void Main()
        {
            ThreadServer server = new ThreadServer();//뜨레드 서버 생성
            server.Start();
        }
    }
}
