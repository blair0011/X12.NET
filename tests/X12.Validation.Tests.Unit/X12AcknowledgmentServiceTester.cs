﻿namespace X12.Validation.Tests.Unit
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Xunit;

    using X12.Shared.Models;

    public class X12AcknowledgmentServiceTester
    {
        [Fact]
        public void Acknowledge837ITest()
        {
            var service = new InstitutionalClaimAcknowledgmentService();
            var responses = service.AcknowledgeTransactions(this.GetEdi("837I_4010_Batch1.txt"));

            Assert.Single(responses);
            var response = responses.First();
            Assert.Equal("612200041", response.GroupControlNumber);
            Assert.Equal(54, response.TransactionSetResponses.Count);

            var interchange = new Interchange(DateTime.Now, 1, true);
            var group = interchange.AddFunctionGroup("FA", DateTime.Now, 1);
            group.VersionIdentifierCode = "005010X231A1";
            group.Add999Transaction(responses);
        }

        private Stream GetEdi(string filename)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("X12.Validation.Tests.Unit.Data." + filename);
        }
    }
}
