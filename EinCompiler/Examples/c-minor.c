
// single line comment
/* multi

line

comment*/

int variable;
int variable = 10;
int variable = 0x10;

private int variable; // one per instance
static  int variable; // one per thread
private static int variable; // one per instance and thread
global  int variable; // one per process (default)
shared  int variable; // one per executing system

export int main(int argc, const char ** argv)
{
	return 1;
}

void main(int i)
{

}

void main(int a, int b)
{

}