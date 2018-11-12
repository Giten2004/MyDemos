using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Globalization;

namespace SocketClientAsyncTester
{
    public class ConfigFileHandler
    {
        
        string configFilenameString = "saea_test_config.ini";
        string saveDirDefault = @"c:\LogForSaeaTest\Client\";

        internal string theWay;
        internal string host;
        internal Int32 portOnHost;
        internal string saveDirectory;
        internal Int32 testBufferSize;

        // This will be the total number of client connections that will be generated
        // by this test program. These connections will not
        // all be simultaneous, unless you set numberOfSentMessagesPerConnection to be a 
        // large number to keep the connection open a long time.        
        internal Int32 totalNumberOfClientConnectionsToRun;

        // If you put too large of a number it could crash your client machine. 
        // If you want to run more than about 3000 client connections simultaneously, 
        // and this client is on a machine that has operating system of Windows 2000,
        // Windows XP, or Windows Server 2003, then you may need to increase the number
        // of ephemeral ports above the default value. See the following link:
        //http://support.microsoft.com/kb/196271        
        internal Int32 maximumNumberOfSimultaneousClientConnections = 1000;

        //numberOfSentMessagesPerConnection variable will be the number of messages that
        //the client will send before disconnecting.
        //So a value of 1 will cause the client to send no more than 1 message
        //per connected session.  Value of 5 means send 5 messages and disconnect.
        //These two NEED TO BE THE SAME unless you change the code.
        internal Int32 numberOfReceivedMessagesPerConnection;
        internal Int32 numberOfSentMessagesPerConnection;     

        internal IPEndPoint remoteEndPoint;


        public ConfigFileHandler()
        {
            //We're only creating this config file the first time we use the app
            //or if the config file has been deleted.
            //Otherwise we just read from it.
            if (File.Exists(configFilenameString) == false)
            {
                //buffer size. start with 25
                //number of connections. start with 1.
                //number of messages per connection. start with 1.     
                //max simultaneous clients. start with 1000.
                CreateConfigFile(25, 1, 1, 1000, true);                
            }
            else
            {
                if (ReadConfigFile() == true)
                {
                    CheckForChanges();
                }
                else
                {
                    //buffer size. start with 25
                    //number of connections. start with 1.
                    //number of messages per connection. start with 1.    
                    //max simultaneous clients. start with 1000.
                    CreateConfigFile(25, 1, 1, 1000, true); 
                }
            }
        }


        public void CreateConfigFile(Int32 bufferSize, Int32 totalClientsToRun, Int32 numOfMessages, Int32 theMaximumNumberOfSimultaneousClientConnections, bool isNew)
        {
            HostFinder hostFinder = new HostFinder();

            if (isNew == true)
            {              
                this.remoteEndPoint = hostFinder.GetValidHost();
                this.theWay = hostFinder.theWay;
                this.host = hostFinder.host;
                this.portOnHost = hostFinder.portOnHost;                
                this.saveDirectory = GetLogFilePath();
                this.testBufferSize = bufferSize;
                this.totalNumberOfClientConnectionsToRun = totalClientsToRun;
                this.numberOfReceivedMessagesPerConnection = numOfMessages;
                this.numberOfSentMessagesPerConnection = this.numberOfReceivedMessagesPerConnection;
                this.maximumNumberOfSimultaneousClientConnections = theMaximumNumberOfSimultaneousClientConnections;
            }
            else
            {
                this.remoteEndPoint = hostFinder.GetValidHost(this.theWay, this.host, this.portOnHost);
            }
                        
            try
            {
                // create the config file and write to it.
                TextWriter tw = new StreamWriter(configFilenameString);

                //0                
                tw.WriteLine(hostFinder.theWay);                

                //1                
                tw.WriteLine(hostFinder.host);                

                //2
                tw.WriteLine(hostFinder.portString);                

                //3
                tw.WriteLine(this.saveDirectory);

                //4                
                tw.WriteLine(this.testBufferSize.ToString(CultureInfo.InvariantCulture));                

                //5
                tw.WriteLine(this.totalNumberOfClientConnectionsToRun.ToString(CultureInfo.InvariantCulture));                

                //6
                tw.WriteLine(this.numberOfReceivedMessagesPerConnection.ToString(CultureInfo.InvariantCulture));

                //7
                tw.WriteLine(this.maximumNumberOfSimultaneousClientConnections.ToString(CultureInfo.InvariantCulture));
         
                //close the writer
                tw.Close();
            }
            catch
            {
                Console.WriteLine("Could not create config file.");
            }
        }

        public bool ReadConfigFile()
        {
            bool configFileIsOkay = true;
            
            TextReader tr;
                try
                {
                    tr = new StreamReader(configFilenameString);

                    //0
                    this.theWay = tr.ReadLine();
                    if (this.theWay == "N")
                    {
                        Console.WriteLine("Will use machine name in config file to find host.");
                    }
                    if (this.theWay == "I")
                    {
                        Console.WriteLine("Will use IP address in config file to find host.");
                    } 
                    
                    //1
                    this.host = tr.ReadLine();
                    Console.WriteLine("host = " + this.host);
                    
                    //2
                    string portString = tr.ReadLine();
                    this.portOnHost = Convert.ToInt32(portString, CultureInfo.InvariantCulture);
                    Console.WriteLine("port = " + portString);
                    
                    //3
                    this.saveDirectory = tr.ReadLine();
                    
                    //4
                    this.testBufferSize = Convert.ToInt32(tr.ReadLine(), CultureInfo.InvariantCulture);
                    
                    //5
                    this.totalNumberOfClientConnectionsToRun = Convert.ToInt32(tr.ReadLine(), CultureInfo.InvariantCulture);
                    
                    //6
                    this.numberOfReceivedMessagesPerConnection = Convert.ToInt32(tr.ReadLine(), CultureInfo.InvariantCulture);
                    this.numberOfSentMessagesPerConnection = numberOfReceivedMessagesPerConnection;

                    //7
                    this.maximumNumberOfSimultaneousClientConnections = Convert.ToInt32(tr.ReadLine(), CultureInfo.InvariantCulture);
                    if (this.maximumNumberOfSimultaneousClientConnections == 0)
                    {
                        this.maximumNumberOfSimultaneousClientConnections = 1000;
                    }
                    
                    //close the reader
                    tr.Close();

                    HostFinder hostFinder = new HostFinder();  
                    remoteEndPoint = hostFinder.GetValidHost(this.theWay, this.host, this.portOnHost);
                    if (remoteEndPoint == null)
                    {
                        Console.WriteLine("Do you want to make a new config file? Type 'Y' for 'yes' or 'N' for 'no' and press Enter.");
                        string question = Console.ReadLine();
                        question = question.ToUpper(CultureInfo.InvariantCulture);
                        if (question == "Y")
                        {
                            DeleteConfigFile();
                            configFileIsOkay = false;
                        }
                        else
                        {
                            Console.WriteLine("\r\nMake sure the server is available. If it is, check the config file please.\r\nThen restart the app. Click 'X' to close.");
                            Console.ReadLine();
                            Console.WriteLine("\r\n****LOOK**** Click 'X' to close.");
                            Console.WriteLine("\r\n****LOOK**** Click 'X' to close.");
                            Console.WriteLine("\r\n****LOOK**** Click 'X' to close.");
                            Console.ReadLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            if ((this.theWay != "N") && (this.theWay != "I"))
            {                
                DeleteConfigFile();
                configFileIsOkay = false;
            }   
            if (this.host.Length == 0)
            {
                DeleteConfigFile();
                configFileIsOkay = false;
            }
            if ((this.portOnHost > 0) == false)
            {
                DeleteConfigFile();
                configFileIsOkay = false;
            }
            return configFileIsOkay;
        }

        private void CheckForChanges()
        {
            bool changeTracker = false;
            Console.WriteLine("Buffer size = " + this.testBufferSize);
            Console.WriteLine("Press Enter to accept that number, or type a number to change it.");
            string bufferSizeString = Console.ReadLine();
            if (bufferSizeString.Length > 0)
            {
                //user entered data to change buffer size
                try
                {
                    Int32 tempNumber = Convert.ToInt32(bufferSizeString, CultureInfo.InvariantCulture);
                    if ((tempNumber > 0)  && (tempNumber < Int32.MaxValue))
                    {
                        this.testBufferSize = tempNumber;
                        changeTracker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid buffer size. Value not changed.");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid buffer size. Value not changed.");
                }
            }
            NumberFormatInfo numFormat = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).NumberFormat;
            numFormat.NumberDecimalDigits = 0;

            Console.WriteLine("Total number of connections to run = " + this.totalNumberOfClientConnectionsToRun.ToString("n", numFormat));
            Console.WriteLine("Press Enter to accept that number, or type a number to change it.");
            string theConnectionNumber = Console.ReadLine();
            if (theConnectionNumber.Length > 0)
            {
                //user entered data to change number of connections in test
                try
                {
                    Int32 tempNumber = Convert.ToInt32(theConnectionNumber);
                    if ((tempNumber > 0) && (tempNumber < Int32.MaxValue))                    
                    {
                        this.totalNumberOfClientConnectionsToRun = tempNumber;
                        changeTracker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid total number of client connections to run. Value not changed.");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid total number of client connections to run. Value not changed.");
                }
            }
            Console.WriteLine("Number of messages to send per connection = " + numberOfReceivedMessagesPerConnection.ToString("n", numFormat));
            Console.WriteLine("Press Enter to accept that number, or type a number to change it.");
            string howManyMessagesPerConnection = Console.ReadLine();
            
            if (howManyMessagesPerConnection.Length > 0)
                
            {
                //user entered data to change number of messages per connection
                try
                {
                    Int32 tempNumber = Convert.ToInt32(howManyMessagesPerConnection);
                    if ((tempNumber > 0) && (tempNumber < Int32.MaxValue))
                    {
                        this.numberOfReceivedMessagesPerConnection = tempNumber;
                        this.numberOfSentMessagesPerConnection = this.numberOfReceivedMessagesPerConnection;
                        changeTracker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number of messages. Value not changed.");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid number of messages. Value not changed.");
                }
            }
                        
            string howManySimultaneousConnects = "";
            if (this.totalNumberOfClientConnectionsToRun > this.maximumNumberOfSimultaneousClientConnections)
            {
                Console.WriteLine("Maximum number of simultaneous connections (open ports) = " + this.maximumNumberOfSimultaneousClientConnections.ToString("n", numFormat));
                Console.WriteLine("This number is only relevant when you are trying to run a test\r\nthat has more client connections than that number.");
                Console.WriteLine("Press Enter to accept that number, or type a number to change it.");
                howManySimultaneousConnects = Console.ReadLine();
            }
            
            
            if (howManySimultaneousConnects.Length > 0)
            {
                //user entered data to change number of messages per connection
                try
                {
                    Int32 tempNumber = Convert.ToInt32(howManySimultaneousConnects);
                    if ((tempNumber > 0) && (tempNumber < Int32.MaxValue))
                    {
                        this.maximumNumberOfSimultaneousClientConnections = tempNumber;
                        changeTracker = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number of simultaneous connections. Value not changed.");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid number of simultaneous connections. Value not changed.");
                }
            }
            else
            {
                changeTracker = true;
            }


            if (changeTracker == true)
            {
                Console.WriteLine("Updating config file.");
                CreateConfigFile(this.testBufferSize, this.totalNumberOfClientConnectionsToRun, this.numberOfReceivedMessagesPerConnection, this.maximumNumberOfSimultaneousClientConnections, false);
            }
        }

        private void DeleteConfigFile()
        {
            Console.WriteLine("Error in the config file.");
            if (File.Exists(configFilenameString) == true)
            {
                try
                {
                    File.Delete(configFilenameString);
                    Console.WriteLine("The bad config file was deleted. Restart the program to correct the error.");
                }
                catch
                {
                    Console.WriteLine("The config file " + configFilenameString + " needs to be manually deleted. It is not right. Delete it and restart the program.");
                    Console.ReadLine();
                }
            }
        }
        
private string GetLogFilePath()
        {            
            string saveDirectory = "";
            
            
            Console.WriteLine("Type the folder to save the log file to, and press Enter.");
            Console.WriteLine("Or just press Enter to accept the default folder of " + saveDirDefault + ".");
            Console.WriteLine("The folder will be created if necessary.");
            Console.Write("logfile path = ");
            saveDirectory = Console.ReadLine();
            if (saveDirectory.Length == 0)
            {
                saveDirectory = saveDirDefault;
                
            }
            else
            {
                if (saveDirectory.EndsWith("\\") == false)
                {
                    saveDirectory = saveDirectory + "\\";
                }                
            }
            try
            {
                if (Directory.Exists(saveDirectory) == false)
                {
                    Directory.CreateDirectory(saveDirectory);
                }
            }
            catch
            {
                Console.WriteLine("Could not create save directory for log. See ConfigFileHandler.GetLogFilePath()."); Console.ReadLine();
            }
    return saveDirectory;
        }
    }
}
