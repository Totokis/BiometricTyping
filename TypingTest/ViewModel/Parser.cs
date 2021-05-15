using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypingTest.Model;

namespace TypingTest.ViewModel
{
    static class Parser
    {
        public static List<KeyData> Parse(List<DataChunk> keyChunks)
        {
            //keyChunks.RemoveAt(0); // !!!
            List<KeyData> keyDataList = new List<KeyData>();

            while (keyChunks.Count != 0) //!!!
            {
                //last
                DataChunk examinedKeyHold = new DataChunk()
                {
                    DataType = DataType.Unknown,
                };

                //first
                DataChunk examinedKeySpeed = keyChunks.First();
                keyChunks.RemoveAt(0);


                if (examinedKeySpeed.Key == " ") examinedKeySpeed.Key = "Space";

                if (examinedKeySpeed.DataType == DataType.KeyHoldedTime)
                {

                    String keyPressed = null;

                    //if(exam )

                   KeyData keyData = new KeyData()
                    {
                        Hold = examinedKeySpeed.TimeMs,
                        KeyPressed = examinedKeySpeed.Key,
                    };

                    keyDataList.Add(keyData);
                    continue;
                }

                for (int i = 0; i < keyChunks.Count; i++)
                {
                    if (keyChunks[i].Key.ToLower() == examinedKeySpeed.Key.ToLower() || IsMatching(examinedKeySpeed.Key, keyChunks[i].Key))
                    {
                        //holding key really long results in multiple speeds and one hold, it's necessary to remove duplicates
                        if (keyChunks[i].DataType == DataType.KeyPressedSpeed)
                        {
                            keyChunks.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            examinedKeyHold = keyChunks[i]; 
                            keyChunks.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (examinedKeyHold.DataType != DataType.Unknown)
                {
                    KeyData keyData = new KeyData()
                    {
                        KeyPressed = examinedKeySpeed.Key,
                        Speed = examinedKeySpeed.TimeMs,
                        Hold = examinedKeyHold.TimeMs,
                    };

                    keyDataList.Add(keyData);
                }
            }

            return keyDataList;
        }

        private static Boolean IsMatching(String speedKey, String holdKey)
        {
            switch(speedKey)
            {
                case "1": case "!":
                    if (holdKey == "D1") 
                        return true;
                    break;
                case "2":
                case "@":
                    if (holdKey == "D2")
                        return true;
                    break;
                case "3":
                case "#":
                    if (holdKey == "D3")
                        return true;
                    break;
                case "4":
                case "$":
                    if (holdKey == "D4")
                        return true;
                    break;
                case "5":
                case "%":
                    if (holdKey == "D5")
                        return true;
                    break;
                case "6":
                case "^":
                    if (holdKey == "D6")
                        return true;
                    break;
                case "7":
                case "&":
                    if (holdKey == "D7")
                        return true;
                    break;
                case "8":
                case "*":
                    if (holdKey == "D8")
                        return true;
                    break;
                case "9":
                case "(":
                    if (holdKey == "D9")
                        return true;
                    break;
                case "0":
                case ")":
                    if (holdKey == "D0")
                        return true;
                    break;
            }
            return false;
        }
    }
}