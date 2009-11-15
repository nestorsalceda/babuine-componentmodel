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
	
	class ClientWithVariousInstances {
		[InjectDependency] IThirdVariousImplementationComponent component;

		public IThirdVariousImplementationComponent ReturnComponent ()
		{
			return component;
		}
	}
	
	[TestFixture]
	public class VariousInstancesSelectionInjectionTest {
		ClientWithVariousInstances client;

		[TestFixtureSetUp]
		public void FixtureSetUp () 
		{
			client = ObjectProviderFactory.CreateObjectProvider (new VariousInstancesComponentAssembler ()).GetInjectedObject <ClientWithVariousInstances> ();
		}

		[Test]
		public void ClientRetrievedNotNullTest () 
		{
			Assert.IsNotNull (client);
		}

		[Test]
		public void FirstLevelInjectionTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ());
			Assert.IsTrue (client.ReturnComponent () is IThirdVariousImplementationComponent);
			Assert.IsTrue (client.ReturnComponent () is ThirdVariousImplementationComponentImpl);
		}

		[Test]
		public void SecondLevelInjectionSelectionOneTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnOne ());
			Assert.IsTrue (client.ReturnComponent ().ReturnOne () is ISecondVariousImplementationComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnOne () is SecondVariousImplementationComponentOneImpl);
		}

		[Test]
		public void SecondLevelInjectionSelectionTwoTest () 
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnTwo ());
			Assert.IsTrue (client.ReturnComponent ().ReturnTwo () is ISecondVariousImplementationComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnTwo () is SecondVariousImplementationComponentTwoImpl);
		}

		[Test]
		public void ThirdLevelInjectionSelectionOneTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnOne ().ReturnOne ());
			Assert.IsTrue (client.ReturnComponent ().ReturnOne ().ReturnOne () is IFirstVariousImplementationComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnOne ().ReturnOne () is FirstVariousImplementationComponentOneImpl);

			Assert.IsNotNull (client.ReturnComponent ().ReturnTwo ().ReturnOne ());
			Assert.IsTrue (client.ReturnComponent ().ReturnTwo ().ReturnOne () is IFirstVariousImplementationComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnTwo ().ReturnOne () is FirstVariousImplementationComponentOneImpl);
		}

		[Test]
		public void ThirdLevelInjectionSelectionTwoTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnOne ().ReturnTwo ());
			Assert.IsTrue (client.ReturnComponent ().ReturnOne ().ReturnTwo () is IFirstVariousImplementationComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnOne ().ReturnTwo () is FirstVariousImplementationComponentTwoImpl);

			Assert.IsNotNull (client.ReturnComponent ().ReturnTwo ().ReturnOne ());
			Assert.IsTrue (client.ReturnComponent ().ReturnTwo ().ReturnTwo () is IFirstVariousImplementationComponent);
			Assert.IsTrue (client.ReturnComponent ().ReturnTwo ().ReturnTwo () is FirstVariousImplementationComponentTwoImpl);
		}
	}
}
