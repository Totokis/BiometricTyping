using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTest.Model
{
    public enum DataType
    {
        KeyPressedSpeed, 
        KeyHoldedTime,
        Unknown,
    }

    static class DataTypeCast
    {
        public static Dictionary<DataType, String> DataTypeToString = new Dictionary<DataType, string>()
        {
            [DataType.KeyPressedSpeed] = "Pressed in",
            [DataType.KeyHoldedTime] = "Holded for",
            [DataType.Unknown] = "UNKNOWN",
        };
    }

    public struct DataChunk
    {

        public String Key { get; set; }
        public DataType DataType { get; set; } 
        public Int64 TimeMs { get; set; }


        public override string ToString()
        {
            return $"{Key} {DataTypeCast.DataTypeToString[DataType]} {TimeMs}";
        }
    }
}
