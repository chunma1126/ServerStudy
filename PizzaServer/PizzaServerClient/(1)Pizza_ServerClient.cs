using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Server
    {
        private const int BUFFER_SIZE = 1024;
        private static readonly IPEndPoint ADDRESS = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
        private TcpListener serverSocket;

        public Server() 
        {
            try
            {
                Console.WriteLine($"Starting up at: {ADDRESS}");
                serverSocket = new TcpListener(ADDRESS);
                serverSocket.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine("\nServer failed to start.");
                serverSocket?.Stop();
            }
        }   

        public TcpClient Accept()   
        {
            TcpClient client = serverSocket.AcceptTcpClient();
            IPEndPoint clientEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            Console.WriteLine($"Connected to {clientEndPoint}");
            return client;
        }

        public void Serve(TcpClient client)    
        {
            NetworkStream stream = client.GetStream();//스트림을 받아서
            byte[] buffer = new byte[BUFFER_SIZE];
            
            try//클라이언트 하나에 묶여 잇어서 다른 클라이언트가 들어와도 입력받을수 없음.
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, BUFFER_SIZE);//여기서 받은 스트림을 써주고
                    if (bytesRead == 0) break;

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);//변환해서
                    string response;
                    
                    //주문한 피자 만큼 출력
                    if (int.TryParse(receivedData, out int order))
                    {
                        response = $"Thank you for ordering {order} pizzas!\n";
                    }
                    else
                    {
                        response = "Wrong number of pizzas, please try again\n";
                    }

                    //클라이언트의 주소?
                    Console.WriteLine($"Sending message to {client.Client.RemoteEndPoint}");
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseData, 0, responseData.Length);
                }
            }
            finally
            {
                //서버와의 연결을 클리언트가 끊으면 생기는 일.text
                Console.WriteLine($"Connection with {client.Client.RemoteEndPoint} has been closed");
                client.Close();
            }
        }   

        public void Start()
        {
            Console.WriteLine("Server listening for incoming connections");
            
            try
            {
                while (true)
                {
                    TcpClient client = Accept();//응답 받기
                    Serve(client);
                }
            }
            finally
            {
                serverSocket.Stop();//무한 루프에서 튕겨져 나오면 => 서버 연결이 끊기면?
                Console.WriteLine("\nServer stopped.");
            }
        }

        static void Main(string[] args) //진입점
        {
            Server server = new Server();//서버 생성
            server.Start();//시작
        }
    }
