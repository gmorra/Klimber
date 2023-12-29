/******************************************************************************************************************/
/******* ¿Qué pasa si debemos soportar un nuevo idioma para los reportes, o agregar más formas geométricas? *******/
/******************************************************************************************************************/

/*
 * TODO: 
 * Refactorizar la clase para respetar principios de la programación orientada a objetos.
 * Implementar la forma Trapecio/Rectangulo. 
 * Agregar el idioma Italiano (o el deseado) al reporte.
 * Se agradece la inclusión de nuevos tests unitarios para validar el comportamiento de la nueva funcionalidad agregada (los tests deben pasar correctamente al entregar la solución, incluso los actuales.)
 * Una vez finalizado, hay que subir el código a un repo GIT y ofrecernos la URL para que podamos utilizar la nueva versión :).
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace DevelopmentChallenge.Data.Classes
{
    public class FormaGeometrica
    {
        #region Formas

        public const int Cuadrado = 1;
        public const int TrianguloEquilatero = 2;
        public const int Circulo = 3;
        public const int Trapecio = 4;

        #endregion

        #region Idiomas

        public const int Castellano = 1;
        public const int Ingles = 2;
        public const int Italiano = 3;

        #endregion

        private readonly decimal _lado;
        private readonly decimal _altura;

        public int Tipo { get; set; }

        /// <summary>
        /// Implementa parametro opcional en contructor tambien podria ser una sobrecarga de metodo
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="ancho"></param>
        /// <param name="altura"></param>
        public FormaGeometrica(int tipo, decimal ancho, decimal? altura = null)
        {
            Tipo = tipo;
            _lado = ancho;
            if (tipo == Trapecio)
                _altura = altura.GetValueOrDefault(0m);
        }

        /// <summary>
        /// Implementa el idioma por default "Ingles"
        /// </summary>
        /// <param name="formas"></param>
        /// <returns></returns>
        public static string Imprimir(List<FormaGeometrica> formas)
        {
            return Imprimir(formas, Ingles);
        }

        public static string Imprimir(List<FormaGeometrica> formas, int idioma)
        {
            var sb = new StringBuilder();

            CultureInfo ci;
            switch (idioma)
            {
                case Castellano:
                    ci = new CultureInfo("es-AR");
                    break;
                case Italiano:
                    ci = new CultureInfo("it-IT");
                    break;
                default:
                    ci = new CultureInfo("en-US");
                    break;
            }
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;


            if (formas.Count < 1) /*entiendo es mas performante el count que el Any*/
            {
                sb.Append($"<h1>{Properties.Resources.ResourceManager.GetString("Titulo_Sin_Formas")}</h1>");
            }
            else
            {
                // Hay por lo menos una forma
                // HEADER
                sb.Append($"<h1>{Properties.Resources.ResourceManager.GetString("Titulo_Con_Formas")}</h1>");


                var numeroCuadrados = (formas?.Count(x => x.Tipo == Cuadrado)).GetValueOrDefault(0);
                var numeroCirculos = (formas?.Count(x => x.Tipo == Circulo)).GetValueOrDefault(0);
                var numeroTriangulos = (formas?.Count(x => x.Tipo == TrianguloEquilatero)).GetValueOrDefault(0);
                var numeroTrapecios = (formas?.Count(x => x.Tipo == Trapecio)).GetValueOrDefault(0);

                var areaCuadrados = (formas?.Where(x => x.Tipo == Cuadrado)?.Sum(x => x.CalcularArea())).GetValueOrDefault(0m);
                var perimetroCuadrados = (formas?.Where(x => x.Tipo == Cuadrado)?.Sum(x => x.CalcularPerimetro())).GetValueOrDefault(0m);
                sb.Append(ObtenerLinea(numeroCuadrados, areaCuadrados, perimetroCuadrados, Cuadrado, idioma));

                var areaCirculos = (formas?.Where(x => x.Tipo == Circulo)?.Sum(x => x.CalcularArea())).GetValueOrDefault(0m);
                var perimetroCirculos = (formas?.Where(x => x.Tipo == Circulo)?.Sum(x => x.CalcularPerimetro())).GetValueOrDefault(0m);
                sb.Append(ObtenerLinea(numeroCirculos, areaCirculos, perimetroCirculos, Circulo, idioma));

                var areaTriangulos = (formas?.Where(x => x.Tipo == TrianguloEquilatero)?.Sum(x => x.CalcularArea())).GetValueOrDefault(0m);
                var perimetroTriangulos = (formas?.Where(x => x.Tipo == TrianguloEquilatero)?.Sum(x => x.CalcularPerimetro())).GetValueOrDefault(0m);
                sb.Append(ObtenerLinea(numeroTriangulos, areaTriangulos, perimetroTriangulos, TrianguloEquilatero, idioma));

                var areaTrapecio = (formas?.Where(x => x.Tipo == Trapecio)?.Sum(x => x.CalcularArea())).GetValueOrDefault(0m);
                var perimetroTrapecio = (formas?.Where(x => x.Tipo == Trapecio)?.Sum(x => x.CalcularPerimetro())).GetValueOrDefault(0m);
                sb.Append(ObtenerLinea(numeroTrapecios, areaTrapecio, perimetroTrapecio, Trapecio, idioma));

                // FOOTER
                sb.Append($"{Properties.Resources.ResourceManager.GetString("Footer_Total")}:<br/>");
                sb.Append($"{numeroCuadrados + numeroCirculos + numeroTriangulos + numeroTrapecios} {Properties.Resources.ResourceManager.GetString("Label_Formas")} ");
                sb.Append($"{Properties.Resources.ResourceManager.GetString("Label_Perimetro")} {(perimetroCuadrados + perimetroTriangulos + perimetroCirculos + perimetroTrapecio):#.##} ");
                sb.Append($"{Properties.Resources.ResourceManager.GetString("Label_Area")} {(areaCuadrados + areaCirculos + areaTriangulos + areaTrapecio):#.##}");

            }

            return sb.ToString();
        }

        private static string ObtenerLinea(int cantidad, decimal area, decimal perimetro, int tipo, int idioma)
        {
            if (cantidad > 0) /*los tres idiomas esto se escribe igual no es necesario una condicion mas que para pluralizar*/
                return $"{cantidad} {TraducirForma(tipo, cantidad, idioma)} | {Properties.Resources.ResourceManager.GetString("Label_Area")} {area:#.##} | {Properties.Resources.ResourceManager.GetString("Label_Perimetro")} {perimetro:#.##} <br/>";

            return string.Empty;
        }

        private static string TraducirForma(int tipo, int cantidad, int idioma)
        {
            switch (tipo)
            {
                case Cuadrado:
                    return cantidad == 1 ? Properties.Resources.ResourceManager.GetString("Nombre_Frm_Cuadrado") : Properties.Resources.ResourceManager.GetString("Nombre_Frm_Cuadrado_Plural");
                case Circulo:
                    return cantidad == 1 ? Properties.Resources.ResourceManager.GetString("Nombre_Frm_Circulo") : Properties.Resources.ResourceManager.GetString("Nombre_Frm_Circulo_Plural");
                case TrianguloEquilatero:
                    return cantidad == 1 ? Properties.Resources.ResourceManager.GetString("Nombre_Frm_Triangulo") : Properties.Resources.ResourceManager.GetString("Nombre_Frm_Triangulo_Plural");
                case Trapecio:
                    return cantidad == 1 ? Properties.Resources.ResourceManager.GetString("Nombre_Frm_Trapecio") : Properties.Resources.ResourceManager.GetString("Nombre_Frm_Trapecio_Plural");
            }

            return string.Empty;
        }


        public decimal CalcularArea()
        {
            switch (Tipo)
            {
                case Cuadrado: return _lado * _lado;
                case Circulo: return (decimal)Math.PI * (_lado / 2) * (_lado / 2);
                case TrianguloEquilatero: return ((decimal)Math.Sqrt(3) / 4) * _lado * _lado;
                case Trapecio: return (_lado + _lado) * _altura / 2;
                default:
                    throw new ArgumentOutOfRangeException(Properties.Resources.ResourceManager.GetString("Nombre_Frm_Trapecio"));
            }
        }

        public decimal CalcularPerimetro()
        {
            switch (Tipo)
            {
                case Cuadrado: return _lado * 4;
                case Circulo: return (decimal)Math.PI * _lado;
                case TrianguloEquilatero: return _lado * 3;
                case Trapecio: return _lado + _lado + _lado + _lado;
                default:
                    throw new ArgumentOutOfRangeException(Properties.Resources.ResourceManager.GetString("Nombre_Frm_Trapecio"));
            }
        }
    }
}
