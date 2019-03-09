# Bignum-Arithmetics #

Simple library to proceed numbers that overflow all numeric types in C#

There is a BigNumber abstract class that can be used as a base for different big number classes.
- It has abstract functions for simple operators: '+', '-', '*', '/', '%'
- There are also two already implemented child classes, BigInteger and BigDecimal. 
- You can also convert them between each other.

BigDecimal class has a precision representing number of fractional digits for division operation, which is a static variable that can be changed.

Project contains extension class for List<int>, whose methods do operations for reversed lists representing digits. These are used actively. You may change them. For example implement some known algorithms in multiplication method in order to fastern calculations.

There is an abstract RPNParser<T> class for calculating string expressions for BigNumber children.
- Expression can contain any whitespaces, numbers, operators, brackets and custom functions. 
- This class can be inherited for any custom BigNumber and all you need to do is implement two functions: 
-- first is creating an instance of your class 
-- and second is splitting string into lexems combining parent regex, which is stored in regexFormat variable, and regex for your class.
- You may add some custom unary functions there, folowing the example of abs
- There are two parser classes already implemented: BigIntegerRPNParser and BigDecimalRPNParser. You may have a look at their implementation as a code sample.

There is also a project named BigNumArithmeticsCoreLib.Tests included. It runs library tests
Code is filled with xml documentation.
