
var x : int;
var a : int;
var i : int;

fn main() -> int
{
	print_str(33);
	print_str_native(33);
}

fn print_str(str : int)
{
	i = str;
	while (1)
	{
		x = read_mem(i);
		if(x) { }
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

	get -3
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

	get -3 ; Get address
	loadi
	set -4 ; Set return value

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

	get -3; Get address
	[i0:pop] syscall [ci:1]

	bpget
	spset
	bpset
	jmpi
]]