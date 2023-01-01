using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using NLog;
using Buckeye.Common.DataTransferObjects;

namespace EventImportTask
{
	public class Layer
	{

		private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string ProcessPoints(string responseString, string template)
        {
            StringBuilder resultStr = new StringBuilder();

            // We need to have converters access method set to 'public' instead of internal, and to call DeserializeObject
            // with converter method ( SegmentItem.Converter.Settings )



            DateTime now = DateTime.Now;
            try
            {
                PointItem.PointObject points = JsonConvert.DeserializeObject<PointItem.PointObject>(responseString, PointItem.Converter.Settings);
                foreach (var feature in points.Features)
                {

                    string eventId = feature.attribute.Objectid.ToString();

                    if (feature.attribute.Startdate == null) continue;
                    long startTimeEpoch = (long)(feature.attribute.Startdate);
                    DateTimeOffset startTime = FromUnixTime(startTimeEpoch / 1000);
                    if (feature.attribute.Enddate == null) continue;
                    long endTimeEpoch = (long)feature.attribute.Enddate;
                    DateTimeOffset endTime = FromUnixTime(endTimeEpoch / 1000);
                    if (endTime < now) continue;
                    DateTimeOffset createTime = new DateTimeOffset(now);
                    /*
					if (feature.Attributes != null)
					{
						createTime = FromUnixTime(feature.Attributes.CreationDate.Value / 1000);
					}
					*/
                    string xmlStr = new string(template);

                    // Fill In the Date Fields
                    xmlStr = FillInNowDate(xmlStr, now);
                    xmlStr = FillInStartDate(xmlStr, startTime);
                    xmlStr = FillInEndDate(xmlStr, endTime);
                    //xmlStr = FillInWhenAddedDate(xmlStr, createTime);

                    // Fill in the Route
                    string route = feature.attribute.Street;
                    if (route == null) continue;
                    route = ConvertXmlCodes(route);
                    xmlStr = xmlStr.Replace("ROUTE", route);

                    //Fill in the description

                    string description = ConvertXmlCodes(feature.attribute.Description);

                    xmlStr = xmlStr.Replace("DESCRIPTION", description);

                    // Fill in the Direction field
                    string direction = "";
                    xmlStr = FillInDirection(xmlStr, direction);

                    if (feature.geometry != null)
                    {
						double x = feature.geometry.X;
						double y = feature.geometry.Y;
						xmlStr = xmlStr.Replace("LONGIT", Math.Round(x * 1000000.0) + "");
                        xmlStr = xmlStr.Replace("LATIT", Math.Round(y * 1000000.0) + "");
                    }

                    // Fill in the geometry
                    //xmlStr = xmlStr.Replace("LONGIT", Math.Round((firstPoint.Longitude * 1000000), 0) + "");
                    //xmlStr = xmlStr.Replace("LATIT", Math.Round((firstPoint.Latitude * 1000000), 0) + "");

                    // Fill in Event Id
                    xmlStr = xmlStr.Replace("EVENT_ID", eventId);

                    resultStr = resultStr.Append(xmlStr);

                }
            }
            catch (Exception e)
            {
                //EmailNotifications notify = new EmailNotifications();
                //string body = String.Format("Unparsable data found in FaciltyID {0} Laver0", badId);
                //notify.SendNotifications(body, "Y", ORG_ID, CENTER_ID, EmailNotifications.GIS_DATA_INVALID, "Invalid Data in Layer0");
                //logger.Error("{0}", body);
                int isl = 0;
            }

            return resultStr.ToString();
        }

        public static string ProcessSegments(string responseString, string template)
		{
			StringBuilder resultStr = new StringBuilder();

			// We need to have converters access method set to 'public' instead of internal, and to call DeserializeObject
			// with converter method ( SegmentItem.Converter.Settings )
			
			DateTime now = DateTime.Now;
			try
			{
                LineItem.LineObject segs = JsonConvert.DeserializeObject<LineItem.LineObject>(responseString, LineItem.Converter.Settings);
                foreach (var feature in segs.Features)
				{

					string eventId = feature.Attributes.Objectid.ToString();

					if (feature.Attributes.Startdate == null) continue;
					long startTimeEpoch = (long)(feature.Attributes.Startdate);
					DateTimeOffset startTime = FromUnixTime(startTimeEpoch / 1000);
					if (feature.Attributes.Enddate == null) continue;
                    long endTimeEpoch = (long)feature.Attributes.Enddate;
					DateTimeOffset endTime = FromUnixTime(endTimeEpoch / 1000);
					if (endTime < now) continue;
					DateTimeOffset createTime = new DateTimeOffset(now);
					/*
					if (feature.Attributes != null)
					{
						createTime = FromUnixTime(feature.Attributes.CreationDate.Value / 1000);
					}
					*/
					string xmlStr = new string(template);

					// Fill In the Date Fields
					xmlStr = FillInNowDate(xmlStr, now);
					xmlStr = FillInStartDate(xmlStr, startTime);
					xmlStr = FillInEndDate(xmlStr, endTime);
					//xmlStr = FillInWhenAddedDate(xmlStr, createTime);

					// Fill in the Route
					string route = feature.Attributes.Street;
					if (route == null) continue;
                    route = ConvertXmlCodes (route);
					xmlStr = xmlStr.Replace("ROUTE", route);

					//Fill in the description

                    string description = ConvertXmlCodes(feature.Attributes.Description);

                    xmlStr = xmlStr.Replace("DESCRIPTION", description);

					// Fill in the Direction field
					string direction = "";
					xmlStr = FillInDirection(xmlStr, direction);

					List<MapPointItem> points = new List<MapPointItem>();

					if (feature.Geometry != null)
					{
						foreach (double[][] ring in feature.Geometry.Paths)
						{


							foreach (double[] pntList in ring)
							{
								MapPointItem mapItem = new MapPointItem();
								mapItem.Longitude = pntList[0];
								mapItem.Latitude = pntList[1];
								points.Add(mapItem);
							}
						}
					}

					if (points.Count() == 0) continue;
					MapPointItem firstPoint = points.FirstOrDefault();
					// Fill in the geometry
					xmlStr = xmlStr.Replace("LONGIT", Math.Round((firstPoint.Longitude * 1000000),0) + "");
					xmlStr = xmlStr.Replace("LATIT", Math.Round((firstPoint.Latitude * 1000000), 0) + "");

					// Fill in Event Id
					xmlStr = xmlStr.Replace("EVENT_ID", eventId);

					resultStr = resultStr.Append(xmlStr);

				}
			}
			catch (Exception e)
			{
				//EmailNotifications notify = new EmailNotifications();
				//string body = String.Format("Unparsable data found in FaciltyID {0} Laver0", badId);
				//notify.SendNotifications(body, "Y", ORG_ID, CENTER_ID, EmailNotifications.GIS_DATA_INVALID, "Invalid Data in Layer0");
				//logger.Error("{0}", body);
				int isl = 0;
			}
			
			return resultStr.ToString();
		}

        public static string ConvertXmlCodes(string description)
        {
			if (description != null)
			{
				description = description.Replace("&", "&#38;");
				description = description.Replace("'", "&#39;");
				description = description.Replace("\"", "&#34;");
				description = description.Replace("=", "&#61;");
				description = description.Replace("<", "&#60;");
				description = description.Replace(">", "&#62;");
			}

            return description;
        }

		public static string FillInNowDate(string template, DateTimeOffset now)
		{
			string newTemplate = template;
			DateTimeOffset localDate = now.ToLocalTime();
			string nowDate = localDate.ToString("yyyyMMdd");
			string nowTime = localDate.ToString("HHmmss");
			string nowOffset = localDate.ToString("zzz");
			nowOffset = nowOffset.Replace(":", "");

			newTemplate = newTemplate.Replace("NOW_DATE", nowDate);
			newTemplate = newTemplate.Replace("NOW_TIME", nowTime);
			newTemplate = newTemplate.Replace("NOW_OFFSET", nowOffset);

			return newTemplate;
		}

		public static string FillInStartDate(string template, DateTimeOffset startDate)
		{
			string newTemplate = template;
			DateTimeOffset localDate = startDate.ToLocalTime();
			string stDate = localDate.ToString("yyyyMMdd");
			string stTime = localDate.ToString("HHmmss");
			string stOffset = localDate.ToString("zzz");
			stOffset = stOffset.Replace(":", "");

			newTemplate = newTemplate.Replace("START_DATE", stDate);
			newTemplate = newTemplate.Replace("START_TIME", stTime);
			newTemplate = newTemplate.Replace("START_OFFSET", stOffset);

			return newTemplate;
		}

		public static string FillInEndDate(string template, DateTimeOffset endDate)
		{
			string newTemplate = template;
			DateTimeOffset localDate = endDate.ToLocalTime();
			string eDate = localDate.ToString("yyyyMMdd");
			string eTime = localDate.ToString("HHmmss");
			string eOffset = localDate.ToString("zzz");
			eOffset = eOffset.Replace(":", "");

			newTemplate = newTemplate.Replace("END_DATE", eDate);
			newTemplate = newTemplate.Replace("END_TIME", eTime);
			newTemplate = newTemplate.Replace("END_OFFSET", eOffset);

			return newTemplate;
		}

		public static string FillInWhenAddedDate(string template, DateTimeOffset endDate)
		{
			string newTemplate = template;
			DateTimeOffset localDate = endDate.ToLocalTime();
			string eDate = localDate.ToString("yyyyMMdd");
			string eTime = localDate.ToString("HHmmss");
			string eOffset = localDate.ToString("zzz");
			eOffset = eOffset.Replace(":", "");

			newTemplate = newTemplate.Replace("WHEN_ADDED_DATE", eDate);
			newTemplate = newTemplate.Replace("WHEN_ADDED_TIME", eTime);
			newTemplate = newTemplate.Replace("WHEN_ADDED_OFFSET", eOffset);

			return newTemplate;
		}

		public static string FillInDirection(string template, string direction) {
			string newTemplate = template;
			if (direction != null)
			{
				direction = direction.Replace(" ", "");
				direction = direction.ToLowerInvariant();

				switch (direction.ToLower())
				{
					case "southbound":
						{
							direction = "s";
							break;
						}

					case "northbound":
						{
							direction = "n";
							break;
						}

					case "alldirections":
						{
							direction = "all directions";
							break;
						}

					case "eastbound":
						{
							direction = "e";
							break;
						}

					case "westbound":
						{
							direction = "w";
							break;
						}

					case "eastboundwestbound":
						{
							direction = "both directions";
							break;
						}
					case "northboundsouthbound":
						{
							direction = "both directions";
							break;
						}
					case "both":
						{
							direction = "both directions";
							break;
						}
					default:
						{
							direction = "all directions";
							break;
						}
				}
			}
			else
			{
				direction = "all directions";
			}

			newTemplate = newTemplate.Replace("DIRECTION", direction); ;
			return newTemplate;
		}

		private static DateTimeOffset FromUnixTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTime);
		}


	}
}
