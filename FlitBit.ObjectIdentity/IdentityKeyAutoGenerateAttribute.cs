#region COPYRIGHT© 2009-2014 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
#endregion

using System;
using FlitBit.Core.Factory;
using FlitBit.Core.Meta;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	///   Framework: Binds the abstract IdentityKey&lt;T> to the emitter that constructs implementations.
	///   You won't need to use this class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class IdentityKeyAutoGenerateAttribute : AutoImplementedAttribute
	{
		/// <summary>
		///   Creates a new instance.
		/// </summary>
		public IdentityKeyAutoGenerateAttribute()
		{
			this.RecommemdedScope = InstanceScopeKind.ContainerScope;
		}

		/// <summary>
		///   Gets the implementation for type
		/// </summary>
		/// <param name="factory">the factory from which the type was requested.</param>
		/// <param name="type">the target types</param>
		/// <param name="complete">callback invoked when the implementation is available</param>
		/// <returns>
		///   <em>true</em> if implemented; otherwise <em>false</em>.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		///   thrown if <paramref name="type" /> is not eligible for implementation
		/// </exception>
		/// <remarks>
		///   If the <paramref name="complete" /> callback is invoked, it must be given either an implementation type
		///   assignable to type T, or a factory function that creates implementations of type T.
		/// </remarks>
		public override bool GetImplementation(IFactory factory, Type type, Action<Type, Func<object>> complete)
		{
			if (type.GetGenericTypeDefinition() == typeof(IdentityKey<>))
			{
				var args = type.GetGenericArguments();

				var typeGenerator = IdentityKeyEmitter.TypeEmitter;
				var emittedType = (Type) typeGenerator.MakeGenericMethod(args[0])
																							.Invoke(null, new object[0]);
				complete(emittedType, null);
				return true;
			}
			return false;
		}
	}
}