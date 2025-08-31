namespace Tarea2.Models
{
	public class CurrentForecast
	{
		public Metadata Metadata { get; set; }
		public Units Units { get; set; }
		public DataCurrent DataCurrent { get; set; }


		public CurrentForecast(Metadata metadata, Units units, DataCurrent dataCurrent)
		{
			Metadata = metadata;
			Units = units;
			DataCurrent = dataCurrent;
		}

		public CurrentForecast() : this(null, null, null) { }
	}
}
