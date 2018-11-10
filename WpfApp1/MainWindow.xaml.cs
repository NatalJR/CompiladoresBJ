using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
	/// <summary>
	/// Interação lógica para MainWindow.xam
	/// </summary>
	public partial class MainWindow : Window
	{
		public List<Simbolo> TabelaSimbolos { get; set; }
		public string FileContent;

		public MainWindow()
		{
			TabelaSimbolos = new List<Simbolo>
			{
				new Simbolo()
				{
					Type = "Palavras Reservadas",
					Values = new List<string>() { "BREAK", "CHOICE", "ELSE", "FOR", "IF", "NONE", "OPTION", "PRINT", "READ", "VAR", "WHILE", "INI" }
				},
				new Simbolo()
				{
					Type = "Tipo",
					Values = new List<string>() { "BOOLEAN", "CHAR", "INT", "FLOAT" }
				},
				new Simbolo()
				{
					Type = "Valores Booleanos",
					Values = new List<string>() { "TRUE", "FALSE" }
				},
				new Simbolo()
				{
					Type = "Operadores Lógicos",
					Values = new List<string>() { "NOT", "AND", "OR", "XOR" }
				},
				new Simbolo()
				{
					Type = "Operadores Aritméticos",
					Values = new List<string>() { "ADIÇÃO", "SUBTRAÇÃO", "MULTIPLICAÇÃO", "DIVISÃO" }
				},
				new Simbolo()
				{
					Type = "Operadores Relacionais",
					Values = new List<string>() { "IGUALDADE", "DIFERENÇA", "MAIOR", "MENOR", "MAIOR IGUAL", "MENOR IGUAL" }
				},
				new Simbolo()
				{
					Type = "Operadores Relacionais",
					Values = new List<string>() { "IGUALDADE", "DIFERENÇA", "MAIOR", "MENOR", "MAIOR IGUAL", "MENOR IGUAL" }
				},
				new Simbolo()
				{
					Type = "Outros Símbolos",
					Values = new List<string>() { "ATRIBUIÇÃO", "ABRE PARÊNTESES", "FECHA PARÊNTESES", "ABRE CHAVES", "FECHA CHAVES", "PONTO E VÍRGULA" }
				}
			};

			InitializeComponent();
		}

		private static int LineError(string conteudoArquivo, char caractereErro)
		{
			int contadorLinhas = 1;
			for (int i = 0; i < conteudoArquivo.Length; i++)
			{
				if (conteudoArquivo[i] == '\n' || conteudoArquivo[i] == '\r')
				{
					contadorLinhas++;
				}
				else if (conteudoArquivo[i] == caractereErro)
				{
					return contadorLinhas;
				}
			}

			return 0;
		}

		public List<Token> Automato()
		{
			List<Token> tokens = new List<Token>();
			int state = 38;
			string buffer = "";
			int line = 1;
			for (int i = 0; i < FileContent.Length; i++)
			{
				switch (state)
				{
					case 38:
						{
							if (FileContent[i] == '=')
							{
								state = 6;
								continue;
							}
							if (FileContent[i] == '!')
							{
								state = 9;
								continue;
							}
							if (FileContent[i] == '>')
							{
								state = 12;
								continue;
							}
							if (FileContent[i] == '<')
							{
								state = 15;
								continue;
							}
							if (FileContent[i] == '&')
							{
								state = 18;
								continue;
							}
							if (FileContent[i] == '|')
							{
								state = 19;
								continue;
							}
							if (FileContent[i] == '^')
							{
								state = 20;
								continue;
							}
							if (FileContent[i] == '+')
							{
								buffer += "+";
								state = 21;
								continue;
							}
							if (FileContent[i] == '-')
							{
								buffer += "-";
								state = 27;
								continue;
							}
							if (FileContent[i] == '*')
							{
								state = 30;
								continue;
							}
							if (FileContent[i] == '/')
							{
								state = 31;
								continue;
							}
							if (FileContent[i] == '\"')
							{
								state = 4;
								continue;
							}
							if (FileContent[i] == ';')
							{
								tokens.Add(new Token("ponto e virgula", ";", ";", line));
								buffer = "";
								state = 38;
								continue;
							}
							if (FileContent[i] == '(')
							{
								tokens.Add(new Token("ABRE PARÊNTESES", "(", "(", line));
								buffer = "";
								state = 38;
								continue;
							}
							if (FileContent[i] == ')')
							{
								tokens.Add(new Token("FECHA PARÊNTESES", ")", ")", line));
								buffer = "";
								state = 38;
								continue;
							}
							if (FileContent[i] == '{')
							{
								tokens.Add(new Token("ABRE CHAVES", "{", "{", line));
								buffer = "";
								state = 38;
								continue;
							}
							if (FileContent[i] == '}')
							{
								tokens.Add(new Token("FECHA CHAVES", "}", "}", line));
								buffer = "";
								state = 38;
								continue;
							}
							if (FileContent[i] == '\'')
							{
								state = 40;
								buffer = "";
								continue;
							}
							if (char.IsDigit(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 24;
								continue;
							}
							if (char.IsLetter(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 2;
								continue;
							}
							if (FileContent[i] == ' ' || FileContent[i] == '\t' || FileContent[i] == '\n')
							{

								if (FileContent[i] == '\n')
								{
									line++;
								}

								state = 36;
								continue;
							}
							//Erro: caractere invalido
							txtEditor.Text += "Erro. Caractere não permitido, " + FileContent[i] + " linha " + LineError(FileContent, FileContent[i]) + ".";
						}
						break;
					case 6:
						{
							if (FileContent[i] == '=')
							{
								state = 8;
								continue;
							}
							else
							{
								state = 7;
								i--;
							}
						}
						break;
					case 8:
						{
							tokens.Add(new Token("IGUALDADE", "==", "operador_igualdade", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 12:
						{
							if (FileContent[i] == '=')
							{
								state = 14;
								continue;
							}
							else
							{
								state = 13;
								i--;
							}
						}
						break;
					case 14:
						{
							tokens.Add(new Token("MAIOR IGUAL", ">=", "operador_relacional", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 15:
						{
							if (FileContent[i] == '=')
							{
								state = 17;
								continue;
							}
							else
							{
								state = 16;
								i--;
							}
						}
						break;
					case 17:
						{
							tokens.Add(new Token("MENOR IGUAL", "<=", "operador_relacional", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 9:
						{
							if (FileContent[i] == '=')
							{
								state = 11;
								continue;
							}
							else
							{
								state = 10;
								i--;
							}
						}
						break;
					case 11:
						{
							tokens.Add(new Token("DIFERENÇA", "!=", "operador_igualdade", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 18:
						{
							tokens.Add(new Token("AND", "&", "operador_booleano", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 19:
						{
							tokens.Add(new Token("OR", "|", "operador_booleano", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 20:
						{
							tokens.Add(new Token("XOR", "^", "operador_booleano", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 21:
						{
							if (char.IsDigit(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 24;
								continue;
							}
							else
							{
								state = 22;
								i--;
							}
						}
						break;
					case 27:
						{

							if (char.IsDigit(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 24;
								continue;
							}
							else
							{
								state = 28;
								i--;
							}
						}
						break;
					case 30:
						{
							tokens.Add(new Token("MULTIPLICAÇÃO", "*", "operador_aritmetico", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 31:
						{
							if (FileContent[i] == '/')
							{
								state = 32;
								continue;
							}
							else
							{
								state = 34;
								continue;
							}
						}
					case 7:
						{
							tokens.Add(new Token("ATRIBUIÇÃO", "=", "=", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 10:
						{
							tokens.Add(new Token("NEGAÇÃO", "!", "operador_booleano", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 13:
						{
							tokens.Add(new Token("MAIOR", ">", "operador_relacional", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 16:
						{
							tokens.Add(new Token("MENOR", "<", "operador_relacional", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 22:
						{
							tokens.Add(new Token("ADIÇÃO", "+", "operador_aritmetico", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 28:
						{
							tokens.Add(new Token("SUBTRAÇÃO", "-", "operador_aritmetico", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 36:
						{
							if (FileContent[i] == ' ' || FileContent[i] == '\t' || FileContent[i] == '\n')
							{
								state = 36;
								if (FileContent[i] == '\n')
								{
									line++;
								}

								continue;
							}
							else
							{
								state = 38;
								i--;
								continue;
							}
						}
					case 3:
						{
							if (char.IsLetter(FileContent[i]) || char.IsDigit(FileContent[i]) || FileContent[i] == '_')
							{
								buffer += FileContent[i];
								state = 3;
								continue;
							}
							else
							{
								tokens.Add(new Token("ID", buffer, "id", line));
								buffer = "";
								state = 38;
								i--;
							}
						}
						break;
					case 34:
						{
							tokens.Add(new Token("DIVISÃO", "/", "operador_aritmetico", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 32:
						{
							if (FileContent[i] == '\n')
							{

								if (FileContent[i] == '\n')
								{
									line++;
								}

								buffer = "";
								state = 33;
								continue;
							}
							else
							{
								state = 32;
								continue;
							}
						}
					case 33:
						{
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 24:
						{
							if (char.IsDigit(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 24;
								continue;
							}
							else
							{
								if (FileContent[i] == '.')
								{
									buffer += FileContent[i];
									state = 25;
									continue;
								}
								else
								{
									tokens.Add(new Token("NÚMERO", buffer, "valor_inteiro", line));
									buffer = "";
									state = 38;
									i--;
								}
							}
						}
						break;
					case 25:
						{
							if (char.IsDigit(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 26;
								continue;
							}
						}
						break;
					case 26:
						{
							if (char.IsDigit(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 26;
								continue;
							}
							else
							{
								tokens.Add(new Token("NÚMERO", buffer, "valor_float", line));
								buffer = "";
								state = 38;
								i--;
							}
						}
						break;
					case 2:
						{
							if (char.IsLetter(FileContent[i]))
							{
								buffer += FileContent[i];
								state = 2;
								continue;
							}
							else
							{
								if (char.IsDigit(FileContent[i]) || FileContent[i] == '_')
								{
									buffer += FileContent[i];
									state = 3;
									continue;
								}
								else
								{
									if (TabelaSimbolos.Find(x=>x.Type=="Palavras Reservadas").Values.Contains(buffer.ToUpper()))
									{
										tokens.Add(new Token(buffer.ToUpper(), buffer, buffer, line));
									}
									else if (TabelaSimbolos.Find(x=>x.Type=="Tipo").Values.Contains(buffer.ToUpper()))
									{
										tokens.Add(new Token(buffer.ToUpper(), buffer, buffer, line));
									}
									else if (TabelaSimbolos.Find(x=>x.Type=="Valores Booleanos").Values.Contains(buffer.ToUpper()))
									{
										tokens.Add(new Token(buffer.ToUpper(), buffer, "valor_booleano", line));
									}
									else
									{
										tokens.Add(new Token("id", buffer, "id", line));
									}

									buffer = "";
									state = 38;
									i--;
								}
							}
						}
						break;
					case 4:
						{
							if (FileContent[i] == '\"')
							{
								state = 5;
								continue;
							}
							else
							{
								buffer += FileContent[i];
								state = 4;
								continue;
							}
						}
					case 5:
						{
							tokens.Add(new Token("STRING", buffer, "string", line));
							buffer = "";
							state = 38;
							i--;
						}
						break;
					case 40:
						{
							tokens.Add(new Token("CARACTERE", char.ToString(FileContent[i]), "valor_caractere", line));
							buffer = "";
							state = 41;
						}
						break;
					case 41:
						{
							if (FileContent[i] == '\'')
							{
								buffer = "";
								state = 38;
							}
						}
						break;

				}
			}
			return tokens;

		}
		private void BtnLexicAnalyser_Click(object sender, RoutedEventArgs e)
		{
			List<Token> tokens = Automato();
			txtEditor.Text += "\r\n\r";
			foreach(var token in tokens)
			{
				txtEditor.Text+=token.ToString();
			}
		}

		private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "BJ (*.bj)|*.bj|All files (*.*)|*.*"
			};

			if (openFileDialog.ShowDialog() == true)
			{
				var fileContent = File.ReadAllText(openFileDialog.FileName);

				//Regex.Replace(fileContent, @"(""[^""\\]*(?:\\.[^""\\]*)*"")|\s+", "$1");
				var aidstring = fileContent.Replace("\r", string.Empty);
				FileContent = aidstring;
				txtEditor.Text = openFileDialog.FileName;
			}

		}
	}
}
