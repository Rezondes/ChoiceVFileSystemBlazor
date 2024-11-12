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

        if (string.IsNullOrEmpty(value) && !typeof(T).IsValueType)
        {
            return (false, default);
        }

        try
        {
            if (typeof(T) == typeof(string))
            {
                return (true, (T?)(object)value);
            }
        
            
            if (typeof(T).IsEnum)
            {
                if (Enum.TryParse(typeof(T), value, true, out var enumValue))
                {
                    return (true, (T?)enumValue);
                }
                return (false, default);
            }

            
            var parsedValue = Convert.ChangeType(value, typeof(T));
            return (true, (T?)parsedValue);
        }
        catch
        {
            return (false, default);
        }
    }
    public static (bool, IEnumerable<T>) ValidateInputAsEnumerable<T>(this InputModel inputModel)
    {
        var value = inputModel.Value;
        
        if (string.IsNullOrEmpty(value))
        {
            return (false, Enumerable.Empty<T>());
        }

        try
        {
            var values = value.Split(',')
              .Select(val => val.Trim())
              .Where(val => !string.IsNullOrEmpty(val))
              .Select(val =>
              {
                  if (typeof(T).IsEnum)
                  {
                      if (Enum.TryParse(typeof(T), val, true, out var enumValue))
                      {
                          return (T?)enumValue;
                      }
                      return default(T?); 
                  }

                  try
                  {
                      return (T?)Convert.ChangeType(val, typeof(T));
                  }
                  catch
                  {
                      return default(T?);
                  }
              })
              .Where(val => val != null)
              .Cast<T>();

            return values.Any() ? (true, values) : (false, Enumerable.Empty<T>());
        }
        catch
        {
            return (false, Enumerable.Empty<T>());
        }
    }
}