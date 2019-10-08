using System;

namespace System
{
    public static class DoubleExtension
    {
        public static Tuple<int,short> GetDecimals(this double data)
        {
            string str = data.ToString();
            short dec = (short)(str.Length - str.IndexOf(".") - 1);
            int mcr = (int)(data * Math.Pow(10, dec));

            return new Tuple<int, short>(mcr, dec);
        }

        public static Tuple<int, short> GetDecimalsWithReference(this double data,double reference)
        {
            string str_data = data.ToString();
            short dec_data = (short)(str_data.Length - str_data.IndexOf(".") - 1);

            string str_ref = reference.ToString();
            short dec_ref = (short)(str_ref.Length - str_ref.IndexOf(".") - 1);

            var dec = dec_data > dec_ref ? dec_data : dec_ref;

            int mcr = (int)(data * Math.Pow(10, dec));

            return new Tuple<int, short>(mcr, dec);
        }
    }
}
