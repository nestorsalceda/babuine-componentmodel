//
// Babuine.ComponentModel.Linker class 
//
// Authors:
//      Néstor Salceda <nestor.salceda@gmail.com>
//
//      (C) 2007-2008-2009 Néstor Salceda
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Babuine.ComponentModel {
	public class Linker {
		Dictionary<Identifier, Type> linkingTable = new Dictionary<Identifier, Type> ();
		Dictionary<Identifier, Func<object>> instancingTable = new Dictionary<Identifier, Func<object>> ();
		Dictionary<Type, bool> uniqueInstancesTable = new Dictionary<Type, bool> ();
		IList<Type> implementationSelectors = new List<Type> ();

		internal Linker () 
		{
		}

		Type GetImplementationType (Type type)
		{
			AssociateImplementationAttribute attribute = (AssociateImplementationAttribute) type.GetCustomAttributes (typeof (AssociateImplementationAttribute), false)[0];
			return attribute.AssociatedImplementation;
		}

		Type GetSelectorAttributeIn (ICustomAttributeProvider context)
		{
			if (context == null)
				return null;

			foreach (Type selector in implementationSelectors){
				if (context.GetCustomAttributes (selector, false).Length != 0)
					return context.GetCustomAttributes (selector, false) [0].GetType ();
			}
			return null;
		}

		internal Func<object> ResolveInstancing (Type service, ICustomAttributeProvider context) 
		{
			Identifier identifier = new Identifier (service, GetSelectorAttributeIn (context));
			if (instancingTable.ContainsKey (identifier))
				return instancingTable[identifier];
			return null;
		}

		internal Type ResolveImplementation (Type service, ICustomAttributeProvider context)
		{
			Identifier identifier = new Identifier (service, GetSelectorAttributeIn (context));

			if (!linkingTable.ContainsKey (identifier)) {
				if (service.IsDefined (typeof (AssociateImplementationAttribute), false)) 
					WireInterface (service, GetImplementationType (service));
			}

			if (linkingTable.ContainsKey (identifier))
				return linkingTable[identifier];

			//TODO: Perhaps returning null better?
			return service;
		}

		internal bool IsUniqueInstance (Type service) {
			if (!uniqueInstancesTable.ContainsKey (service)) 
				uniqueInstancesTable.Add (service, service.IsDefined (typeof (UniqueInstanceAttribute), false));

			return uniqueInstancesTable[service];
		}

		
		public void WireInterface (Type service, Type implementation) 
		{
			WireInterface (service, null, implementation);
		}

		public void WireInterface (Type service, Type implementation, bool uniqueInstance)
		{
			WireInterface (service, null, implementation, uniqueInstance);
		}

		public void WireInterface (Type service, Type attribute, Type implementation, bool uniqueInstance)
		{
			WireInterface (service, attribute, implementation);
			uniqueInstancesTable.Add (implementation, uniqueInstance);
		}

		
		public void WireInterface (Type service, Type attribute, Type implementation)
		{
			if (service == null)
				throw new ArgumentNullException ("service", "Should be a non-null reference");

			if (implementation == null)
				throw new ArgumentNullException ("implementation", "Should be a non-null reference");

			if (!service.IsInterface)
				throw new ArgumentException ("Should be an interface type", "service");

			if (!implementation.IsClass || implementation.IsAbstract)
				throw new ArgumentException ("Should be a class", "implementation");

			if (attribute != null && !attribute.IsSubclassOf (typeof (Attribute)))
				throw new ArgumentException ("Should be an System.Attribute subclass", "attribute");

			if (!service.IsAssignableFrom (implementation))
				throw new ArgumentException ("Should be an implementation of the service", "implementation");

			linkingTable.Add (new Identifier (service, attribute), implementation);

			if (attribute != null && !implementationSelectors.Contains (attribute))
				implementationSelectors.Add (attribute);
		}

		public void WireInterface (Type service, Func<object> createInstance) 
		{
			instancingTable.Add (new Identifier (service, null), createInstance);
		}

		public void WireInterface (Type service, Func<object> createInstance, bool uniqueInstance)
		{
			WireInterface (service, createInstance);
			uniqueInstancesTable.Add (service, uniqueInstance);
		}

		public void WireInterface (Type service, Type attribute, Func<object> createInstance)
		{
			instancingTable.Add (new Identifier (service, attribute), createInstance);
			if (attribute != null && !implementationSelectors.Contains (attribute))
				implementationSelectors.Add (attribute);
		}
	}
}	
