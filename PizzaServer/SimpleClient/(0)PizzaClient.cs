using System;
using System.Net.Sockets;
using System.Text;

namespace _0_SimpleClient
{
    internal class PizzaClient
    {
        private const int BUFFER_SIZE = 1024;  //버퍼의 최대 크기
        private const string SERVER_ADDRESS = "127.0.0.1"; //로컬
        private const int SERVER_PORT = 12345;

        static void Main()
        {
            try
            {  
                using TcpClient client = new TcpClient(SERVER_ADDRESS, SERVER_PORT);//클라이언트 생성
                NetworkStream stream = client.GetStream();

                while (true)//데이터 처리를 위해 무한 루프
                {
                    Console.Write("How many pizzas do you want? ");
                    string? order = Console.ReadLine();                     

                    if (string.IsNullOrEmpty(order))
                        break;
                        
                    byte[] dataToSend = Encoding.UTF8.GetBytes(order);//데이터 보내기
                    stream.Write(dataToSend, 0, dataToSend.Length);        //데이터 받기
                    byte[] buffer = new byte[BUFFER_SIZE]; //최대 길이 만큼 만들어서
                    int bytesRead = stream.Read(buffer, 0, BUFFER_SIZE);  //쓰고
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimEnd();//알아먹을수 있게 변환해서   

                    Console.WriteLine($"Server replied '{response}'");//출력
                }

                Console.WriteLine("Client closing");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }
    }
}
