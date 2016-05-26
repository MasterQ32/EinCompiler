
// single line comment
/* multi

line

comment*/

/*
	constants are typed values that can't be changed and
	must be initialized. Their address can't be taken.
*/
const ANSWER : int = 42;

const ANSWER_2 : int = 84;

const ANSWER_1over2 : int = 21;

/*
	variables are a typed mutable value.
	It's declaration consists of a type and a name
	and may also define an initial value.
*/
var variable : int;
var variable : int = 10;
var variable : int = 0x10;

/*
	variables may also have a storage class
	specifier that defines where the variable is
	stored and how it's memory is shared.
*/
private var variable : int; // one per instance
static  var variable : int ; // one per thread
private static var  variable : int; // one per instance and thread
global  var variable : int ; // one per process (default)
shared  var variable : int ; // one per executing system

/* 
	methods are declared by a type and a name
	followed by an argument list enclosed in
	round brackets.

	parameters are separated by commands are are
	declared ba a type followed by the parameter
	name.

	each method header declaration must follow a method
	body enclosed in curly brackets.

	each method can be exported by placing an 'export'
	keyword in front of the method header.
*/
fn main() -> int
{
	1; // RawLiteralExpressionNode
	variable; // RawVariableExpressionNode
	; // NopInstructionNode
	1 + 2;
	a + 1;
	1 + a;
	a + a;
	1 + 2 + 3;
	1 * 2 + 3;
	1 + 2 * 3;
	(1);
	(a);
	(1 + 2);
	(1 * (2 + 3));
	(1 * (2 - (3 + 4)));
}

fn square(i : int) -> int
{
//	return i * i;
}

export fn sum(a : int, b : int) -> int
{
//	return a + b;
}