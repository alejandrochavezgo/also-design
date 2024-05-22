namespace common.conversions
{
     public sealed class RomanNumeral
    {
        #region Attributes
        private static string[] _romans = new string[50];

        #endregion

        #region Public Methods
        public static string Cast(int pNumber)
        {
            string _letters = "";
            InitializeArray();
            switch (pNumber.ToString().Length)
            {
                case 1:
                    _letters += CastOne(pNumber.ToString());
                    break;
                case 2:
                    _letters += CastTwo(pNumber.ToString());
                    break;
                case 3:
                    _letters += CastThree(pNumber.ToString());
                    break;
                case 4:
                    _letters += CastFour(pNumber.ToString());
                    break;
                case 5:
                    _letters += CastFive(pNumber.ToString());
                    break;
            }

            return _letters;
        }
        #endregion

        #region Private Methods
        private static string CastOne(string pNumber)
        {
            return _romans[int.Parse(pNumber)];
        }

        private static string CastTwo(string pNumber)
        {
            //Obtiene el primer número
            string _letters = "";
            string _number = pNumber.ToString().Substring(0, 1);
            string _temp = "";

            if (int.Parse(_number) > 0)
            {
                _letters = _romans[int.Parse(_number) + 9];
            }
            _temp = CastOne(pNumber.ToString().Substring(1, 1));

            return _letters + _temp;
        }

        private static string CastThree(string pNumber)
        {
            string _letters = "";
            string _temp = "";

            if (int.Parse(pNumber.ToString().Substring(0, 1)) > 0)
            {
                _letters += _romans[int.Parse(pNumber.ToString().Substring(0, 1)) + 18];
            }

            _temp = CastTwo(pNumber.ToString().Substring(1, 2));

            return _letters + _temp;
        }

        private static string CastFour(string pNumber)
        {
            string _letters = "";
            string _temp = "";

            if (int.Parse(pNumber.ToString().Substring(0, 1)) > 0)
            {
                _letters += _romans[int.Parse(pNumber.ToString().Substring(0, 1)) + 27];
            }
            _temp += CastThree(pNumber.Substring(1, 3));

            return _letters + _temp;
        }

        private static string CastFive(string pNumber)
        {
            string _letters = "";
            string _temp = "";

            _letters += _romans[int.Parse(pNumber.ToString().Substring(0, 1)) + 36];
            _temp += CastFour(pNumber.Substring(1, 4));

            return _letters + _temp;
        }

        private static void InitializeArray()
        {
            //We list tthe roman numbers
            _romans[0] = "";
            _romans[1] = "I";
            _romans[2] = "II";
            _romans[3] = "III";
            _romans[4] = "IV";
            _romans[5] = "V";
            _romans[6] = "VI";
            _romans[7] = "VII";
            _romans[8] = "VIII";
            _romans[9] = "IX";
            _romans[10] = "X";
            _romans[11] = "XX";
            _romans[12] = "XXX";
            _romans[13] = "XL";
            _romans[14] = "L";
            _romans[15] = "LX";
            _romans[16] = "LXX";
            _romans[17] = "LXXX";
            _romans[18] = "XC";
            _romans[19] = "C";
            _romans[20] = "CC";
            _romans[21] = "CCC";
            _romans[22] = "CD";
            _romans[23] = "D";
            _romans[24] = "DC";
            _romans[25] = "DCC";
            _romans[26] = "DCCC";
            _romans[27] = "CM";
            _romans[28] = "M";
            _romans[29] = "MM";
            _romans[30] = "MMM";
            _romans[31] = "M(V)";
            _romans[32] = "(V)";
            _romans[33] = "(V)M";
            _romans[34] = "(V)MM";
            _romans[35] = "(V)MMM";
            _romans[36] = "M(X)";
            _romans[37] = "(X)";
            _romans[38] = "(XX)";
            _romans[39] = "(XXX)";
            _romans[40] = "(XL)";
            _romans[41] = "(L)";
            _romans[42] = "(LX)";
            _romans[43] = "(LXX)";
            _romans[44] = "(LXXX)";
            _romans[45] = "(XC)";
        }
        #endregion
    }
}