using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class ArrestSearchPage : ContentPage
	{

		bool _birthdayChanged = false; //This flag will prevent age bracket from clearing the birthday.

		public ArrestSearchPage()
		{
			InitializeComponent();
			Birthday.NullableDate = null;
			Bail.SelectedIndex = 0;
			AgeRange.SelectedIndex = 0;
			DateStart.Date = new DateTime(DateTime.Now.Year - 5, 1, 1);
		}

		void AgeRangeChanged(object sender, System.EventArgs e)
		{
			if (_birthdayChanged)
			{
				_birthdayChanged = false;
			}
		}

		void BailChanged(object sender, System.EventArgs e)
		{
			// No-op for now.
		}

		void StartDateChanged(object sender, Xamarin.Forms.DateChangedEventArgs e)
		{
			if (DateStart.Date < new DateTime(DateTime.Now.Year - 5, 1, 1))
			{
				DisplayAlert("Incorrect Start Date", $"There is no arrest information available before Jan 1, {DateTime.Now.Year - 5}", "OK");
			}
		}

		void Search_Clicked(object sender, System.EventArgs e)
		{

		}

		void Handle_DateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
		{
			// A date was selected.  To give a user an idea of age, we'll auto-select
			// the age bracket (even though it won't be used)
			if (Birthday.NullableDate.HasValue && Birthday.NullableDate.Value != DateTime.Today)
			{
				_birthdayChanged = true;
				int age = CalculateAge(Birthday.NullableDate.Value);

				if (age <= 21) AgeRange.SelectedIndex = 1;
				if (age > 21 && age < 30) AgeRange.SelectedIndex = 2;
				if (age >= 30 && age < 40) AgeRange.SelectedIndex = 3;
				if (age >= 40 && age <= 55) AgeRange.SelectedIndex = 4;
				if (age >= 56) AgeRange.SelectedIndex = 5;

			}
		}

		int CalculateAge(DateTime date)
		{
			var today = DateTime.Today;

			var age = today.Year - date.Year;
			if (date > today.AddYears(-age)) age--;

			return age;
		}

		void DCNChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (e.OldTextValue == null) return;

			if (e.NewTextValue.Length > e.OldTextValue.Length)
			{

				// How many dashes are there
				int newDashes = e.NewTextValue.Split('-').Length - 1;
				int oldDashes = e.OldTextValue.Split('-').Length - 1;

				// if this is just 1 increase in dashes then fuck it
				if (newDashes - oldDashes == 1)
				{
					DCN.Text = e.NewTextValue;
					return;
				}

				string s = e.NewTextValue;
				while (s.Contains("-"))
				{
					s = s.Remove(s.IndexOf('-'), 1);
				}
				int len = s.Length;
				string v = "";

				switch (len)
				{
					case 0:
						break;
					case 1:
						v += s[0];
						break;
					case 2:
						v += s[0];
						v += s[1];
						break;
					case 3:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						break;
					case 4:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						break;
					case 5:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						break;
					case 6:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						v += s[5];
						break;
					case 7:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						v += s[5];
						v += s[6];
						break;
					case 8:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						v += s[5];
						v += s[6];
						v += s[7];
						break;
					case 9:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += s[6];
						v += s[7];
						v += s[8];
						break;
					case 10:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						break;
					case 11:
						v += s[0];
						v += s[1];
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += '-';
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						v += s[10];
						break;
					case 12:
						v += s[0];
						v += s[1];
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += '-';
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						v += s[10];
						v += s[11];
						break;
					default:
						v += s[0];
						v += s[1];
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += '-';
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						v += s[10];
						v += s[11];
						break;
				}

				DCN.Text = v;
			}
		}
	}
}
