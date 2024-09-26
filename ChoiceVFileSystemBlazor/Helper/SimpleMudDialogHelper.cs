using ChoiceVFileSystemBlazor.Components._Shared;
using ChoiceVFileSystemBlazor.Models;
using MudBlazor;

namespace ChoiceVFileSystemBlazor.Helper;

public static class SimpleMudDialogHelper
{
    public static async Task<List<InputModel>?> OpenDialog(this IDialogService dialogService, string title, string description, string submitButtonText, List<InputModel> inputModels)
    {
        var parameter = new DialogParameters<SimpleMudDialog>
        {
            { x => x.Description, description },
            { x => x.Inputs, inputModels },
            { x => x.SubmitButtonText, submitButtonText },
        };

        var dialog = await dialogService.ShowAsync<SimpleMudDialog>(title, parameter, new DialogOptions { FullWidth = true });
        var dialogResult = await dialog.Result;

        if (dialogResult == null || dialogResult.Canceled) return null;
        
        return dialogResult.Data as List<InputModel>;
    }

    public static (bool, T?) ValidateInput<T>(this InputModel inputModel)
    {
        var value = inputModel.Value;
        
        if (typeof(T) == typeof(string))
        {
            return string.IsNullOrEmpty(inputModel.Value) ? (false, default) : (true, (T?)(object)inputModel.Value);
        }
    
        if (typeof(T) == typeof(ulong))
        {
            return ulong.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(long))
        {
            return long.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(int))
        {
            return int.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(double))
        {
            return double.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(decimal))
        {
            return decimal.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(float))
        {
            return float.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }   
        
        if (typeof(T) == typeof(DateTime))
        {
            return DateTime.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }
        
        if (typeof(T) == typeof(bool))
        {
            return bool.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }
        
        if (typeof(T) == typeof(byte))
        {
            return byte.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(sbyte))
        {
            return sbyte.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(short))
        {
            return short.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(ushort))
        {
            return ushort.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }

        if (typeof(T) == typeof(char))
        {
            return char.TryParse(value, out var parsedValue) ? (true, (T?)(object)parsedValue) : (false, default);
        }
        
        if (typeof(T).IsEnum)
        {
            try
            {
                var parsedValue = (T)Enum.Parse(typeof(T), value, true);
                return (true, parsedValue);
            }
            catch
            {
                return (false, default);
            }
        }
        
        return (false, default);
    }
}