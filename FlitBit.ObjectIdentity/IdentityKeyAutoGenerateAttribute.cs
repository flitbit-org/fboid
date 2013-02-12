using System;
using System.Linq;
using System.Reflection;
using FlitBit.Core.Factory;
using FlitBit.Emit;
using FlitBit.Meta;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	/// Framework: Binds the abstract IdentityKey&lt;T> to the emitter that constructs implementations.
	/// You won't need to use this class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class IdentityKeyAutoGenerateAttribute : AutoImplementedAttribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public IdentityKeyAutoGenerateAttribute()
			: base()
		{
			this.RecommemdedScope = Core.Meta.InstanceScopeKind.ContainerScope;
		}

		/// <summary>
		/// Implements the stereotypical IdentityKey behavior for interfaces of type T.
		/// </summary>
		/// <typeparam name="T">interface type T</typeparam>
		/// <param name="factory">the requesting factory</param>
		/// <param name="complete">completion callback.</param>
		/// <returns>true if the type was registered with the container, otherwise <em>false</em></returns>
		public override bool GetImplementation<T>(IFactory factory, Action<Type, Func<T>> complete)
		{
			if (typeof(T).GetGenericTypeDefinition() == typeof(IdentityKey<>))
			{
				var args = typeof(T).GetGenericArguments();
				var keyProp = args[0].GetReadablePropertiesFromHierarchy(BindingFlags.Public)
					.FirstOrDefault(p => p.IsDefined(typeof(IdentityKeyAttribute), true)
					);

				var typeGenerator = IdentityKeyEmitter.TypeEmitter;
				var emittedType = (Type)typeGenerator.MakeGenericMethod(args[0]).Invoke(null, Type.EmptyTypes);
				complete(emittedType, null);
				return true;
			}
			return false;
		}
	}
}
