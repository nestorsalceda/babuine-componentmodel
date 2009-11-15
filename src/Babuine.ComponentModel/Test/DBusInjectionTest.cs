//
// Unit tests for the dependency injection.
// 
// Authors:
//	Néstor Salceda <nestor.salceda@gmail.com>
//
// 	(C) 2009 Néstor Salceda
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
using Babuine.ComponentModel;
using NDesk.DBus;
using org.freedesktop.DBus;
using NUnit.Framework;

namespace Babuine.ComponentModel.Test { 

	[Interface("org.freedesktop.DBus.Introspectable")]
	public interface Introspectable {
		string Introspect();
	}

	[Interface("org.freedesktop.DBus.Properties")]
	public interface Properties {
		object Get(string iface, string propname);
		void Set(string iface, string propname, object value);
		Dictionary<string, object> GetAll(string iface);
	}

	[Interface("org.freedesktop.Notifications")]
	public interface Notifications {
		string GetServerInformation();
		string[] GetCapabilities();
		void CloseNotification(uint id);
		uint Notify(string app_name, uint id, string icon, string summary, string body, string[] actions, Dictionary<string, object> hints, int timeout);
	}
	
	class DBusAssembler : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (Notifications), delegate () {
				return Bus.Session.GetObject<Notifications> ("org.freedesktop.Notifications", new ObjectPath ("/org/freedesktop/Notifications"));	
			});
		}
	}

	class ClientFromDBusInterface {
		[InjectDependency]
		Notifications notifications;

		public Notifications ReturnNotifications ()
		{
			return notifications;
		}
	}

	[TestFixture]
	public class DBusInjectionTest {
		ClientFromDBusInterface client;
		
		[TestFixtureSetUp]
		public void FixtureSetUp () 
		{
			client = ObjectProviderFactory.CreateObjectProvider (new DBusAssembler ()).GetInjectedObject<ClientFromDBusInterface> ();
		}

		[Test]
		public void ClientRetrievedNotNull () 
		{
			Assert.IsNotNull (client);
		}

		[Test]
		public void DBusObjectNotNull () 
		{
			Assert.IsNotNull (client.ReturnNotifications ());
		}

		[Test]
		public void DBusObjectMatchImplementation () 
		{
			Assert.IsTrue (client.ReturnNotifications () is Notifications);
		}
	}
}
