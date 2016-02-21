using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace T8_Telecommunication_Lab5_Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("MachineName: {0}", Environment.MachineName);

            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(Environment.MachineName);

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);

                    var receivedRow = new List<byte>();
                    for (var i = 0; i < bytesRec; i++)
                        receivedRow.Add(bytes[i]);
                    Console.WriteLine("I received:");
                    foreach (var item in receivedRow)
                        Console.Write($"{item,4}");
                    Console.WriteLine();

                    receivedRow.Sort();
                    msg = receivedRow.ToArray();
                    sender.Send(msg);
                    //Console.WriteLine("Echoed test = {0}",
                    //Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
