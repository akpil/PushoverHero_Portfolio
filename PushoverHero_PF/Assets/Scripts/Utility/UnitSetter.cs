/* Copyright (C) Team Lucid Dream (JJSoft), Inc - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Kim Junyoung <jyk0802@gmail.com>, January 2017-fixed at August 2017 -last update at 2021 September
 */

using System;
using System.Globalization;

namespace Utility
{
    public static class UnitSetter
    {
        public static readonly string[] POSTFIX_KOR_ARR = { "", "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극", "항아사", "아승기", "나유타", "불가사의", "무량대수" };
        public static readonly string[] SUFFIX_ENG_ARR = { "", "K", "M", "B", "T",
            "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az",
            "Aa", "Ab", "Ac", "Ad", "Ae", "Af", "Ag", "Ah", "Ai", "Aj", "Ak", "Al", "Am", "An", "Ao", "Ap", "Aq", "Ar", "As", "At", "Au", "Av", "Aw", "Ax", "Ay", "Az",
            "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz",
            "Ba", "Bb", "Bc", "Bd", "Be", "Bf", "Bg", "Bh", "Bi", "Bj", "Bk", "Bl", "Bm", "Bn", "Bo", "Bp", "Bq", "Br", "Bs", "Bt"}; //right before the limite of double 1E+306
        public const int UNIT_STANDARD = 1000;
        
        private static NumberFormatInfo _krNumberFormatInfo;
        public static bool IsKorean { get; set; } = false;

        public static string FloatToTimeHMS(float sec)
        {
            int hour = (int)sec / 3600;
            int minutes = (int)sec / 60;
            int seconds = (int)sec % 60;

            return $"{hour} : {minutes} : {seconds:00}";
        }

        public static string FloatToTimeMS(float time)
        {
            int minutes = (int)time / 60;
            int seconds = (int)time % 60;

            return $"{minutes} : {seconds:00}";
        }

        public static string FloatToTimeS(float time)
        {
            return $"{time:00}";
        }

        public static string IntToTime(int time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            return timeSpan.ToString(timeSpan.Hours > 0 ? @"hh\:mm\:ss" : @"mm\:ss");
        }

        public const string PROFIT_FORMAT = "<color=#820000>+{0}</color>";
        public const string LOSE_FORMAT = "<color=#003582>{0}</color>";

        public static string SetPMPercentUint(double input, string profitFormat = PROFIT_FORMAT,
            string loseFormat = LOSE_FORMAT)
        {
            return string.Format(input < 0 ? loseFormat : profitFormat, input.ToString("P0"));
        }

        public static string SetMoneyUnit(double input, string unit = "")
        {
            if (IsKorean)
            {
                return SetMoneyUnitKorstyle(input, unit);
            }
            else
            {
                return SetMoneyUnitEngStyle(input, unit);
            }
        }

        private static string SetMoneyUnitKorstyle(double input, string unit = "")
        {
            if (_krNumberFormatInfo == null)
            {
                _krNumberFormatInfo = new NumberFormatInfo();
                _krNumberFormatInfo.NumberGroupSeparator = ",";
                _krNumberFormatInfo.NumberGroupSizes = new[] { 4 };
            }

            var inputStr = input.ToString("N0", _krNumberFormatInfo);
            var splited = inputStr.Split(',');
            var prefix1 = POSTFIX_KOR_ARR[splited.Length - 1];

            if (input >= 1e+4)
            {
                string prefix2;
                if (int.Parse(splited[1]) == 0)
                {
                    splited[1] = "";
                    prefix2 = "";
                }
                else
                {
                    prefix2 = POSTFIX_KOR_ARR[splited.Length - 2];
                }

                return string.Format("{0}{1} {2}{3} {4}", splited[0], prefix1, splited[1], prefix2, unit);
            }
            else
            {
                return string.Format("{0}{1} {2}", splited[0], prefix1, unit);
            }
        }

        private static string SetMoneyUnitEngStyle(double input, string unit = "")
        {
            string inputStr = input.ToString("N0");
            string[] splited = inputStr.Split(',');
            string suffix = SUFFIX_ENG_ARR[splited.Length - 1];

            if (input >= 1e+3 && splited.Length > 1)
            {
                return string.Format("{0} {1}.{2} {3}", unit, splited[0], splited[1], suffix);
            }
            else
            {
                return string.Format("{0} {1}", unit, splited[0]);
            }
        }
    }
}
