using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlitBit.ObjectIdentity.Tests.Models
{
	public class My
	{
		[IdentityKey]
		public string Name { get; set; }
		public string Tag { get; set; }
	}
}
