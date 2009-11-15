AC_INIT([babuine-componentmodel], [0.4.9.0])
AC_CANONICAL_SYSTEM
AM_INIT_AUTOMAKE([tar-ustar])
AM_MAINTAINER_MODE

AC_PROG_INSTALL

dnl pkg-config
AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
	AC_MSG_ERROR([You need to install pkg-config])
fi

AC_PATH_PROG(CSC, csc, no)
AC_PATH_PROG(MCS, gmcs, no)
AC_PATH_PROG(MONO, mono, no)

CS="C#"
if test "x$CSC" = "xno" -a "x$MCS" = "xno"  ; then
	AC_MSG_ERROR([No $CS compiler found])
fi

if test "x$MCS" = "xno" ; then
	MCS=$CSC
fi

if test "x$MONO" = "xno"; then
	AC_MSG_ERROR([No mono runtime found])
fi

AC_SUBST(MCS)

AC_OUTPUT([
Makefile
src/Babuine.ComponentModel/Makefile
src/Babuine.ComponentModel/AssemblyInfo.cs
src/Babuine.ComponentModel/babuine-componentmodel.pc
])
