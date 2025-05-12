using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _99_BusyClient
{
    internal class BusyClient
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)///1000번 보내기 서버한테
            {
                new Thread(() =>
                {
                    try
                    {
                        using var client = new TcpClient("127.0.0.1", 12345);//클라이언트 만들고
                        var stream = client.GetStream();
                        var msg = Encoding.UTF8.GetBytes("5\n");
                        stream.Write(msg, 0, msg.Length);//메시지 보내기

                        byte[] buffer = new byte[1024];
                        int read = stream.Read(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer, 0, read);//메세지 읽기
                        Console.WriteLine(response.Trim());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }).Start();
            }
        }
    }
}
