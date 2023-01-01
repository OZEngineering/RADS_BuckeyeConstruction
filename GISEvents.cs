using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;


using NLog;

namespace EventImportTask
{
	public class GISEvents
	{
		private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public static string GetEventsAsJson(string getUrl, string baseAddr)
		{
			logger.Debug("Sending request to get all GIS events for Url: ", getUrl);
			Task<string> getTask = GetAllEvents(getUrl, baseAddr);
			getTask.Wait();
			return getTask.Result;
		}

		private static async Task<string> GetAllEvents(String getUrl, String baseAddr)
		{
			// request zone data
			//logger.Info("In retriever data");


			String orgId = "BuckeyeAZ.gov";
			HttpResponseMessage response = null;

			//HttpWebRequest request = WebRequest.Create(zones_requestUri) as HttpWebRequest;
			//logger.Info("Calling webrequest to get  hcrs events");
			using (var client = new HttpClient())
			{

				client.BaseAddress = new Uri(baseAddr);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string val = "";

				try
				{

					logger.Debug("Getting Buckeye events ");
					response =
						await client.GetAsync(getUrl).ConfigureAwait(false);

					if (response.IsSuccessStatusCode)
					{
						logger.Info("GetAllEvents - was able to retrieve GIS events");
						val = await response.Content.ReadAsStringAsync();
					}
					else
					{
						logger.Debug("GetAllEvents - publish request response = {0}", response.StatusCode);
					}
				}
				catch (Exception e)
				{
					logger.Error(e);
				}
				return val;
			}
		}

	}
}
