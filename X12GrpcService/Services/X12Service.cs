using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X12.Parsing;
using X12.Shared.Models;
using proto = X12GrpcService.Protos;

namespace X12GrpcService.Services
{
    public class X12Service : proto.X12GrpcService.X12GrpcServiceBase
    {
        private readonly ILogger<X12Service> _logger;

        public X12Service(ILogger<X12Service> logger)
        {
            _logger = logger;
        }

        public override Task<proto.X12Response> X12Parser(proto.X12Request request, ServerCallContext context)
        {
            string xml = null;
            var parser = new X12Parser(false);
            IList<Interchange> interchanges = parser.ParseMultiple(request.Xmi);
            var ms = new MemoryStream();

            foreach (var item in interchanges)
            {
                item.Serialize(ms);
            }
            ms.Position = 0;
            using (StreamReader aReader = new StreamReader(ms))
            {
                xml = aReader.ReadToEnd();
            }

            return Task.FromResult(new proto.X12Response
            {
                Xml = xml
            });
        }
    }
}
