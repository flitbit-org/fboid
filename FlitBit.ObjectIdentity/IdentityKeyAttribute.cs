#region COPYRIGHT© 2009-2014 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using System;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	///   Identifies a single property as an IdentityKey, for generated types,
	///   will cause an implementation of IIdentifiable&lt;IK> where IK is the
	///   property's type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class IdentityKeyAttribute : Attribute
	{}
}