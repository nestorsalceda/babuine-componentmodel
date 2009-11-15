//
// Babuine.ComponentModel.ObjectProvider class 
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
	public class ObjectProvider {
		Linker linker;
		Dictionary<Type, object> uniqueInstancesTable = new Dictionary<Type, object> ();

		internal ObjectProvider (Linker linker) 
		{
			this.linker = linker;
		}

		public T GetInjectedObject<T> () where T : class
		{
			return (T) GetInjectedObject (typeof (T), null);
		}

		object GetAlreadyInstancedObject (Type type, ICustomAttributeProvider context)
		{
			if (!uniqueInstancesTable.ContainsKey (type))
				uniqueInstancesTable.Add (type, CreateInstance (type, context));

			return uniqueInstancesTable [type];
		}
		
		object GetInjectedObject (Type type, ICustomAttributeProvider context)
		{			
			object objectInjected = null;
			Type resolved = linker.ResolveImplementation (type, context);
			
			if (linker.IsUniqueInstance (resolved)) 
				objectInjected = GetAlreadyInstancedObject (resolved, context);
			else { 
				if (resolved.IsInterface)
					objectInjected = linker.ResolveInstancing (resolved, context).Invoke ();
				else 
					objectInjected = CreateInstance (resolved, context);
			}
			
			if (ContainsInjectableFields (objectInjected.GetType ())) 
				objectInjected = InjectFields (objectInjected.GetType (), objectInjected);
			
			return objectInjected;
		}

		object InjectFields (Type type, object target) 
		{
			foreach (FieldInfo field in GetInjectableFields (type))  
				field.SetValue (target, GetInjectedObject (field.FieldType, field));			
			return target;
		}
		
		object CreateInstance (Type type, ICustomAttributeProvider context) 
		{
			ConstructorInfo constructor = GetInjectableConstructor (type);
			if (constructor != null)
				return CreateInjectedObject (constructor);
			
			if (type.IsInterface)
				return linker.ResolveInstancing (type, context).Invoke ();

			return Activator.CreateInstance (type);
		}
	
		bool ContainsInjectableFields (Type type)
		{
			return GetInjectableFields (type).Length != 0;
		}

		FieldInfo[] GetInjectableFields (Type type)
		{
			List<FieldInfo> fields = new List<FieldInfo> ();
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.DeclaredOnly | BindingFlags.Instance;
			foreach (FieldInfo field in type.GetFields (flags)) {
				if (field.IsDefined (typeof (InjectDependencyAttribute), true))
					fields.Add (field);
			}
			return fields.ToArray ();
		}

		ConstructorInfo GetInjectableConstructor (Type type) 
		{
			ConstructorInfo result = null;
			foreach (ConstructorInfo constructor in type.GetConstructors ()) {
				if (constructor.IsDefined (typeof (InjectDependencyAttribute), true)) {
					if (result == null) 
						result = constructor;
					if (constructor.GetParameters ().Length < result.GetParameters ().Length)
						result = constructor;
				}
			}
			return result;
		}

		object CreateInjectedObject (ConstructorInfo constructor)
		{
			List<object> parameters = new List<object> ();
			foreach (ParameterInfo parameter in constructor.GetParameters ()) 
				parameters.Add (GetInjectedObject (parameter.ParameterType, parameter));
			return constructor.Invoke (parameters.ToArray ());
		}


	}
}
