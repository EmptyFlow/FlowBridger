namespace FlowBridger.Models.ConsoleCommands {

    internal class GenerateFilesCommand {

        public IEnumerable<string> Languages { get; set; } = [];

        public string Schema { get; set; } = "";

    }

}
