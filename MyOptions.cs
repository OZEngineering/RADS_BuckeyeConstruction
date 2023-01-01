using System;
using System.Collections.Generic;
using System.Text;

namespace EventImportTask
{
	public class MyOptions
	{

	   public string BaseAddress {
			get;
			set;
	   }
		public string LinesLayer
		{
			get;
			set;
		}
		
		public string PointsLayer
		{
			get;
			set;
		}

		public string XmlFilePathAndName
		{
			get;
			set;
		}

		public string Debug 
		{
			get;
			set;
		}
	}
}
