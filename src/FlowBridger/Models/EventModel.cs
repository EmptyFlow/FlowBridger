namespace FlowBridger.Models {

    internal record EventModel : MethodModel {

        public bool InEvent { get; set; }

        public bool OutEvent { get; set; }

    }

}
