using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace Yorot.UI.SystemApp
{
    public partial class calc : Form
    {
        #region UI
        public calc()
        {
            InitializeComponent();
            Icon = YorotGlobal.IconFromImage(Properties.Resources.calc);
            panel1.AutoScroll = false;
            pbHamMenu.Location = new Point(panel1.Width - (pbHamMenu.Width + 7), pbHamMenu.Location.Y);
            panel1.Invalidate();
            tmrAnimate.Start();
        }

        bool cntrlDown = false;
        bool shiftDown = false;
        bool altDown = false;
        private void calc_KeyDown(object sender, KeyEventArgs e)
        {
            cntrlDown = e.Control;
            shiftDown = e.Shift;
            altDown = e.Alt;
            if (tabControl1.SelectedTab == tpTemel)
            {
                switch(e.KeyCode)
                {
                    case Keys.NumPad0:
                    case Keys.D0:
                        btNum_Click(btNum0, e);
                        break;
                    case Keys.NumPad1:
                    case Keys.D1:
                        btNum_Click(btNum1, e);
                        break;
                    case Keys.NumPad2:
                    case Keys.D2:
                        btNum_Click(btNum2, e);
                        break;
                    case Keys.NumPad3:
                    case Keys.D3:
                        btNum_Click(btNum3, e);
                        break;
                    case Keys.NumPad4:
                    case Keys.D4:
                        btNum_Click(btNum4, e);
                        break;
                    case Keys.NumPad5:
                    case Keys.D5:
                        btNum_Click(btNum5, e);
                        break;
                    case Keys.NumPad6:
                    case Keys.D6:
                        btNum_Click(btNum6, e);
                        break;
                    case Keys.NumPad7:
                    case Keys.D7:
                        btNum_Click(btNum7, e);
                        break;
                    case Keys.NumPad8:
                    case Keys.D8:
                        btNum_Click(btNum8, e);
                        break;
                    case Keys.NumPad9:
                    case Keys.D9:
                        btNum_Click(btNum9, e);
                        break;
                    case Keys.Add:
                        btNumPlus_Click(btNumPlus, e);
                        break;
                    case Keys.Subtract:
                        btNumMinus_Click(btNumMinus, e);
                        break;
                    case Keys.Multiply:
                        btNumX_Click(btNumX, e);
                        break;
                    case Keys.Decimal:
                        btNum_Click(btNumCOMMA, e);
                        break;
                    case Keys.Separator:
                    case Keys.Divide:
                        btNumDiv_Click(btNumDiv, e);
                        break;
                    case Keys.Delete:
                        btNumC_Click(btNumC, e);
                        break;
                    case Keys.Enter:
                        btNumEQ_Click(btNumEQ, e);
                        break;
                    case Keys.Back:
                        btNumBack_Click(btNumBack, e);
                        break;
                    case Keys.C:
                        if (cntrlDown) { Clipboard.SetText(lbCalc.Text); }
                        break;
                    case Keys.X:
                        if (cntrlDown) { Clipboard.SetText(lbCalc.Text); lbCalc.Text = "0"; }
                        break;
                    case Keys.V:
                        if (cntrlDown) { lbCalc.Text = Clipboard.GetText().TrimToNumbers(); }
                        break;
                }
            }
        }

        private void calc_KeyUp(object sender, KeyEventArgs e)
        {
            cntrlDown = e.Control;
            shiftDown = e.Shift;
            altDown = e.Alt;
        }

        private void pbHamMenu_Click(object sender, EventArgs e)
        {
            if (panel1.AutoScroll)
            {
                panel1.AutoScroll = false;
                pbHamMenu.Location = new Point(panel1.Width - (pbHamMenu.Width + 5), pbHamMenu.Location.Y);
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
                pbHamMenu.Location = new Point(panel1.Width - (25 + pbHamMenu.Width), pbHamMenu.Location.Y);
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

        bool allowSwitch = false;
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (allowSwitch) { allowSwitch = false; } else { e.Cancel = true; }
        }

        private void switchTab(Label senderLB, TabPage tp)
        {
            // Switch tab
            allowSwitch = true;
            tabControl1.SelectedTab = tp;
            // Set fonts
            lbTemel.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbTarih.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbProgramlama.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbZaman.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbBilimsel.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbYas.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbYatirim.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbSicaklik.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbVeri.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbKredi.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbHiz.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbBoyut.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbKutle.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbParaBirimi.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbIndirim.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbUzunluk.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            lbHakkinda.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular);
            // Make label bold
            senderLB.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
        }

        private void lbTemel_Click(object sender, EventArgs e) => switchTab(lbTemel, tpTemel);
        private void lbTarih_Click(object sender, EventArgs e) => switchTab(lbTarih, tpTarih);
        private void lbProgramlama_Click(object sender, EventArgs e) => switchTab(lbProgramlama, tpProgramlama);
        private void lbZaman_Click(object sender, EventArgs e) => switchTab(lbZaman, tpZaman);
        private void lbBilimsel_Click(object sender, EventArgs e) => switchTab(lbBilimsel, tpBilimsel);
        private void lbYas_Click(object sender, EventArgs e) => switchTab(lbYas, tpYas);
        private void lbYatirim_Click(object sender, EventArgs e) => switchTab(lbYatirim, tpYatirim);
        private void lbSicaklik_Click(object sender, EventArgs e) => switchTab(lbSicaklik, tpSicaklik);
        private void lbVeri_Click(object sender, EventArgs e) => switchTab(lbVeri, tpVeri);
        private void lbKredi_Click(object sender, EventArgs e) => switchTab(lbKredi, tpKredi);
        private void lbHiz_Click(object sender, EventArgs e) => switchTab(lbHiz, tpHiz);
        private void lbBoyut_Click(object sender, EventArgs e) => switchTab(lbBoyut, tpBoyut);
        private void lbKutle_Click(object sender, EventArgs e) => switchTab(lbKutle, tpKutle);
        private void lbParaBirimi_Click(object sender, EventArgs e) => switchTab(lbParaBirimi, tpParaBirimi);
        private void lbIndirim_Click(object sender, EventArgs e) => switchTab(lbIndirim, tpIndirim);
        private void lbUzunluk_Click(object sender, EventArgs e) => switchTab(lbUzunluk, tpUzunluk);
        private void lbHakkinda_Click(object sender, EventArgs e) => switchTab(lbHakkinda, tpHakkinda);

        #endregion UI

        #region Calculators

        #region Basic

        bool basicShowHistory = false;
        private void tmrBasicAnimate_Tick(object sender, EventArgs e)
        {
            switch (basicShowHistory)
            {
                case true: // Show menu
                    pCalcHis.Width += pCalcHis.Width != 200 ? 50 : 0;
                    if (pCalcHis.Width >= 400)
                    {
                        pCalcHis.Width = 400;
                        pCalcHis.Invalidate();
                        tmrBasicAnimate.Stop();
                    }
                    break;
                case false: // Hide menu
                    pCalcHis.Width -= pCalcHis.Width != 50 ? 50 : 0;
                    if (pCalcHis.Width <= 50)
                    {
                        pCalcHis.Width = 50;
                        pCalcHis.Invalidate();
                        tmrBasicAnimate.Stop();
                    }
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            basicShowHistory = !basicShowHistory;
            tmrBasicAnimate.Start();
        }

        internal class CalcOperation
        {
            public CalcOperation(string value) { Value = value; }
            public CalcOperation() { }
            public string Value { get; set; }
        }
        List<CalcOperation> calcList { get; set; } = new List<CalcOperation>();

        bool doCalcOperation = false;

        void doCalc()
        {
            double result = 0;
            lbPrevCalc.Text = "";
            try
            {
                for (int i = 0; i < calcList.Count; i++)
                {
                    lbPrevCalc.Text += calcList[i].Value;
                    switch (calcList[i].Value)
                    {
                        case "+":
                        case "-":
                        case "/":
                        case "x":
                        case "√":
                            break;
                        default:
                            if (i <= 0) { result = double.Parse(calcList[i].Value); }
                            else
                            {
                                switch (calcList[i - 1].Value)
                                {
                                    case "+":
                                        result = result + double.Parse(calcList[i].Value);
                                        break;
                                    case "-":
                                        result = result - double.Parse(calcList[i].Value);
                                        break;
                                    case "/":
                                        result = result / double.Parse(calcList[i].Value);
                                        break;
                                    case "√":
                                        result = Math.Sqrt(double.Parse(calcList[i].Value));
                                        break;
                                    case "x":
                                    default:
                                        result = result * double.Parse(calcList[i].Value);
                                        break;

                                }
                            }
                            break;
                    }
                }
                lbCalc.Text = "" + result;
            }
            catch(DivideByZeroException)
            {
                lbCalc.Text = "Infinity";
            }catch(OverflowException)
            {
                lbCalc.Text = "Infinity";
            }
        }
        bool eqMode = false;
        private void btNum_Click(object sender, EventArgs e)
        {
            var cntrl = sender as Control;
            if (lbCalc.Text == "0" || string.IsNullOrWhiteSpace(lbCalc.Text) ||doCalcOperation)
            {
                lbCalc.Text = cntrl.Text;
                doCalcOperation = doCalcOperation ? false : doCalcOperation;
            }else
            {
                lbCalc.Text += cntrl.Text;
            }
        }

        private void btNumPlus_Click(object sender, EventArgs e)
        {
            if (eqMode) 
            {
                calcList.Clear();
            }
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            calcList.Add(new CalcOperation("+"));
            doCalc();
        }

        private void btNumMinus_Click(object sender, EventArgs e)
        {
            if (eqMode)
            {
                calcList.Clear();
            }
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            calcList.Add(new CalcOperation("-"));
            doCalc();
        }

        private void btNumX_Click(object sender, EventArgs e)
        {
            if (eqMode)
            {
                calcList.Clear();
            }
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            calcList.Add(new CalcOperation("x"));
            doCalc();
        }

        private void btNumDiv_Click(object sender, EventArgs e)
        {
            if (eqMode)
            {
                calcList.Clear();
            }
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            calcList.Add(new CalcOperation("/"));
            doCalc();
        }

        private void btNumCE_Click(object sender, EventArgs e)
        {
            lbPrevCalc.Text = "";
            lbCalc.Text = "0";
            doCalcOperation = false;
            calcList.Clear();
        }

        private void btNumBack_Click(object sender, EventArgs e)
        {
            lbCalc.Text = lbCalc.Text.Length > 1 ? lbCalc.Text.Substring(0, lbCalc.Text.Length - 1) : "0";
        }

        private void btNumPlusMinus_Click(object sender, EventArgs e)
        {
            if (lbCalc.Text.StartsWith("-")) { lbCalc.Text = lbCalc.Text.Substring(1); }else { lbCalc.Text = "-" + lbCalc.Text; }
        }

        private void btNum1Div_Click(object sender, EventArgs e)
        {
            calcList.Clear();
            calcList.Add(new CalcOperation("1"));
            calcList.Add(new CalcOperation("/"));
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            eqMode = true;
            doCalc();
            Label lb = new Label()
            {
                Text = lbPrevCalc.Text + "=" + lbCalc.Text,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 15F, FontStyle.Regular)
            };
            lb.Click += lbHisItem_Click;
            clList.Add(lb);
            flpCalcHistory.Controls.Add(lb);
        }

        private void btNumSQ_Click(object sender, EventArgs e)
        {
            calcList.Clear();
            calcList.Add(new CalcOperation(lbCalc.Text));
            calcList.Add(new CalcOperation("x"));
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            eqMode = true;
            doCalc();
            Label lb = new Label()
            {
                Text = lbPrevCalc.Text + "=" + lbCalc.Text,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 15F, FontStyle.Regular)
            };
            lb.Click += lbHisItem_Click;
            clList.Add(lb);
            flpCalcHistory.Controls.Add(lb);
        }

        private void btNumRT_Click(object sender, EventArgs e)
        {
            calcList.Clear();
            calcList.Add(new CalcOperation("√"));
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            eqMode = true;
            doCalc();
            Label lb = new Label()
            {
                Text = lbPrevCalc.Text + "=" + lbCalc.Text,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 15F, FontStyle.Regular)
            };
            lb.Click += lbHisItem_Click;
            clList.Add(lb);
            flpCalcHistory.Controls.Add(lb);
        }

        private void btNumP_Click(object sender, EventArgs e)
        {
            calcList.Clear();
            calcList.Add(new CalcOperation(lbCalc.Text));
            calcList.Add(new CalcOperation("/"));
            calcList.Add(new CalcOperation("100"));
            doCalcOperation = true;
            eqMode = true;
            doCalc();
            Label lb = new Label()
            {
                Text = lbPrevCalc.Text + "=" + lbCalc.Text,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 15F, FontStyle.Regular)
            };
            lb.Click += lbHisItem_Click;
            clList.Add(lb);
            flpCalcHistory.Controls.Add(lb);
        }

        private void btNumC_Click(object sender, EventArgs e)
        {
            lbCalc.Text = "0";
            doCalcOperation = false;
        }

        private void btNumEQ_Click(object sender, EventArgs e)
        {
            calcList.Add(new CalcOperation(lbCalc.Text));
            doCalcOperation = true;
            eqMode = true;
            doCalc();
            Label lb = new Label()
            {
                Text = lbPrevCalc.Text + "=" + lbCalc.Text,
                AutoSize = true,
                Font = new Font(Font.FontFamily, 15F, FontStyle.Regular)
            };
            lb.Click += lbHisItem_Click;
            clList.Add(lb);
            flpCalcHistory.Controls.Add(lb);
        }

        List<Label> clList = new List<Label>();
        List<Label> lbSelected = new List<Label>();

        void lbHisItem_Click (object sender, EventArgs e)
        {
            var cntrl = sender as Label;
            if (cntrlDown)  // Selection mode
            {
                if (lbSelected.Contains(cntrl))
                {
                    lbSelected.Remove(cntrl);
                }else
                {
                    lbSelected.Add(cntrl);
                }
                btHisRemove.Visible = lbSelected.Count > 0;
                btHisRemove.Enabled = lbSelected.Count > 0;
            }
            else // Apply mode
            {
                var result = cntrl.Text.Substring(cntrl.Text.IndexOf("=") + 1);
                lbCalc.Text = result;
                calcList.Clear();
                calcList.Add(new CalcOperation(result));
                lbPrevCalc.Text = cntrl.Text.Substring(0,cntrl.Text.IndexOf("="));
            }
        }

        private void btHisRemove_Click(object sender, EventArgs e)
        {
            Label[] lbA = lbSelected.ToArray();
            foreach (Label lb in lbA)
            {
                clList.Remove(lb);
                lbSelected.Remove(lb);
                flpCalcHistory.Controls.Remove(lb);
            }
        }

        private void btHisClear_Click(object sender, EventArgs e)
        {
            clList.Clear();
            lbSelected.Clear();
            flpCalcHistory.Controls.Clear();
        }

        void getList(XmlNode mnode)
        {
            clList.Clear();

            lbCalc.Text = mnode.Attributes["Last"] != null ? mnode.Attributes["Last"].Value : "0";
            foreach (XmlNode node in mnode.ChildNodes)
            {
                if (node.Name == "HistoryItem")
                {
                    Label lb = new Label()
                    {
                        Text = node.InnerXml,
                        AutoSize = true,
                        Font = new Font(Font.FontFamily, 15F, FontStyle.Regular)
                    };
                    lb.Click += lbHisItem_Click;
                    clList.Add(lb);
                    flpCalcHistory.Controls.Add(lb);
                }
            }
        }

        string sendList()
        {
            string x = "<Basic Last=\"" + lbCalc.Text + "\" >";
            if (doCalcOperation) { lbCalc.Text = "0"; }
            btNumEQ_Click(this, new EventArgs());
            for(int i = 0; i< clList.Count;i++)
            {
                x += "<HistoryItem>" + clList[i].Text + "</HistoryItem>";
            }
            x += "</Basic>";
            return x;
        }

        #endregion Basic

        #region Currency
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


        #endregion Currency

        #endregion Calculators

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }

}
