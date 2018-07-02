using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Calc1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Set_Res("0");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            String Button_Text = (sender as Button).Text;

            switch (Button_Text)
            {

                case "C":
                    Set_Res("0");
                    break;
                case "DEL":
                    if (Get_Res() != "0")
                    {
                        if (Get_Res().Length > 1)
                            Set_Res(Get_Res().Substring(0, Get_Res().Length - 1));
                        else Set_Res("0");
                    }
                    break;
                case "^":
                    Set_Res(Get_Res() + "^");
                    break;
                case "( )":
                    break;
                case "/":
                    Set_Res(Get_Res() + "/");
                    break;
                case "X":
                    Set_Res(Get_Res() + "X");
                    break;
                case "-":
                    Set_Res(Get_Res() + "-");
                    break;
                case "+":
                    Set_Res(Get_Res() + "+");
                    break;
                case ".":
                    Set_Res(Get_Res() + ".");
                    break;
                case "=":
                    Analize_Content(Get_Res());
                    break;
                case "1":
                    Set_Res((Get_Res() == "0") ? "1" : (Get_Res() + "1"));
                    break;
                case "2":
                    Set_Res((Get_Res() == "0") ? "2" : (Get_Res() + "2"));
                    break;
                case "3":
                    Set_Res((Get_Res() == "0") ? "3" : (Get_Res() + "3"));
                    break;
                case "4":
                    Set_Res((Get_Res() == "0") ? "4" : (Get_Res() + "4"));
                    break;
                case "5":
                    Set_Res((Get_Res() == "0") ? "5" : (Get_Res() + "5"));
                    break;
                case "6":
                    Set_Res((Get_Res() == "0") ? "6" : (Get_Res() + "6"));
                    break;
                case "7":
                    Set_Res((Get_Res() == "0") ? "7" : (Get_Res() + "7"));
                    break;
                case "8":
                    Set_Res((Get_Res() == "0") ? "8" : (Get_Res() + "8"));
                    break;
                case "9":
                    Set_Res((Get_Res() == "0") ? "9" : (Get_Res() + "9"));
                    break;
                case "0":
                    Set_Res((Get_Res() == "0") ? "0" : (Get_Res() + "0"));
                    break;
            }
        }

        private void Set_Res(String s)
        {
            Label res = this.FindByName<Label>("Res");
            res.Text = s;
        }

        private String Get_Res()
        {
            Label res = this.FindByName<Label>("Res");
            return res.Text;
        }

        private void Display_Syntax_Error_Message()
        {
            Set_Res("Syntax Error");
        }

        private void Analize_Content(String r)
        {
            IList v = new List<String>();
            int numLength = 1;
            int start = 0;
            for (int i = 0; i < r.Length + 1; i++)//devide the Res string to make a logical statement
            {
                if (start + numLength > r.Length)//in case of deviation from the string, add the remaiming to v
                {
                    v.Add(r.Substring(start));
                    break;
                }
                if (int.TryParse(r.Substring(start, numLength), out int temp))//if nL digits from str is a number
                {
                    numLength++;
                }
                else
                {
                    v.Add(r.Substring(start, numLength - 1));
                    v.Add(r[i].ToString());
                    numLength = 1;
                    start = i + 1;
                }
            }
            Calculate(v);
        }

        private void Calculate(IList v)
        {
            for (int i = 1; i < v.Count; i++)
            {
                switch(v[i] as String)
                {
                    case "^":
                        Set_Res(Math.Pow(int.Parse(v[i - 1] as String), int.Parse(v[i + 1] as String)).ToString());
                        break;

                    case "+":
                        Set_Res((int.Parse(v[i - 1] as String) + int.Parse(v[i + 1] as String)).ToString());
                        break;

                    case "-":
                        Set_Res((int.Parse(v[i - 1] as String) - int.Parse(v[i + 1] as String)).ToString());
                        break;

                    case "/":
                        Set_Res((int.Parse(v[i - 1] as String) / int.Parse(v[i + 1] as String)).ToString());
                        break;

                    case "X":
                        Set_Res((int.Parse(v[i - 1] as String) * int.Parse(v[i + 1] as String)).ToString());
                        break;
                }

            }
        }
    }
}
