using System.Runtime.InteropServices;
using System.Text;

namespace FlowBridger.Export {
    public static partial class FlowBridgerExports {
        private static List<byte> m_zeroBytes = [0, 0, 0, 0];

        private static string GetUniStringFromPointer ( nint pointer ) {
            if ( RuntimeInformation.IsOSPlatform ( OSPlatform.Linux ) || RuntimeInformation.IsOSPlatform ( OSPlatform.OSX ) ) {
                if ( pointer == nint.Zero ) return "";

                var buffer = new List<byte> ();
                var offset = 0;
                while ( true ) {
                    var readedByte = Marshal.ReadByte ( pointer, offset );
                    offset++;

                    buffer.Add ( readedByte );

                    if ( buffer.Count () % 4 == 0 && buffer[^4..].SequenceEqual ( m_zeroBytes ) ) break;
                }

                if ( buffer.Count () == 4 && buffer.SequenceEqual ( m_zeroBytes ) ) return "";

                return Encoding.UTF8.GetString ( buffer.ToArray () ).Replace ( "\u0000", "" );
            }

            return Marshal.PtrToStringUni ( pointer ) ?? "";
        }

        public delegate void VoidDelegate ( int int32Value, long int64Value );

        public delegate int Int32Delegate ( int int32Value, long int64Value );

        public delegate void SimpleDelegate ( int int32Value );

        public delegate void EventClick1Callback ( int eventId );

        public delegate void EventClick3Callback ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "digital_method" )]
        public static double DigitalMethod ( int int32Value, long int64Value, float floatValue, double doubleValue, uint uint32Value, ulong uint64Value ) {
            return DigitalMethodInternal(int32Value, int64Value, floatValue, doubleValue, uint32Value, uint64Value);
        }

        public static partial double DigitalMethodInternal ( int int32Value, long int64Value, float floatValue, double doubleValue, uint uint32Value, ulong uint64Value );

        [UnmanagedCallersOnly ( EntryPoint = "string_method" )]
        public static nint StringMethod ( nint stringAnsi, nint stringUni ) {
            return Marshal.StringToHGlobalAnsi(StringMethodInternal(Marshal.PtrToStringAnsi(stringAnsi) ?? "", GetUniStringFromPointer(stringUni)));
        }

        public static partial string StringMethodInternal ( string stringAnsi, string stringUni );

        [UnmanagedCallersOnly ( EntryPoint = "callback_method" )]
        public static bool CallbackMethod ( nint callbackWithVoid, nint callbackWithoutVoid ) {
            return CallbackMethodInternal(Marshal.GetDelegateForFunctionPointer<VoidDelegate>(callbackWithVoid), Marshal.GetDelegateForFunctionPointer<Int32Delegate>(callbackWithoutVoid));
        }

        public static partial bool CallbackMethodInternal ( VoidDelegate callbackWithVoid, Int32Delegate callbackWithoutVoid );

        [UnmanagedCallersOnly ( EntryPoint = "pointer_method" )]
        public static bool PointerMethod ( nint arrayPointer, int length ) {
            return PointerMethodInternal(arrayPointer, length);
        }

        public static partial bool PointerMethodInternal ( nint arrayPointer, int length );

        [UnmanagedCallersOnly ( EntryPoint = "callback_return_method" )]
        public static nint CallbackReturnMethod (  ) {
            return CallbackReturnMethodInternal();
        }

        public static partial nint CallbackReturnMethodInternal (  );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_set_order" )]
        public static void EventClick1SetOrder ( int eventId, int value ) {
            EventClick1SetOrderInternal(eventId, value);
        }

        public static partial void EventClick1SetOrderInternal ( int eventId, int value );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_set_name" )]
        public static void EventClick1SetName ( int eventId, nint value ) {
            EventClick1SetNameInternal(eventId, Marshal.PtrToStringAnsi(value) ?? "");
        }

        public static partial void EventClick1SetNameInternal ( int eventId, string value );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_create" )]
        public static int EventClick1Create (  ) {
            return EventClick1CreateInternal();
        }

        public static partial int EventClick1CreateInternal (  );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_complete_set" )]
        public static void EventClick1CompleteSet ( int eventId ) {
            EventClick1CompleteSetInternal(eventId);
        }

        public static partial void EventClick1CompleteSetInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_get_order" )]
        public static int EventClick1GetOrder ( int eventId ) {
            return EventClick1GetOrderInternal(eventId);
        }

        public static partial int EventClick1GetOrderInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_get_name" )]
        public static nint EventClick1GetName ( int eventId ) {
            return Marshal.StringToHGlobalAnsi(EventClick1GetNameInternal(eventId));
        }

        public static partial string EventClick1GetNameInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_callback_get" )]
        public static nint EventClick1CallbackGet ( int eventId ) {
            return EventClick1CallbackGetInternal(eventId);
        }

        public static partial nint EventClick1CallbackGetInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click1_complete_get" )]
        public static void EventClick1CompleteGet ( int eventId ) {
            EventClick1CompleteGetInternal(eventId);
        }

        public static partial void EventClick1CompleteGetInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click2_set_order" )]
        public static void EventClick2SetOrder ( int eventId, int value ) {
            EventClick2SetOrderInternal(eventId, value);
        }

        public static partial void EventClick2SetOrderInternal ( int eventId, int value );

        [UnmanagedCallersOnly ( EntryPoint = "event_click2_set_items" )]
        public static void EventClick2SetItems ( int eventId, int value ) {
            EventClick2SetItemsInternal(eventId, value);
        }

        public static partial void EventClick2SetItemsInternal ( int eventId, int value );

        [UnmanagedCallersOnly ( EntryPoint = "event_click2_create" )]
        public static int EventClick2Create (  ) {
            return EventClick2CreateInternal();
        }

        public static partial int EventClick2CreateInternal (  );

        [UnmanagedCallersOnly ( EntryPoint = "event_click2_complete_set" )]
        public static void EventClick2CompleteSet ( int eventId ) {
            EventClick2CompleteSetInternal(eventId);
        }

        public static partial void EventClick2CompleteSetInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click3_get_order" )]
        public static int EventClick3GetOrder ( int eventId ) {
            return EventClick3GetOrderInternal(eventId);
        }

        public static partial int EventClick3GetOrderInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click3_get_items" )]
        public static int EventClick3GetItems ( int eventId ) {
            return EventClick3GetItemsInternal(eventId);
        }

        public static partial int EventClick3GetItemsInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click3_callback_get" )]
        public static nint EventClick3CallbackGet ( int eventId ) {
            return EventClick3CallbackGetInternal(eventId);
        }

        public static partial nint EventClick3CallbackGetInternal ( int eventId );

        [UnmanagedCallersOnly ( EntryPoint = "event_click3_complete_get" )]
        public static void EventClick3CompleteGet ( int eventId ) {
            EventClick3CompleteGetInternal(eventId);
        }

        public static partial void EventClick3CompleteGetInternal ( int eventId );

    }
}