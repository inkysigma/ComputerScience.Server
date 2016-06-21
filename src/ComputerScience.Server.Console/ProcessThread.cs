using System.Diagnostics;
using System.Threading;

namespace ComputerScience.Server.Console
{
    public class ProcessThread
    {
        public Process Process { get; set; }
        public Thread Thread { get; }
        public volatile bool IsRunning = true;
        public volatile bool IsPaused = false;

        private Mutex LockMutex { get; }

        public ProcessThread(Process process, Mutex mutex = null)
        {
            Process = process;
            Thread = new Thread(Read);
            LockMutex = mutex ?? new Mutex();
        }

        private void Read()
        {
            while (IsRunning)
            {
                if (IsPaused)
                {
                    LockMutex.ReleaseMutex();
                    LockMutex.WaitOne();
                }
                var output = Process.StandardOutput.ReadLine();
                if (!string.IsNullOrEmpty(output))
                    System.Console.WriteLine(output);
            }
        }

        public void Write(string input)
        {
            Process.StandardInput.WriteLine(input);
        }

        public void Pause()
        {
            IsPaused = true;
            LockMutex.WaitOne();
        }

        public void Resume()
        {
            IsPaused = false;
            LockMutex.ReleaseMutex();
        }

        public void Abort()
        {
            IsRunning = false;
        }
    }
}
