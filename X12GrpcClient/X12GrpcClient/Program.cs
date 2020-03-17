using Grpc.Net.Client;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using X12GrpcService.Protos;
using static X12GrpcService.Protos.X12GrpcService;

namespace X12GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string xml;
            Encoding e = Utilities.DetectTextEncoding(args[0],
                out xml);
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new X12GrpcServiceClient(channel);
            var reply = await client.X12ParserAsync(new X12Request { Xmi=xml });
            Console.WriteLine("XML: " + reply.Xml);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reply.Xml);
            Console.WriteLine("JSON: "+ JsonConvert.SerializeXmlNode(doc));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
