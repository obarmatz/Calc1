using System;
using System.Collections.Generic;

namespace Calc1
{
	class BinTree<T>
    {
        public BinTree<Object> Right { get; set; }
        public BinTree<Object> Left { get; set; }
		public Object Val { get; set; }

		public BinTree(Object Val){
            this.Val = Val;
			Parse();
        }

        public BinTree(Object Val, BinTree<Object> Right, BinTree<Object> Left)
        {
            this.Val = Val;
            this.Right = Right;
            this.Left = Left;
        }

        public bool IsLeave()
        {
            return (Right == null && Left == null);
        }

		//takes a leave in a tree that is an algebric expression and makes it a new valid calculation tree 
        public void Parse()
		{
					Console.WriteLine("parse");
            if (IsLeave())
            {
                if(Val is String)
                {
                    if (Double.TryParse(Val as String, out double temp)) return;
					Str_To_L1_List();//this string is an algebric expression - it needs to be analized as a tree
                }
				else if (Val is List<String>)
				{
					if((Val as List<String>).Count == 1)
					{
						if(Double.TryParse(( Val as List<String> )[0], out double temp))
							Val = Double.Parse(( Val as List<String> )[0]);
						else
							FindLevel1();//this is an expression
					} else
						FindLevel1();
					
				}
			}
        }

		//Takes an algebric expression and sends FindLevel1() a list of all the sub-expressions that are being added/subbed
        public void Str_To_L1_List()
        {
			Console.WriteLine("str to l1");
            String s = Val as String;
            int digits = 1;
            int start = 0;
            List<String> L1_List = new List<string>();
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i] == '+' || s[i] == '-')
                {
                    L1_List.Add(s.Substring(start, digits));
					L1_List.Add(s[i].ToString());
                    start = i + 1;
                    digits = 0;
                } 
				else if(s[i] == '(')
				{
					int ExLength = FindLengthTillCloser(s, i);
					L1_List.Add(s.Substring(i + 1, ExLength));
					digits = 0;
					i += ExLength + 1;//continue on the )
				} 
				else if(s[i] == ')')
					continue;
				else digits++;
            }
            L1_List.Add(s.Substring(start));
			Val = L1_List;
			FindLevel1();
        }

		//takes a Level 1(+/-)-analized list and makes it a tree
		private void FindLevel1()
		{
			Console.WriteLine("find l1");
			List<String> ListVal = ( Val as List<String> );
			if(ListVal.Count == 1)
			{
				Val = ListVal[0];
				Str_To_L2_List();
				return;
			}
			Left = new BinTree<object>(ListVal[0]);
			Val = ListVal[1];
			ListVal.RemoveAt(0);
			ListVal.RemoveAt(0);
			Right = new BinTree<object>(ListVal);
			Left.Parse();
			Right.Parse();
			Left.Str_To_L2_List();
		}

		public void Str_To_L2_List()
        {
			Console.WriteLine("str to l2");
			String s = Val as String;
            int digits = 1;
            int start = 0;
            List<String> L2_List = new List<string>();
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i] == 'X' || s[i] == '/')
                {
                    L2_List.Add(s.Substring(start, digits));
					L2_List.Add(s[i].ToString());
					start = i + 1;
                    digits = 0;
                }
                else if (s[i] == '(')
                {
                    int ExLength = FindLengthTillCloser(s, i);
                    L2_List.Add(s.Substring(i+1, ExLength));
					digits = 0;
					i += ExLength + 1;//continue on the )
                }
				else if(s[i] == ')')
					continue;
				else
					digits++;
            }
            L2_List.Add(s.Substring(start));
			Val = L2_List;
			FindLevel2();
		}

		private void FindLevel2()
		{
			List<String> ListVal = ( Val as List<String> );
			if(ListVal.Count == 1)
			{
				Val = ListVal[0];
				Str_To_L3_List();
				return;
			}
			Left = new BinTree<object>(ListVal[0]);
			Val = ListVal[1];
			ListVal.RemoveAt(0);
			ListVal.RemoveAt(0);
			Right = new BinTree<object>(ListVal);
			Left.Parse();
			Right.Parse();
			Left.Str_To_L3_List();
		}

		public void Str_To_L3_List()
		{
			Console.WriteLine("str to l3");
			String s = Val as String;
            int digits = 1;
            int start = 0;
            List<String> L3_List = new List<string>();
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i] == '^')
                {
                    L3_List.Add(s.Substring(start, digits));
					L3_List.Add(s[i].ToString());
					start = i + 1;
                    digits = 0;
                }
                else if (s[i] == '(')
                {
                    int ExLength = FindLengthTillCloser(s, i);
                    L3_List.Add(s.Substring(i+1, ExLength));
					digits = 0;
					i += ExLength + 1;//continue on the )
                }
				else if(s[i] == ')')
					continue;
				else
					digits++;
            }
            L3_List.Add(s.Substring(start));
			Val = L3_List;
			FindLevel3();
		}

		private void FindLevel3()
		{
			List<String> ListVal = ( Val as List<String> );
			if(ListVal.Count == 1)
			{
				Val = ListVal[0];
				Parse();
				return;
			}
			Left = new BinTree<object>(ListVal[0]);
			Val = ListVal[1];
			ListVal.RemoveAt(0);
			ListVal.RemoveAt(0);
			Right = new BinTree<object>(ListVal);
			Left.Parse();
			Right.Parse();
		}



		//takes the beggining of a parenthesis-wraped expression '(' and returns it's length
		//example: for the input "3*(1+2)", 2 - the output is 3 since there are 3 chars inside the parenthesis
		private int FindLengthTillCloser(String s, int start)
        {
			Stack<char> st = new Stack<char>();
			st.Push('(');
			int i = start;
			while( i<s.Length){
				i++;
				if(s[i] == '(')
					st.Push(s[i]);

				if(s[i] == ')')
					st.Pop();

				if(st.Count == 0)
					break;
			}
			return i - start - 1;
        }
    }
}
