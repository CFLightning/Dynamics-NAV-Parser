namespace NAV_Comment_tool.parserClass
{
    class ChangeClass
    {
        private string changelogCode;
        private string contents;

        public ChangeClass()
        {
        }
        public ChangeClass(string code, string content)
        {
            this.ChangelogCode = code;
            this.contents = content;
        }
        public string Contents { get => contents; set => contents = value; }
        public string ChangelogCode { get => changelogCode; set => changelogCode = value; }
    }
}
