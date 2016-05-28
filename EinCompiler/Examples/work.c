var x : int;
var a : int;
var i : int;

fn main() -> int
{
	print_str(33);
	putc(9);
	print_str_native(33);
}

fn print_str(str : int)
{
	i = str;
	while (1)
	{
		x = read_mem(i);
		if (x) {}
		else {
			break;
		}
		putc(x);
		i = i + 1;
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
export naked fn putc(ptr : int)
[[
	bpget
	spget
	bpset
	get -2
	[i0:pop] syscall[ci:1]
	bpget
	spset
	bpset
	jmpi
]]