using FlitBit.Dto;

namespace FlitBit.ObjectIdentity.Tests.Models
{
	[DTO]
	public interface IDerived : IMy
	{
		string Description { get; set; }
	}
}
