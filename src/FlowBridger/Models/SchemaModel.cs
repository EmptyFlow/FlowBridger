namespace FlowBridger.Models {

    internal record SchemaModel {

        public string Version { get; init; } = "";

        public IEnumerable<MethodModel> GlobalMethods { get; init; } = Enumerable.Empty<MethodModel>();

        public Dictionary<string, string> GlobalOptions { get; init; } = new Dictionary<string, string> ();

    }

}
