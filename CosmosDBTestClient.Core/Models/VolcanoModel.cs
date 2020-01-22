namespace CosmosDBTestClient.Core.Models
{
    public class VolcanoModel
    {
        public string FolderId { get; set; }
        public double[] Coordinates { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public double Elevation { get; set; }
        public string Status { get; set; }
        public short LastEruption { get; set; }

        public override string ToString()
        {
            return $"Coordinates: {string.Join(", ", Coordinates)}\nName: {Name}\nCountry: {Country}\nRegion: {Region}\nElevation: {Elevation}\nStatus: {Status}\nLastEruption: {LastEruption}\n";
        }
    }
}
