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
using Babuine.ComponentModel;
using NUnit.Framework;

namespace Babuine.ComponentModel.Test {

	class ClientWithMixedAttributedInjection {
		[InjectDependency] IThirdAttributedComponent component;

		public IThirdAttributedComponent ReturnComponent ()
		{
			return component;
		}
	}
	
	[TestFixture]
	public class MixedAttributedInjectionTest {
		ClientWithMixedAttributedInjection client;

		[TestFixtureSetUp]
		public void FixtureSetUp () 
		{
			client = ObjectProviderFactory.CreateObjectProvider (new MixedAttributedInjectionAssembler ()).GetInjectedObject <ClientWithMixedAttributedInjection> ();
		}

		[Test]
		public void ClientRetrievedNotNullTest () 
		{
			Assert.IsNotNull (client);
		}

		[Test]
		public void FirstLevelNotNullTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ());
		}

		[Test]
		public void FirstLevelMatchesImplementationTest ()
		{
			Assert.IsTrue (client.ReturnComponent () is IThirdAttributedComponent);
			Assert.IsTrue (client.ReturnComponent () is ThirdAttributedComponentImpl);
		}

		[Test]
		public void SecondLevelNotNullTest () 
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnComponent ());
		}
		
		[Test]
		public void SecondLevelMatchesImplementationTest ()
		{
			Assert.IsTrue (client.ReturnComponent ().ReturnComponent () is ISecondAttributedComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnComponent () is SecondNonAttributedComponentImpl);
		}

		[Test]
		public void ThirdLevelNotNullTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnComponent ().ReturnComponent ());
		}

		[Test]
		public void ThirdLevelMatchImplementationTest ()
		{
			Assert.IsTrue (client.ReturnComponent ().ReturnComponent ().ReturnComponent () is IFirstAttributedComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnComponent ().ReturnComponent () is FirstAttributedComponentImpl);
		}
	}


}
