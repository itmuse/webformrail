using System;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Web;

namespace WebFormRail
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CleanViewStatePersisterModule : IHttpModule
    {
        private const int CleanUpThreadSleep = 300000; //5 minutes (300000)
        private const int DaysToLeaveFiles = 1;

        private static readonly string cleanUpPath =
            string.Format(@"{0}\{1}\",
                          Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine).TrimEnd('\\'),
                          "WebFormRailViewStatePersist");

        private static bool isRunningCleanUpThread;

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            if (!Directory.Exists(cleanUpPath))
                Directory.CreateDirectory(cleanUpPath);

            if (!isRunningCleanUpThread)
            {
                // creates a new background thread to run the clean-up code.
#pragma warning disable RedundantDelegateCreation
                Thread thread = new Thread(new ThreadStart(CleanUp));
#pragma warning restore RedundantDelegateCreation

                // signals that the thread has started
                isRunningCleanUpThread = true;
                // starts the background thread
                thread.Start();
            }
        }

        public void Dispose()
        {
        }

        #endregion

        private static void CleanUp()
        {
            // inifinite loop is created so the thread will never stop executing
            // until application restart (a DLL is placed into the bin directory or the web.config file changes
            while (true)
            {
                // calls the code to constantly clean up the view state files
                TimeSpan timeSpan = new TimeSpan(DaysToLeaveFiles, 0, 0, 0);

                foreach (string filePath in Directory.GetFiles(cleanUpPath))
                {
                    FileInfo file = new FileInfo(filePath);

                    // if the difference between now and the last access time is greater than the time span
                    // delete the file.
                    if (DateTime.Now.Subtract(file.LastAccessTime) >= timeSpan)
                        file.Delete();
                }
                // tells the clean up thread to sleep for a period
                Thread.Sleep(CleanUpThreadSleep);
            }
#pragma warning disable FunctionNeverReturns
        }
#pragma warning restore FunctionNeverReturns
    }
}