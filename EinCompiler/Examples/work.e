﻿
// Module Support planned:
// import "";
// import "" as name;

include("das-os.e");

/*
fn main() -> int
{
	arg_order_test('a', 'b', 'c');
	putc('d');
	putc('e');
	putc('f');

	exit(0);
}
*/

var i : int;
var x : int;

fn main() -> int
{
	print_str(33);
	putc('\n');
	print_str_native(33);
}

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

export naked fn print_str_native(str : int)
[[
	bpget
	spget
	bpset

	get -2
print_str_loop:
	[i0:peek] loadi[f:yes]
	[ex(z)=1] jmp @print_str_end_loop
	[i0:pop] syscall[ci:1]
	[i0:arg] add 1
	jmp @print_str_loop
print_str_end_loop:
	drop
	drop

	bpget
	spset
	bpset
	jmpi
]]

/*
export naked fn read_mem(ptr : int) -> int
[[
	bpget
	spget
	bpset
	get -2
	loadi
	set -3
	bpget
	spset
	bpset
	jmpi
]]
*/


/**
* Should output 'acdehkn'
**
fn relational_test()
{
	a := 10;
	if (a = 10) {
		putc('a');
	}
	if (a != 10) {
		putc('b');
	}

	if (a >= 10) {
		putc('c');
	}
	if (a <= 10) {
		putc('d');
	}

	if (a >= 9) {
		putc('e');
	}
	if (a <= 9) {
		putc('f');
	}

	if (a >= 11) {
		putc('g');
	}
	if (a <= 11) {
		putc('h');
	}




	if (a > 10) {
		putc('i');
	}
	if (a < 10) {
		putc('j');
	}

	if (a > 9) {
		putc('k');
	}
	if (a < 9) {
		putc('l');
	}

	if (a > 11) {
		putc('m');
	}
	if (a < 11) {
		putc('n');
	}
}
//*/