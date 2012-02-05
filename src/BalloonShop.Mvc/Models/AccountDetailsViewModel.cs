﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BalloonShop.Mvc.Models
{
	public class AccountDetailsViewModel
	{
		public virtual string Address1 { get; set; }
		public virtual string Address2 { get; set; }
		public virtual string City { get; set; }
		public virtual string Region { get; set; }
		public virtual string PostalCode { get; set; }
		public virtual string Country { get; set; }
		public virtual int ShippingRegion { get; set; }
		public virtual string DaytimePhone { get; set; }
		public virtual string EveningPhone { get; set; }
		public virtual string MobilePhone { get; set; }
	}
}