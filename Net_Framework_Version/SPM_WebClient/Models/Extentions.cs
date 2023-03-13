using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SPM_WebClient.Models
{
    public static class Extentions
    {
        //extention для типа double - Представление в string с проблеми разделителями
        /// <summary>
        /// String representation with delimeter spaces
        /// </summary>        
        public static string ToFormatedString(this double input)
        {
            try
            {
                string input_string = input.ToString();
                string[] input_strings;
               // char point;
                switch (input_string.Contains(','))
                {
                    case true:
                        input_strings = input_string.Split(',');
                       // point = ',';
                        break;
                    default:
                        input_strings = input_string.Split('.');
                       // point = '.';
                        break;
                }


                if (input_strings.Count() > 0)
                {
                    string input_integers = input_strings[0];


                    if (input_integers.Count() > 3)
                    {
                        StringBuilder result_integers = new StringBuilder();

                        int counter = 0;

                        for (int i = input_integers.Count() - 1; i >= 0; i--)
                        {
                            counter++;
                            result_integers.Insert(0, input_integers[i]);
                            if (counter >= 3)
                            {
                                result_integers.Insert(0, " ");
                                counter = 0;
                            }
                        }

                        if (input_strings.Count() > 1)

                        { return result_integers.ToString().Trim() + "." + input_strings[1]; }
                        return result_integers.ToString().Trim();
                    }
                    else
                    { return input_string; }
                }
                else { return input_string; }

            }
            catch { return input.ToString(); }
        }

    }
}
