using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection; //for assembly name


namespace SocketClientAsyncTester
{
    public class TestFileWriter
    {
        private object lockerForLog = new object();
        internal string saveFile;
        StreamWriter tw;

        public TestFileWriter(string saveDirectory)
        {    
            //We create a new log file every time we run the app.
            this.saveFile = GetSaveFileName(saveDirectory);
            // create a writer and open the file
            tw = new StreamWriter(this.saveFile);
            WriteLine("Starting log.");
        }

        private string GetSaveFileName(string saveDirectory)
        {            
            string assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            Int32 index = assemblyFullName.IndexOf(',');
            string saveFile = assemblyFullName.Substring(0, index);
            string dt = DateTime.Now.ToString("yyMMddHHmmss");
            //Save directory is created in ConfigFileHandler
            saveFile = saveDirectory + saveFile + "-" + dt + ".txt";
            return saveFile;
        }

        internal void WriteLine(string lineToWrite)
        {
            if (Program.consoleWatch == true)
            {
                Console.WriteLine(lineToWrite);
            }

            lock (this.lockerForLog)
            {
                tw.WriteLine(lineToWrite);
            }
        }

        internal void Close()
        {
            tw.Close();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("This session was logged to " + saveFile);
            Console.WriteLine();
            Console.WriteLine();
        }

    }
}
