using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;


Console.WriteLine("TCP server");

TcpListener listener = new TcpListener(IPAddress.Any, 7);

listener.Start();

while (true)
{


    TcpClient socket = listener.AcceptTcpClient();
    IPEndPoint clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
    Console.WriteLine("Client connected: " + clientEndPoint.Address);

    
    Task.Run(() => HandleClient(socket));


}


void HandleClient(TcpClient socket)
{

    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);

    while(socket.Connected)
    {

        
        string? message = reader.ReadLine().ToLower();
        string? numberMessage;
        int[] splitMessage;


        Console.WriteLine("test: " + message);
        
        switch (message)
        {
            case "random":
                //writer.WriteLine("Input numbers");
                //writer.Flush();
                numberMessage = GetString();
                splitMessage = SplitString(numberMessage);
                Random rnd = new Random();
                int random = rnd.Next(splitMessage[0], splitMessage[1]);
                writer.WriteLine(random);
                break;
            case "add":
                numberMessage = GetString();
                splitMessage = SplitString(numberMessage);
                int sum = splitMessage[0] + splitMessage[1];
                writer.WriteLine(sum);
                break;
            case "subtract":
                numberMessage = GetString();
                splitMessage = SplitString(numberMessage);
                int result = splitMessage[0] - splitMessage[1];
                writer.WriteLine(result);
                break;
            case "stop":
                writer.WriteLine("Server has been stopped");
                break;
            default:
                writer.WriteLine("Invalid command");
                break;


        }


        writer.Flush();
        

        if (message == "stop")
        {
            socket.Close();
        }
    }

    string GetString()
    {
        writer.WriteLine("Input numbers");
        writer.Flush();
        return reader.ReadLine();

    }



}

int[] SplitString(string numberString)
{

    return numberString.Split(' ').Select(int.Parse).ToArray(); 

}

