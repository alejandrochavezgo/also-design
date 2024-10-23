namespace common.conversions;

using System.Text;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using common.logging;
using entities.models;
using common.constants;
using entities.enums;

public sealed class conversionManager
{
    public static log logger = new log();
    private const string englishDateFormat = "yyyy-dd-MM HH:mm:ss.fff";
    private const string standardDateformat = "yyyy-MM-dd HH:mm:ss.fff";
    private const string listDateFormat = "yyyyMMdd_HHmmss";
    private const string dateFormatString = "yyyyMMddHHmmss";
    private const string watermarkDateFormatString = "ddMMyyyy-HH:mm:ss";
    private const string kendoCalDateFormatString = "MMM/dd/yyyy";
    private static DateTime minDBDate { get; set; }
    public static string toEnglishFormat(string date)
    {
        try
        {

        }
        catch
        {

            throw;
        }
        string Result = string.Empty;
        try
        {
            DateTime dtTemp = new DateTime();
            minDBDate = new DateTime(1900, 1, 1);
            Result = (DateTime.TryParse(date, out dtTemp)) ? dtTemp.ToString(englishDateFormat) : minDBDate.ToString(englishDateFormat);
        }
        catch
        {
            throw;
        }
        return Result;
    }

    /// <summary>
    /// Method returning a standard date format
    /// </summary>
    /// <param name="utcDate"></param>
    /// <returns></returns>
    public static string toStandardDate(string utcDate)
    {
        string Result = string.Empty;
        try
        {
            DateTime dtTemp = new DateTime();
            minDBDate = new DateTime(1900, 1, 1);
            Result = (DateTime.TryParse(utcDate, out dtTemp)) ? dtTemp.ToString(standardDateformat) : minDBDate.ToString(standardDateformat);
        }
        catch
        {
            throw;
        }
        return Result;
    }

    /// <summary>
    /// Method returning a datetime from a kendo calendar formatted date MMM/dd/yyyy
    /// </summary>
    /// <param name="kendoDate"></param>
    /// <returns></returns>
    public static DateTime kendoCalToDate(string kendoDate)
    {
        DateTime result = DateTime.Now;
        try
        {
            result = DateTime.ParseExact(kendoDate, "MMM/dd/yyyy", CultureInfo.InvariantCulture);
        }
        catch
        {
            throw;
        }
        return result;
    }

    /// <summary>
    /// Method that retrieves a date with the format established for a watermark
    /// </summary>
    /// <param name="Date"></param>
    /// <returns></returns>
    public static string toWatermarkDate(string Date)
    {
        string result = string.Empty;
        try
        {
            DateTime dtTemp = new DateTime();
            minDBDate = new DateTime(1900, 1, 1);
            result = (DateTime.TryParse(Date, out dtTemp)) ? dtTemp.ToString(watermarkDateFormatString) : minDBDate.ToString(watermarkDateFormatString);
        }
        catch
        {
            throw;
        }
        return result;
    }

    /// <summary>
    /// Method that retrieves a date with the format established for report exportations
    /// </summary>
    /// <param name="dtDate"></param>
    /// <returns></returns>
    public static string toReportDate(DateTime dtDate)
    {
        string result = string.Empty;
        try
        {
            result = dtDate.ToString(listDateFormat);
        }
        catch
        {
            throw;
        }
        return result;
    }

    /// <summary>
    /// Method that retrieves a date with the format yyyyMMddhhmmss
    /// </summary>
    /// <param name="dtDate"></param>
    /// <returns></returns>
    public static string toStringDate(DateTime dtDate)
    {
        string result = string.Empty;
        try
        {
            result = dtDate.ToString(dateFormatString);
        }
        catch
        {
            throw;
        }
        return result;
    }

    /// <summary>
    /// Method that casts any List into a DynamicList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<dynamicDictionary> toDynamicList<T>(IList<T> list)
    {
        List<dynamicDictionary> listResult = new List<dynamicDictionary>();

        try
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                dynamicDictionary Element = new dynamicDictionary();
                for (int i = 0; i < values.Length; i++)
                {
                    PropertyDescriptor prop = props[i];
                    Element.tryAddMember(prop.Name, prop.GetValue(item));
                }
                listResult.Add(Element);
            }
            return listResult;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Method that casts from byte Array to Bitmap
    /// </summary>
    /// <param name="imageBytes"></param>
    /// <returns></returns>
    private Bitmap getImage(byte[] imageBytes)
    {
        try
        {
            MemoryStream ms = new MemoryStream(imageBytes, 0,
                imageBytes.Length, true, false);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            ms.Flush();
            ms.Close();
            ms.Dispose();
            return (Bitmap)image;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Method that casts one dynamic list of objects to a datatable
    /// </summary>
    /// <param name="dataList">List instance of csDynamicDictionary objects, with data to cast into a Datatable.</param>
    /// <returns></returns>
    public static DataTable toDataTable(List<dynamicDictionary> dataList)
    {
        DataTable table = new DataTable();
        try
        {
            //If there's list elements
            if (dataList.Count > 0)
            {
                //Retrieve the first element of the list, with this we generate columns in the datatable
                dynamicDictionary FirstElement = dataList.First();

                //Read the properties of the first dictionary and create the columns
                foreach (var d in FirstElement.getDictionary)
                {
                    table.Columns.Add(d.Key);
                }

                //Now we retrieve the values of the given properties and we add them as rows in the result datatable
                object[] values = new object[FirstElement.count];
                foreach (dynamicDictionary item in dataList)
                {
                    Int32 index = 0;
                    foreach (var i in item.getDictionary)
                    {
                        values[index] = i.Value;
                        index++;

                    }
                    table.Rows.Add(values);
                }
            }

            return table;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Method that transforms one strongly typed list into a DataTable.(Doesn't work on nested lists)
    /// </summary>
    /// <typeparam name="T">Generic type of list elements.</typeparam>
    /// <param name="datos"> T type instance, with data to pass to the datatable.</param>
    /// <returns></returns>
    public static DataTable toDataTable<T>(IList<T> datos)
    {
        DataTable table = new DataTable();
        try
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in datos)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }

            return table;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Creates one byte array from an object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    //public static byte[] ObjectToByteArray(object obj)
    //{
    //    try
    //    {
    //        if (obj == null)
    //            return null;
    //        BinaryFormatter bf = new BinaryFormatter();
    //        MemoryStream ms = new MemoryStream();
    //        bf.Serialize(ms, obj);
    //        return ms.ToArray();
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}

    /// <summary>
    /// Cast an Object to a byte array 
    /// </summary>
    /// <param name="arrBytes"></param>
    /// <returns></returns>
    //public static object ByteArrayToObject(byte[] arrBytes)
    //{
    //    try
    //    {
    //        MemoryStream memStream = new MemoryStream();
    //        BinaryFormatter binForm = new BinaryFormatter();
    //        memStream.Write(arrBytes, 0, arrBytes.Length);
    //        memStream.Seek(0, SeekOrigin.Begin);
    //        object obj = (object)binForm.Deserialize(memStream);
    //        return obj;
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}

    /// <summary>
    /// Método que convierte una cadena del tipo: #,##,##,##,### (Dias,Horas,Minutos,Segundos,Milisegundos)
    /// a TimeSpan.
    /// HMATEO 2014_03_25
    /// </summary>
    /// <param name="expresion"></param>
    /// <returns></returns>
    public static TimeSpan aTimeSpan(object expresion)
    {
        TimeSpan resultado = new TimeSpan();
        if (expresion == null || expresion == DBNull.Value)
            return resultado;
        try
        {
            string sExp = expresion.ToString();
            char[] delimiters = new char[] { ',', '.', '-', '_', ':' };
            string[] parts = sExp.Split(delimiters);
            if (parts.Length.Equals(5))
            {
                resultado = new TimeSpan(
                                            toInt(parts[0])
                                            , toInt(parts[1])
                                            , toInt(parts[2])
                                            , toInt(parts[3])
                                            , toInt(parts[4])
                                        );
            }
            if (parts.Length.Equals(4))
            {
                resultado = new TimeSpan(
                                            toInt(parts[0])
                                            , toInt(parts[1])
                                            , toInt(parts[2])
                                            , toInt(parts[3])
                                        );
            }
            if (parts.Length.Equals(3))
            {
                resultado = new TimeSpan(
                                            toInt(parts[0])
                                            , toInt(parts[1])
                                            , toInt(parts[2])
                                        );
            }
            if (parts.Length.Equals(1))
            {
                resultado = new TimeSpan(
                                            toInt(parts[0])
                                        );
            }
        }
        catch { resultado = new TimeSpan(); }
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a cadena lógica (1) StrTrue, (0) StrFalse.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string toActive(bool expresion)
    {
        string resultado = "0";
        if (expresion)
            resultado = "1";
        return resultado;
    }
    
    public static string toSearchPatternString(object expresion)
    {
        string resultado = string.Empty;
        if (expresion == null || expresion == DBNull.Value)
            return resultado;
        try
        {
            resultado = expresion.ToString().Trim();
            resultado = resultado.Replace(" ", "%");
        }
        catch { resultado = string.Empty; }
        return resultado;
    }

    /// <summary>
    /// Devuelve una cadena válida, sin caracteres extraños, sin espacios dobles, sin acentos
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    /// <summary>
    /// Devuelve una cadena válida, sin caracteres extraños, sin espacios dobles, sin acentos
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string toString(object expresion)
    {
        string resultado = string.Empty;
        if (expresion == null || expresion == DBNull.Value)
            return resultado;
        try { resultado = expresion.ToString(); }
        catch { resultado = string.Empty; }
        return resultado.ToString();
    }

    public static string toStringB(object expresion)
    {
        string resultado = string.Empty;
        if (expresion == null || expresion == DBNull.Value)
            return resultado;
        try { resultado = expresion.ToString(); }
        catch { resultado = string.Empty; }
        resultado = deleteDoubleSpaces(resultado);
        resultado = replaceAcuteChar(resultado);
        resultado = validString(resultado);
        return resultado;
    }

    public static string toSimpleString(object expresion)
    {
        string resultado = string.Empty;
        if (expresion == null || expresion == DBNull.Value)
            return resultado;
        try { resultado = expresion.ToString().Trim(); }
        catch { resultado = string.Empty; }
        return resultado;
    }

    public static string toSimpleString2(object expresion)
    {
        string resultado = string.Empty;
        if (expresion == null || expresion == DBNull.Value)
            return resultado;
        try
        {
            resultado = expresion.ToString().Trim();
            resultado = resultado.Replace("'", "´");
        }
        catch { resultado = string.Empty; }
        return resultado;
    }

    public static string toSimpleString(object expresion, int longitud)
    {
        string resultado = toSimpleString(expresion);
        if (resultado.Length > longitud)
            resultado = resultado.Substring(0, longitud);
        return resultado;
    }

    public static string toSimpleString2(object expresion, int longitud)
    {
        string resultado = toSimpleString2(expresion);
        if (resultado.Length > longitud)
            resultado = resultado.Substring(0, longitud);
        return resultado;
    }

    /// <summary>
    /// Devuelve una cadena válida, sin caracteres extraños, sin espacios dobles, sin acentos 
    ///  y recortada a la longitud especificada.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string toString(object expresion, int longitud)
    {
        string resultado = toString(expresion);
        if (resultado.Length > longitud)
            resultado = resultado.Substring(0, longitud);
        return resultado;
    }

    public static string toStringB(object expresion, int longitud)
    {
        string resultado = toStringB(expresion);
        if (resultado.Length > longitud)
            resultado = resultado.Substring(0, longitud);
        return resultado;
    }

    /// <summary>
    /// Devuelve una cadena válida para e-mail, sin caracteres extraños, sin espacios, sin acentos y en minúsculas
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string toEmailString(object expresion)
    {
        string resultado = string.Empty;
        if (expresion == null || expresion == DBNull.Value)
            return resultado;
        try { resultado = expresion.ToString(); }
        catch { resultado = string.Empty; }
        resultado = deleteDoubleSpaces(resultado);
        resultado = deleteSpaces(resultado);
        resultado = replaceAcuteChar(resultado);
        resultado = Regex.Replace(resultado, @"[^\w.@-_]", string.Empty);
        resultado = resultado.ToLower().Replace("ñ", string.Empty);
        resultado = resultado.Replace(@"\", string.Empty);
        return resultado;
    }

    /// <summary>
    /// Devuelve el valor lógico de los valores de la Base de Datos
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    private static bool toVerificationBox(string expresion)
    {
        bool resultado = false;
        if (expresion.Equals(enviroment.strTrue))
            resultado = true;
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos decimal.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static decimal toDecimal(object expresion)
    {
        decimal resultado = 0.0M;
        if (expresion == null || expresion == DBNull.Value || expresion.ToString().Length.Equals(0))
            return resultado;
        try
        {
            CultureInfo Cultura = CultureInfo.CurrentCulture;
            string SimboloPorcentaje = Cultura.NumberFormat.PercentSymbol;

            if (expresion.ToString().Contains(SimboloPorcentaje))
                resultado = decimal.Parse(deleteChar(deleteFormat(expresion.ToString()), SimboloPorcentaje)) / 100;
            else
                resultado = decimal.Parse(deleteFormat(expresion.ToString()));
        }
        catch { resultado = 0.0M; }
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos double.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static double toDouble(object expresion)
    {
        double resultado = 0;
        if (expresion == null || expresion == DBNull.Value || expresion.ToString().Length.Equals(0))
            return resultado;
        try
        {
            resultado = double.Parse(expresion.ToString());
        }
        catch { resultado = 0; }
        return resultado;
    }

    /// <summary>
    /// Devuelve una cadena que contiene solo dígitos
    /// </summary>
    /// <param name="expresion"></param>
    /// <returns></returns>
    public static string toDigits(object expresion)
    {
        string resultado = string.Empty;

        for (int i = 0; i < expresion.ToString().Length; i++)
            resultado += isNumber(expresion.ToString().Substring(i, 1))
                        ? expresion.ToString().Substring(i, 1)
                        : string.Empty;

        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos entero.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static int toInt(object expresion)
    {
        int resultado = 0;
        if (expresion == null || expresion == DBNull.Value || expresion.ToString().Length.Equals(0))
            return resultado;
        try
        {
            if (expresion is bool)
            {
                resultado = (bool)expresion ? 1 : 0;
            }
            else
            {
                if (!int.TryParse(expresion.ToString(), out resultado))
                {
                    resultado = Convert.ToInt32(Convert.ToDecimal(deleteFormat(expresion.ToString())));
                }
            }
        }
        catch { resultado = 0; }
        return resultado;
    }

    /// <summary>
    /// Devuelve un entero en su equivalente número romano.
    /// HMMO
    /// </summary>
    /// <param name="numero">Número arábigo que será convertido a su equivalente romano.</param>
    /// <returns></returns>
    public static string toRoman(int numero)
    {
        string resultado = string.Empty;
        if (numero < 0 || numero > 99999)
            return "overflow!";
        try
        {
            resultado = romanNumeral.cast(numero);

        }
        catch { resultado = "error!"; }
        return resultado;
    }
    /// <summary>
    /// Método para retornar el mes correspondiente a un entero del 1 al 12
    /// HMMO
    /// </summary>
    /// <param name="month">Mes entero Dominio {1-12}</param>
    /// <returns></returns>
    public static string monthName(int month)
    {
        try
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-MX", false).DateTimeFormat;

            return dtinfo.GetMonthName(month);

        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Método para capitalizar textos.
    /// HMMO
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string capitalizeFirstLetter(string value)
    {
        try
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);

        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos entero corto.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static short aEnteroCorto(object expresion)
    {
        short resultado = 0;
        if (expresion == null || expresion == DBNull.Value || expresion.ToString().Length.Equals(0))
            return resultado;
        try
        {
            if (expresion is bool)
            {
                resultado = (bool)expresion ? (short)1.0 : (short)0.0;
            }
            else
            {
                if (!short.TryParse(expresion.ToString(), out resultado))
                {
                    resultado = Convert.ToInt16(Convert.ToDecimal(deleteFormat(expresion.ToString())));
                }
            }
        }
        catch { resultado = 0; }
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos byte.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static byte toByte(object expresion)
    {
        byte resultado = 0;
        if (expresion == null || expresion == DBNull.Value || expresion.ToString().Length.Equals(0))
            return resultado;
        try
        {
            if (expresion is bool)
            {
                resultado = (bool)expresion ? (byte)1.0 : (byte)0.0;
            }
            else
            {
                if (!byte.TryParse(expresion.ToString(), out resultado))
                {
                    resultado = Convert.ToByte(Convert.ToDecimal(deleteFormat(expresion.ToString())));
                }
            }
        }
        catch { resultado = 0; }
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos entero largo.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static long toLongInt(object expresion)
    {
        long resultado = 0;
        if (expresion == null || expresion == DBNull.Value || expresion.ToString().Length.Equals(0))
            return resultado;
        try
        {
            if (expresion is bool)
            {
                resultado = (bool)expresion ? 1 : 0;
            }
            else
            {
                if (!long.TryParse(expresion.ToString(), out resultado))
                {
                    resultado = Convert.ToInt64(Convert.ToDecimal(deleteFormat(expresion.ToString())));
                }
            }
        }
        catch { resultado = 0; }
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos cadena de fecha.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string toDate(object expresion)
    {
        string resultado = string.Empty;
        try
        {
            DateTime fecha;
            DateTime.TryParse(expresion.ToString(), new CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
            if (!fecha.Equals(DateTime.MinValue))
                resultado = fecha.ToString(format.dateTime);
        }
        catch
        {
            resultado = string.Empty;
        }
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos cadena de fecha.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string toShortDate(object expresion)
    {
        string resultado = string.Empty;
        try
        {
            DateTime fecha;
            DateTime.TryParse(expresion.ToString(), new CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
            if (!fecha.Equals(DateTime.MinValue))
                resultado = fecha.ToString(format.shortDate);
        }
        catch
        {
            resultado = string.Empty;
        }
        return resultado;
    }

    /// <summary>
    /// Devuelve una fecha válida utilizando la cultura "es-MX". En caso de que la fecha no pueda convertirse, devuelve DateTime.MinValue
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static DateTime toValidDate(Object expresion)
    {
        DateTime fecha = DateTime.MinValue;
        //try
        //{
        //    DateTime.TryParse(expresion.ToString().Trim(), new System.Globalization.CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
        //}
        //catch
        //{
        //    fecha = DateTime.MinValue;
        //}
        try
        {
            if (expresion != null && expresion.ToString() != string.Empty)
            {
                fecha = (DateTime)expresion;
            }
        }catch (Exception ex)
        {
        }
        return fecha;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo fecha, en caso de que se trate de un MinValue devuelve DBNull.Value.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static object toStandardDate(object expresion)
    {
        object resultado = DBNull.Value;
        try
        {
            if (!expresion.ToString().Trim().Length.Equals(0))
            {
                DateTime fecha;
                DateTime.TryParse(expresion.ToString().Trim(), new System.Globalization.CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
                if (!fecha.Equals(DateTime.MinValue))
                    resultado = fecha;
            }
        }
        catch
        {
            resultado = DBNull.Value;
        }
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a imagen
    /// </summary>
    /// <param name="expresion">Expresión que se desea convertir</param>
    /// <returns></returns>
    public static Image toImage(object expresion)
    {
        Image resultado;
        resultado = Image.FromStream(new MemoryStream((Byte[])expresion));
        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a tipo de datos lógico.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static bool toBoolean(object expresion)
    {
        bool resultado = false;

        if (expresion == null || expresion == DBNull.Value || expresion.ToString().Length.Equals(0))
            return resultado;

        switch (expresion.GetType().Name)
        {
            case "String":
                resultado = expresion.ToString().ToLower().Equals("true");
                break;
            case "Int":
            case "Int32":
            case "Int64":
                resultado = !expresion.ToString().Equals("0");
                break;
            default:
                bool.TryParse(deleteFormat(expresion.ToString()), out resultado);
                break;
        }

        return resultado;
    }

    /// <summary>
    /// Devuelve la expresión convertida a DBNull en caso de cadena vacía o entero = 0.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static object toParameter(object expresion)
    {
        if (expresion is string && ((string)expresion).Length.Equals(0))
            expresion = DBNull.Value;
        if ((expresion is short && ((short)expresion).Equals(0)) || (expresion is int && ((int)expresion).Equals(0)) || (expresion is long && ((long)expresion).Equals(0)))
            expresion = DBNull.Value;
        if (expresion is DateTime && ((DateTime)expresion).Equals(DateTime.MinValue))
            expresion = Convert.ToDateTime("01/01/1900");// DBNull.Value;
        return expresion;
    }

    /// <summary>
    /// Devuelve una cadena sin caracteres extraños
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string validString(string expresion)
    {
        string resultado = string.Empty;
        if (expresion.Length.Equals(0))
            return resultado;
        for (int i = 0; i < expresion.Length; i++)
        {
            resultado += validChar(
                Convert.ToChar(expresion.Substring(i, 1))).ToString();
        }
        return resultado;
    }

    /// <summary>
    /// Devuelve sólo caracteres previamente definidos como válidos
    /// </summary>
    /// <param name="caracter">Caracter a evaluar</param>
    /// <returns></returns>
    public static string validChar(char caracter)
    {
        string resultado = string.Empty;
        if (caracter.Equals(10))
            resultado = caracter.ToString();
        if (caracter.Equals('\r'))
            resultado = caracter.ToString();
        if (caracter.Equals('\n'))
            resultado = caracter.ToString();
        if (caracter >= 32 && caracter <= 38)
            resultado = caracter.ToString();
        if (caracter >= 40 && caracter <= 90)
            resultado = caracter.ToString();
        if (caracter >= 96 && caracter <= 122)
            resultado = caracter.ToString();
        if (caracter.Equals('¡'))
            resultado = caracter.ToString();
        if (caracter.Equals('¿'))
            resultado = caracter.ToString();
        if (caracter.Equals('Ñ'))
            resultado = caracter.ToString();
        if (caracter.Equals('ñ'))
            resultado = caracter.ToString();
        return resultado;
    }

    /// <summary>
    /// Devuelve la concatenación del nombre y apellidos
    /// </summary>
    /// <param name="nombre">Nombre(s)</param>
    /// <param name="apellidoPaterno">Apellido(s) Paterno</param>
    /// <param name="apellidoMaterno">Apellidos(s) Materno</param>
    /// <returns></returns>
    public static string concatName(string nombre, string apellidoPaterno, string apellidoMaterno)
    {
        string resultado = string.Empty;
        resultado = string.Concat(string.Concat(nombre.Trim(), " ", apellidoPaterno.Trim()).Trim(), " ", apellidoMaterno.Trim()).Trim();
        return resultado;
    }

    /// <summary>
    /// Devuelve una expresion, eliminando todas las incidencias de un caracter.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <param name="valor">Caracter que se desea remover</param>
    /// <returns></returns>
    public static string deleteChar(string expresion, string caracter)
    {
        string resultado = expresion;
        while (resultado.IndexOf(caracter) > -1)
            resultado = resultado.Remove(resultado.IndexOf(caracter), 1);
        return resultado;
    }

    /// <summary>
    /// Devuelve una cadena sin formato monetario o numérico.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string deleteFormat(string expresion)
    {
        string resultado = string.Empty;
        CultureInfo Cultura = CultureInfo.CurrentCulture;
        string SimboloMoneda = Cultura.NumberFormat.CurrencySymbol;
        string SeparadorMiles = Cultura.NumberFormat.CurrencyGroupSeparator;

        resultado = deleteChar(expresion, SimboloMoneda);
        resultado = deleteChar(resultado, SeparadorMiles);

        Cultura = null;

        return resultado;
    }

    /// <summary>
    /// Devuelve la cadena equivalente para StrTrue y StrFalse
    /// </summary>
    /// <param name="expresion"></param>
    /// <returns></returns>
    public static string isActive(bool expresion)
    {
        string resultado = string.Empty;
        if (expresion)
            resultado = enviroment.strTrue;
        else
            resultado = enviroment.strFalse;
        return resultado;
    }

    /// <summary>
    /// Evalúa si la expresión es un número válido
    /// </summary>
    /// <param name="expresion"></param>
    /// <returns></returns>
    public static bool isNumber(string expresion)
    {
        bool resultado = false;
        for (int i = 0; i < expresion.Length; i++)
        {
            resultado = Regex.IsMatch(expresion.Substring(i, 1), @"[0-9]");
            if (!resultado) break;
        }
        return resultado;
    }

    /// <summary>
    /// Evalúa si la expresión es una fecha válida
    /// </summary>
    /// <param name="expresion"></param>
    /// <returns></returns>
    public static bool isDate(string expresion)
    {
        bool resultado = false;
        if (!expresion.ToString().Trim().Length.Equals(0))
        {
            DateTime fecha;
            resultado = DateTime.TryParse(expresion.ToString().Trim(), new System.Globalization.CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
        }
        return resultado;
    }

    /// <summary>
    /// Genera una cadena de caracterres aleatoria de acuerdo a la longitud especificada.
    /// </summary>
    /// <param name="longitud"></param>
    /// <returns></returns>
    public static string generarCadena(int longitud)
    {
        StringBuilder resultado = new StringBuilder();
        Random aleatorio = new Random();
        char caracter;
        for (int i = 0; i < longitud; i++)
        {
            caracter = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * aleatorio.NextDouble() + 65)));
            resultado.Append(caracter);
        }
        return resultado.ToString();
    }

    /// <summary>
    /// Devuelve sólo los caracteres del 0-9
    /// </summary>
    /// <param name="caracter">Caracter a evaluar</param>
    /// <returns></returns>
    public static string validNumber(string caracter)
    {
        return Regex.IsMatch(caracter, @"[0-9]")
                    ? caracter
                    : string.Empty;
    }

    /// <summary>
    /// Devuelve el caracter evaluado sin acento.
    /// </summary>
    /// <param name="caracter">Caracter a evaluar</param>
    /// <returns></returns>
    public static string replaceAcuteChar(char caracter)
    {
        string resultado = string.Empty;
        string[] vocal = new string[6] { "", "a", "e", "i", "o", "u" };
        bool mayuscula = (caracter.ToString().Equals(caracter.ToString()));
        resultado = caracter.ToString().ToLower();
        switch (resultado)
        {
            // a
            case "á": resultado = vocal[1]; break;
            case "à": resultado = vocal[1]; break;
            case "â": resultado = vocal[1]; break;
            case "ä": resultado = vocal[1]; break;
            case "ã": resultado = vocal[1]; break;
            case "å": resultado = vocal[1]; break;
            // e
            case "é": resultado = vocal[2]; break;
            case "è": resultado = vocal[2]; break;
            case "ê": resultado = vocal[2]; break;
            case "ë": resultado = vocal[2]; break;
            // i
            case "í": resultado = vocal[3]; break;
            case "ì": resultado = vocal[3]; break;
            case "î": resultado = vocal[3]; break;
            case "ï": resultado = vocal[3]; break;
            // o
            case "ó": resultado = vocal[4]; break;
            case "ò": resultado = vocal[4]; break;
            case "ô": resultado = vocal[4]; break;
            case "ö": resultado = vocal[4]; break;
            // u
            case "ú": resultado = vocal[5]; break;
            case "ù": resultado = vocal[5]; break;
            case "û": resultado = vocal[5]; break;
            case "ü": resultado = vocal[5]; break;
            default: resultado = caracter.ToString(); break;
        }
        if (mayuscula)
            resultado = resultado.ToString();
        return resultado;
    }

    /// <summary>
    /// Devuelve la cadena evaluada sin acentos.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string replaceAcuteChar(string expresion)
    {
        string resultado = string.Empty;
        if (expresion.Length.Equals(0))
            return resultado;
        for (int i = 0; i < expresion.Length; i++)
        {
            resultado += replaceAcuteChar(
                Convert.ToChar(expresion.Substring(i, 1))).ToString();
        }
        return resultado;
    }

    /// <summary>
    /// Devuelve una cadena sin espacios.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string deleteSpaces(string expresion)
    {
        string resultado = expresion;
        resultado = deleteChar(expresion, " ");
        return resultado;
    }

    /// <summary>
    /// Devuelve una cadena para Sello de Date integrado por: año, mes, dia a dos dígitos.
    /// </summary>
    /// <returns></returns>
    public static string dateSeal()
    {
        return DateTime.Now.ToString("yyMMdd");
    }

    /// <summary>
    /// Devuelve una cadena para Sello de Movimiento integrado por: año, mes, dia, hora, minutos y segundos a dos dígitos más un sufijo de tres caracteres aleatorios.
    /// </summary>
    public static string MovSeal()
    {
        return string.Concat(timeSeal(), generarCadena(3));
    }

    /// <summary>
    /// Devuelve una cadena para Sello de Tiempo integrado por: año, mes, dia, hora, minutos y segundos a dos dígitos.
    /// </summary>
    public static string timeSeal()
    {
        return DateTime.Now.ToString("yyMMddHHmmss");
    }

    /// <summary>
    /// Devuelve una cadena sin dobles espacios sustituyendolos por espacios sencillos.
    /// </summary>
    /// <param name="expresion">Expresión que se desea evaluar</param>
    /// <returns></returns>
    public static string deleteDoubleSpaces(string expresion)
    {
        string resultado = expresion;
        string dobleEspacio = "  ";
        while (resultado.IndexOf(dobleEspacio) > -1)
            resultado = resultado.Remove(resultado.IndexOf(dobleEspacio), 1);
        return resultado;
    }

    public static string toCurrencyFormat(string expresion)
    {
        return toDecimal(expresion).ToString(format.currency);
    }

    public static string toCurrencyFormat(decimal expresion)
    {
        return expresion.ToString(format.currency);
    }

    public static string toNumericFormat(object valor, int decimales)
    {
        return toNumericFormat(toDecimal(valor), decimales);
    }

    public static string toNumericFormat(decimal valor, int decimales)
    {
        string formatoNumerico = "#,##0";
        if (decimales > 0)
            formatoNumerico = string.Concat(formatoNumerico, ".");
        for (int i = 0; i < decimales; i++)
        {
            formatoNumerico = string.Concat(formatoNumerico, "0");
        }
        return valor.ToString(formatoNumerico);
    }

    public static string toDecimalFormat(string expresion)
    {
        return toDecimal(expresion).ToString(format.Decimal);
    }

    public static string toDecimalFormat(decimal expresion)
    {
        return expresion.ToString(format.Decimal);
    }

    public static string toPercentageFormat(string expresion, bool APorciento)
    {
        decimal numero = toDecimal(expresion);
        string resultado = string.Empty;

        CultureInfo Cultura = CultureInfo.CurrentCulture;
        string SimboloPorcentaje = Cultura.NumberFormat.PercentSymbol;
        Cultura = null;
        if (APorciento)
        {
            numero = numero / 100;
            resultado = numero.ToString(format.percentage);
        }
        else
        {
            numero = numero * 100;
            resultado = numero.ToString(format.decimal4);
        }
        return resultado;
    }

    public static string toPercentageFormat(decimal expresion)
    {
        return expresion.ToString(format.percentage);
    }

    public static string toShorDateFormat(object expresion)
    {
        string resultado = string.Empty;
        try
        {
            DateTime fecha;
            if (expresion.ToString().Length.Equals(6))
                expresion = expresion.ToString().Insert(2, "/").Insert(5, "/");
            else if (expresion.ToString().Length.Equals(8))
                expresion = expresion.ToString().Insert(2, "/").Insert(5, "/");

            DateTime.TryParse(expresion.ToString(), new CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
            if (!fecha.Equals(DateTime.MinValue))
                resultado = fecha.ToString(format.shortDate);
        }
        catch
        {
            resultado = string.Empty;
        }
        return resultado;
    }

    public static string toDateTimeFormat(object expresion)
    {
        string resultado = string.Empty;
        try
        {
            DateTime fecha;
            if (expresion.ToString().Length.Equals(6))
                expresion = expresion.ToString().Insert(2, "/").Insert(5, "/");
            else if (expresion.ToString().Length.Equals(8))
                expresion = expresion.ToString().Insert(2, "/").Insert(5, "/");

            DateTime.TryParse(expresion.ToString(), new System.Globalization.CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
            if (!fecha.Equals(DateTime.MinValue))
                resultado = fecha.ToString(format.dateTime);
        }
        catch
        {
            resultado = string.Empty;
        }
        return resultado;
    }

    public static string toHourFormat(object expresion)
    {
        string resultado = string.Empty;
        try
        {
            DateTime fecha;
            DateTime.TryParse(expresion.ToString(), new CultureInfo("es-MX"), DateTimeStyles.None, out fecha);
            if (!fecha.Equals(DateTime.MinValue))
                resultado = fecha.ToString(format.hour);
        }
        catch
        {
            resultado = string.Empty;
        }
        return resultado;
    }

    public static bool isValidIP(string ip)
    {
        int i;                          /* Contador para los bucles */
        const int NUMOCT = 4;           /* Numero de octetos en una direccion IP */
        int[] IP = new int[NUMOCT];     /* Un arreglo de 4 enteros que simula la IP */
        string[] r = ip.Split('.');     /* Esta funcion crea un arreglo de cadenas a partir de una cadena por medio de un token que toma como
                                            * argumento para mayor informacion y entendimiento de esta funcion se puede usar el MSDN online o el
                                            * que viene con su compilador */

        if (r.Length != NUMOCT)          /* Yes el numero de arreglos es diferente de 4 regresa false */
        {
            return false;

        }
        else if (r.Length == NUMOCT)    /* En caso de que sean 4 empieza la validacion */
        {
            for (i = 0; i <= (NUMOCT - 1); i++)  /* Iniciamos un ciclo que recorrera cada Octeto */
            {
                try
                {
                    IP[i] = int.Parse(r[i]);  /* Convertimos la cadena a un entero de 32 bits*/
                }
                catch
                {
                    return false;    /* Yes ocurrio un error al intentar convertir una cadena a entero regresa a la funcion llamadora con un false,
                                        * esto por que la cadena contenia texto en vez de numeros */
                }
            }

            for (i = 0; i <= (NUMOCT - 1); i++)   /* Yes llegamos a esta parte indica que no hubo un error al convertir una cadena a entero,empezamos otro ciclo */
            {
                if (IP[i] < 0 || IP[i] > 255)     /* Yes algun octeto esta fuera de rango (0 - 255) regresa a la funcion llamador con un false */
                    return false;
            }
        }

        return true;    /* Todo salio bien y podemos regresar un true =) */
    }

    #region Int validation
    /// <summary>
    /// Valid is a string is a valid integer
    /// </summary>
    /// <Autor>Geovanny Montiel</Autor>
    /// <param name="value">Any string</param>
    /// <returns>Result of the int parse</returns>
    public static bool isInteger(string value)
    {
        int integer;
        return int.TryParse(value, out integer);
    }
    #endregion

    #region Conversion of date
    /// <summary>
    /// Convert one datetime to a format depending on the language
    /// </summary>
    /// <Autor>Geovanny Montiel</Autor>
    /// <param name="language">Language to convert the date</param>
    /// <param name="date">Value of the date</param>
    /// <returns>Datetime in a specific language</returns>
    public static DateTime toDateTimeLanguage(string language, DateTime date)
    {
        if (language != null && date != null)
        {
            CultureInfo culture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = culture;
            return DateTime.ParseExact(date.ToString(format.FORMAT_DDMMYYYYHHMMSSTT), format.FORMAT_DDMMYYYYHHMMSSTT, culture.DateTimeFormat);
        }
        return date;
    }

    /// <summary>
    /// Convert one datetime to a format depending on the language
    /// </summary>
    /// <param name="language">Language to convert the date</param>
    /// <param name="date">Value of the date</param>
    /// <returns>Datetime in a specific language</returns>
    public static DateTime toDateTimeLanguage(CultureInfo culture, DateTime date, string format)
    {
        if (culture != null && date != null)
        {               
            Thread.CurrentThread.CurrentCulture = culture;
            return DateTime.ParseExact(date.ToString(format), format, culture.DateTimeFormat);
        }
        return date;
    }

    #endregion

    #region Nullable data
    /// <summary> Method that convert a int nullable to valid value to BD</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 09/09/2018 </CreationDate>
    /// <param name="data"> Int nullable</param>
    /// <returns>DbNull when data it null, int value in other case</returns>
    public static object intNullableToDbValue(int? data)
    {
        if (data == null)
        {
            return DBNull.Value;
        }
        return data;
    }

    /// <summary> Method that convert a string to int nullable. Used in factories when the int column can have null</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 09/09/2018 </CreationDate>
    /// <param name="data">Int nullable</param>
    /// <returns>Null value when string is null or empty, in other case the conversion to int</returns>
    public static int? stringToIntNulleable(string stringInteger)
    {
        int? integer = null;
        if (string.IsNullOrEmpty(stringInteger))
        {
            return integer;
        }
        else
        {
            return Convert.ToInt32(stringInteger);
        }
    }

    /// <summary> Method that convert a specified column of a DATAREADER to INT value
    /// Used when the column cannot come, and we need avoid an exception of COLUMNS didn find</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 09/09/2018 </CreationDate>
    /// <param name="dr">Data reader with results selected of a store procedure</param>
    /// <returns>Int value when if exist the column in the datareader, Zero in other cases</returns>
    public static int columnToInt(IDataReader dr, string columnName)
    {
        try
        {
            dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", columnName);
            if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                return Convert.ToInt32(dr[columnName]);
            }
        }
        catch (Exception ex)
        {
            logger.logError("ERROR AT CONVERT COLUMN OF DATA READER TO INT. Exception" + ex.Message);
            throw;
        }
        return 0;
    }

    /// <summary> Method that convert a specified column of a DATAREADER to Decimal value
    /// Used when the column cannot come, and we need avoid an exception of COLUMNS didn find</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 02/03/2020 </CreationDate>
    /// <param name="dr">Data reader with results selected of a store procedure</param>
    /// <returns>Decimal value when if exist the column in the datareader, Zero in other cases</returns>
    public static decimal columnToDecimal(IDataReader dr, string columnName)
    {
        try
        {
            dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", columnName);
            if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                return Convert.ToDecimal(dr[columnName]);
            }
        }
        catch (Exception ex)
        {
            logger.logError("ERROR AT CONVERT COLUMN OF DATA READER TO INT. Exception" + ex.Message);
            throw;
        }
        return 0;
    }

    /// <summary> Method that convert a specified column of a DATAREADER to STRING value
    /// Used when the column cannot come, and we need avoid an exception of COLUMNS didn find</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 09/09/2018 </CreationDate>
    /// <param name="dr">Data reader with results selected of a store procedure</param>
    /// <returns>String value when if exist the column in the datareader, Null in other cases</returns>
    public static string columnToString(IDataReader dr, string columnName)
    {
        try
        {
            dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", columnName);
            if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                return dr[columnName].ToString();
            }
        }
        catch (Exception ex)
        {
            logger.logError("ERROR AT CONVERT COLUMN DATA READER TO DATETIME. Exception" + ex.Message);
            throw;
        }
        return null;
    }

    /// <summary> Method that convert a specified column of a DATAREADER to DATETIME value
    /// Used when the column cannot come, and we need avoid an exception of COLUMNS didn find</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 09/09/2018 </CreationDate>
    /// <param name="dr">Data reader with results selected of a store procedure</param>
    /// <returns>Datetime value converted when if exist the column in the datareader, Minimun date in other cases</returns>
    public static DateTime columnToDateTime(IDataReader dr, string columnName)
    {
        try
        {
            dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", columnName);
            if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                return Convert.ToDateTime(dr[columnName].ToString());
            }
        }
        catch (Exception ex)
        {
            logger.logError("ERROR AT CONVERT COLUMN DATA READER TO STRING. Exception" + ex.Message);
            throw;
        }
        return DateTime.MinValue;
    }

    /// <summary> Method that convert a specified column of a DATAREADER to BOOLEAN value
    /// Used when the column cannot come, and we need avoid an exception of COLUMNS didn find</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 09/09/2018 </CreationDate>
    /// <param name="dr">Data reader with results selected of a store procedure</param>
    /// <returns>Boolean value converted when if exist the column in the datareader, false in other cases</returns>
    public static bool columnToBoolean(IDataReader dr, string columnName)
    {
        try
        {
            dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", columnName);
            if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                return Convert.ToBoolean(dr[columnName].ToString());
            }
        }
        catch (Exception ex)
        {
            logger.logError("ERROR AT CONVERT COLUMN DATA READER TO BOOLEAN. Exception" + ex.Message);
            throw;
        }
        return false;
    }

    /// <summary> Method that convert a specified column from DATAREADER to a BASE64 value
    /// Used when the column is not in the SELECT and we want to avoid an exception of COLUMNS didn find</summary>
    /// <Author> Geovanny Montiel </Author>
    /// <CreationDate> 09/09/2018 </CreationDate>
    /// <param name="dr">Data reader with results selected of a store procedure</param>
    /// <returns>Boolean value converted when if exist the column in the datareader, false in other cases</returns>
    public static byte[] columnFromBase64(IDataReader dr, string columnName)
    {
        try
        {
            dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", columnName);
            if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                if (!String.IsNullOrEmpty(dr[columnName]+""))
                {
                    return Convert.FromBase64String(dr[columnName].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            logger.logError("ERROR AT CONVERT COLUMN DATA READER TO BOOLEAN. Exception" + ex.Message);
            throw;
        }
        return null;
    }
    #endregion
}

public sealed class julianToDateTime
{
    private static int leapYear;
    private static readonly int[] DaysToMonth366;
    private static readonly int[] DaysToMonth365;

    static julianToDateTime()
    {
        DaysToMonth365 = new int[] { 0, 0x1f, 0x3b, 90, 120, 0x97, 0xb5, 0xd4, 0xf3, 0x111, 0x130, 0x14e, 0x16d };
        DaysToMonth366 = new int[] { 0, 0x1f, 60, 0x5b, 0x79, 0x98, 0xb6, 0xd5, 0xf4, 0x112, 0x131, 0x14f, 0x16e };
        leapYear = 1095;
    }

    public julianToDateTime()
    {
    }

    public static DateTime toDateTime(int julianDate)
    {
        julianDate = julianDate - 1;
        DateTime date;
        long day = new long();
        int year = DateTime.Now.Year;

        if (isLeapYear(year))
            day = (TimeSpan.TicksPerDay * julianDate) + (TimeSpan.TicksPerDay * leapYear);

        else
            day = (TimeSpan.TicksPerDay * julianDate);

        date = new DateTime(day);
        date = new DateTime(dateToTicks(year, date.Month, date.Day));

        return date;
    }
    
    private static long dateToTicks(int year, int month, int day)
    {
        if (((year >= 1) && (year <= 0x270f)) && ((month >= 1) && (month <= 12)))
        {
            int[] numArray = isLeapYear(year) ? DaysToMonth366 : DaysToMonth365;
            if ((day >= 1) && (day <= (numArray[month] - numArray[month - 1])))
            {
                int num = year - 1;
                int num2 = ((((((num * 0x16d) + (num / 4)) - (num / 100)) + (num / 400)) + numArray[month - 1]) + day) - 1;
                return (num2 * 0xc92a69c000L);
            }
        }
        throw new ArgumentOutOfRangeException(null, "ArgumentOutOfRange_BadYearMonthDay");
    }

    public static bool isLeapYear(int year)
    {
        if ((year < 1) || (year > 0x270f))
        {
            throw new ArgumentOutOfRangeException("year", "ArgumentOutOfRange_Year");
        }
        if ((year % 4) != 0)
        {
            return false;
        }
        if ((year % 100) == 0)
        {
            return ((year % 400) == 0);
        }
        return true;
    }
}