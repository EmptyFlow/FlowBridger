version 1.0

globaloption CsEventClass enabled

# delegates
globaldelegate VoidDelegate
int32 Int32Value
int64 Int64Value

globaldelegate Int32Delegate
int32 Int32Value
int64 Int64Value
returnType int32

globaldelegate SimpleDelegate
int32 Int32Value

# methods

globalmethod DigitalMethod
int32 Int32Value
int64 Int64Value
float FloatValue
double DoubleValue
uint32 Uint32Value
uint64 Uint64Value
returnType double

globalmethod StringMethod
stringansi StringAnsi
stringuni StringUni
returnType stringansi

globalmethod CallbackMethod
method-VoidDelegate CallbackWithVoid
method-Int32Delegate CallbackWithoutVoid
returnType boolean

globalmethod PointerMethod
pointer ArrayPointer
int32 Length
returnType boolean

globalmethod CallbackReturnMethod
returnType method-SimpleDelegate

# events

event-inout Click1
int32 Order
stringansi Name

event-in Click2
int32 Order
int32-array Items

event-out Click3
int32 Order
int32-array Items