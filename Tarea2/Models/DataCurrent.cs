namespace Tarea2.Models
{
	public class DataCurrent
	{
		public DateTime Time { get; set; }
		public bool IsObservedData { get; set; }
		public string? MetarID { get; set; }
		public bool IsDaylight { get; set; }
		public double WindSpeed { get; set; }
		public double ZenithAngle { get; set; }
		public int PictocodeDetailed { get; set; }
		public int Pictocode { get; set; }
		public double Temperature { get; set; }


		public DataCurrent(DateTime time, bool isObservedData, string? metarID, bool isDaylight,
			double windSpeed, double zenithAngle, int pictocodeDetailed, int pictocode, double temperature)
		{
			Time = time;
			IsObservedData = isObservedData;
			MetarID = metarID;
			IsDaylight = isDaylight;
			WindSpeed = windSpeed;
			ZenithAngle = zenithAngle;
			PictocodeDetailed = pictocodeDetailed;
			Pictocode = pictocode;
			Temperature = temperature;
		}
	}
}
