using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlowBridger.Export {

    public static partial class FlowBridgerExports {

        public static partial double DigitalMethodInternal ( int int32Value, long int64Value, float floatValue, double doubleValue, uint uint32Value, ulong uint64Value ) {
            var isCorrect = int32Value == 1 && int64Value == 1000 && floatValue == 0.5f && doubleValue == 10.89 && uint32Value == 256 && uint64Value == 5559;
            Console.WriteLine ( $"Passed is {( isCorrect ? "correct" : "incorrect" )} parameters" );

            return isCorrect ? 138.890 : 0;
        }

        public static partial string StringMethodInternal ( string stringAnsi, string stringUni ) {
            if ( stringAnsi == "lalalalal" && stringUni == "Lalalala1212" ) return "return string123";

            return "-";
        }

        public static partial bool CallbackMethodInternal ( VoidDelegate callbackWithVoid, Int32Delegate callbackWithoutVoid ) {
            callbackWithVoid ( 100, 400 );

            var returnInt = callbackWithoutVoid ( 1000, 232323 );

            return returnInt == 10;
        }

        public static partial bool PointerMethodInternal ( nint arrayPointer, int length ) {
            var offsetSize = Marshal.SizeOf<int> ();
            var array = new List<int> ( length );
            for ( var i = 0; i < length; i++ ) {
                Console.WriteLine ( "Element offset: " + ( offsetSize * i ) );
                array.Add ( Marshal.ReadInt32 ( arrayPointer, offsetSize * i ) );
                Console.WriteLine ( "Read element: " + array.Last () );
            }
            var expectedArray = new List<int> { 500, 3489, 126890, 565767, 984545 };
            return expectedArray.SequenceEqual ( array );
        }

        [UnmanagedCallersOnly ( CallConvs = new[] { typeof ( CallConvCdecl ) } )]
        public static void MyManagedCallback ( int value ) {
            Console.WriteLine ( $"Managed callback received! {value}" );
        }

        public static partial nint CallbackReturnMethodInternal () {
            try {
                unsafe {
                    delegate* unmanaged[Cdecl]< int, void > fp = &MyManagedCallback;
                    return (nint) fp;
                }
            } catch(Exception e) {
                Console.WriteLine ( e.Message );
                return 0;
            }
        }

        public static partial void EventClick1SetOrderInternal ( int eventId, int value ) {

        }

        public static partial void EventClick1SetNameInternal ( int eventId, string value ) {

        }

        public static partial int EventClick1CreateInternal () {
            return 0;
        }

        public static partial void EventClick1CompleteSetInternal ( int eventId ) {

        }

        public static partial int EventClick1GetOrderInternal ( int eventId ) {
            return 0; 
        }

        public static partial string EventClick1GetNameInternal ( int eventId ) {
            return "";
        }

        public static partial nint EventClick1CallbackGetInternal ( int eventId ) {
            return nint.Zero;
        }

        public static partial void EventClick1CompleteGetInternal ( int eventId ) {

        }

        public static partial void EventClick2SetOrderInternal ( int eventId, int value ) {

        }

        public static partial void EventClick2SetItemsInternal ( int eventId, int value ) {

        }

        public static partial int EventClick2CreateInternal () {
            return 0;
        }

        public static partial void EventClick2CompleteSetInternal ( int eventId ) {

        }

        public static partial int EventClick3GetOrderInternal ( int eventId ) {
            return 0;
        }

        public static partial int EventClick3GetItemsInternal ( int eventId ) {
            return 0;
        }

        public static partial nint EventClick3CallbackGetInternal ( int eventId ) {
            return 0;
        }

        public static partial void EventClick3CompleteGetInternal ( int eventId ) {

        }

    }

}