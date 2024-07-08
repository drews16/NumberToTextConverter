using NumberToTextConverter.Constants;
using System.Text;

namespace NumberToTextConverter
{
    internal class Number
    {
        private static readonly IDictionary<int, string> _digits = new Dictionary<int, string>()
        {
             {1, "один" },
             {2, "два" },
             {3, "три" },
             {4, "четыре" },
             {5, "пять" },
             {6, "шесть" },
             {7, "семь" },
             {8, "восемь" },
             {9, "девять" },
        };

        private static readonly IDictionary<int, string> _digitsGenitive = new Dictionary<int, string>()
        {
             {1, "одна" },
             {2, "две" },
             {3, "три" },
             {4, "четыре" },
             {5, "пять" },
             {6, "шесть" },
             {7, "семь" },
             {8, "восемь" },
             {9, "девять" },
        };

        private static readonly IDictionary<int, string> _decade = new Dictionary<int, string>
        {
             {1, "десять" },
             {2, "двадцать" },
             {3, "тридцать" },
             {4, "сорок" },
             {5, "пятьдесят" },
             {6, "шестьдесят" },
             {7, "семьдесят" },
             {8, "восемьдесят" },
             {9, "девяносто" },
        };

        private static readonly IDictionary<int, string> _secondDecade = new Dictionary<int, string>
        {
             {1, "одиннадцать" },
             {2, "двенадцать" },
             {3, "тринадцать" },
             {4, "четырнадцать" },
             {5, "пятьнадцать" },
             {6, "шестнадцать" },
             {7, "семнадцать" },
             {8, "восемнадцать" },
             {9, "девятнадцать" },
        };

        private static readonly IDictionary<int, string> _hundreds = new Dictionary<int, string>
        {
             {0, string.Empty},
             {1, "сто" },
             {2, "двести" },
             {3, "триста" },
             {4, "четыреста" },
             {5, "пятьсот" },
             {6, "шестьсот" },
             {7, "семьссот" },
             {8, "восемьсот" },
             {9, "девятьсот" },
        };

        public static string ToText(decimal number)
        {
            var sb = new UnitStringBuilder(new StringBuilder());

            int whole = (int)Math.Truncate(number);
            int fractial = (int)Math.Truncate((number - whole) * 100);

            int hundreds = (whole % 1000);
            int thousands = (whole / 1000) % 1000;

            if (thousands > 0)
            {
                ThousandsText(thousands, sb);
            }

            if (hundreds > 0)
            {
                HundredsText(hundreds, sb);
            }

            if (fractial > 0)
            {
                FractionalText(fractial, sb);
            }

            return sb.ToString();
        }

        // Тысячи
        private static void ThousandsText(int number, UnitStringBuilder sb)
        {
            int countUnit = number % 10;
            int countDecade = (number / 10) % 10;
            int countHundred = number / 100;

            sb.Append(_hundreds[countHundred]);

            if (countDecade != 0)
            {
                if (number % 100 > 10 && number % 100 < 20)
                {
                    sb.Append(_secondDecade[countUnit]);
                    sb.Append(ThousandsDeclensionText(number % 100));
                    return;
                }

                sb.Append(_decade[countDecade]);
            }

            if (countUnit != 0)
            {
                sb.Append(_digitsGenitive[countUnit]);
            }

            sb.Append(ThousandsDeclensionText(number));
        }

        private  static string ThousandsDeclensionText(int number)
        {
            int endingNumber = number % 10;

            if (number == 11)
            {
                return ThousandDeclension.GenitiveMulty;
            }

            switch (endingNumber)
            {
                case 1: return ThousandDeclension.Name;
                case 2:
                case 3:
                case 4: return ThousandDeclension.GenitiveSimple;
                default: return ThousandDeclension.GenitiveMulty;
            }
        }

        // Сотни
        private static void HundredsText(int number, UnitStringBuilder sb)
        {
            int countUnit = number % 10;
            int countDecade = (number / 10) % 10;
            int countHundred = number / 100;

            sb.Append(_hundreds[countHundred]);

            if (countDecade != 0)
            {
                if (number % 100 > 10 && number % 100 < 20)
                {
                    sb.Append(_secondDecade[countUnit]);
                    sb.Append(sb.UnitWholeForm(number));
                    return;
                }

                sb.Append(_decade[countDecade]);
            }

            if (countUnit != 0)
            {
                sb.Append(_digits[countUnit]);
            }

            sb.Append(sb.UnitWholeForm(number));
        }

        private static void FractionalText(int number, UnitStringBuilder sb)
        {
            int countUnit = number % 10;
            int countDecade = number / 10;

            if (countDecade != 0)
            {
                if (number % 100 > 10 && number % 100 < 20)
                {
                    sb.Append(_secondDecade[countUnit]);
                    sb.Append(sb.UnitPenniesForm(number));
                    return;
                }

                sb.Append(_decade[countDecade]);
            }

            if (countUnit != 0)
            {
                sb.Append(_digitsGenitive[countUnit]);
            }

            sb.Append(sb.UnitPenniesForm(number));
        }

        private class UnitStringBuilder
        {
            private StringBuilder _sb;

            public UnitStringBuilder(StringBuilder sb)
            {
                _sb = sb;
            }

            public string UnitWholeForm(int number)
            {
                int endingNumber = number % 10;

                if (number == 11)
                {
                    return UnitRubles.GenitiveMulty;
                }

                switch(endingNumber)
                {
                    case 1: return UnitRubles.Name;
                    case 2:
                    case 3:
                    case 4: return UnitRubles.GenitiveSimple;
                    default: return UnitRubles.GenitiveMulty;
                }
            }

            public string UnitPenniesForm(int number)
            {
                int endingNumber = number % 10;

                if (number == 11)
                {
                    return UnitPennies.GenitiveMulty;
                }

                switch (endingNumber)
                {
                    case 1: return UnitPennies.Name;
                    case 2:
                    case 3:
                    case 4: return UnitPennies.GenitiveSimple;
                    default: return UnitPennies.GenitiveMulty;
                }
            }

            public void Append(string s)
            {
                _sb.Append(s + " ");
            }

            public override string ToString()
            {
                return _sb.ToString();
            }
        }
    }
}