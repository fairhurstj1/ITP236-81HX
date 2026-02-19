using System;
using System.Collections.Generic;
namespace EID
{
    //Extension class
    public class Extension
    {
        //ProperName extension method
        public static string ProperName(string name)
        {
            //Catch null or empty string
            if(name == null || name.Length == 0)
            {
                return "No Name";
            }//if

            //Convert to lower case
            name = name.ToLower();

            //Convert string to array of words
            string[] words = name.Split(' ');

            //Iterate through the array
            for (int i = 0; i < words.Length; i++)
            {
                //If the word has at least one character,
                if(words[i].Length > 0)
                {
                    //Capitalize the first character and concatenate with the rest of the word
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }//if
            }//for

            //Reconstruct the string from the array
            name = string.Join(" ", words);

            return name;
        }//ProperName

        //Method of my choice
        public static string Ordinal(int number)
        {
            //Declare variables
            int lastTwoDigits = 0;
            int lastDigit = 0;
            string suffix = "";

            //Handle non-positive numbers
            if (number < 0)
            {
                return number.ToString();
            }
            
            //Use modulo to determine the suffix
            lastTwoDigits = number % 100;
            lastDigit = number % 10;

            //All numbers ending in 11, 12, or 13 use "th"
            if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            {
                suffix = "th";
            }

            //All other numbers use standard rules
            else
            {
                switch (lastDigit)
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            }
            
            //Concatenate the number and suffix
            return number.ToString("N0") + suffix;
        }//Ordinal
    }
}