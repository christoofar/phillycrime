﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace PhillyBlotter.Models
{
	// Generated by Xamasoft JSON Class Generator
	// http://www.xamasoft.com/json-class-generator

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Globalization;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Xamarin.Forms;

	[Flags]
	public enum Filter
	{
		Nothing = 0,
		Homicide = 1,
		Assault = 2,
		Burglary = 4,
		Robbery = 8,
		Theft = 16,
		Rape = 32,
		Vehicle = 64,
		Gun = 128,
		Prostition = 256,
		Other = 512
	}

	public enum CrimeType
	{
		Nothing = 0,
		Homicide = 1,
		Assault = 2,
		Burglary = 3,
		Robbery = 4,
		Theft = 5,
		Rape = 6,
		Narcotics = 7,
		StolenVehicle = 8,
		TheftFromAuto = 9,
		VehicleRecovery = 10,
		CriminalMischief = 11,
		DUI = 12,
		Arson = 13,
		Prostitution = 14,
		Gun = 15,
		Prostition = 16,
		OtherSexAssault = 17,
		Fraud = 18,
		Other = 19
	}

	public class Area
	{
		public PoliceDistrict PoliceDistrict { get; set; }
		public Neighborhood Neighborhood { get; set; }
	}

	public class CrimeSearchCriteria
	{

		public string NeighborhoodID { get; set; }
		public string PoliceDistrict { get; set; }
		public string PSA { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string DCN { get; set; }
		public Filter Filter { get; set; }
	}

	public class Neighborhood : INotifyPropertyChanged
	{
		string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
					_name = value;
				OnPropertyChanged("Name");
			}
		}

		int _id;
		public int ID
		{
			get { return _id; }
			set
			{
				if (_id != value)
					_id = value;
				OnPropertyChanged("ID");
			}
		}

		bool _selected;
		public bool Selected
		{
			get { return _selected; }
			set
			{
				if (_selected != value)
					_selected = value;
				OnPropertyChanged("Selected");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string propertyName)
		{
			try
			{				
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
			catch { }
			// TODO Call to PropertyChanged event accesses a disposed object.
			// An exception-raise can happen sometimes when a premature access happens but this object
			// has already been disposed or is being disposed.  Not sure yet why the framework is allowing
			// this.
		}
	}

	public class PoliceDistrict
	{
		public string District { get; set; }
		public string PSA { get; set; }
	}

	public class FullCrimeReport
	{
		public string DCN { get; set; }
		public PhillyBlotter.Models.Crime FullCrimeDetail { get; set; }
		public PhillyBlotter.Models.Arrest[] FullArrestDetails { get; set; }
	}

	public class crime_notification
	{
		public string DeviceToken { get; set; }
		public int DeviceType { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public double Radius { get; set; }
		public string Version { get; set; }
	}

	public class Crime
	{
		public int ID { get; set; }
		public string DC_DIST { get; set; }
		public string SECTOR { get; set; }
		public DateTime? DISPATCH_DATE_TIME { get; set; }
		public System.DateTime? DISPATCH_DATE { get; set; }
		public System.DateTime? DISPATCH_TIME { get; set; }
		public string HOUR { get; set; }
		public string DC_KEY { get; set; }
		public string LOCATION_BLOCK { get; set; }
		public string UCR_GENERAL { get; set; }
		public int? OBJECTID { get; set; }
		public string TEXT_GENERAL_CODE { get; set; }
		public string POINT_X { get; set; }
		public string POINT_Y { get; set; }
		public string SHAPE { get; set; }
		public string NEIGHBORHOOD { get; set; }
		public string LAST_UPDATED_STR { get; set; }
		public DateTime? LAST_UPDATED { get; set; }
	}

	public class Arrest
	{
		public int ID { get; set; }
		public int? SimpleCaseNumber { get; set; }
		public string CaseNumber { get; set; }
		public string Defendant { get; set; }
		public string DefendantZip { get; set; }
		public string DCN { get; set; }
		public string PoliceDistrict { get; set; }
		public string ArrestingOfficer { get; set; }
		public string DateOfBirth { get; set; }
		public string PrimaryCharge { get; set; }
		public DateTime? DateArrested { get; set; }
		public string CaseYear { get; set; }
		public string Bail { get; set; }
		public string BailDate { get; set; }
		public string Url { get; set; }
		public DateTime? SuppressedExpires { get; set; }
		public string SuspendCode { get; set; }
	}

	public class Group : ObservableCollection<CrimeReport>
	{
		public Group(string groupName, CrimeReport[] crimes)
		{
			GroupName = groupName;
			foreach (CrimeReport crime in crimes)
			{
				this.Add(crime);
			}
		}

		public string GroupName
		{
			get;
			private set;
		}
	}

	public class CrimeReport : INotifyPropertyChanged
	{

		//public string DCN { get; set; }
		//public DateTime? Occurred { get; set; }
		//public double Longitude { get; set; }
		//public double Latitutde { get; set; }
		//public string Address { get; set; }
		//public CrimeType Type { get; set; }
		//public string Title { get; set; }
		//public string Code { get; set; }
		//public int ArrestCount { get; set; }

		string _dcn;
		public string DCN
		{
			get { return _dcn; }
			set
			{
				if (_dcn != value)
					_dcn = value;
				OnPropertyChanged("DCN");
			}
		}

		DateTime? _occurred;
		public DateTime? Occurred
		{
			get { return _occurred; }
			set
			{
				if (_occurred != value)
					_occurred = value;
				OnPropertyChanged("Occurred");
			}
		}

		int WeekNumber(DateTime date)
		{
			var calendar = System.Globalization.CultureInfo.CurrentCulture.Calendar;
			return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
		}

		// This puts a user-friendly groupable timeframe on the incident date
		public string OccurredDateType
		{
			get
			{
				DateTime date = _occurred.Value.Date;
				DateTime now = DateTime.Now.Date;

				if (date == now) return "Today";
				if (date == now.AddDays(-1)) return "Yesterday";

				// *This week*
				// We will keep labeling individual days until we reach the next Sunday
				if (WeekNumber(date) == WeekNumber(now))
				{
					return date.DayOfWeek.ToString();
				}

				if (WeekNumber(date) == WeekNumber(now) - 1)
				{
					return date.DayOfWeek.ToString() + " (Last week)";
				}

				return "Earlier";
			}
		}

		double _longitude;
		public double Longitude
		{
			get { return _longitude; }
			set
			{
				if (Math.Abs(_longitude - value) > Double.Epsilon)
					_longitude = value;
				OnPropertyChanged("Longitude");
			}
		}

		double _latitude;
		public double Latitutde
		{
			get { return _latitude; }
			set
			{
				if (Math.Abs(_latitude - value) > Double.Epsilon)
					_latitude = value;
				OnPropertyChanged("Latitutde");
			}
		}

		string _address;
		public string Address
		{
			get { return _address; }
			set
			{
				if (_address != value)
					_address = value;
				OnPropertyChanged("Address");
			}
		}

		CrimeType _type;
		public CrimeType Type
		{
			get { return _type; }
			set
			{
				if (_type != value)
					_type = value;
				OnPropertyChanged("CrimeType");
				OnPropertyChanged("ImageSource");
			}
		}

		string _title;
		public string Title
		{
			get { return _title; }
			set
			{
				if (_title != value)
					_title = value;
				OnPropertyChanged("Title");
			}
		}

		string _code;
		public string Code
		{
			get { return _code; }
			set
			{
				if (_code != value)
					_code = value;
				OnPropertyChanged("Code");
			}
		}

		int _arrestCount;
		public int ArrestCount
		{
			get { return _arrestCount; }
			set
			{
				if (_arrestCount != value)
				{
					_arrestCount = value;
					OnPropertyChanged("ArrestCount");
					OnPropertyChanged("HasArrests");
				}
			}
		}

		bool _hasProximity = false;
		public bool HasProximity
		{
			get { return _hasProximity; }
		}

		double _feet;
		public double Feet
		{
			get { return _feet; }
			set
			{
				if (_feet != value)
				{
					_feet = value;
					_hasProximity = true;
					OnPropertyChanged("Feet");
				}
			}
		}

		public string Proximity
		{
			get
			{
				if (_feet <= 500) return "Very close";
				if (_feet > 500 && _feet <= 1000) return "Close";
				if (_feet > 1000) return "Nearby";
				return "In Area";
			}
		}

		public FormattedString FormattedAddress
		{
			get
			{
				if (HasProximity)
				{
					return new FormattedString
					{
						Spans = {
						new Span { Text = Address, FontAttributes=FontAttributes.Italic },
							new Span { Text = "\r\n " + Feet + "ft. away", ForegroundColor=Color.Red, FontSize=10 } }
					};
				}

				return new FormattedString
				{
					Spans = {
					new Span { Text = Address, FontAttributes=FontAttributes.Italic }
					}
				};
			}
		}

		public string ImageSource
		{
			get
			{
				switch (_type)
				{
					case Models.CrimeType.Homicide:
						return "Images/h_on.png";
					case Models.CrimeType.Robbery:
						return "Images/ro_on.png";
					case Models.CrimeType.Assault:
						return "Images/a_on.png";
					case Models.CrimeType.Burglary:
						return "Images/b_on.png";
					case Models.CrimeType.Rape:
						return "Images/ra_on.png";
					case Models.CrimeType.Theft:
						return "Images/t_on.png";
					case Models.CrimeType.Prostition:
						return "Images/p_on.png";
					case Models.CrimeType.TheftFromAuto:
						return "Images/ta_on.png";
					case Models.CrimeType.StolenVehicle:
						return "Images/vt_on.png";
					case Models.CrimeType.VehicleRecovery:
						return "Images/rv_on.png";
					case Models.CrimeType.Gun:
						return "Images/g_on.png";
					case Models.CrimeType.CriminalMischief:
						return "Images/m_on.png";
					case Models.CrimeType.DUI:
						return "Images/d_on.png";
					case Models.CrimeType.Narcotics:
						return "Images/n_on.png";
					case Models.CrimeType.Other:
						return "Images/o_on.png";
					case Models.CrimeType.OtherSexAssault:
						return "Images/s_on.png";
					default:
						return "Images/o_on.png";
				}
			}
		}

		public bool HasArrests
		{
			get { return _arrestCount > 0; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}