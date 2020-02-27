using System;

namespace x12_837parser
{
    class BillingCodes
    {
        public int EncounterID { get; set; }
        public int AccountNumber { get; set; }
        public string Coder { get; set; }
        public string Final_DRG { get; set; }
        public string MRN { get; set; }
        public DateTime? Discharge_Date { get; set; }
        public float CDRG_Weight { get; set; }
        public string SOI { get; set; }
        public string ROM { get; set; }
        public int Primary_Insurance_ID { get; set; }
        public string Agnostic_DRG_Code { get; set; }
        public string Agnostic_Weight { get; set; }
        public string Agnostic_SOI { get; set; }
        public string Agnostic_ROM { get; set; }
        public string Principal_CM_Code { get; set; }
        public string CM_Codes { get; set; }
        public string CM_Description { get; set; }
        public string POA { get; set; }
        public string Coding_Impact_Designations { get; set; }
        public string Principal_PCS_Code { get; set; }
        public string PCS_Codes { get; set; }
        public string PCS_Descriptions { get; set; }
        public string PCS_Code_Flag { get; set; }
        public string PCS_Date { get; set; }
        public string Surgeon { get; set; }
        public float Coding_AMLOS { get; set; }
        public float Coding_GMLOS { get; set; }
        public int Facility_ID { get; set; }
        public DateTime? Admit_Date { get; set; }
        public string Visit_Type { get; set; }
        public DateTime? DOB { get; set; }
        public string Sex { get; set; }
        public string Diagnosis_Type { get; set; }
        public string Diagnosis_Priority { get; set; }
        public string DRG_Type { get; set; }
        public string Payer { get; set; }
        public string Financial_Class { get; set; }
        public string Discharge_Disposition { get; set; }
        public int institutionID { get; set; }
    }
}
