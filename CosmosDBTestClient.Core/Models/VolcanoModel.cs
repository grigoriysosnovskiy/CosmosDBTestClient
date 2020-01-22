using System;
namespace CosmosDBTestClient.Core.Models
{
    public class VolcanoModel
    {
        #region Contructors
        public VolcanoModel() { }

        public VolcanoModel(string folderId)
        {
            FolderId = folderId;

            Console.WriteLine("Randomize coordinates... ");
            Coordinates = new double[3] { Math.Round(new Random().NextDouble() * (180 - 1) + 1, 2), Math.Round(new Random().NextDouble() * (180 - 1) + 1, 2), Math.Round(new Random().NextDouble() * (180 - 1) + 1, 2) };
            System.Threading.Thread.Sleep(500);

            Console.Write("Enter name -> ");
            Name = Console.ReadLine();

            Console.Write("Enter country -> ");
            Country = Console.ReadLine();

            Console.Write("Enter region -> ");
            Region = Console.ReadLine();

            Console.Write("Enter elevation -> ");
            Elevation = Math.Round(Convert.ToDouble(Console.ReadLine()), 2);

            Console.Write("Enter status -> ");
            Status = Console.ReadLine();

            Console.Write("Enter last eruption -> ");
            LastEruption = Convert.ToInt16(Console.ReadLine());
        }
        #endregion

        #region Properties
        public string FolderId { get; set; }
        public double[] Coordinates { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public double Elevation { get; set; }
        public string Status { get; set; }
        public short LastEruption { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Coordinates: {string.Join(", ", Coordinates)}\nName: {Name}\nCountry: {Country}\nRegion: {Region}\nElevation: {Elevation}\nStatus: {Status}\nLastEruption: {LastEruption}\n";
        }
        #endregion
    }
}
