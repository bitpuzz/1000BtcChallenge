using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using NBitcoin;

class Program
{
    private readonly static HttpClient client = new HttpClient();
    private readonly static string targetAddress = "1BY8GQbnueYofwSuFAT3USAhGjPrkxDdW9";
    private readonly static BigInteger startKey = BigInteger.Parse("40000000000000000", System.Globalization.NumberStyles.HexNumber);
    private readonly static BigInteger endKey = BigInteger.Parse("7ffffffffffffffff", System.Globalization.NumberStyles.HexNumber);
    private static readonly string? apiKey = Environment.GetEnvironmentVariable("API_KEY");
    private static readonly string? phone = Environment.GetEnvironmentVariable("PHONE");
    private static readonly bool enableMessageSending = Environment.GetEnvironmentVariable("ENABLE_MESSAGE_SENDING") == "true";

    static void Main()
    {
        Console.WriteLine("Starting Key Search for Puzzle 67");        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        long privateKeyCount = 0;
        object lockObj = new object();

        Parallel.For(0, Environment.ProcessorCount, async i =>
        {
            Random random = new Random();
            while (true)
            {
                BigInteger randomKeyInt = GenerateRandomKeyWithinRange(random);
                Key privateKey = new Key(randomKeyInt.ToByteArray());

                PubKey publicKeyCompressed = privateKey.PubKey;
                string p2pkhCompressed = publicKeyCompressed.GetAddress(ScriptPubKeyType.Legacy, Network.Main).ToString();

                if (p2pkhCompressed == targetAddress)
                {
                    if(enableMessageSending)
                        await SendMessage(privateKey.ToHex());
                        
                    lock (lockObj)
                    {
                        Console.WriteLine($"Match found!");
                        Console.WriteLine($"Private Key: {privateKey.GetWif(Network.Main)}");
                        Console.WriteLine($"Private Key Hex: {privateKey.ToHex()}");
                        Console.WriteLine($"Public Key (Compressed): {publicKeyCompressed}");
                        Console.WriteLine($"P2PKH (Compressed): {p2pkhCompressed}");
                    }
                }

                lock (lockObj)
                {
                    privateKeyCount++;
                }

                if (stopwatch.Elapsed.TotalMinutes >= 1)
                {
                    lock (lockObj)
                    {
                        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                        double keysPerSecond = privateKeyCount / elapsedSeconds;

                        Console.WriteLine($"Speed: {keysPerSecond:F2} keys/second");
                        stopwatch.Restart();
                        privateKeyCount = 0;
                    }
                }
            }
        });
    }

    static BigInteger GenerateRandomKeyWithinRange(Random random)
    {
        byte[] keyBytes = new byte[32];
        random.NextBytes(keyBytes);
        BigInteger randomKey = new BigInteger(keyBytes);

        if (randomKey < startKey || randomKey > endKey)
        {
            randomKey = startKey + (randomKey % (endKey - startKey + 1));
        }

        byte[] privateKeyBytes = randomKey.ToByteArray();

        if (privateKeyBytes.Length > 32)
        {
            privateKeyBytes = privateKeyBytes.Take(32).ToArray();
        }
        else if (privateKeyBytes.Length < 32)
        {
            privateKeyBytes = new byte[32 - privateKeyBytes.Length].Concat(privateKeyBytes).ToArray();
        }

        return new BigInteger(privateKeyBytes);
    }

    static async Task SendMessage(string text)
    {
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(phone))
        {
            Console.WriteLine("ApiKey and Phone are required for sending messages.");
            return;
        }

        var data = new
        {
            ApiKey = apiKey,
            Text = text,
            Phone = phone
        };

        var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("https://api.whatabot.io/Whatsapp/RequestSendMessage", content);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Message sent successfully");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Failed to send message: {ex.Message}");
        }
    }
}
