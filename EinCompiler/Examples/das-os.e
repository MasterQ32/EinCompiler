
inline fn putc(chr : int)
[[
	[i0:pop] syscall[ci:1]
]]

inline fn read8(address : ptr) -> int
[[
	loadi [ci:0]
]]

inline fn read16(address : ptr) -> int
[[
	loadi [ci:1]
]]

inline fn read32(address : ptr) -> int
[[
	loadi [ci:2]
]]

inline fn exit(exitCode : int)
[[
	[i0:pop] syscall[ci:1]
]]