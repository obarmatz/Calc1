using System;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calc1
{
	public partial class Cchange : ContentPage
	{
        private double[,] GatesMat;

		public Cchange()
        {
            InitializeComponent();

            Picker from = this.FindByName<Picker>("FromType");
            from.Items.Add("$");
            from.Items.Add("¤");
            from.Items.Add("€");
            from.Items.Add("¥");
            from.Items.Add("£");
            from.SelectedIndex = 1;

            Picker to = this.FindByName<Picker>("ToType");
            to.Items.Add("$");
            to.Items.Add("¤");
            to.Items.Add("€");
            to.Items.Add("¥");
            to.Items.Add("£");
            to.SelectedIndex = 0;

            GatesMat = GenerateGatesTabel();
        }

        private double[,] GenerateGatesTabel()
        {
            double[] NisValue = new double[4];//the order is: 0=USD 1=GBP 2=JPY 3=EUR

            XElement currency = XElement.Load("https://www.boi.org.il/currency.xml");
            
                //set the first cell in the array to be the USD gate
            foreach(XElement x in currency.Elements())
            {
                if (x.Name.LocalName.Equals("LAST_UPDATE")) continue;
                if (x.XPathSelectElement("CURRENCYCODE").Value.Equals("USD"))
                {
                    NisValue[0] = Double.Parse(((XElement)x.XPathSelectElement("CURRENCYCODE").NextNode.NextNode).Value);
                    break;
                }
            }

                //set the second cell in the array to be the GBP gate
            foreach (XElement x in currency.Elements())
            {
                if (x.Name.LocalName.Equals("LAST_UPDATE")) continue;
                if (x.XPathSelectElement("CURRENCYCODE").Value.Equals("GBP"))
                {
                    NisValue[1] = Double.Parse(((XElement)x.XPathSelectElement("CURRENCYCODE").NextNode.NextNode).Value);
                    break;
                }
            }

                //set the third cell in the array to be the JPY gate
            foreach (XElement x in currency.Elements())
            {
                if (x.Name.LocalName.Equals("LAST_UPDATE")) continue;
                if (x.XPathSelectElement("CURRENCYCODE").Value.Equals("JPY"))
                {
                    NisValue[2] = Double.Parse(((XElement)x.XPathSelectElement("CURRENCYCODE").NextNode.NextNode).Value);
                    break;
                }
            }

                //set the fourth cell in the array to be the EUR gate
            foreach (XElement x in currency.Elements())
            {
                if (x.Name.LocalName.Equals("LAST_UPDATE")) continue;
                if (x.XPathSelectElement("CURRENCYCODE").Value.Equals("EUR"))
                {
                    NisValue[3] = Double.Parse(((XElement)x.XPathSelectElement("CURRENCYCODE").NextNode.NextNode).Value);
                    break;
                }
            }

            //now we have a NIS to every currency gate
            //we want to create a matrix of every currency to every currency gate
            //same order, the first is NIS - 0=NIS 1=USD 2=GBP 3=JPY 4=EUR
            //note that the gate is from the rows type to the columns type (for [1,2] it's the USD-EUR gate)
            double[ , ] mat = new double[5,5];

            for(int i =0; i<5; i++)
            {
                for(int j=0; j<5; j++)
                {
                    if (i == j) mat[i,j] = 1;
                    else if (j == 0) mat[i,0] = 1 / NisValue[i - 1];
                    else if (i == 0) mat[0,j] = NisValue[j - 1];
                    else
                    {
                        mat[i,j] = NisValue[i - 1] / NisValue[j - 1];
                    }
                }
            }
            
            return mat;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}
