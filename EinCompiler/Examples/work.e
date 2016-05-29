include("das-os.e");

var scalar : int;
var array  : int[10];

fn main() -> int
	var i : int;
{
	scalar := 'A';
	putc(scalar);
	putc(arrayToInt(array));
	putc('B');
}

naked inline fn arrayToInt(a : int[10]) -> int [[ ]]

// Module Support planned:
// import "";
// import "" as name;