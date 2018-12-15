using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace OpsMgrAlertConsole 
{
    static class OpsMgrConsole
    {
        private static DateTime dtLastFault;

        /// <summary>
        /// The main entry point for the application.
        /// </summary> 
        [STAThread]
        static void Main() 
        {
            EventLog evtLog = new EventLog();
            evtLog.Source = "OpsMgrAlertConsole";

            dtLastFault = new DateTime((DateTime.Now).Year - 1, 1, 1, 1, 1, 1);

            try
            {
                //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                Trace.TraceInformation("Main...start");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMainWindow());
            }
            catch (Exception ex0)
            {
                DateTime dtNewFault = DateTime.Now;

                evtLog.WriteEntry(String.Format("Unhandled exception`n{0}`n{1}", ex0.Message, ex0.StackTrace), EventLogEntryType.Error, 44500);

                int iMunutesFromLastFault = (int)(dtNewFault - dtLastFault).TotalMinutes;
                if (iMunutesFromLastFault > 10)
                {
                    evtLog.WriteEntry("Appication restart", EventLogEntryType.Warning, 44501);
                    Application.Restart();
                }
                else
                {
                    evtLog.WriteEntry(String.Format("{0} min from last fault - exit application", iMunutesFromLastFault), EventLogEntryType.Error, 44502);
                }
            }

        }
    }
}
