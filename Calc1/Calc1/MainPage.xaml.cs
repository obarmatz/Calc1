﻿using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Calc1
{
	public partial class MainPage : ContentPage, ActivityCompat.IOnRequestPermissionsResultCallback
	{
		public IntPtr Handle => default(IntPtr);

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
                case "Load Eq":
                    CheckPermissions();
                    break;
                case "⇋":
                    Navigation.PushAsync(new Cchange());
                    break;
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
                case "(":
					Set_Res(Get_Res() + "(");
                    break;
                case ")":
					Set_Res(Get_Res() + ")");
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
					Set_Res(CalculateBinTree(new BinTree<object>(Get_Res())).ToString());
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
        private void Display_File_Fault_Error_Message()
        {
            Set_Res("File syntax error!");
        }

		private double CalculateBinTree(BinTree<Object> bt)
		{
			if(bt.IsLeave())
				return Double.Parse(bt.Val.ToString());

			switch(bt.Val as String)
			{
				case "+":
					return CalculateBinTree(bt.Left) + CalculateBinTree(bt.Right);
				case "-":
					return CalculateBinTree(bt.Left) - CalculateBinTree(bt.Right);
				case "X":
					return CalculateBinTree(bt.Left) * CalculateBinTree(bt.Right);
				case "/":
					return CalculateBinTree(bt.Left) / CalculateBinTree(bt.Right);
			}

			return Math.Pow(CalculateBinTree(bt.Left), CalculateBinTree(bt.Right));
		}

        private Double SolveEq(String eq)
        {
            //1)devide to 2 strings
            String Right = "";
            String Left = "";
            for (int i = 0; i < eq.Length - 1; i++)
            {
                if (eq[i] == '=')
                {
                    Right = eq.Substring(0, i);
                    Left = eq.Substring(i + 1);
                    break;
                }
            }

            //2)devide to arrays
            int digits = 1;
            int start = 0;
            List<String> RightArr = new List<string>();
            for (int i = 1; i < Right.Length; i++)
            {
                if (!Char.IsDigit(Right, i) && Right[i] != 'X')
                {
                    RightArr.Add(Right.Substring(start, digits));
                    start = i;
                    if (Right[i] == '+') start++;
                    digits = 0;
                    if (Right[i] == '-') digits++;
                }
                else digits++;
            }
            RightArr.Add(Right.Substring(start));

            List<String> LeftArr = new List<string>();
            digits = 1;
            start = 0;
            for (int i = 1; i < Left.Length; i++)
            {
                if (Left[i] == '+' || Left[i] == '-')
                {
                    LeftArr.Add(Left.Substring(start, digits));
                    start = i;
                    if (Left[i] == '+') start++;
                    digits = 0;
                    if (Left[i] == '-') digits++;
                }
                else digits++;
            }
            LeftArr.Add(Left.Substring(start));

            //3)devide numbers and Xs
            List<string> RightArrX = new List<string>();
            List<string> RightArrNum = new List<string>();
            List<string> LeftArrX = new List<string>();
            List<string> LeftArrNum = new List<string>();

            foreach (string s in RightArr)
            {
                if (s[s.Length - 1] == 'X') RightArrX.Add(s);
                else RightArrNum.Add(s);
            }

            foreach (string s in LeftArr)
            {
                if (s[s.Length - 1] == 'X') LeftArrX.Add(s);
                else LeftArrNum.Add(s);
            }

            //4)turn string array into a number (sum the arrays)
            Double RightX = 0;
            Double RightNum = 0;
            Double LeftX = 0;
            Double LeftNum = 0;

            foreach (String s in RightArrX)
            {
                if (s.Equals("X")) RightX++;//because if we short the X we are left with nothing, as it is common to not write 1X 
                else RightX += Double.Parse(s.Substring(0, s.Length - 1));//because the last char is X
            }
            foreach (String s in RightArrNum)
            {
                RightNum += Double.Parse(s);
            }
            foreach (String s in LeftArrX)
            {
                if (s.Equals("X")) LeftX++;
                LeftX += Double.Parse(s.Substring(0, s.Length - 1));
            }
            foreach (String s in LeftArrNum)
            {
                LeftNum += Double.Parse(s);
            }

            //5)X to the right num to the left
            Double X = RightX - LeftX;
            Double Num = LeftNum - RightNum;

            //6)devide num in X
            Double Ans = Num / X;
            return Ans;
        }

		private void CheckPermissions()
		{
			Console.WriteLine("checking permission................");
			var thisActivity = Forms.Context as Activity;
			if(ContextCompat.CheckSelfPermission(thisActivity, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
			{// Permission is not granted
				Console.WriteLine("per is not granted, requesting....................");
				ActivityCompat.RequestPermissions(thisActivity, new String[] { Manifest.Permission.ReadExternalStorage }, 1);//1 is just the code to retrive this permission
			} else
			{//if permission is already granted
				Console.WriteLine("per already granted~~~~~~~~~~~~~~~~~~~~");
				Pick_File();
			}
		}

        private async void Pick_File()
        {
			try
			{
				var file = await CrossFilePicker.Current.PickFile();
				if(file == null){
					Set_Res("Canceled");
					return;
				}
				String contents = System.Text.Encoding.UTF8.GetString(file.DataArray);

				Console.WriteLine("File name: " + file.FileName);
				Console.WriteLine("File contents: " + contents);

				var BinAns = new BinTree<Object>(contents);
				String ans = CalculateBinTree(BinAns).ToString();
				Set_Res(ans);
			} catch(Exception ex)
			{
				Display_File_Fault_Error_Message();
				Console.WriteLine(ex.Source);
			}
        }

		public void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			if(requestCode == 1)
			{
				if(grantResults.Length > 0 && grantResults[0] == Permission.Granted)
				{//permission has been granted by the user
					Console.WriteLine("per granted, picking file.........................");
					Pick_File();
				} else
				{//permission denied
					Console.WriteLine("per denied!!!!!!!!!!!!!!!!!!!!");
				}
			}
		}

		public void Dispose(){}
	}
}
