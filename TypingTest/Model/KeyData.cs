using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTest.Model
{
    class KeyData
    {
        public String KeyPressed { get; set; }
        public Int64 Speed { get; set; } //time before pressed
        public Int64 Hold { get; set; }

        public KeyData() { }

        public override string ToString()
        {
            return $"{KeyPressed};{Speed};{Hold};";
        }
    }
}
