using FlowBridger.Enums;

namespace FlowBridger.Models {

    internal record MethodParameterModel {

        public string Name { get; init; } = "";

        public DataTypeModel ParameterType { get; init; } = new DataTypeModel ( ParsedDataType.Unknown, ParsedContainerDataType.NotContainer );

    }

}
