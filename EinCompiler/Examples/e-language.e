﻿
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
var variable1 : int;
var variable2 : int = 10;
var variable3 : int = 0x10;

/*
	variables may also have a storage class
	specifier that defines where the variable is
	stored and how it's memory is shared.
*/
private        var variable4 : int; // one per instance
static         var variable5 : int; // one per thread
private static var variable6 : int; // one per instance and thread
global         var variable7 : int; // one per process (default)
shared         var variable8 : int; // one per executing system

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

var x : int;
var a : int;
var i : int;

fn main() -> int
{
	
}

fn test_expressions()
{
	x = 1; 
	x = variable1; 
	x = 1 + 2;
	x = a + 1;
	x = 1 + a;
	x = a + a;
	x = 1 + 2 + 3;
	x = 1 * 2 + 3;
	x = 1 + 2 * 3;
	x = (1);
	x = (a);
	x = (1 + 2);
	x = (1 * (2 + 3));
	x = (1 * (2 - (3 + 4)));
}

fn test_return()
{
	return i * i;
}

fn test_if()
{
	if(1 + 2)
	{
		i = x;
		i = x;
	}

	if(1 + 2)
	{
		i = x;
		i = x;
	}
	else
	{
		i = x;
	}
}

fn test_while()
{
	while (i * i)
	{
		i = x;
		i = 10 * 10;
	}

	while(i * i)
	{
		break;
	}
}

fn test_call()
{
	sum(10, 5 + 5);
	i = sum(10, 5 + 5);
}

export fn sum(a : int, b : int) -> int
{
	return a + b;
}

fn square(i : int) -> int
{
	return i * i;
}

export naked fn print_str(str : int)
[[
	bpget
	spget
	bpset

	get -2
print_str_loop:
	[i0:peek] loadi [f:yes] ; write flags, also load result, don't discard pointer
	[ex(z)=1] jmp @print_str_end_loop ; when *ptr == 0, then goto print_str_end_loop
	[i0:pop] syscall [ci:1] ; removes the loaded character and prints it
	[i0:arg] add 1 ; adds 1 to the stack top
	jmp @print_str_loop
print_str_end_loop:
	drop ; discard the result from loadi
	drop ; discard our pointer
	
	bpget ; leave function
	spset ; by restoring parent base pointer
	bpset
	jmpi  ; and jumping back.
]]

export naked fn read_mem(ptr : int) -> int
[[
	bpget
	spget
	bpset

	get -2 ; Get address
	loadi
	set -3 ; Set return value

	bpget
	spset
	bpset
	jmpi
]]



/**
 * Unoptimized string printing
 */
fn print_str(str : int)
	var c : int;
{
	while (1)
	{
		c := read8(str);
		if (c = 0) {
			break;
		}
		putc(c);
		str := str + 1;
	}
}