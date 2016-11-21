; Emulator Runtime:
jmp @_start
ret ; Interrupt Handler 1
ret ; Interrupt Handler 2
ret ; Interrupt Handler 3
ret ; Interrupt Handler 4
ret ; Interrupt Handler 5
ret ; Interrupt Handler 6
ret ; Interrupt Handler 7
ret ; Interrupt Handler 8

_start:
	push @_halt
	jmp @main

_halt:
	syscall [ci:1] [i0:arg] '\n'
	syscall [ci:0]
	[i0:arg] push -4 [r:jumpr]