using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace TypingTest.Model
{
    public class KeyInfo
    {
        public Key Key { get; set; }
        public Stopwatch Stopwatch { get; set; }

        public KeyInfo(Key key)
        {
            Key = Key;
            Stopwatch = new Stopwatch();
        }
    }

}
