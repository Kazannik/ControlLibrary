using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ControlLibrary.Controls.QRCoder
{
	public static class PayloadGenerator
	{
		public abstract class Payload
		{
			public virtual int Version => -1;
			public virtual QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;
			public virtual QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Default;
			public abstract override string ToString();
		}

		public class WiFi : Payload
		{
			private readonly string ssid, password, authenticationMode;
			private readonly bool isHiddenSSID;

			/// <summary>
			/// Generates a WiFi payload. Scanned by a QR Code scanner app, the device will connect to the WiFi.
			/// </summary>
			/// <param name="ssid">SSID of the WiFi network</param>
			/// <param name="password">Password of the WiFi network</param>
			/// <param name="authenticationMode">Authentification mode (WEP, WPA, WPA2)</param>
			/// <param name="isHiddenSSID">Set flag, if the WiFi network hides its SSID</param>
			/// <param name="escapeHexStrings">Set flag, if ssid/password is delivered as HEX string. Note: May not be supported on iOS devices.</param>
			public WiFi(string ssid, string password, Authentication authenticationMode, bool isHiddenSSID = false, bool escapeHexStrings = true)
			{
				this.ssid = EscapeInput(ssid);
				this.ssid = escapeHexStrings && IsHexStyle(this.ssid) ? "\"" + this.ssid + "\"" : this.ssid;
				this.password = EscapeInput(password);
				this.password = escapeHexStrings && IsHexStyle(this.password) ? "\"" + this.password + "\"" : this.password;
				this.authenticationMode = authenticationMode.ToString();
				this.isHiddenSSID = isHiddenSSID;
			}

			public override string ToString()
			{
				return
					$"WIFI:T:{authenticationMode};S:{ssid};P:{password};{(isHiddenSSID ? "H:true" : string.Empty)};";
			}

			public enum Authentication
			{
				WEP,
				WPA,
				nopass
			}
		}

		public class Mail : Payload
		{
			private readonly string mailReceiver, subject, message;
			private readonly MailEncoding encoding;

			/// <summary>
			/// Creates an email payload with subject and message/text
			/// </summary>
			/// <param name="mailReceiver">Receiver's email address</param>
			/// <param name="subject">Subject line of the email</param>
			/// <param name="message">Message content of the email</param>
			/// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
			public Mail(string mailReceiver = null, string subject = null, string message = null, MailEncoding encoding = MailEncoding.MAILTO)
			{
				this.mailReceiver = mailReceiver;
				this.subject = subject;
				this.message = message;
				this.encoding = encoding;
			}

			public override string ToString()
			{
				var returnVal = string.Empty;
				switch (this.encoding)
				{
					case MailEncoding.MAILTO:
						var parts = new List<string>();
						if (!string.IsNullOrEmpty(this.subject))
							parts.Add("subject=" + Uri.EscapeDataString(this.subject));
						if (!string.IsNullOrEmpty(this.message))
							parts.Add("body=" + Uri.EscapeDataString(this.message));
						var queryString = parts.Any() ? $"?{string.Join("&", parts.ToArray())}" : "";
						returnVal = $"mailto:{this.mailReceiver}{queryString}";
						break;
					case MailEncoding.MATMSG:
						returnVal = $"MATMSG:TO:{this.mailReceiver};SUB:{EscapeInput(this.subject)};BODY:{EscapeInput(this.message)};;";
						break;
					case MailEncoding.SMTP:
						returnVal = $"SMTP:{this.mailReceiver}:{EscapeInput(this.subject, true)}:{EscapeInput(this.message, true)}";
						break;
				}
				return returnVal;
			}

			public enum MailEncoding
			{
				MAILTO,
				MATMSG,
				SMTP
			}
		}

		public class SMS : Payload
		{
			private readonly string number, subject;
			private readonly SMSEncoding encoding;

			/// <summary>
			/// Creates a SMS payload without text
			/// </summary>
			/// <param name="number">Receiver phone number</param>
			/// <param name="encoding">Encoding type</param>
			public SMS(string number, SMSEncoding encoding = SMSEncoding.SMS) : this(number: number, subject: string.Empty, encoding: encoding) { }

			/// <summary>
			/// Creates a SMS payload with text (subject)
			/// </summary>
			/// <param name="number">Receiver phone number</param>
			/// <param name="subject">Text of the SMS</param>
			/// <param name="encoding">Encoding type</param>
			public SMS(string number, string subject, SMSEncoding encoding = SMSEncoding.SMS)
			{
				this.number = number;
				this.subject = subject;
				this.encoding = encoding;
			}

			public override string ToString()
			{
				var returnVal = string.Empty;
				switch (this.encoding)
				{
					case SMSEncoding.SMS:
						var queryString = string.Empty;
						if (!string.IsNullOrEmpty(this.subject))
							queryString = $"?body={Uri.EscapeDataString(this.subject)}";
						returnVal = $"sms:{this.number}{queryString}";
						break;
					case SMSEncoding.SMS_iOS:
						var queryStringiOS = string.Empty;
						if (!string.IsNullOrEmpty(this.subject))
							queryStringiOS = $";body={Uri.EscapeDataString(this.subject)}";
						returnVal = $"sms:{this.number}{queryStringiOS}";
						break;
					case SMSEncoding.SMSTO:
						returnVal = $"SMSTO:{this.number}:{this.subject}";
						break;
				}
				return returnVal;
			}

			public enum SMSEncoding
			{
				SMS,
				SMSTO,
				SMS_iOS
			}
		}

		public class MMS : Payload
		{
			private readonly string number, subject;
			private readonly MMSEncoding encoding;

			/// <summary>
			/// Creates a MMS payload without text
			/// </summary>
			/// <param name="number">Receiver phone number</param>
			/// <param name="encoding">Encoding type</param>
			public MMS(string number, MMSEncoding encoding = MMSEncoding.MMS) : this(number: number, subject: string.Empty, encoding: encoding) { }

			/// <summary>
			/// Creates a MMS payload with text (subject)
			/// </summary>
			/// <param name="number">Receiver phone number</param>
			/// <param name="subject">Text of the MMS</param>
			/// <param name="encoding">Encoding type</param>
			public MMS(string number, string subject, MMSEncoding encoding = MMSEncoding.MMS)
			{
				this.number = number;
				this.subject = subject;
				this.encoding = encoding;
			}

			public override string ToString()
			{
				var returnVal = string.Empty;
				switch (this.encoding)
				{
					case MMSEncoding.MMSTO:
						var queryStringMmsTo = string.Empty;
						if (!string.IsNullOrEmpty(this.subject))
							queryStringMmsTo = $"?subject={Uri.EscapeDataString(this.subject)}";
						returnVal = $"mmsto:{this.number}{queryStringMmsTo}";
						break;
					case MMSEncoding.MMS:
						var queryStringMms = string.Empty;
						if (!string.IsNullOrEmpty(this.subject))
							queryStringMms = $"?body={Uri.EscapeDataString(this.subject)}";
						returnVal = $"mms:{this.number}{queryStringMms}";
						break;
				}
				return returnVal;
			}

			public enum MMSEncoding
			{
				MMS,
				MMSTO
			}
		}

		public class Geolocation : Payload
		{
			private readonly string latitude, longitude;
			private readonly GeolocationEncoding encoding;

			/// <summary>
			/// Generates a geo location payload. Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding)
			/// </summary>
			/// <param name="latitude">Latitude with . as splitter</param>
			/// <param name="longitude">Longitude with . as splitter</param>
			/// <param name="encoding">Encoding type - GEO or GoogleMaps</param>
			public Geolocation(string latitude, string longitude, GeolocationEncoding encoding = GeolocationEncoding.GEO)
			{
				this.latitude = latitude.Replace(",", ".");
				this.longitude = longitude.Replace(",", ".");
				this.encoding = encoding;
			}

			public override string ToString()
			{
				switch (this.encoding)
				{
					case GeolocationEncoding.GEO:
						return $"geo:{this.latitude},{this.longitude}";
					case GeolocationEncoding.GoogleMaps:
						return $"http://maps.google.com/maps?q={this.latitude},{this.longitude}";
					default:
						return "geo:";
				}
			}

			public enum GeolocationEncoding
			{
				GEO,
				GoogleMaps
			}
		}

		public class PhoneNumber : Payload
		{
			private readonly string number;

			/// <summary>
			/// Generates a phone call payload
			/// </summary>
			/// <param name="number">Phonenumber of the receiver</param>
			public PhoneNumber(string number)
			{
				this.number = number;
			}

			public override string ToString()
			{
				return $"tel:{this.number}";
			}
		}

		public class SkypeCall : Payload
		{
			private readonly string skypeUsername;

			/// <summary>
			/// Generates a Skype call payload
			/// </summary>
			/// <param name="skypeUsername">Skype username which will be called</param>
			public SkypeCall(string skypeUsername)
			{
				this.skypeUsername = skypeUsername;
			}

			public override string ToString()
			{
				return $"skype:{this.skypeUsername}?call";
			}
		}

		public class Url : Payload
		{
			private readonly string url;

			/// <summary>
			/// Generates a link. If not given, http/https protocol will be added.
			/// </summary>
			/// <param name="url">Link url target</param>
			public Url(string url)
			{
				this.url = url;
			}

			public override string ToString()
			{
				return !this.url.StartsWith("http") ? "http://" + this.url : this.url;
			}
		}

		public class WhatsAppMessage : Payload
		{
			private readonly string number, message;

			/// <summary>
			/// Let's you compose a WhatApp message and send it the receiver number.
			/// </summary>
			/// <param name="number">Receiver phone number where the <number> is a full phone number in international format. 
			/// Omit any zeroes, brackets, or dashes when adding the phone number in international format.
			/// Use: 1XXXXXXXXXX | Don't use: +001-(XXX)XXXXXXX
			/// </param>
			/// <param name="message">The message</param>
			public WhatsAppMessage(string number, string message)
			{
				this.number = number;
				this.message = message;
			}

			/// <summary>
			/// Let's you compose a WhatApp message. When scanned the user is asked to choose a contact who will receive the message.
			/// </summary>
			/// <param name="message">The message</param>
			public WhatsAppMessage(string message)
			{
				this.number = string.Empty;
				this.message = message;
			}

			public override string ToString()
			{
				var cleanedPhone = Regex.Replace(this.number, @"^[0+]+|[ ()-]", string.Empty);
				return $"https://wa.me/{cleanedPhone}?text={Uri.EscapeDataString(message)}";
			}
		}

		public class Bookmark : Payload
		{
			private readonly string url, title;

			/// <summary>
			/// Generates a bookmark payload. Scanned by an QR Code reader, this one creates a browser bookmark.
			/// </summary>
			/// <param name="url">Url of the bookmark</param>
			/// <param name="title">Title of the bookmark</param>
			public Bookmark(string url, string title)
			{
				this.url = EscapeInput(url);
				this.title = EscapeInput(title);
			}

			public override string ToString()
			{
				return $"MEBKM:TITLE:{this.title};URL:{this.url};;";
			}
		}

		public class ContactData : Payload
		{
			private readonly string firstname;
			private readonly string lastname;
			private readonly string nickname;
			private readonly string org;
			private readonly string orgTitle;
			private readonly string phone;
			private readonly string mobilePhone;
			private readonly string workPhone;
			private readonly string email;
			private readonly DateTime? birthday;
			private readonly string website;
			private readonly string street;
			private readonly string houseNumber;
			private readonly string city;
			private readonly string zipCode;
			private readonly string stateRegion;
			private readonly string country;
			private readonly string note;
			private readonly ContactOutputType outputType;


			/// <summary>
			/// Generates a vCard or meCard contact dataset
			/// </summary>
			/// <param name="outputType">Payload output type</param>
			/// <param name="firstname">The firstname</param>
			/// <param name="lastname">The lastname</param>
			/// <param name="nickname">The displayname</param>
			/// <param name="phone">Normal phone number</param>
			/// <param name="mobilePhone">Mobile phone</param>
			/// <param name="workPhone">Office phone number</param>
			/// <param name="email">E-Mail address</param>
			/// <param name="birthday">Birthday</param>
			/// <param name="website">Website / Homepage</param>
			/// <param name="street">Street</param>
			/// <param name="houseNumber">Housenumber</param>
			/// <param name="city">City</param>
			/// <param name="stateRegion">State or Region</param>
			/// <param name="zipCode">Zip code</param>
			/// <param name="country">Country</param>
			/// <param name="addressOrder">The address order format to use</param>
			/// <param name="note">Коментарий</param>            
			/// <param name="org">Organisation/Company</param>            
			/// <param name="orgTitle">Organisation/Company Title</param>            
			public ContactData(ContactOutputType outputType, string firstname, string lastname, string nickname = null, string phone = null, string mobilePhone = null, string workPhone = null, string email = null, DateTime? birthday = null, string website = null, string street = null, string houseNumber = null, string city = null, string zipCode = null, string country = null, string note = null, string stateRegion = null, string org = null, string orgTitle = null)
			{
				this.firstname = firstname;
				this.lastname = lastname;
				this.nickname = nickname;
				this.org = org;
				this.orgTitle = orgTitle;
				this.phone = phone;
				this.mobilePhone = mobilePhone;
				this.workPhone = workPhone;
				this.email = email;
				this.birthday = birthday;
				this.website = website;
				this.street = street;
				this.houseNumber = houseNumber;
				this.city = city;
				this.stateRegion = stateRegion;
				this.zipCode = zipCode;
				this.country = country;
				this.note = note;
				this.outputType = outputType;
			}

			public override string ToString()
			{
				string payload = string.Empty;

				var version = outputType.ToString().Substring(5);
				if (version.Length > 1)
					version = version.Insert(1, ".");
				else
					version += ".0";

				payload += "BEGIN:VCARD\r\n";
				payload += $"VERSION:{version}\r\n";

				payload += $"N:{(!string.IsNullOrEmpty(lastname) ? lastname : "")};{(!string.IsNullOrEmpty(firstname) ? firstname : "")};;;\r\n";
				payload += $"FN:{(!string.IsNullOrEmpty(firstname) ? firstname + " " : "")}{(!string.IsNullOrEmpty(lastname) ? lastname : "")}\r\n";
				if (!string.IsNullOrEmpty(org))
				{
					payload += $"ORG:" + org + "\r\n";
				}
				if (!string.IsNullOrEmpty(orgTitle))
				{
					payload += $"TITLE:" + orgTitle + "\r\n";
				}
				if (!string.IsNullOrEmpty(phone))
				{
					payload += $"TEL;";
					if (outputType == ContactOutputType.VCard21)
						payload += $"HOME;VOICE:{phone}";
					else if (outputType == ContactOutputType.VCard3)
						payload += $"TYPE=HOME,VOICE:{phone}";
					else
						payload += $"TYPE=home,voice;VALUE=uri:tel:{phone}";
					payload += "\r\n";
				}

				if (!string.IsNullOrEmpty(mobilePhone))
				{
					payload += $"TEL;";
					if (outputType == ContactOutputType.VCard21)
						payload += $"HOME;CELL:{mobilePhone}";
					else if (outputType == ContactOutputType.VCard3)
						payload += $"TYPE=HOME,CELL:{mobilePhone}";
					else
						payload += $"TYPE=home,cell;VALUE=uri:tel:{mobilePhone}";
					payload += "\r\n";
				}

				if (!string.IsNullOrEmpty(workPhone))
				{
					payload += $"TEL;";
					if (outputType == ContactOutputType.VCard21)
						payload += $"WORK;VOICE:{workPhone}";
					else if (outputType == ContactOutputType.VCard3)
						payload += $"TYPE=WORK,VOICE:{workPhone}";
					else
						payload += $"TYPE=work,voice;VALUE=uri:tel:{workPhone}";
					payload += "\r\n";
				}


				payload += "ADR;";
				if (outputType == ContactOutputType.VCard21)
					payload += "HOME;PREF:";
				else if (outputType == ContactOutputType.VCard3)
					payload += "TYPE=HOME,PREF:";
				else
					payload += "TYPE=home,pref:";
				string addressString = string.Empty;

				addressString = $";;{(!string.IsNullOrEmpty(street) ? street + " " : "")}{(!string.IsNullOrEmpty(houseNumber) ? houseNumber : "")};{(!string.IsNullOrEmpty(zipCode) ? zipCode : "")};{(!string.IsNullOrEmpty(city) ? city : "")};{(!string.IsNullOrEmpty(stateRegion) ? stateRegion : "")};{(!string.IsNullOrEmpty(country) ? country : "")}\r\n";

				payload += addressString;

				if (birthday != null)
					payload += $"BDAY:{((DateTime)birthday).ToString("yyyyMMdd")}\r\n";
				if (!string.IsNullOrEmpty(website))
					payload += $"URL:{website}\r\n";
				if (!string.IsNullOrEmpty(email))
					payload += $"EMAIL:{email}\r\n";
				if (!string.IsNullOrEmpty(note))
					payload += $"NOTE:{note}\r\n";
				if (outputType != ContactOutputType.VCard21 && !string.IsNullOrEmpty(nickname))
					payload += $"NICKNAME:{nickname}\r\n";

				payload += "END:VCARD";


				return payload;
			}

			/// <summary>
			/// Possible output types. Either vCard 2.1, vCard 3.0, vCard 4.0 or MeCard.
			/// </summary>
			public enum ContactOutputType
			{
				VCard21,
				VCard3,
				VCard4
			}
		}

		public class CalendarEvent : Payload
		{
			private readonly string subject, description, location, start, end;
			private readonly EventEncoding encoding;

			/// <summary>
			/// Generates a calender entry/event payload.
			/// </summary>
			/// <param name="subject">Subject/title of the calender event</param>
			/// <param name="description">Description of the event</param>
			/// <param name="location">Location (lat:long or address) of the event</param>
			/// <param name="start">Start time of the event</param>
			/// <param name="end">End time of the event</param>
			/// <param name="allDayEvent">Is it a full day event?</param>
			/// <param name="encoding">Type of encoding (universal or iCal)</param>
			public CalendarEvent(string subject, string description, string location, DateTime start, DateTime end, bool allDayEvent, EventEncoding encoding = EventEncoding.Universal)
			{
				this.subject = subject;
				this.description = description;
				this.location = location;
				this.encoding = encoding;
				string dtFormat = allDayEvent ? "yyyyMMdd" : "yyyyMMddTHHmmss";
				this.start = start.ToString(dtFormat);
				this.end = end.ToString(dtFormat);
			}

			public override string ToString()
			{
				var vEvent = $"BEGIN:VEVENT{Environment.NewLine}";
				vEvent += $"SUMMARY:{this.subject}{Environment.NewLine}";
				vEvent += !string.IsNullOrEmpty(this.description) ? $"DESCRIPTION:{this.description}{Environment.NewLine}" : "";
				vEvent += !string.IsNullOrEmpty(this.location) ? $"LOCATION:{this.location}{Environment.NewLine}" : "";
				vEvent += $"DTSTART:{this.start}{Environment.NewLine}";
				vEvent += $"DTEND:{this.end}{Environment.NewLine}";
				vEvent += "END:VEVENT";

				if (this.encoding == EventEncoding.iCalComplete)
					vEvent = $@"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}{vEvent}{Environment.NewLine}END:VCALENDAR";

				return vEvent;
			}

			public enum EventEncoding
			{
				iCalComplete,
				Universal
			}
		}

		private static string EscapeInput(string inp, bool simple = false)
		{
			char[] forbiddenChars = { '\\', ';', ',', ':' };
			if (simple)
			{
				forbiddenChars = new char[1] { ':' };
			}
			foreach (var c in forbiddenChars)
			{
				inp = inp.Replace(c.ToString(), "\\" + c);
			}
			return inp;
		}


		private static bool IsHexStyle(string inp)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b[0-9a-fA-F]+\b\Z") || System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z");
		}
	}
}
