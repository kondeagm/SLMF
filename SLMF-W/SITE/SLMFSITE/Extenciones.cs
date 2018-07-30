namespace SLMFSITE
{
    public static class Extensiones
    {
        public static string QuitaAcentos(this string sCadena)
        {
            string sLetrasConSignos = "áéíóúñÁÉÍÓÚÑ";
            string sLetrasSinSignos = "aeiounAEIOUN";
            for (int v = 0; v < sLetrasSinSignos.Length; v++)
            {
                string i = sLetrasConSignos.Substring(v, 1);
                string j = sLetrasSinSignos.Substring(v, 1);
                sCadena = sCadena.Replace(i, j);
            }
            return sCadena;
        }

        public static string DivideTexto(this string sCadena)
        {
            string sNuevaCadena = sCadena;
            int iIteracciones = sNuevaCadena.Length;
            int iNoCaracteres = 0;
            int iPosicion = 0;
            int iTamanoCadena = sNuevaCadena.Length;
            while (iIteracciones > 0)
            {
                iPosicion = sNuevaCadena.IndexOf(" ");
                if (iPosicion == -1)
                {
                    iIteracciones = 0;
                }
                else
                {
                    if ((1 + iPosicion + iNoCaracteres) > 21)
                    {
                        iIteracciones = 0;
                    }
                    else
                    {
                        iNoCaracteres = iNoCaracteres + iPosicion + 1;
                        sNuevaCadena = sCadena.Substring((iNoCaracteres), (@iTamanoCadena - iNoCaracteres));
                    }
                }
                iIteracciones--;
            }
            sCadena = sCadena.Substring(0, iNoCaracteres);
            return sCadena;
        }

        public static string SegundaPalabra(this string sCadena)
        {
            string sNuevaCadena = sCadena;
            int iNoCaracteres = 0;
            int iPosicion = 0;
            int iTamanoCadena = sNuevaCadena.Length;
            iPosicion = sNuevaCadena.IndexOf(" ");
            if (iPosicion != -1)
            {
                iNoCaracteres = iPosicion + 1;
                sNuevaCadena = sCadena.Substring((iNoCaracteres), (@iTamanoCadena - iNoCaracteres));
                sCadena = sNuevaCadena;
            }
            return sCadena;
        }

        public static string DivideTitulo(this string sCadena, int iParte)
        {
            string sNuevaCadena = sCadena;
            int iIteracciones = sNuevaCadena.Length;
            int iNoCaracteres = 0;
            int iPosicion = 0;
            int iTamanoCadena = sNuevaCadena.Length;
            while (iIteracciones > 0)
            {
                iPosicion = sNuevaCadena.IndexOf(" ");
                if (iPosicion == -1)
                {
                    iIteracciones = 0;
                }
                else
                {
                    if ((1 + iPosicion + iNoCaracteres) > 10)
                    {
                        iIteracciones = 0;
                    }
                    else
                    {
                        iNoCaracteres = iNoCaracteres + iPosicion + 1;
                        sNuevaCadena = sCadena.Substring((iNoCaracteres), (@iTamanoCadena - iNoCaracteres));
                    }
                }
                iIteracciones--;
            }
            if (iParte == 1)
            {
                sCadena = sCadena.Substring(0, iNoCaracteres);
            }
            else
            {
                sCadena = sCadena.Substring((iNoCaracteres), (@iTamanoCadena - iNoCaracteres));
            }
            return sCadena;
        }
    }
}