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

    }
}