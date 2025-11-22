version 1.0

# delegates
globaldelegate VoidDelegate
Int32Value int32
Int64Value int64

globaldelegate Int32Delegate
Int32Value int32
Int64Value int64
returnType int32

globaldelegate SimpleDelegate
Int32Value int32

# methods

globalmethod DigitalMethod
Int32Value int32
Int64Value int64
FloatValue float
DoubleValue double
Uint32Value uint32
Uint64Value uint64
returnType double

globalmethod StringMethod
StringAnsi stringansi
StringUni stringuni
returnType stringansi

globalmethod CallbackMethod
CallbackWithVoid method-VoidDelegate
CallbackWithoutVoid method-Int32Delegate
returnType boolean

globalmethod PointerMethod
ArrayPointer pointer
Length int32
returnType boolean

globalmethod CallbackReturnMethod
returnType method-SimpleDelegate

# events

event-inout Click1
Order int32
Name stringansi

event-in Click2
Order int32
Items int32-array

event-out Click3
Order int32
Items int32-array