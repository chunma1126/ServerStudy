using _4_Pizza_Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_Pizza_Event
{
    internal class Program
    {
        static void Main()
        {
            EventLoop eventLoop = new EventLoop();//�̺�Ʈ ���������
            EventServer server = new EventServer(eventLoop);//���� ����鼭 �̺�Ʈ ���� ���
            server.Start();//���� ��ŸƮ
            eventLoop.RunForever();//�̺�Ʈ ��� ������
        }
    }
}
