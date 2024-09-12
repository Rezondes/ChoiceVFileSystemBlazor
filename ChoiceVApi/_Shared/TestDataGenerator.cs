using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChoiceVApi._Shared;

public static class TestDataGenerator
{
    private static Random _random = new Random();

    // Generiert eine Liste von zufälligen Daten für ein beliebiges Modell
    public static List<T> GenerateList<T>(int count) where T : new()
    {
        var list = new List<T>();
        for (int i = 0; i < count; i++)
        {
            list.Add(GenerateSingle<T>());
        }
        return list;
    }

    // Generiert ein einzelnes zufälliges Objekt für ein beliebiges Modell
    public static T GenerateSingle<T>() where T : new()
    {
        var obj = new T();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(prop => prop.CanWrite); // Nur schreibbare Properties

        foreach (var property in properties)
        {
            object randomValue = GetRandomValue(property.PropertyType);
            property.SetValue(obj, randomValue);
        }

        return obj;
    }

    // Hilfsmethode zum Generieren eines zufälligen Wertes basierend auf dem Datentyp
    private static object GetRandomValue(Type type)
    {
        if (type == typeof(int))
        {
            return _random.Next(1, 100);
        }
        else if (type == typeof(double))
        {
            return _random.NextDouble() * 100;
        }
        else if (type == typeof(string))
        {
            return GenerateRandomString(10);
        }
        else if (type == typeof(DateTime))
        {
            return DateTime.Now.AddDays(_random.Next(-1000, 1000));
        }
        else if (type == typeof(bool))
        {
            return _random.Next(0, 2) == 1;
        }
        else if (type.IsEnum)
        {
            Array values = Enum.GetValues(type);
            return values.GetValue(_random.Next(values.Length));
        }
        else
        {
            // Für komplexe Typen rekursiv das Objekt erstellen
            return Activator.CreateInstance(type);
        }
    }

    // Hilfsmethode zum Generieren eines zufälligen Strings
    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
