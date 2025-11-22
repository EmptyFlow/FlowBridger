namespace FlowBridger.Models {

    internal record SchemaModel {

        public string Version { get; init; } = "";

        public IEnumerable<MethodModel> GlobalMethods { get; init; } = Enumerable.Empty<MethodModel>();

        public IEnumerable<MethodModel> GlobalDelegates { get; init; } = Enumerable.Empty<MethodModel> ();

        public IEnumerable<EventModel> Events { get; init; } = Enumerable.Empty<EventModel> ();

        public Dictionary<string, string> GlobalOptions { get; init; } = new Dictionary<string, string> ();

    }

}
