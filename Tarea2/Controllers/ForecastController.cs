using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using Tarea2.Models;

namespace Tarea2.Controllers
{
	public class ForecastController : Controller
	{
		public CurrentForecast currentForecast = null;
		public List<Cabecera> cabeceras = null;

		// Cliente http para conectarse con la API
		private readonly HttpClient _httpClient = new HttpClient();


		/* Recuperar los datos de la url y asignarlos a la lista */
		private async Task FetchData(string url)
		{
			try
			{
				string json = await _httpClient.GetStringAsync(url);

				JObject jsonObj = JObject.Parse(json);
				
				currentForecast = InitializeCurrentForecast(jsonObj);
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine("\nException Caught!");
				Console.WriteLine("Message: {0}", e.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine("\nGeneral exception...");
				Console.WriteLine("Message: {0}", e.Message);
			}
		}

		/* Firmar el llamado a la API con un secreto compartido */
		private string SignUrl(string query)
		{
			string sharedSecret = "TestSecret";
			
			using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(sharedSecret)))
			{
				byte[] data = Encoding.UTF8.GetBytes(query);
				byte[] sigBytes = hmac.ComputeHash(data);
				string sig = BitConverter.ToString(sigBytes).Replace("-", "").ToLower();
				string signedUrl = $"https://my.meteoblue.com{query}&sig={sig}";

				return signedUrl;
			}
		}

		private CurrentForecast InitializeCurrentForecast(JObject jsonObj)
		{
			// Get the metadata
			DateTime modelrunUpdateTimeUtc = jsonObj["metadata"]["modelrun_updatetime_utc"].Value<DateTime>();
			string? locationName = jsonObj["metadata"]["name"].ToString();
			int altitude = jsonObj["metadata"]["height"].Value<int>();
			string? timezone = jsonObj["metadata"]["timezone_abbrevation"].ToString();
			decimal latitude = jsonObj["metadata"]["latitude"].Value<decimal>();
			DateTime modelrunUtc = jsonObj["metadata"]["modelrun_utc"].Value<DateTime>();
			decimal longitude = jsonObj["metadata"]["longitude"].Value<decimal>();
			int utcTimeOffset = jsonObj["metadata"]["utc_timeoffset"].Value<int>();
			double generationTimeInMs = jsonObj["metadata"]["generation_time_ms"].Value<double>();

			Metadata metadata = new Metadata(modelrunUpdateTimeUtc, locationName, altitude, timezone, latitude, modelrunUtc, longitude, utcTimeOffset, generationTimeInMs);


			// Get the units
			string? unitsTemperature = jsonObj["units"]["temperature"].ToString();
			string? unitsTime = jsonObj["units"]["time"].ToString();
			string? unitsWindSpeed = jsonObj["units"]["windspeed"].ToString();

			Units units = new Units(unitsTemperature, unitsTime, unitsWindSpeed);


			// Get the current data
			DateTime currentTime = jsonObj["data_current"]["time"].Value<DateTime>();
			bool isObservedData = jsonObj["data_current"]["isobserveddata"].Value<bool>();

			JToken? aux = jsonObj["data_current"]["metarid"];
			string metarID = ( !aux.HasValues ) ? "N/A" : aux.ToString();

			bool isDaylight = jsonObj["data_current"]["isdaylight"].Value<bool>();

			aux = jsonObj["data_current"]["windspeed"];
			double windSpeed = ( !aux.HasValues ) ? 0 : aux.Value<double>();

			double zenithAngle = jsonObj["data_current"]["zenithangle"].Value<double>();
			int pictocodeDetailed = jsonObj["data_current"]["pictocode_detailed"].Value<int>();
			int pictocode = jsonObj["data_current"]["pictocode"].Value<int>();
			double currentTemperature = jsonObj["data_current"]["temperature"].Value<double>();

			DataCurrent dataCurrent = new DataCurrent(currentTime, isObservedData, metarID, isDaylight, windSpeed, zenithAngle, pictocodeDetailed, pictocode, currentTemperature);


			return new CurrentForecast(metadata, units, dataCurrent);
		}


		public ForecastController()
		{
			// Leer el archivo .json de cabeceras departamentales
			string json = System.IO.File.ReadAllText("Models/Cabeceras.json");
			cabeceras = JsonConvert.DeserializeObject<List<Cabecera>>(json);
		}

		public IActionResult Index()
		{
			return View(cabeceras);
		}

		public IActionResult Find(string cabecera)
		{
			List<Cabecera> resultados = new List<Cabecera>();

			foreach (var item in cabeceras)
			{
				if (item.Nombre.ToLower().Contains(cabecera.ToLower()))
				{
					resultados.Add(item);
				}
			}

			return View("Index", resultados);
		}

		public IActionResult Details(int ID)
		{
			foreach (var item in cabeceras)
			{
				if (item.ID == ID)
				{
					// Conseguir la longitud y latitud de la cabecera departamental
					string longitud = item.Longitud.ToString();
					string latitud = item.Latitud.ToString();

					// Firmar el url
					string query = $"/packages/current?apikey=O8uYcXZ6bWeVmNkh&lon={longitud}&lat={latitud}";
					string signedUrl = SignUrl(query);

					// Recuperar los datos y esperar a que se hayan recuperado
					Task datos = FetchData(signedUrl);
					datos.Wait();

					ViewBag.imagen = item.Imagen;
					ViewBag.nombre = item.Nombre;
					return View(currentForecast);
				}
			}

			return View(new CurrentForecast());
		}
	}
}
