using FlitBit.Core;
using FlitBit.Wireup;
using FlitBit.Wireup.Meta;

[assembly: WireupDependency(typeof(FlitBit.Emit.WireupThisAssembly))]
[assembly: Wireup(typeof(FlitBit.ObjectIdentity.WireupThisAssembly))]

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	/// Wires up this assembly.
	/// </summary>
	public sealed class WireupThisAssembly : IWireupCommand
	{
		/// <summary>
		/// Wires up this assembly.
		/// </summary>
		/// <param name="coordinator"></param>
		public void Execute(IWireupCoordinator coordinator)
		{			
		}
	}
}
