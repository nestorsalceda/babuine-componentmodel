componentmodeldir=$(pkglibdir)
componentmodel_SCRIPTS = Babuine.ComponentModel.dll
EXTRA_DIST = $(componentmodel_sources) $(componentmodel_sources_in) $(test_build_sources) babuine-componentmodel.pc.in
CLEANFILES = *.dll *.mdb TestResult.xml
DISTCLEANFILES = AssemblyInfo.cs Makefile.in

pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = babuine-componentmodel.pc

componentmodel_sources_in = AssemblyInfo.cs.in
componentmodel_generated_sources = $(componentmodel_sources_in:.in=)
componentmodel_sources = \
	AssociateImplementationAttribute.cs \
	IAssembler.cs \
	Identifier.cs \
	InjectDependencyAttribute.cs \
	Linker.cs \
	ObjectProvider.cs \
	ObjectProviderFactory.cs \
	UniqueInstanceAttribute.cs 

componentmodel_build_sources = $(addprefix $(srcdir)/, $(componentmodel_sources))
componentmodel_build_sources += $(componentmodel_generated_sources)

Babuine.ComponentModel.dll: $(componentmodel_build_sources)
	$(MCS) -debug -target:library -out:$@ $(componentmodel_build_sources)

test_sources = \
	ConstructorInjectionTest.cs \
	FieldInjectionTest.cs \
	InjectionComponents.cs \
	LinkerTest.cs \
	MixedAttributedInjectionTest.cs \
	MixedInjectionTest.cs \
	UniqueInstanceAttributedInjectionTest.cs \
	UniqueInstanceInjectionTest.cs \
	VariousInstancesSelectionInjectionTest.cs \
	InstancedFieldInjectionTest.cs \
	InstancedConstructorInjectionTest.cs \
	UniqueInstanceInstancedInjectionTest.cs \
	DBusInjectionTest.cs \
	VariousInstancesInstancedSelectionInjectionTest.cs

test_build_sources = $(addprefix $(srcdir)/Test/, $(test_sources))

Babuine.ComponentModel.Test.dll: $(test_build_sources) Babuine.ComponentModel.dll
	$(MCS) -debug -target:library -r:../Babuine.ComponentModel/Babuine.ComponentModel.dll -pkg:nunit -pkg:ndesk-dbus-1.0 -out:$@ $(test_build_sources)

run-test: Babuine.ComponentModel.Test.dll
	/usr/bin/nunit-console  Babuine.ComponentModel.Test.dll
