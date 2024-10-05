using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

class Program
{
    const string ServerAddress = "127.0.0.1";
    const int ServerPort = 3000;

    static void Main(string[] args)
    {
        try
        {
            using (TcpClient client = new TcpClient(ServerAddress, ServerPort))
            using (NetworkStream stream = client.GetStream())
            {
                SendInitialRequest(stream);

                List<byte[]> packets = new List<byte[]>();
                HashSet<int> receivedSequences = new HashSet<int>();

                ReadPackets(stream, packets, receivedSequences);

                RequestMissingPackets(receivedSequences, stream);

                SavePacketsToJson(packets);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Network error: " + ex.Message);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Socket error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        Console.ReadLine();
    }

    static void SendInitialRequest(NetworkStream stream)
    {
        byte[] request = { 1, 0 };
        stream.Write(request, 0, request.Length);
    }

    static void ReadPackets(NetworkStream stream, List<byte[]> packets, HashSet<int> receivedSequences)
    {
        while (true)
        {
            byte[] buffer = new byte[17];
            int bytesRead = ReadFully(stream, buffer);
            if (bytesRead == 0) break; 

            byte[] packetCopy = buffer.ToArray();
            packets.Add(packetCopy);

            int sequence = ParsePacket(packetCopy);
            receivedSequences.Add(sequence);
        }
    }

    static void RequestMissingPackets(HashSet<int> receivedSequences, NetworkStream stream)
    {
        if (receivedSequences.Count == 0) return; // No packets received

        int maxSequence = receivedSequences.Max();
        List<int> missingSequences = Enumerable.Range(1, maxSequence).Except(receivedSequences).ToList();

        foreach (int missingSequence in missingSequences)
        {
            RequestResendPacket(missingSequence, stream);
        }
    }

    static int ParsePacket(byte[] packet)
    {
        string symbol = Encoding.ASCII.GetString(packet, 0, 4);
        char buySellIndicator = (char)packet[4];
        int quantity = BitConverter.ToInt32(packet.Skip(5).Take(4).Reverse().ToArray(), 0);
        int price = BitConverter.ToInt32(packet.Skip(9).Take(4).Reverse().ToArray(), 0);
        int sequence = BitConverter.ToInt32(packet.Skip(13).Take(4).Reverse().ToArray(), 0);

        Console.WriteLine($"Symbol: {symbol}, Buy/Sell: {buySellIndicator}, Quantity: {quantity}, Price: {price}, Sequence: {sequence}");
        return sequence;
    }

    static void RequestResendPacket(int missingSequenceNumber, NetworkStream stream)
    {
        Console.WriteLine($"Requesting missing packet with sequence number: {missingSequenceNumber}");
        try
        {
            byte[] resendRequest = new byte[5];
            resendRequest[0] = 2; // Code for request
            Array.Copy(BitConverter.GetBytes(missingSequenceNumber).Reverse().ToArray(), 0, resendRequest, 1, 4);

            stream.Write(resendRequest, 0, resendRequest.Length);

            // Wait for the response and parse the missing packet
            byte[] buffer = new byte[17];
            int bytesRead = ReadFully(stream, buffer);
            if (bytesRead == buffer.Length)
            {
                ParsePacket(buffer);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error requesting missing packet: " + ex.Message);
        }
    }

    static int ReadFully(NetworkStream stream, byte[] buffer)
    {
        int totalBytesRead = 0;
        while (totalBytesRead < buffer.Length)
        {
            int bytesRead = stream.Read(buffer, totalBytesRead, buffer.Length - totalBytesRead);
            if (bytesRead == 0) break; 

            totalBytesRead += bytesRead;
        }
        return totalBytesRead;
    }

    static void SavePacketsToJson(List<byte[]> packets)
    {
        var packetList = packets.Select(packet =>
        {
            return new
            {
                Symbol = Encoding.ASCII.GetString(packet, 0, 4),
                BuySell = (char)packet[4],
                Quantity = BitConverter.ToInt32(packet.Skip(5).Take(4).Reverse().ToArray(), 0),
                Price = BitConverter.ToInt32(packet.Skip(9).Take(4).Reverse().ToArray(), 0),
                Sequence = BitConverter.ToInt32(packet.Skip(13).Take(4).Reverse().ToArray(), 0)
            };
        }).ToList();

        string json = JsonConvert.SerializeObject(packetList, Formatting.Indented);
        File.WriteAllText("output.json", json);
        Console.WriteLine("Saved packets to output.json");
    }
}
