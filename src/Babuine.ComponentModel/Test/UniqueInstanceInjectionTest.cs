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

	[TestFixture]
	public class UniqueInstanceInjectionTest {
		ObjectProvider objectProvider;
		ClientWithFieldInjection client, otherClient;

		[TestFixtureSetUp]
		public void FixtureSetUp () 
		{
			objectProvider = ObjectProviderFactory.CreateObjectProvider (new UniqueInstanceComponentAssembler ());
			client = objectProvider.GetInjectedObject <ClientWithFieldInjection> ();
		}
		
		[SetUp]
		public void SetUp ()
		{
			otherClient = objectProvider.GetInjectedObject <ClientWithFieldInjection> ();
		}

		[Test]
		public void FirstLevelNotNullTest () 
		{
			Assert.IsNotNull (client.ReturnComponent ());
			Assert.IsNotNull (otherClient.ReturnComponent ());
		}

		[Test]
		public void SecondLevelNotNullTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnComponent ());
			Assert.IsNotNull (otherClient.ReturnComponent ().ReturnComponent ());
		}

		[Test]
		public void ThirdLevelNotNullTest ()
		{
			Assert.IsNotNull (client.ReturnComponent ().ReturnComponent ().ReturnComponent ());
			Assert.IsNotNull (otherClient.ReturnComponent ().ReturnComponent ().ReturnComponent ());
		}


		[Test]
		public void FirstLevelNotSameInstanceTest ()
		{
			Assert.IsFalse (client.ReturnComponent () == otherClient.ReturnComponent ());
			Assert.AreNotSame (client.ReturnComponent (), otherClient.ReturnComponent ());
		}

		[Test]
		public void SecondLevelSameInstanceTest ()
		{
			Assert.IsTrue (client.ReturnComponent ().ReturnComponent () == otherClient.ReturnComponent ().ReturnComponent ());
			Assert.AreSame (client.ReturnComponent ().ReturnComponent (), otherClient.ReturnComponent ().ReturnComponent ());
		}

		[Test]
		public void ThirdLevelNotSameInstanceTest ()
		{
			//Because the last level is a unique instance, the
			//contained objects in the second instance are the same.
			Assert.IsTrue (client.ReturnComponent ().ReturnComponent ().ReturnComponent () == otherClient.ReturnComponent ().ReturnComponent ().ReturnComponent ());
			Assert.AreSame (client.ReturnComponent ().ReturnComponent ().ReturnComponent (), otherClient.ReturnComponent ().ReturnComponent ().ReturnComponent ());
		}
	}
}
