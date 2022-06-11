using TextBankSpace;

public enum PetTypeName
{
	nulo = -1,
	Agua,
	Fogo,
	Planta,
	Terra,
	Pedra,
	Psiquico,
	Eletrico,
	Normal,
	Veneno,
	Inseto,
	Voador,
	Gas,
	Gelo
}

public static class TypeNameInLanguages
{
	public static string Get(PetTypeName p)
	{
		return TextBank.RetornaListaDeTextoDoIdioma(TextKey.nomeTipos)[(int)p];
	}
}