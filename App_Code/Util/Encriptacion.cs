using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Descripción breve de Encriptacion
/// </summary>
public static class Encriptacion
{

    private static string key = "CadenaDeEncriptacion_3.1416";

    // Función para cifrar una cadena.
    public static string CifrarCadena(string cadena){

        byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.
        byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena); //Arreglo donde guardaremos la cadena descifrada.

        // Ciframos utilizando el Algoritmo MD5.
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
        md5.Clear();

        //Ciframos utilizando el Algoritmo 3DES.
        TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
        tripledes.Key = llave;
        tripledes.Mode = CipherMode.ECB;
        tripledes.Padding = PaddingMode.PKCS7;
        ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
        byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
        tripledes.Clear();

        return Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.
    }
}