# EinCompiler Type System

This document describes the type system with its primitive types that is used by EinCompiler.

## Integer Types

| Name    | Size in Bits | Signed |
|---------|--------------|--------|
| i8      | 8            | yes    |
| i16     | 16           | yes    |
| i32     | 32           | yes    |
| i64     | 64           | yes    |
| u8      | 8            | no     |
| u16     | 16           | no     |
| u32     | 32           | no     |
| u64     | 64           | no     |
| int     | System Width | yes    |
| uint    | System Width | no     |

Each integer has a subscript `'size` which contains the size in bytes of the type and a subscript `'signed` which returns a bool saying if the integer is signed or not.

## Floating Point Types

| Name    | Size in Bits |
|---------|--------------|
| half    | 16           |
| single  | 32           |
| double  | 64           |
| float   | System Width |

Each floating point type has a subscript `'size` which contains the size in bytes of the type. There are also the subscripts `'exponent` and `'mantissa` which will return the size of either exponent or the mantissa of the floating point value in bits.

## String Type

| Name    | Storage Mode     |
|---------|------------------|
| string  | Length + Pointer |
| cstring | Nulltermination  |

### string
A `string` is a reference-counted block of memory, with a defined size. It has a subscript `.size` which denotes the length of the memory block and a subscript `.data` which references to the raw string data as a pointer to an `u8`'

The data of a `string` is implicitly allocated on creation and reference counted. As soon as the reference count reaches zero, the string data will be freed.

### cstring
A `cstring` is a raw pointer to an `u8` where as the length of the string is defined by the distance to the first `0` value in the string.

## Pointer Type
A pointer is an address value that references either `nullptr`, the invalid pointer, or a memory address where the value is located.

The naming scheme for a pointer type is `ptr(base)` where `base` is the type of the value that is referenced. Each pointer has a subscript `.data` which is the referenced value.

## Reference Type
A reference is a reference-counted, implicit pointer type that will reference any kind of values except references.

A reference can be allocated by a language-dependant mechanism. The type is denoted by `ref(base)` where `base` is the referenced type and exposes all subscripts and operators of the `base` type.

References can take the special value `null` which marks that the reference does not contain any referenced data. Accessing a `null`-value will result in undefined behaviour.

Assignments to a reference type from a value type will override the referenced value, assignments from a reference type to another reference type will change the referenced object. Assignments from a reference type to a non-reference value will behave the same as a non-referenced assignment.

## Enumeration Type
An enumeration type is a type that can only accept a defined list of distinct values. The enumeration type can be mapped to an underlying integer type that allows the size definition of the type as well as the maximum number of enumeration values. Each enumeration value can be associated with an integer to allow easy interfacing with storage.

Every enumeration value has the subscript `'size` which returns the size of the type in bytes and the subscript `'string` which will return the enumerations value name as a string representation. The subscript `'cstring` will return the same value as `'string` but of the type `cstring`'.

## Record Type
A record type is a compound type defined by a list of (name, type) tuples. Each *name* will be available as a `.name` subscript representing a value of the *type*.

The record type also provides the subscript `'size` which returns the size of the type.

## Array Type
An array type is a list of values of the same type. Each array has a length accessible by `.length` and the values are accessible by `[index]`.

The total size of the array can be accessed by `'size`.

## Alias List

| Type Name | Aliases             |
|-----------|---------------------|
| single    | float               |