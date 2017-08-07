namespace NAV_Comment_tool.parserClass
{
    class ChangeClass
    {
        private string changelogCode;
        private string contents;
        private string changeType;  // code, field, maybe function
        private string location;    //  trigger name or function name
        private string sourceObject;

        public ChangeClass()
        {
        }
        public ChangeClass(string code, string content, string type, string name, string sObject)
        {
            this.ChangelogCode = code;
            this.contents = content;
            this.changeType = type;
            this.location = name;
            this.sourceObject = sObject;
        }
        public string Contents { get => contents; set => contents = value; }
        public string ChangelogCode { get => changelogCode; set => changelogCode = value; }
        public string ChangeType { get => changeType; set => changeType = value; }
        public string Location { get => location; set => location = value; }
        public string SourceObject { get => sourceObject; set => sourceObject = value; }
    }
}
