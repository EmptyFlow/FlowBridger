namespace FlowBridger.Export {

    public static partial class FlowBridgerExports {

        public static partial double DigitalMethodInternal ( int int32Value, long int64Value, float floatValue, double doubleValue, uint uint32Value, ulong uint64Value ) {
            var isCorrect = int32Value == 1 && int64Value == 1000 && floatValue == 0.5f && doubleValue == 10.89 && uint32Value == 256 && uint64Value == 5559;
            Console.WriteLine ( $"Passed is {(isCorrect ? "correct" : "incorrect")} parameters" );

            return isCorrect ? 138.890 : 0;
        }

        public static partial string StringMethodInternal ( string stringAnsi, string stringUni ) {
            if ( stringAnsi == "lalalalal" && stringUni == "Lalalala1212" ) return "return string123";

            return "-";
        }

        public static partial bool CallbackMethodInternal ( VoidDelegate callbackWithVoid, Int32Delegate callbackWithoutVoid ) {
            return false;
        }

    }

}