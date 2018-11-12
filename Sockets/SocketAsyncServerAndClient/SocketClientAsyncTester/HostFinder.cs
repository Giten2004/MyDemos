using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;

namespace SocketClientAsyncTester
{
    class HostFinder
    {
        internal string theWay;
        internal string host;
        internal Int32 portOnHost;
        internal string portString;

        public HostFinder()
        {         
        }

        public IPEndPoint GetValidHost()
        {
            IPEndPoint hostEndPoint = null;
            
            string host = "";
            Int32 portOnHost = -1;

            host = GetHostString();
            portOnHost = GetHostPortInteger();
            if (this.theWay == "N")
            {
                hostEndPoint = GetServerEndpointUsingMachineName(host, portOnHost);
            }
            else
            {
                hostEndPoint = GetServerEndpointUsingIpAddress(host, portOnHost);
            }
            
            TestConnection(hostEndPoint);
            this.host = host;
            this.portOnHost = portOnHost;
            this.portString = portOnHost.ToString();

            
            return hostEndPoint;
        }

        public IPEndPoint GetValidHost(string theTheWay, string theHost, Int32 thePortOnHost)
        {
            IPEndPoint hostEndPoint = null;            
            this.theWay = theTheWay;
            this.host = theHost;
            this.portOnHost = thePortOnHost;
            this.portString = this.portOnHost.ToString(CultureInfo.InvariantCulture);
            if (this.theWay == "N")
            {
                hostEndPoint = GetServerEndpointUsingMachineName(host, portOnHost);
            }
            else
            {
                hostEndPoint = GetServerEndpointUsingIpAddress(host, portOnHost);
            }

            if (hostEndPoint != null)
            {
                TestConnection(hostEndPoint);
            }
            return hostEndPoint;
        }

        public bool TestConnection(IPEndPoint theHostEndPoint)
        {
            bool connectedSuccessfully = false;
            IPEndPoint hostEndPoint = theHostEndPoint;
            Console.WriteLine("Testing connection to server.");

            Socket socket = new Socket(hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(hostEndPoint);
                connectedSuccessfully = true;
                Console.WriteLine("Test connection to host is okay.");
                Console.WriteLine();
                try
                {
                    socket.Disconnect(false);                    
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Connected. But disconnect failed after test connection.");
                }
            }
            catch (SocketException ex)
            {
                connectedSuccessfully = false;
                Console.Beep();
                Console.Beep();
                Console.Beep();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Cannot connect to " + IPAddress.Parse(((IPEndPoint)hostEndPoint).Address.ToString()) + ": " + ((IPEndPoint)hostEndPoint).Port.ToString(CultureInfo.InvariantCulture));
                Console.WriteLine("Make sure that the host Endpoint is correct, and the server app is started.");
                Console.WriteLine("And your firewalls on both client and server will need to allow the connection.");
                
                Console.WriteLine();
                Console.WriteLine("Restart the application, please.");
                Console.Read();
                
            }
            return connectedSuccessfully;
        }



        public IPEndPoint GetServerEndpointUsingMachineName(string host, Int32 portOnHost)
        {
            
            IPEndPoint hostEndPoint = null;
            try
            {
                IPHostEntry theIpHostEntry = Dns.GetHostEntry(host);
                // Address of the host.
                IPAddress[] serverAddressList = theIpHostEntry.AddressList;

                bool gotIpv4Address = false;
                AddressFamily addressFamily;
                Int32 count = -1;
                for (int i = 0; i < serverAddressList.Length; i++)
                {                    
                    count++;
                    addressFamily = serverAddressList[i].AddressFamily;                    
                    if (addressFamily == AddressFamily.InterNetwork)
                    {
                        gotIpv4Address = true;
                        i = serverAddressList.Length;
                    }
                }

                if (gotIpv4Address == false)
                {
                    Console.WriteLine("Could not resolve name to IPv4 address. Need IP address. Failure!");
                }
                else
                {
                    Console.WriteLine("Server name resolved to IPv4 address.");
                    // Instantiates the endpoint.
                    hostEndPoint = new IPEndPoint(serverAddressList[count], portOnHost);
                }
            }    
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Could not resolve server address.");
                Console.WriteLine("host = " + host);                
            }
        
            return hostEndPoint;
        }

        public IPEndPoint GetServerEndpointUsingIpAddress(string host, Int32 portOnHost)
        {
            IPEndPoint hostEndPoint = null;
            try
            {
                IPAddress theIpAddress = IPAddress.Parse(host);
                // Instantiates the Endpoint.
                hostEndPoint = new IPEndPoint(theIpAddress, portOnHost);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);                
            }
            catch (FormatException e)
            {
                Console.WriteLine("FormatException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            return hostEndPoint;
        }

        public string GetHostString()
        {
            string host;
            bool gotHost = false;
            bool lightTheWay = false;
            
            while (lightTheWay == false)
                    {
                        Console.WriteLine("Choose whether to use the machine name or\r\nthe IP address of the host to find it on the network.\r\nMachine name is usually better, unless the host has a fixed IP address.\r\n\r\nType 'N' for Name, or 'I' for IP address. Then press Enter.");
                        theWay = Console.ReadLine().ToUpper(CultureInfo.InvariantCulture);
                        if ((theWay == "N") | (theWay == "I"))
                        {
                            lightTheWay = true;
                            Console.WriteLine("Got it.\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Invalid entry. Try again.");
                        }
                    }


                    if (theWay == "N")
                    {                        
                        Console.WriteLine("Type host machine name and press Enter.\r\nIf you do not know the machine name of the machine\r\nthat you want to run the server app on, then look at the\r\nConsole that is displayed when you start the SERVER app.\r\nThe machine name should be displayed in the Console.\r\n\r\nType that server machine name and press Enter.");
                    }
                    else if (theWay == "I")
                    {                        
                        Console.WriteLine("Type host IP address and press Enter.\r\n");
                    }



                    Console.Write("Host = ");
                    host = Console.ReadLine();
                    while (gotHost == false)
                    {
                        if (host.Length == 0)
                            Console.WriteLine("Host cannot be null or empty string. Try again.");
                        else
                            gotHost = true;
                    }
                    Console.WriteLine("Got it.\r\n");
            return host;
        }

        private Int32 GetHostPortInteger()
        {
            Int32 portOnHost = -1;
            //This is the port on the remote machine.
            string portString;    

            Console.WriteLine("Type the port to use on the SERVER.\r\nThe port should be displayed to the right of the colon\r\nin the SERVER Console on the line that says 'local endpoint ='.\r\nType the server port and press Enter.");
                    Console.WriteLine("Or just press Enter to accept the default port number of 4444.");
                    Console.Write("Server port = ");
                    portString = Console.ReadLine();
                    if (portString.Length < 1)
                        portString = "4444";


                    while (portOnHost < 1)
                    {
                        try
                        {
                            portOnHost = Convert.ToInt32(portString, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            Console.WriteLine("Please enter a number for the port");
                        }
                    }
                    Console.WriteLine("Got it.\r\n");
            return portOnHost;
        }
    }
}
