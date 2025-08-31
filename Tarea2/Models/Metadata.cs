namespace Tarea2.Models
{
	public class Metadata
	{
		public DateTime ModelrunUpdateTimeUtc { get; set; }
		public string? LocationName { get; set; } // The name of the place, e.g. country name
		public int Altitude { get; set; } // Altitude from the sea level
		public string? Timezone { get; set; }
		public double Latitude { get; set; }
		public DateTime ModelrunUtc { get; set; }
		public double Longitude { get; set; }
		public int UtcTimeOffset { get; set; }
		public double GenerationTimeInMs { get; set; }


		public Metadata(DateTime modelrunUpdateTimeUtc, string? locationName, int altitude, string? timezone, 
			double latitude, DateTime modelrunUtc, double longitude, int utcTimeOffset, double generationTimeInMs)
		{
			ModelrunUpdateTimeUtc = modelrunUpdateTimeUtc;
			LocationName = locationName;
			Altitude = altitude;
			Timezone = timezone;
			Latitude = latitude;
			ModelrunUtc = modelrunUtc;
			Longitude = longitude;
			UtcTimeOffset = utcTimeOffset;
			GenerationTimeInMs = generationTimeInMs;
		}
	}
}
