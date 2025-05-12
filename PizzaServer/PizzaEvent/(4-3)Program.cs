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
            EventLoop eventLoop = new EventLoop();//이벤트 루프만들고
            EventServer server = new EventServer(eventLoop);//서버 만들면서 이벤트 루프 등록
            server.Start();//서버 스타트
            eventLoop.RunForever();//이벤트 계속 돌리기
        }
    }
}
