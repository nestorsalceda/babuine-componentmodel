//
// Unit tests for the dependency injection. 
//
// Authors:
//      Néstor Salceda <nestor.salceda@gmail.com>
//
//      (C) 2007 Néstor Salceda
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
using System.Collections;
using Babuine.ComponentModel;
using NUnit.Framework;

namespace Babuine.ComponentModel.Test {

	class AssemblerWithNoInterface : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (object), typeof (object));
		}
	}

	class AssemblerWithNoImplementation : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (ICollection), typeof (ICollection));
		}
	}

	class AssemblerWithNullInterface : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (null, typeof (object));
		}

	}

	class AssemblerWithNullImplementation : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			//linker.WireInterface (typeof (ICollection), null);
		}
	}

	class AssemblerWithAbstractClassAsImplementation : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (ICollection), typeof (DictionaryBase));
		}
	}

	class AssemblerWithClassNoInheritFromService : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (ICollection), typeof (object));
		}
	}
	
	class AssemblerWithClassAsAttribute : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (ICollection), typeof (ICollection), typeof (object));
		}
	}

	[TestFixture]
	public class LinkerTest {
		
		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void AssemblerWithNoInterfaceTest ()
		{
			ObjectProviderFactory.CreateObjectProvider (new AssemblerWithNoInterface ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void AssemblerWithNoImplementationTest ()
		{
			ObjectProviderFactory.CreateObjectProvider (new AssemblerWithNoImplementation ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AssemblerWithNullInterfaceTest () 
		{
			ObjectProviderFactory.CreateObjectProvider (new AssemblerWithNullInterface ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		[Ignore ("The compiler avoids this, because can't resolve the overloaded call")]
		public void AssemblerWithNullImplementationTest ()
		{
			ObjectProviderFactory.CreateObjectProvider (new AssemblerWithNullImplementation ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void AssemblerWithAbstractClassAsImplementationTest ()
		{
			ObjectProviderFactory.CreateObjectProvider (new AssemblerWithAbstractClassAsImplementation ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void AssemblerWithClassAsAttributeTest ()
		{
			ObjectProviderFactory.CreateObjectProvider (new AssemblerWithClassAsAttribute ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void AssemblerWithClassNoInheritFromServiceTest ()
		{
			ObjectProviderFactory.CreateObjectProvider (new AssemblerWithClassNoInheritFromService ());
		}
	}
}
