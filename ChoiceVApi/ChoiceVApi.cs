using System.Net;
using System.Text;
using ChoiceVApi.Account;
using ChoiceVApi.Character;
using ChoiceVApi.Inventory;
using ChoiceVSharedApiModels.Characters;

namespace ChoiceVApi;

public class ChoiceVApi
{
    private const string Prefix = "/api/v1/";
    private const string Url = "http://localhost:5269/";

    public static async Task Start()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add(Url);
        listener.Start();
        Console.WriteLine("Now listening on: " + Url);

        while (!false)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;

            var response = string.Empty;
            
            try
            {
                Console.WriteLine($"Received request for {request.Url}");

                var splittedUrl = request.RawUrl?.Replace(Prefix, "").Split("/");
                if (splittedUrl is null)
                {
                    await SendResponse(response, context, HttpStatusCode.BadRequest);
                    return;
                }

                var controller = splittedUrl.First();
                var action = string.Empty;
                var data = splittedUrl.Last();
                if (splittedUrl.Length >= 3)
                {
                    action = splittedUrl[1];
                }

                response = controller switch
                {
                    "account" => await AccountController.Handle(request.HttpMethod, action, data),
                    "character" => await CharacterController.Handle(request.HttpMethod, action, data),
                    "inventory" => await InventoryController.Handle(request.HttpMethod, action, data),
                    _ => throw new Exception("Unknown controller")
                };

                await SendResponse(response, context);
            }
            catch (Exception e)
            {
                response = e.Message;
                await SendResponse(response, context, HttpStatusCode.InternalServerError);
            }
        }
    }

    private static async Task SendResponse(string responseString, HttpListenerContext context, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        context.Response.StatusCode = (int)statusCode;
        
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = Encoding.UTF8;

        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
    }
}