using FlowBridger.Models;
using System.Text.Json.Serialization;

namespace FlowBridger.Generators {

    [JsonSerializable ( typeof ( SchemaModel ) )]
    internal partial class GeneratorSerializerContext : JsonSerializerContext {
    }

}
