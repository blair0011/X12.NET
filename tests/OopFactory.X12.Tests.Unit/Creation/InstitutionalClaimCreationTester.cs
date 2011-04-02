﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using OopFactory.X12;
using System.Reflection;
using System.IO;
using OopFactory.X12.Parsing;
using OopFactory.X12.Parsing.Model;
using OopFactory.X12.Parsing.Specification;

namespace OopFactory.X12.Tests.Unit.Creation
{
    [TestClass]
    public class InstitutionalClaimCreationTester
    {
        private const string InterchangeSample1 =
@"ISA*00*          *00*          *01*9012345720000  *01*9088877320000  *020816*1144*U*00401*000000031*1*T*:~
IEA*0*000000031~";

        [TestMethod]
        public void SerializeSegmentSet()
        {
            SegmentSet set = new SegmentSet { Name = "4010" };
            SegmentSpecification isa = new SegmentSpecification { SegmentId = "ISA" };
            set.Segments.Add(isa);
            isa.Elements.Add(new ElementSpecification
            {
                Name = "Author Info Qualifier",
                Required = true,
                MinLength = 2,
                MaxLength = 2,
                Type = ElementDataTypeEnum.Identifier
            });
            string xml = set.Serialize();
            Trace.Write(xml);
            SegmentSet copy = SegmentSet.Deserialize(xml);
            SegmentSpecification isaCopy = copy.Segments.FirstOrDefault(s => s.SegmentId == "ISA");
            Assert.IsNotNull(isaCopy);
            Assert.AreEqual("ISA", isaCopy.SegmentId);
            Assert.AreEqual("Author Info Qualifier", isaCopy.Elements[0].Name);
            Assert.AreEqual(2, isaCopy.Elements[0].MinLength);
                
        }

        [TestMethod]
        public void InterchangeCreationTest()
        {
            DateTime date = DateTime.Parse("2002-08-16 11:44AM");
            Interchange interchange = new Interchange(date, 31, false);
            interchange.InterchangeSenderId = "9012345720000";
            interchange.InterchangeReceiverId = "9088877320000";
            
            string actualX12 = interchange.SerializeToX12(true);
            Assert.AreEqual(InterchangeSample1, actualX12);
            Assert.AreEqual("00", interchange.AuthorInfoQualifier);
            Assert.AreEqual("00", interchange.SecurityInfoQualifier);
            Assert.AreEqual("01", interchange.InterchangeSenderIdQualifier);
            Assert.AreEqual("01", interchange.InterchangeReceiverIdQualifier);
            Assert.AreEqual(date, interchange.InterchangeDate);
        }

        [TestMethod]
        public void InterchangeSenderIdQualifierValidationTest()
        {
            try
            {
                DateTime date = DateTime.Parse("2002-08-16 11:44AM");
                Interchange interchange = new Interchange(date, 31, false);
                interchange.InterchangeSenderIdQualifier = "ER";
                Assert.Fail("An ElementValidationException was expected.");
            }
            catch (ElementValidationException exc) {
                if (exc.ElementId != "ISA05")
                    Assert.Fail(string.Format("Exception expected on ISA05, but got exception on {0} instead.", exc.ElementId));
            }
        }
    }
}
