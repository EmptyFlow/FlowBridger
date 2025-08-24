namespace FlowBridger.Exceptions {

    public class BridgerParseException : Exception {

        public BridgerParseException () : base () {
        }

        public BridgerParseException ( string message ) : base ( message ) {
        }

        public BridgerParseException ( string message, Exception innerException ) : base ( message, innerException ) {
        }

    }

}
