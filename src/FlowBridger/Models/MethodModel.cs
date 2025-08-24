namespace FlowBridger.Models {

    internal record MethodModel {

        public string Name { get; init; } = "";

        public IDictionary<string, string> Options { get; init; } = new Dictionary<string, string> ();

        public DataTypeModel? ReturnType { get; init; }

        public IEnumerable<MethodParameterModel> Parameters { get; init; } = Enumerable.Empty<MethodParameterModel> ();

    }

}
