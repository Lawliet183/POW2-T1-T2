namespace Tarea2.Models
{
	public class Units
	{
		public string? Temperature { get; set; }
		public string? Time { get; set; }
		public string? WindSpeed { get; set; }


		public Units(string temperature, string time, string windSpeed)
		{
			Temperature = temperature;
			Time = time;
			WindSpeed = windSpeed;
		}
	}
}
