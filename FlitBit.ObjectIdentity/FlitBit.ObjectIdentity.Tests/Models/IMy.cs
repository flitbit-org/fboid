using FlitBit.Dto;

namespace FlitBit.ObjectIdentity.Tests.Models
{
	[DTO]
	public interface IMy
	{
		string Name { get; set; }
		[IdentityKey]
		string Tag { get; set; }
	}
}
