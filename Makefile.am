SUBDIRS = src/Babuine.ComponentModel
DISTCLEANFILES = Makefile.in

run-test:
	for ASM in $(SUBDIRS); do \
		cd $$ASM;	\
		make run-test;	\
		cd ..;		\
	done;

doc: all
	monodocer -assembly:src/Babuine.ComponentModel/Babuine.ComponentModel.dll -path:doc
