//
// Components for the recursive scenario testing.
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

namespace Babuine.ComponentModel.Test {

	interface IFirstComponent {
	}

	interface ISecondComponent {
		IFirstComponent ReturnComponent ();
	}

	interface IThirdComponent {
		ISecondComponent ReturnComponent ();
	}

	class FirstComponentImpl : IFirstComponent {
	}

	class SecondComponentFieldInjectionImpl : ISecondComponent {
		[InjectDependency] IFirstComponent component;

		public IFirstComponent ReturnComponent ()
		{
			return component;
		}
	}

	class SecondComponentConstructorInjectionImpl : ISecondComponent {
		IFirstComponent component;

		[InjectDependency]
		public SecondComponentConstructorInjectionImpl (IFirstComponent component)
		{
			this.component = component;
		}

		public IFirstComponent ReturnComponent ()
		{
			return component;
		}
	}

	class ThirdComponentFieldInjectionImpl : IThirdComponent {
		[InjectDependency] ISecondComponent component;

		public ISecondComponent ReturnComponent ()
		{
			return component;
		}
	}

	class ThirdComponentConstructorInjectionImpl : IThirdComponent {
		ISecondComponent component;

		[InjectDependency]
		public ThirdComponentConstructorInjectionImpl (ISecondComponent component)
		{
			this.component = component;
		}

		public ISecondComponent ReturnComponent ()
		{
			return component;
		}
	}

	class FieldInjectionAssembler : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (IFirstComponent), typeof (FirstComponentImpl));
			linker.WireInterface (typeof (ISecondComponent), typeof (SecondComponentFieldInjectionImpl));
			linker.WireInterface (typeof (IThirdComponent), typeof (ThirdComponentFieldInjectionImpl));
		}
	}

	class ConstructorInjectionAssembler : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (IFirstComponent), typeof (FirstComponentImpl));
			linker.WireInterface (typeof (ISecondComponent), typeof (SecondComponentConstructorInjectionImpl));
			linker.WireInterface (typeof (IThirdComponent), typeof (ThirdComponentConstructorInjectionImpl));
		}
	}

	class MixedInjectionAssembler : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (IFirstComponent), typeof (FirstComponentImpl));
			linker.WireInterface (typeof (ISecondComponent), typeof (SecondComponentFieldInjectionImpl));
			linker.WireInterface (typeof (IThirdComponent), typeof (ThirdComponentConstructorInjectionImpl));
		}	
	}

	[AssociateImplementation (typeof (FirstAttributedComponentImpl))]
	interface IFirstAttributedComponent {
	}

	[AssociateImplementation (typeof (SecondAttributedComponentImpl))]
	interface ISecondAttributedComponent {
		IFirstAttributedComponent ReturnComponent ();
	}
	
	[AssociateImplementation (typeof (ThirdAttributedComponentImpl))]
	interface IThirdAttributedComponent {
		ISecondAttributedComponent ReturnComponent ();
	}

	class FirstAttributedComponentImpl : IFirstAttributedComponent {
	}

	class SecondAttributedComponentImpl : ISecondAttributedComponent {
		[InjectDependency] IFirstAttributedComponent component;

		public IFirstAttributedComponent ReturnComponent ()
		{
			return component;
		}
	}

	class SecondNonAttributedComponentImpl : ISecondAttributedComponent {
		[InjectDependency] IFirstAttributedComponent component;

		public IFirstAttributedComponent ReturnComponent ()
		{
			return component;
		}
	}

	class ThirdAttributedComponentImpl : IThirdAttributedComponent {
		ISecondAttributedComponent component;
		
		[InjectDependency]
		public ThirdAttributedComponentImpl (ISecondAttributedComponent component)
		{
			this.component = component;
		}
		
		public ISecondAttributedComponent ReturnComponent ()
		{
			return component;	
		}
	}

	class MixedAttributedInjectionAssembler : IAssembler {
		public void AssembleComponents (Linker linker) 
		{
			linker.WireInterface (typeof (ISecondAttributedComponent), typeof (SecondNonAttributedComponentImpl));
		}
	}

	
	class UniqueInstanceComponentAssembler : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (IFirstComponent), typeof (FirstComponentImpl));
			linker.WireInterface (typeof (ISecondComponent), typeof (SecondComponentFieldInjectionImpl), true);
			linker.WireInterface (typeof (IThirdComponent), typeof (ThirdComponentConstructorInjectionImpl));
		}
	}

	interface IFirstUniqueInstanceAttributedComponent {
	}
	
	[AssociateImplementation (typeof (SecondUniqueInstanceAttributedComponentImpl))]
	interface ISecondUniqueInstanceAttributedComponent {
		IFirstUniqueInstanceAttributedComponent ReturnComponent ();
	}

	interface IThirdUniqueInstanceAttributedComponent {
		ISecondUniqueInstanceAttributedComponent ReturnComponent ();
	}

	[UniqueInstance]
	class FirstUniqueInstanceAttributedComponent : IFirstUniqueInstanceAttributedComponent {
	}

	[UniqueInstance]
	class SecondUniqueInstanceAttributedComponentImpl : ISecondUniqueInstanceAttributedComponent {
		[InjectDependency] IFirstUniqueInstanceAttributedComponent component;

		public IFirstUniqueInstanceAttributedComponent ReturnComponent ()
		{
			return component;
		}
	}

	class ThirdUniqueInstanceAttributedComponentImpl : IThirdUniqueInstanceAttributedComponent {
		ISecondUniqueInstanceAttributedComponent component;

		[InjectDependency]
		public ThirdUniqueInstanceAttributedComponentImpl (ISecondUniqueInstanceAttributedComponent component)
		{
			this.component = component;
		}

		public ISecondUniqueInstanceAttributedComponent ReturnComponent ()
		{
			return component;
		}
	}

	class UniqueInstanceAttributedComponentAssembler : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (IFirstUniqueInstanceAttributedComponent), typeof (FirstUniqueInstanceAttributedComponent), false);
			linker.WireInterface (typeof (IThirdUniqueInstanceAttributedComponent), typeof (ThirdUniqueInstanceAttributedComponentImpl), false);
		}
	}

	interface IFirstVariousImplementationComponent {
	}

	interface ISecondVariousImplementationComponent {
		IFirstVariousImplementationComponent ReturnOne ();
		IFirstVariousImplementationComponent ReturnTwo ();
	}

	[AssociateImplementation (typeof (ThirdVariousImplementationComponentImpl))]	
	interface IThirdVariousImplementationComponent {
		ISecondVariousImplementationComponent ReturnOne ();
		ISecondVariousImplementationComponent ReturnTwo ();
	}

	class FirstVariousImplementationComponentOneImpl : IFirstVariousImplementationComponent {
	}

	class FirstVariousImplementationComponentTwoImpl : IFirstVariousImplementationComponent {
	}

	class SecondVariousImplementationComponentOneImpl : ISecondVariousImplementationComponent {
		[InjectDependency] [One] IFirstVariousImplementationComponent componentOne;
		IFirstVariousImplementationComponent componentTwo;
		
		[InjectDependency]
		public SecondVariousImplementationComponentOneImpl ([Two] IFirstVariousImplementationComponent componentTwo)
		{
			this.componentTwo = componentTwo;
		}

		public IFirstVariousImplementationComponent ReturnOne ()
		{
			return componentOne;
		}

		public IFirstVariousImplementationComponent ReturnTwo ()
		{
			return componentTwo;
		}
	}

	class SecondVariousImplementationComponentTwoImpl : ISecondVariousImplementationComponent {
		IFirstVariousImplementationComponent componentOne;
		[InjectDependency] [Two] IFirstVariousImplementationComponent componentTwo;
		
		
		[InjectDependency]
		public SecondVariousImplementationComponentTwoImpl ([One] IFirstVariousImplementationComponent componentOne)
		{
			this.componentOne = componentOne;
		}

		public IFirstVariousImplementationComponent ReturnOne ()
		{
			return componentOne;
		}

		public IFirstVariousImplementationComponent ReturnTwo ()
		{
			return componentTwo;
		}
	}
	
	class ThirdVariousImplementationComponentImpl : IThirdVariousImplementationComponent {
		[InjectDependency] [One] ISecondVariousImplementationComponent componentOne;
		[InjectDependency] [Two] ISecondVariousImplementationComponent componentTwo;


		public ISecondVariousImplementationComponent ReturnOne ()
		{
			return componentOne;
		}
		
		public ISecondVariousImplementationComponent ReturnTwo ()
		{
			return componentTwo;
		}
	}
	
	[AttributeUsage (AttributeTargets.Field | AttributeTargets.Parameter)]
	class OneAttribute : Attribute {
	}

	[AttributeUsage (AttributeTargets.Field | AttributeTargets.Parameter)]
	class TwoAttribute : Attribute {
	}

	class VariousInstancesComponentAssembler : IAssembler {
		public void AssembleComponents (Linker linker)
		{
			linker.WireInterface (typeof (IFirstVariousImplementationComponent), typeof (OneAttribute), typeof (FirstVariousImplementationComponentOneImpl));

			linker.WireInterface (typeof (IFirstVariousImplementationComponent), typeof (TwoAttribute), typeof (FirstVariousImplementationComponentTwoImpl));
			linker.WireInterface (typeof (ISecondVariousImplementationComponent), typeof (OneAttribute), typeof (SecondVariousImplementationComponentOneImpl));
			linker.WireInterface (typeof (ISecondVariousImplementationComponent), typeof (TwoAttribute), typeof (SecondVariousImplementationComponentTwoImpl));
		}
	}
}
