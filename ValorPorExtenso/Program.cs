using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ValorPorExtenso
{
	class Program
	{
		static Dictionary<int, string> DicNumero { get; } = new Dictionary<int, string>()
		{
			{1,"um"},
			{2,"dois"},
			{3,"três"},
			{4,"quatro"},
			{5,"cinco"},
			{6,"seis"},
			{7,"sete"},
			{8,"oito"},
			{9,"nove"}
		};

		static Dictionary<int, string> DicNumeroV1 { get; } = new Dictionary<int, string>()
		{
			{2,"vinte"},
			{3,"trinta"},
			{4,"quarenta"},
			{5,"cinquenta"},
			{6,"sessenta"},
			{7,"setenta"},
			{8,"oitotenta"},
			{9,"noventa"}
		};

		static Dictionary<int, string> DicNumeroV2 { get; } = new Dictionary<int, string>()
		{
			{10,"dez"},
			{11,"onze"},
			{12,"doze"},
			{13,"treze"},
			{14,"quatorze"},
			{15,"quinze"},
			{16,"dezesseis"},
			{17,"dezessete"},
			{18,"dezoito"},
			{19,"dezenove"}
		};

		static Dictionary<int, string> DicNumeroV3 { get; } = new Dictionary<int, string>()
		{
			{1,"cento"},
			{2,"duzentos"},
			{3,"trezentos"},
			{4,"quatrocentos"},
			{5,"quinhentos"},
			{6,"seiscentos"},
			{7,"setecentos"},
			{8,"oitocentos"},
			{9,"novecentos"},
		};


		static void Main(string[] args)
		{
			Console.WriteLine(ValorExtenso("988.165.256,25"));
		}

		static string ValorExtenso(string numero)
		{
			var arr = numero.Split(".".ToCharArray());

			if (arr.Length >= 1)
			{
				Match match;
				var sb = new StringBuilder();
				var centavos = numero.Split(",".ToCharArray())[1];
				var currency = new List<string>();

				arr = numero.Remove(numero.LastIndexOf(",")).Split(".".ToCharArray());

				for (int i = 0; i < arr.Length; i++)
				{
					match = Regex.Match(arr[i], "([0-9]){1}([0-9]{1})?([0-9]{1})?");

					if (match.Success)
					{
						if (!string.IsNullOrEmpty(match.Groups[1].Value) &&
							!string.IsNullOrEmpty(match.Groups[2].Value) &&
							!string.IsNullOrEmpty(match.Groups[3].Value))//3 casas
						{
							currency.Add(TresCasas(match.Value));
						}
						else if (!string.IsNullOrEmpty(match.Groups[1].Value) &&
							!string.IsNullOrEmpty(match.Groups[2].Value) &&
							string.IsNullOrEmpty(match.Groups[3].Value)) //2 casas
						{
							currency.Add(DuasCasas($"{match.Groups[1].Value}{match.Groups[2].Value}"));
						}
						else if (!string.IsNullOrEmpty(match.Groups[1].Value) &&
								string.IsNullOrEmpty(match.Groups[2].Value) &&
								string.IsNullOrEmpty(match.Groups[3].Value)) //1 casas
						{
							if (match.Groups[1].Value != "0")
								currency.Add($"{DicNumero[int.Parse(match.Groups[1].Value)]}");
						}
					}
				}

				bool temCentavos = centavos != "00";

				if (temCentavos)
					currency.Add(DuasCasas(numero.Split(",".ToCharArray())[1]));

				string comp;

				for (int i = 0; i < currency.Count; i++)
				{
					if (currency.Count == 4 || currency.Count == 3 && !temCentavos)
						comp = i switch
						{
							0 => currency[i] == "um" ? "milhão " : "milhões ",
							1 => !string.IsNullOrEmpty(currency[i]) ? "mil e " : "e ",
							2 => !string.IsNullOrEmpty(currency[i]) ? temCentavos ? "reais e " : "reais" : string.Empty,
							_ => ""
						};
					else if (currency.Count == 3)
						comp = i switch
						{
							0 => "mil e ",
							1 => "reais e ",
							_ => ""
						};
					else if (currency.Count == 2 && temCentavos)
						comp = i switch
						{
							0 => "reais e ",
							_ => ""
						};
					else if (currency.Count == 2 && !temCentavos)
						comp = i switch
						{
							0 => "mil e ",
							_ => "reais"
						};
					else
						comp = "reais";

					sb.Append($"{(!string.IsNullOrEmpty(currency[i]) ? currency[i] + " " + comp : comp)}");
				}

				return $"{sb}{(temCentavos ? "centavos" : "")}";
			}

			return numero;
		}

		static string TresCasas(string c)
		{
			int casa1 = int.Parse(c[0].ToString()), casa2 = int.Parse(c[1].ToString()), casa3 = int.Parse(c[2].ToString());

			if (casa1 > 0 && casa2 > 0 && casa3 > 0)
				c = $"{DicNumeroV3[casa1]} e {DuasCasas($"{casa2}{casa3}")}";
			else if (casa1 == 0 && casa2 > 0 && casa3 > 0) //023
				c = DuasCasas($"{casa2}{casa3}");
			else if (casa1 > 0 && casa2 == 0 && (casa3 == 0 || casa3 > 0)) //300
			{
				if (casa3 > 0 && casa2 == 0)
					c = $"{DicNumeroV3[casa1]} e {DicNumero[casa3]}";
				else
					c = DicNumeroV3[casa1];
			}
			else if (casa1 == 0 && casa2 == 0 && casa3 > 0)
				c = DicNumero[casa3];
			else if (casa1 == 0 && casa2 == 0 && casa3 == 0)
				c = string.Empty;

			return c;
		}

		static string DuasCasas(string c)
		{
			int casa1 = int.Parse(c[0].ToString()), casa2 = int.Parse(c[1].ToString()), casa3 = int.Parse($"{casa1}{casa2}");

			if (casa1 == 1)
				c = DicNumeroV2[casa3].ToString();
			else if (casa1 == 0)
				c = DicNumero[casa2];
			else if (casa1 > 1 && casa2 == 0)
				c = $"{DicNumeroV1[casa1]}";
			else if (casa1 > 1 && casa2 > 0)
				c = $"{DicNumeroV1[casa1]} e {DicNumero[casa2]}";

			return c;
		}
	}
}
