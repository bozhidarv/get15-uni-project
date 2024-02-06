using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Testing
{
    internal class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();

        static void Main(string[] args)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient("127.0.0.1", port);

                Byte[] data = Encoding.ASCII.GetBytes("testing sth");

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", "testing sth");

                data = new Byte[256];
                string responseData = string.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
