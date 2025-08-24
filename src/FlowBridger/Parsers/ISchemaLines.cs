namespace FlowBridger.Parsers {

    internal interface ISchemaLines {

        string GetLastLine ();

        void TakeNextLine ();

        bool IsEnd ();

        bool IsEndScheme ();

        void ThrowError ( string section, string message );

    }

}
