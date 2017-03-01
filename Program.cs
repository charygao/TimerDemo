using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Edms.Helpers
{
    class Program
    {
       

        static void Main(string[] args)
        {
            var taskTimer = new EdmsTimer();
            taskTimer.Context = new Program();
            taskTimer.TaskTime = new TimeSpan(0, 0, 0, 20);
            taskTimer.WarningTime = new TimeSpan(0,0,0,10);
            taskTimer.TimespanChanged += Timer1_TimespanChanged;
            taskTimer.HasBeenPaused += TaskTimer_HasBeenPaused;
            taskTimer.IsEnable = true;
            taskTimer.Start();

            Console.Read();
        }

        private static void TaskTimer_HasBeenPaused(EdmsTimer sender, bool paused)
        {
          
            Console.WriteLine("==========Timer has been paused==========");
            Thread.Sleep(5000);
            sender.Resume();
        }

        private static void Timer1_TimespanChanged(EdmsTimer sender, TimeSpan timeLeft)
        {
            if (timeLeft.Seconds <= 0)
            {
               
                Console.WriteLine("=================Time Over===============");
                Console.WriteLine("=========================================");
                //you can do something here
                sender.CleanUp();//release the timer

            }
            else if (timeLeft <= sender.WarningTime)
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("任务预设时间:" + sender.TaskTime.Seconds);
                Console.WriteLine("任务警告时间:" + sender.WarningTime.Seconds);
                Console.WriteLine("任务已花费时间:" + sender.TaskTimeUsed.Seconds);
                Console.WriteLine("任务剩余时间:" + timeLeft.Seconds);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                //you can do something for warning time
                //normally, you need to give some warning message for user
                //sender.Pause();
                
            }
            else
            {
                Console.WriteLine("#########################################");
                Console.WriteLine("任务预设时间:" + sender.TaskTime.Seconds);
                Console.WriteLine("任务警告时间:" + sender.WarningTime.Seconds);
                Console.WriteLine("任务已花费时间:" + sender.TaskTimeUsed.Seconds);
                Console.WriteLine("任务剩余时间:" + timeLeft.Seconds);
                Console.WriteLine("#########################################");
                //you can print the timer information for the user
            }
           
        }
    }
}
