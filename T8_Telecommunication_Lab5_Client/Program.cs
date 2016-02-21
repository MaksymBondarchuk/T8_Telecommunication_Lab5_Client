using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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
                IPEndPoint remoteEp = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEp);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(Environment.MachineName);

                    // Send the data through the socket.
                    sender.Send(msg);

                    // Receive the response from the remote device.
                    var bytesRec = sender.Receive(bytes);

                    if (bytesRec == 0)
                    {
                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();
                        Console.WriteLine("\nI was ignored by server (looks like no work for me)\n");
                        return;
                    }

                    var receivedRow = new List<byte>();
                    for (var i = 0; i < bytesRec; i++)
                        receivedRow.Add(bytes[i]);

                    Console.WriteLine("\nI received:");
                    foreach (var item in receivedRow)
                        Console.Write($"{item,4}");
                    Console.WriteLine();

                    receivedRow.Sort();

                    Console.WriteLine("\nThen sorted into:");
                    foreach (var item in receivedRow)
                        Console.Write($"{item,4}");
                    Console.WriteLine();

                    msg = receivedRow.ToArray();
                    sender.Send(msg);
                    Console.WriteLine("And sended that back to server");


                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane);
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
