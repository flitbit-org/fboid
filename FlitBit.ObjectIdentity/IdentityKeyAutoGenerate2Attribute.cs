using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using FlitBit.Emit;
using FlitBit.Core.Factory;

namespace FlitBit.ObjectIdentity
{
	/// <summary>
	/// Framework: Binds the abstract IdentityKey&lt;T,IK> to the emitter that constructs implementations.
	/// You won't need to use this class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class IdentityKeyAutoGenerate2Attribute : IdentityKeyAutoGenerateAttribute
	{
		
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public IdentityKeyAutoGenerate2Attribute() : base()
		{ 
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
			if (typeof(T).GetGenericTypeDefinition() == typeof(IdentityKey<,>))
			{
				var args = typeof(T).GetGenericArguments();
				typeof(IdentityKeyEmitter).GetGenericMethod("VerifyIdentityKeyType", BindingFlags.Static | BindingFlags.NonPublic, 0, 2)
					.MakeGenericMethod(args[0], args[1]).Invoke(null, null);

				var typeGenerator = IdentityKeyEmitter.TypeEmitter;
				var emittedType = (Type)typeGenerator.MakeGenericMethod(args[0]).Invoke(null, Type.EmptyTypes);
				complete(emittedType, null);
				return true;
			}
			return false;
		}		
	}
}
