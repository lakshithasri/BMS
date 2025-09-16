namespace KlarityLive.Core.DTOs.ExcelUpload
{
    public class ExcelImportRowDto
    {
        public string Category { get; set; }
        public string Tenancy { get; set; }
        public string TenantOwner { get; set; }
        public string Lease { get; set; }
        public string MeterName { get; set; }
        public string Register { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Days { get; set; }
        public decimal PrevReading { get; set; }
        public decimal PresReading { get; set; }
        public decimal? Advance { get; set; }
        public decimal Multiplier { get; set; }
        public decimal Quantity { get; set; }
        public string MeasurementUnit { get; set; }
        public string Property { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string Note { get; set; }
        public string UtilityBillUnit { get; set; }
    }
}
