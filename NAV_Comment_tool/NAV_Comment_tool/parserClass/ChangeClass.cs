﻿namespace NAV_Comment_tool.parserClass
{
    class ChangeClass
    {
        private string changelogCode;
        private string contents;
        private string changeType; // code, field, maybe function

        public ChangeClass()
        {
        }
        public ChangeClass(string code, string content, string type)
        {
            this.ChangelogCode = code;
            this.contents = content;
            this.changeType = type;
        }
        public string Contents { get => contents; set => contents = value; }
        public string ChangelogCode { get => changelogCode; set => changelogCode = value; }
        public string ChangeType { get => changeType; set => changeType = value; }
    }
}
