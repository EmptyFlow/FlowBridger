using FlowBridger.Exceptions;
using System.Runtime.CompilerServices;

namespace FlowBridger.Parsers {

    internal class DefaultSchemaLines : ISchemaLines {

        private readonly List<string> m_lines = new List<string> ();

        private int m_index = 0;

        public DefaultSchemaLines ( IEnumerable<string> lines ) => m_lines.AddRange ( lines );

        public string GetLastLine () => m_lines[m_index];

        public bool IsEnd () {
            if ( IsEndScheme () ) return true;
            if ( string.IsNullOrEmpty ( m_lines[m_index].Trim () ) ) return true;

            return false;
        }

        public bool IsEndScheme () => m_index >= m_lines.Count ();

        public void TakeNextLine () {
            if ( m_index == m_lines.Count ) return;

            m_index++;
        }

        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        public void ThrowError ( string section, string message ) => throw new BridgerParseException ( $"({section}) {message}, line {m_index + 1}" );

    }

}
