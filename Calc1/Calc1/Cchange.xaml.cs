using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Xamarin.Forms;

namespace Calc1
{
	public partial class Cchange : ContentPage
    {
        private double[,] GatesMat;
        private Picker from;
        private Picker to;
        public Cchange()
        {
            InitializeComponent();

            GatesMat = GenerateGatesTabel();

            from = this.FindByName<Picker>("FromType");
            from.Items.Add("¤");
            from.Items.Add("$");
            from.Items.Add("£");
            from.Items.Add("¥");
            from.Items.Add("€");
            from.SelectedIndex = 0;

            to = this.FindByName<Picker>("ToType");
            to.Items.Add("¤");
            to.Items.Add("$");
            to.Items.Add("£");
            to.Items.Add("¥");
            to.Items.Add("€");
            to.SelectedIndex = 1;

        }

        private double[,] GenerateGatesTabel()
        {
            double[] NisValue = new double[4];//the order is: 0=USD 1=GBP 2=JPY 3=EUR

            XElement currency = XElement.Load("https://www.boi.org.il/currency.xml");

            //set the first cell in the array to be the USD gate
            foreach (XElement x in currency.Elements())
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
            //note that the gate is from the rows type to the columns type (for [1,4] it's the USD-EUR gate)
            double[,] mat = new double[5, 5];

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == j) mat[i, j] = 1;
                    else if (j == 0) mat[i, 0] = 1 / NisValue[i - 1];
                    else if (i == 0) mat[0, j] = NisValue[j - 1];
                    else
                    {
                        mat[i, j] = NisValue[j - 1] / NisValue[i - 1];
                    }
                    if (i == 3) mat[i, j] *= 100;//just a correction to the yen gate, as it presented in the xml file 100 time smaller
                    if (j == 3) mat[i, j] /= 100;
                }
            }
            
            return mat;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            switch ((sender as Button).Text) {

                case "DEL":
                    if (Get_From_Num().Length == 1) Set_From_Num("0");
                    else Set_From_Num(Get_From_Num().Substring(0, Get_From_Num().Length-1));
                    break;
                case "C":
                    Set_From_Num("0");
                    break;
                default:
                    if(Get_From_Num()=="0") Set_From_Num((sender as Button).Text);
                    else Set_From_Num(Get_From_Num()+(sender as Button).Text);
                    break;
            }
            Update_To();
        }

        private void Update_To()
        {

            double fromNum = Double.Parse(Get_From_Num());

            Set_To_Num(String.Format("{0:0.000}" ,(GatesMat[to.SelectedIndex, from.SelectedIndex] * fromNum)).ToString());//takes the gate by the gate selection method (rows-cols, but inverted because the gate is rows-cols based and we need the value of the cols to rows gate) multyplied by the number that has been typed

        }

        private void Set_From_Num(String s)
        {
            this.FindByName<Label>("From").Text = s ;
        }

        private String Get_From_Num()
        {
            return this.FindByName<Label>("From").Text;
        }

        private void Set_To_Num(String s)
        {
            this.FindByName<Label>("To").Text = s;
        }

        private String Get_To_Num()
        {
            return this.FindByName<Label>("To").Text;
        }

        private void Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Update_To();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Source);
            }
        }
    }
}
