using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yorot
{
    /// <summary>
    /// Handles Math problems
    /// </summary>
    public static class Mat
    {
        /// <summary>
        /// Finds the prime multiplier numbers of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns>An array of <see cref="int"/> containing prime multiplier <see cref="int"/> list.</returns>
        public static int[] PrimeMultipliers(this int n)
        {
            // Teşekkürler : https://www.pinareser.com/c-ile-girilen-sayinin-asal-carpanlara-ayrilmasi/
            int[] asallar = new int[] { };
            int asal = 2;
            int kontrol = 0;
            while (n > 1)
            {
                if (n % asal == 0)
                {
                    if (asal != kontrol)
                    {
                        kontrol = asal;
                        Array.Resize<int>(ref asallar, asallar.Length + 1);
                        asallar[asallar.Length - 1] = asal;
                        n = n / asal;
                    }
                    else
                    {
                        n = n / asal;
                    }
                }
                else
                {
                    asal++;
                }
            }
            return asallar;
        }

        /// <summary>
        /// Gets a <paramref name="n2"/> dividable <see cref="int"/> from <paramref name="n1"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a main number</param>
        /// <param name="n2"><see cref="int"/> representative of a divider number</param>
        /// <param name="balancePoint">Adds remainder of divison if <paramref name="n1"/> is smaller than this paramater. Otherwise, subtracts from <paramref name="n1"/>.</param>
        /// <returns><paramref name="n2"/> dividable close number of <paramref name="n1"/>.</returns>
        public static int MakeItDividable(this int n1,int n2, int balancePoint = 300)
        {
            int kalan = n1 % n2;
            return n1 < balancePoint ? n1 + (n2 - kalan) : n1 - kalan;
        }

        /// <summary>
        /// Divides <paramref name="n1"/> to <paramref name="divider"/> without remainder.
        /// </summary>
        /// <param name="n1">Main number.</param>
        /// <param name="divider">Divider number.</param>
        /// <returns>Result of devision.</returns>
        public static int Divide(this int n1, int divider)
        {
            int dividable = n1.MakeItDividable(divider);
            return dividable / divider;
        }

        /// <summary>
        /// Largest Common Division between <paramref name="n1"/> and <paramref name="n2"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a number</param>
        /// <param name="n2"><see cref="int"/> representative of a number</param>
        /// <returns><see cref="int"/> representative of Largest Common Division between <paramref name="n1"/> and <paramref name="n2"/>.</returns>
        public static int LargestCommonDivision(this int n1, int n2)
        {
            // Teşekkürler: https://www.bilisimogretmeni.com/visual-studio-c/c-dersleri-obeb-okek-bulma-hesaplama.html
            while (n1 != n2)
            {
                if (n1 > n2)
                    n1 = n1 - n2;
                if (n2 > n1)
                    n2 = n2 - n1;
            }
            return n1;
        }
        /// <summary>
        /// Smallest of Common Denominator between <paramref name="n1"/> and <paramref name="n2"/>.
        /// </summary>
        /// <param name="n1"><see cref="int"/> representative of a number</param>
        /// <param name="n2"><see cref="int"/> representative of a number</param>
        /// <returns><see cref="int"/> representative of Smallest of Common Denominator between <paramref name="n1"/> and <paramref name="n2"/>.</returns>
        public static int SmallestOfCommonDenominator(this int n1, int n2)
        {
            // Teşekkürler: https://www.bilisimogretmeni.com/visual-studio-c/c-dersleri-obeb-okek-bulma-hesaplama.html
            return (n1 * n2) / n1.LargestCommonDivision(n2);
        }


        /// <summary>
        /// Biggest prime multiplier number of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns><see cref="int"/> as the biggest prime multiplier of <paramref name="n"/>.</returns>
        public static int BiggestPrimeMultiplier(this int n)
        {
            int[] asallar = n.PrimeMultipliers();
            return asallar[asallar.Length - 1];
        }
        /// <summary>
        /// Smallest prime multiplier number of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"><see cref="int"/> to search in.</param>
        /// <returns><see cref="int"/> as the smallest prime multiplier of <paramref name="n"/>.</returns>
        public static int SmallestPrimeMultiplier(this int n)
        {
            int[] asallar = n.PrimeMultipliers();
            return asallar[0];
        }
    }
}
