using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using X12.Hipaa.Claims;
using X12.Hipaa.Claims.Services;
using X12.Parsing;

namespace x12_837parser
{
    class Program
    {
        static void Main(string[] args)
        => CommandLineApplication.Execute<Program>(args);

        [Option(Description = "Input Directory", ShortName = "d")]
        public string Dir { get; }

        [Option(Description ="Institution ID",ShortName ="i")]
        public int InstitutionID { get; }

        private void OnExecute() { 
            var x12 = new X12Parser(false);
            var service = new ClaimTransformationService(x12);

            foreach (var filename in Directory.GetFiles(Dir))
            {                
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    var document = service.Transform837ToClaimDocument(fs);
                    processClaims(InstitutionID, document.Claims);
                }
            }
        }

        private static void processClaims(int institutionId, List<Claim> claims)
        {
            List<BillingCodes> codes = new List<BillingCodes>();
            foreach (var item in claims)
            {
                var code = new BillingCodes();
                //code.EncounterID = item.;
                //code.AccountNumber = item.;
                code.Coder = item.Submitter.Name.ToString();
                code.Final_DRG = item.DiagnosisRelatedGroup.Code;
                code.MRN = item.MedicalRecordNumber;                
                code.Discharge_Date = item.DischargeTime;                
                code.Primary_Insurance_ID = item.Payer.Identifications[0].Id[0];

                //============================================================
                //NOT VALID FOR DRG CLIENTS 
                //code.CDRG_Weight = ;
                //code.SOI =;
                //code.ROM =;
                //code.Agnostic_DRG_Code =;
                //code.Agnostic_Weight =;
                //code.Agnostic_SOI =;
                //code.Agnostic_ROM =;      
                //code.CM_Description =;
                //code.Coding_Impact_Designations =;
                //code.PCS_Descriptions =;
                //code.PCS_Code_Flag =;
                //code.Coding_AMLOS =;
                //code.Coding_GMLOS =;
                //============================================================

                var temp = item.Diagnoses.Where(c => c.DiagnosisType == X12.Hipaa.Enums.DiagnosisType.Principal).First();
                code.Principal_CM_Code = temp.Code;
                code.CM_Codes = string.Join("~", item.Diagnoses.Select(d => d.Code));
                
                code.POA = string.Join("~", item.Diagnoses.Select(d => d.PoiIndicator));                
                code.Principal_PCS_Code = item.Procedures.Where(p => p.IsPrincipal).First().Code;
                code.PCS_Codes = string.Join("~", item.Procedures.Select(p=>p.Code));
                
                code.PCS_Date = string.Join("~", item.Procedures.Select(p => p.Date.ToShortDateString()));
                code.Surgeon = string.Join("~", item.Providers.Select(p => p.Name));
               
                code.Facility_ID = item.ServiceLocationInfo.FacilityCode[0];
                code.Admit_Date = item.AdmissionDate;
                code.Visit_Type = item.AdmissionType.Code;
                code.DOB = item.Patient.DateOfBirth;
                code.Sex = item.Patient.Gender.ToString();
                code.Diagnosis_Type = item.Diagnoses[0].Qualifier;
                code.Diagnosis_Priority = "0";
                code.DRG_Type = item.Diagnoses[0].DiagnosisType.ToString();
                code.Payer = item.Payer.Name.ToString();
                code.Financial_Class = item.Payer.Identifications[0].Id;
                code.Discharge_Disposition = item.PatientStatus.Code;
                code.institutionID = institutionId;
                codes.Add(code);
            }

            LoadSqlServer(codes);
        }

        private static void LoadSqlServer(List<BillingCodes> codes)
        {
            throw new NotImplementedException();
        }
    }
}