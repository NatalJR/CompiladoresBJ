using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
	public class Token
	{
		public string token { get; set; }
		public string Lexema { get; set; }
		public string Tipo { get; set; }
		public int Linha { get; set; }

		public Token(string _Token, string _Lexema, string _Tipo, int _Linha)
		{
			token = _Token;
			Lexema = _Lexema;
			Tipo = _Tipo;
			Linha = _Linha;
		}

		public Token(string _Token, string _Lexema, string _Tipo)
		{
			token = _Token;
			Lexema = _Lexema;
			Tipo = _Tipo;
		}

		//public override string ToString()
		//{
		//	return "token=" + token + " lexema=" + Lexema + " tipo= " + Tipo + "\n"; 
		//}
	}
}
