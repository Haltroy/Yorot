using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Yorot.UI.SystemApp
{
    public partial class calc : Form
    {
        public calc()
        {
            InitializeComponent();
            panel1.AutoScroll = false;
            pbHamMenu.Location = new Point(panel1.Width - (pbHamMenu.Width + 7), pbHamMenu.Location.Y);
            panel1.Invalidate();
            tmrAnimate.Start();
        }

        /// <summary>
        /// Code from https://www.codementor.io/@dewetvanthomas/tutorial-currency-converter-application-for-c-121yicb1es
        /// </summary>
        private class CurrencyConverter
        {
            /// <summary>
            /// Gets all available currency tags
            /// </summary>
            public static string[] GetCurrencyTags()
            {

                // Hardcoded currency tags neccesairy to parse the ecb xml's
                return new string[] {"eur", "usd", "jpy", "bgn", "czk", "dkk", "gbp", "huf", "ltl", "lvl"
            , "pln", "ron", "sek", "chf", "nok", "hrk", "rub", "try", "aud", "brl", "cad", "cny", "hkd", "idr", "ils"
            , "inr", "krw", "mxn", "myr", "nzd", "php", "sgd", "zar"};
            }

            /// <summary>
            /// Get currency exchange rate in euro's 
            /// </summary>
            public static float GetCurrencyRateInEuro(string currency)
            {
                if (currency.ToLower() == "")
                {
                    throw new ArgumentException("Invalid Argument! currency parameter cannot be empty!");
                }

                if (currency.ToLower() == "eur")
                {
                    throw new ArgumentException("Invalid Argument! Cannot get exchange rate from EURO to EURO");
                }

                try
                {
                    // Create with currency parameter, a valid RSS url to ECB euro exchange rate feed
                    string rssUrl = string.Concat("http://www.ecb.int/rss/fxref-", currency.ToLower() + ".html");

                    // Create & Load New Xml Document
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(rssUrl);

                    // Create XmlNamespaceManager for handling XML namespaces.
                    System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("rdf", "http://purl.org/rss/1.0/");
                    nsmgr.AddNamespace("cb", "http://www.cbwiki.net/wiki/index.php/Specification_1.1");

                    // Get list of daily currency exchange rate between selected "currency" and the EURO
                    System.Xml.XmlNodeList nodeList = doc.SelectNodes("//rdf:item", nsmgr);

                    // Loop Through all XMLNODES with daily exchange rates
                    foreach (System.Xml.XmlNode node in nodeList)
                    {
                        // Create a CultureInfo, this is because EU and USA use different sepperators in float (, or .)
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";

                        try
                        {
                            // Get currency exchange rate with EURO from XMLNODE
                            float exchangeRate = float.Parse(
                                node.SelectSingleNode("//cb:statistics//cb:exchangeRate//cb:value", nsmgr).InnerText,
                                NumberStyles.Any,
                                ci);

                            return exchangeRate;
                        }
                        catch { }
                    }

                    // currency not parsed!! 
                    // return default value
                    return 0;
                }
                catch
                {
                    // currency not parsed!! 
                    // return default value
                    return 0;
                }
            }

            /// <summary>
            /// Get The Exchange Rate Between 2 Currencies
            /// </summary>
            public static float GetExchangeRate(string from, string to, float amount = 1)
            {
                // If currency's are empty abort
                if (from == null || to == null)
                {
                    return 0;
                }

                // Convert Euro to Euro
                if (from.ToLower() == "eur" && to.ToLower() == "eur")
                {
                    return amount;
                }

                try
                {
                    // First Get the exchange rate of both currencies in euro
                    float toRate = GetCurrencyRateInEuro(to);
                    float fromRate = GetCurrencyRateInEuro(from);

                    // Convert Between Euro to Other Currency
                    if (from.ToLower() == "eur")
                    {
                        return (amount * toRate);
                    }
                    else if (to.ToLower() == "eur")
                    {
                        return (amount / fromRate);
                    }
                    else
                    {
                        // Calculate non EURO exchange rates From A to B
                        return (amount * toRate) / fromRate;
                    }
                }
                catch { return 0; }
            }
        }
        private void pbHamMenu_Click(object sender, EventArgs e)
        {
            if (panel1.AutoScroll)
            {
                panel1.AutoScroll = false;
                pbHamMenu.Location = new Point(panel1.Width - (pbHamMenu.Width + 7), pbHamMenu.Location.Y);
            }
            else
            {
                panel1.AutoScroll = true;


                // Sidebar
                int[] biggestpp = new int[] {
                    lbTemel.Location.X + lbTemel.Width + 5,
                    lbBilimsel.Location.X + lbBilimsel.Width + 5,
                    lbProgramlama.Location.X + lbProgramlama.Width + 5,
                    lbVeri.Location.X + lbVeri.Width + 5,
                    lbBoyut.Location.X + lbBoyut.Width + 5,
                    lbUzunluk.Location.X + lbUzunluk.Width + 5,
                    lbAgirlik.Location.X + lbAgirlik.Width + 5,
                    lbKutle.Location.X + lbKutle.Width + 5,
                    lbYas.Location.X + lbYas.Width + 5,
                    lbTarih.Location.X + lbTarih.Width + 5,
                    lbZaman.Location.X + lbZaman.Width + 5,
                    lbSicaklik.Location.X + lbSicaklik.Width + 5,
                    lbHiz.Location.X + lbHiz.Width + 5,
                    lbIndirim.Location.X + lbIndirim.Width + 5,
                    lbParaBirimi.Location.X + lbParaBirimi.Width + 5,
                    lbKredi.Location.X + lbKredi.Width + 5,
                    lbYatirim.Location.X + lbYatirim.Width + 5,
                };
                int? maxVal = null;
                int index = -1;
                for (int i = 0; i < biggestpp.Length; i++)
                {
                    int thisNum = biggestpp[i];
                    if (!maxVal.HasValue || thisNum > maxVal.Value)
                    {
                        maxVal = thisNum;
                        index = i;
                    }
                }
                if (maxVal != null && maxVal > 0)
                {
                    panel1.Width = (int)maxVal + 52;
                    tabControl1.Width = Width - (panel1.Width + 6);
                }
                pbHamMenu.Location = new Point(panel1.Width - (20 + pbHamMenu.Width), pbHamMenu.Location.Y);
            }
            panel1.Invalidate();
            tmrAnimate.Start();
        }

        private void tmrAnimate_Tick(object sender, EventArgs e)
        {
            switch (panel1.AutoScroll)
            {
                case true: // Show menu
                    panel1.Location = new Point(panel1.Location.X + (panel1.Location.X < 0 ? 20 : 0), panel1.Location.Y);
                    if (panel1.Location.X >= 0)
                    {
                        panel1.Location = new Point(0, panel1.Location.Y);
                        panel1.Invalidate();
                        tmrAnimate.Stop();
                    }
                    break;
                case false: // Hide menu
                    panel1.Location = new Point(panel1.Location.X - (panel1.Location.X > (Math.Abs(panel1.Width - 39) * (-1)) ? 20 : 0), panel1.Location.Y);
                    if (panel1.Location.X <= (Math.Abs(panel1.Width - 39) * (-1)))
                    {
                        panel1.Location = new Point((Math.Abs(panel1.Width - 39) * (-1)), panel1.Location.Y);
                        panel1.Invalidate();
                        tmrAnimate.Stop();
                    }
                    break;
            }
            tabControl1.Width = (Width + 6) - (panel1.Width + panel1.Location.X);
            tabControl1.Location = new Point(((panel1.Width + panel1.Location.X) - 3), tabControl1.Location.Y);
        }
    }

}
