using Microsoft.VisualBasic;
using Refit;

namespace ChoiceVRefitClient;

public interface IBaseApiInterface
{
    public async Task<ApiResult<T>> HandleApiRequestAsync<T>(
        Func<CancellationToken, Task<ApiResponse<T>>> apiCall,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await apiCall(cancellationToken);

            if (!response.IsSuccessStatusCode) 
                return ApiResult<T>.FromError(new Exception(response.Error?.Message));
            
            return ApiResult<T>.FromSuccess(response.Content!);
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<T>.FromError(ex);
        }
        catch (TaskCanceledException)
        {
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                return ApiResult<T>.FromSuccess((T)Activator.CreateInstance(typeof(T))!, true);
            }
            
            return ApiResult<T>.FromSuccess(default!, true);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.FromError(ex);
        }
    }
}

public class ApiResult<T>
{
    public T? Data { get; }
    public Exception? Error { get; }
    public bool IsCanceled { get; }

    private ApiResult(T data, bool isCanceled = false)
    {
        Data = data;
        IsCanceled = isCanceled;
    }

    private ApiResult(Exception error) => Error = error;

    public static ApiResult<T> FromSuccess(T result, bool isCanceled = false) => new(result, isCanceled);
    public static ApiResult<T> FromError(Exception error) => new(error);

    public bool IsSuccess => IsCanceled || (Data != null && Error == null);
}