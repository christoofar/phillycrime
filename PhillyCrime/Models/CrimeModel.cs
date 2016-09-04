﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PhillyCrime
{

	// Generated by Xamasoft JSON Class Generator
	// http://www.xamasoft.com/json-class-generator

	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	namespace PhillyCrime
	{

		public class Geometry
		{

			[JsonProperty("CoordinateSystemId")]
			public int CoordinateSystemId { get; set; }

			[JsonProperty("WellKnownText")]
			public string WellKnownText { get; set; }
		}

		public class POINT
		{

			[JsonProperty("Geometry")]
			public Geometry Geometry { get; set; }
		}

		public class CrimeReport
		{

			[JsonProperty("ID")]
			public int ID { get; set; }

			[JsonProperty("DC_DIST")]
			public string DCDIST { get; set; }

			[JsonProperty("SECTOR")]
			public string SECTOR { get; set; }

			[JsonProperty("DISPATCH_DATE_TIME")]
			public string DISPATCHDATETIME { get; set; }

			[JsonProperty("DISPATCH_DATE")]
			public string DISPATCHDATE { get; set; }

			[JsonProperty("DISPATCH_TIME")]
			public string DISPATCHTIME { get; set; }

			[JsonProperty("HOUR")]
			public string HOUR { get; set; }

			[JsonProperty("DC_KEY")]
			public string DCKEY { get; set; }

			[JsonProperty("LOCATION_BLOCK")]
			public string LOCATIONBLOCK { get; set; }

			[JsonProperty("UCR_GENERAL")]
			public string UCRGENERAL { get; set; }

			[JsonProperty("OBJECTID")]
			public object OBJECTID { get; set; }

			[JsonProperty("TEXT_GENERAL_CODE")]
			public string TEXTGENERALCODE { get; set; }

			[JsonProperty("POINT_X")]
			public string POINTX { get; set; }

			[JsonProperty("POINT_Y")]
			public string POINTY { get; set; }

			[JsonProperty("SHAPE")]
			public string SHAPE { get; set; }

			[JsonProperty("POINT")]
			public POINT POINT { get; set; }

			[JsonProperty("NEIGHBORHOOD")]
			public string NEIGHBORHOOD { get; set; }

			[JsonProperty("LAST_UPDATED_STR")]
			public string LASTUPDATEDSTR { get; set; }

			[JsonProperty("LAST_UPDATED")]
			public string LASTUPDATED { get; set; }
		}

	}


}