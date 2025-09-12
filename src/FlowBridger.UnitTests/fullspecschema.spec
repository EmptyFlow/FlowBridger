version 1.0

globaldelegate VoidDelegate
Int32Value int32
Int64Value int64

globaldelegate Int32Delegate
Int32Value int32
Int64Value int64
ReturnType int32

globalmethod DigitalMethod
Int32Value int32
Int64Value int64
FloatValue float
DoubleValue double
Uint32Value uint32
Uint64Value uint64
ReturnType double

globalmethod StringMethod
StringAnsi stringAnsi
StringUni stringUni
ReturnType stringAnsi

globalmethod CallbackMethod
CallbackWithVoid method-VoidDelegate
CallbackWithoutVoid method-Int32Delegate
ReturnType boolean