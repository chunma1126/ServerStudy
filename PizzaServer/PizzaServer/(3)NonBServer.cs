using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _3_Pizza_NonB
{
    internal class NonBServer
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT = 12345;
        private Socket serverSocket;
        private readonly List<Socket> clients = new();

        public NonBServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
                serverSocket.Listen(1000);    // 최대 대기 수
                serverSocket.Blocking = false; // 논블로킹 모드
            }
            catch (SocketException)
            {
                serverSocket?.Close(); // 에러나면 닫고 종료
            }
        }

        private void Accept()
        {
            try
            {
                // 새 클라 받기
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Blocking = false;
                clients.Add(clientSocket);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.WouldBlock)
            {
                // 받을 거 없으면 패스
            }
        }

        private void Serve(Socket client)
        {
            byte[] buffer = new byte[BUFFER_SIZE];

            try
            {
                int bytesRead = client.Receive(buffer); // 데이터 받음
                if (bytesRead == 0)
                {
                    // 끊긴 클라 정리
                    clients.Remove(client);
                    client.Close();
                    return;
                }

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string response = int.TryParse(received, out int order)
                    ? $"Thank you for ordering {order} pizzas!\n"
                    : "Wrong number of pizzas, please try again\n";

                client.Send(Encoding.UTF8.GetBytes(response)); // 응답 보냄
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.WouldBlock)
            {
                
            }
        }

        public void Start()
        {
            try
            {
                while (true)
                {
                    Accept(); // 클라 받기
                    foreach (var client in new List<Socket>(clients))
                    {
                        Serve(client); // 응답 처리
                    }
                    Thread.Sleep(1);
                }
            }
            finally
            {
                serverSocket.Close();
            }
        }
    }
}
