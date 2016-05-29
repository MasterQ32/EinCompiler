include("das-os.e");

var scalar : int;
var array  : int[10];

fn main() -> int
	var i : int;
{
	scalar := 'A';
	putc(scalar);
	putc(array);
	putc('B');
}

// Module Support planned:
// import "";
// import "" as name;