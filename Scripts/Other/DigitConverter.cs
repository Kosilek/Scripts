using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class DigitConverter
{
    public static string digitConverter(ulong count)
    {
        if (count < 10000)
        {
            return count.ToString();
        }
        else if (count >= 10000 && count < 1000000)
        {
            var lenght = count.ToString();
            string number = "";
            if (lenght.Length == 5)
            {
                number += lenght[0];
                number += lenght[1];
                number += "K";
            }
            else if (lenght.Length == 6)
            {
                number += lenght[0];
                number += lenght[1];
                number += lenght[2];
                number += "K";
            }
            return number.ToString();
        }
        else if (count >= 1000000 && count < 1000000000)
        {
            var lenght = count.ToString();
            string number = "";
            if (lenght.Length == 7)
            {
                number += lenght[0];
                number += "M";
            }
            else if (lenght.Length == 8)
            {
                number += lenght[0];
                number += lenght[1];
                number += "M";
            }
            else if (lenght.Length == 9)
            {
                number += lenght[0];
                number += lenght[1];
                number += lenght[2];
                number += "M";
            }
            return number.ToString();
        }
        else if (count >= 1000000000 && count < 1000000000000)
        {
            var lenght = count.ToString();
            string number = "";
            if (lenght.Length == 10)
            {
                number += lenght[0];
                number += "B";
            }
            else if (lenght.Length == 11)
            {
                number += lenght[0];
                number += lenght[1];
                number += "B";
            }
            else if (lenght.Length == 12)
            {
                number += lenght[0];
                number += lenght[1];
                number += lenght[2];
                number += "B";
            }
            return number.ToString();
        }
        else if (count >= 1000000000000 && count < 1000000000000000)
        {
            var lenght = count.ToString();
            string number = "";
            if (lenght.Length == 13)
            {
                number += lenght[0];
                number += "T";
            }
            else if (lenght.Length == 14)
            {
                number += lenght[0];
                number += lenght[1];
                number += "T";
            }
            else if (lenght.Length == 15)
            {
                number += lenght[0];
                number += lenght[1];
                number += lenght[2];
                number += "T";
            }
            return number.ToString();
        }
        else if (count >= 1000000000000000 && count < 1000000000000000000)
        {
            var lenght = count.ToString();
            string number = "";
            if (lenght.Length == 16)
            {
                number += lenght[0];
                number += "Q";
            }
            else if (lenght.Length == 17)
            {
                number += lenght[0];
                number += lenght[1];
                number += "Q";
            }
            else if (lenght.Length == 18)
            {
                number += lenght[0];
                number += lenght[1];
                number += lenght[2];
                number += "Q";
            }
            return number.ToString();
        }
        else
            return count.ToString();
    }
}
