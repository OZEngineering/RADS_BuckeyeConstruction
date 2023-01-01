using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

using NLog;
using NLog.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System.Configuration;

namespace EventImportTask
{
	class Program
	{
		//private static string ORG_ID = "buckeyeaz.com";
		//private static string CENTER_ID = "N/A";
		private static IConfiguration _Configuration;

		private static string ProcessLayer(int layerNumber, string gisEventsUrl, MyOptions opts)
		{

			string resultStr = "";
			string template = File.ReadAllText(@"BuckeyeFEUTemplate.txt");
			string responseString = "";

			try
			{
				responseString = GISEvents.GetEventsAsJson(gisEventsUrl, opts.BaseAddress);
			}
			catch (Exception ex)
			{

				// Send notification for failure to get GIS data
				//EmailNotifications notify = new EmailNotifications();
				//string body = String.Format("Error when querying GIS Layer{0}", layerNumber);
				//notify.SendNotifications(body, "Y", ORG_ID, CENTER_ID, EmailNotifications.GIS_QUERY_FAILED, "GIS Layer Query failed");
				//logger.Error("{0}", body);
				return resultStr;

			}

			if (layerNumber == 1) {
				resultStr = Layer.ProcessSegments(responseString, template);
			
			}
			else {
				resultStr = Layer.ProcessPoints(responseString, template);
			}
			return resultStr;
		}

		private static string RetrieveData(MyOptions opts)
		{

			StringBuilder responseString = new StringBuilder("<fEUMsg>");
			responseString = responseString.Append(System.Environment.NewLine);

			
			int layerNumber = 0;
			string pointStr = ProcessLayer(layerNumber, opts.PointsLayer, opts);
			responseString = responseString.Append(pointStr);
			
			layerNumber = 1;
			string segmentStr = ProcessLayer(layerNumber, opts.LinesLayer, opts);
			responseString = responseString.Append(segmentStr);

			responseString = responseString.Append("</fEUMsg>");
			return responseString.ToString();
		}

		private static void WriteXmlToFile(string xmlString, string docPath) {


			// Write the string array to a new file.
			using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath)))
			{
					outputFile.WriteLine(xmlString);
			}
		}
		
		
		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			_Configuration = new ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.Build();

			serviceCollection.AddLogging(loggingBuilder =>
			{
				loggingBuilder.ClearProviders();
				loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
				loggingBuilder.AddNLog(_Configuration);
			});

			return;

		}


		static  void Main(string[] args)
		{
			var logger = LogManager.GetCurrentClassLogger();

			try
			{
				var serviceCollection = new ServiceCollection();
				ConfigureServices(serviceCollection);

				MyOptions opts = new MyOptions();
				IConfigurationSection mySettingsSection = _Configuration.GetSection("MySettings");
				opts.LinesLayer = mySettingsSection.GetSection("LinesLayer").Value;
                opts.PointsLayer = mySettingsSection.GetSection("PointsLayer").Value;
                opts.BaseAddress = mySettingsSection.GetSection("BaseAddress").Value;
				string filePath = mySettingsSection.GetSection("XmlFilePathAndName").Value;
				opts.XmlFilePathAndName = filePath;
				string xmlString = RetrieveData(opts);

				WriteXmlToFile(xmlString, filePath);
			}
			catch (Exception ex)
			{
				// NLog: catch any exception and log it.
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
				LogManager.Shutdown();
			}
		}
	}
}
