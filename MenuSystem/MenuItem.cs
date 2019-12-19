using System;
using System.Collections.Generic;

namespace MenuSystem
{
    public class MenuItem
    {
        private string _command;
        private string _title;

        public string Title
        {
            get => _title;
            set => _title = Validate(0, 100, true, value);
        }

        public string Command
        {
            get => _command;
            set => _command = Validate(0, 10, true, value);
        }

        private string Validate(int minLength, int maxLength, bool toUpper, string item)
        {
            item = item.Trim();
            if (toUpper)
            {
                item.ToUpper();
            }
            if (item.Length > maxLength || item.Length < minLength)
            {
                throw new ArgumentException($"Command is not correct length ({minLength}-{maxLength}). " +
                                            $"Got {item.Length} characters");
            }

            return item;
        }

        public Func<string> CommandToExecute { get; set; }
        
        

        public override string ToString()
        {
            return Command + " " + Title;
        }
    }
}