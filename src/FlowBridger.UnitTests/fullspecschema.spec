version 1.0

globaldelegate VoidDelegate
Int32Value int32
Int64Value int64

globaldelegate Int32Delegate
Int32Value int32
Int64Value int64
returnType int32

globalmethod DigitalMethod
Int32Value int32
Int64Value int64
FloatValue float
DoubleValue double
Uint32Value uint32
Uint64Value uint64
returnType double

globalmethod StringMethod
StringAnsi stringAnsi
StringUni stringUni
returnType stringAnsi

globalmethod CallbackMethod
CallbackWithVoid method-VoidDelegate
CallbackWithoutVoid method-Int32Delegate
returnType boolean