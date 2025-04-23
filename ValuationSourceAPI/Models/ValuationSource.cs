namespace ValuationSourceAPI.Models
{
    public class ValuationSource
    {
        public int ValuationSourceID { get; set; }
        public string? ValuationSourceName { get; set; }
        public int? Sort { get; set; }
        public string? SourceContactName { get; set; }
        public string? SourceAddress1 { get; set; }
        public string? SourceAddress2 { get; set; }
        public string? SourceCity { get; set; }
        public string? SourceState { get; set; }
        public string? SourceZip { get; set; }
        public string? SourceContactMethod1 { get; set; }
        public string? SourceContactMethod2 { get; set; }
        public string? SourceContactMethod3 { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string? GeoLocation { get; set; }
    }
}
