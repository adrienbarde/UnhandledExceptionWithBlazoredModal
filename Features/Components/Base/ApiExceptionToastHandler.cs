using Blazored.Toast.Services;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ApiRechargement.Web.Common;

namespace ApiRechargement.Web.Features.Components.Base
{
    public static class ApiExceptionToastHandler
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";

        public static void HandleError(IToastService toastService, ApiException apiException)
        {
            if (!CanHandleErrorWithErrorDetails(toastService, apiException))
            {
                var (message, title) = HandleErrorsWithoutProblemDetails(apiException);
                toastService.ShowError(message, title);
            }
        }

        private static string ExtractCorrelationId(ApiException apiException)
        {
            if (!apiException.Headers.TryGetValues(CorrelationIdHeader, out var correlationIds))
            {
                return string.Empty;
            }

            string correlationId = correlationIds.FirstOrDefault();
            return !string.IsNullOrEmpty(correlationId)
                ? correlationId
                : string.Empty;
        }

        private static bool CanHandleErrorWithErrorDetails(IToastService toastService, ApiException apiException)
        {
            if (!TryParseJson<ErrorDetails>(apiException.Content, out var errorDetails) ||
                errorDetails == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(errorDetails.Instance))
            {
                errorDetails.Instance = ExtractCorrelationId(apiException);
            }

            string title;
            string message;
            switch (apiException.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    (message, title) = HandleBadRequest(errorDetails);
                    break;
                case HttpStatusCode.NotFound:
                    (message, title) = HandleNotFound(errorDetails);
                    break;
                case HttpStatusCode.InternalServerError:
                    (message, title) = HandleInternalServerError(errorDetails);
                    break;
                default:
                    message = $"Veuillez transmettre cet identifiant à votre support: {errorDetails.Instance}";
                    title = errorDetails.Title;
                    break;
            }

            toastService.ShowError(message, title);
            return true;

        }

        private static (string message, string title) HandleErrorsWithoutProblemDetails(ApiException apiException)
        {
            string correlationId = ExtractCorrelationId(apiException);
            var errorMessage = !string.IsNullOrEmpty(correlationId)
                ? $"Veuillez transmettre cet identifiant à votre support: {correlationId}"
                : "Un problème inconnu est survenu";

            var title = apiException.StatusCode switch
            {
                HttpStatusCode.BadRequest => "L'action demandée n'est pas valide",
                HttpStatusCode.NotFound => "La ressource demandée n'a pas été trouvée",
                HttpStatusCode.Unauthorized => "Vous n'êtes pas authentifié, veuillez vous reconnecter",
                HttpStatusCode.Forbidden => "Vous n'êtes pas autorisée à accéder à la ressource demandée",
                HttpStatusCode.InternalServerError => "Une erreur interne s'est produite",
                _ => $"Une erreur inconnue est survenue (StatusCode:{apiException.StatusCode})"
            };

            return (message: errorMessage, title);
        }

        private static (string message, string title) HandleInternalServerError(ErrorDetails errorDetails)
        {
            return (message: $"Veuillez transmettre cet identifiant à votre support: {errorDetails.Instance}", title: errorDetails.Title);
        }

        private static (string message, string title) HandleNotFound(ErrorDetails errorDetails)
        {
            if (string.IsNullOrEmpty(errorDetails.Detail))
            {
                return (message: $"Veuillez transmettre cet identifiant à votre support: {errorDetails.Instance}", title: "Une ressource n'a pas été trouvée");
            }

            var notFoundDetails = JsonConvert.DeserializeObject<NotFoundDetails>(errorDetails.Detail);
            return (
                message:
                $"{notFoundDetails.ResourceName}-{notFoundDetails.Key} demandée: {notFoundDetails.Value}  n'a pas été trouvée. Veuillez transmettre cet identifiant à votre support: {errorDetails.Instance}",
                title: "Une ressource n'a pas été trouvée");
        }

        private static (string message, string title) HandleBadRequest(ErrorDetails errorDetails)
        {
            if (string.IsNullOrEmpty(errorDetails.Detail))
            {
                return (message: $"Veuillez transmettre cet identifiant à votre support: {errorDetails.Instance}", title: "L'action demandée n'est pas valide");
            }

            var badRequestDetails = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(errorDetails.Detail);
            var messageBuilder = new StringBuilder();
            foreach (var badRequestDetail in badRequestDetails)
            {
                messageBuilder.AppendLine($"{badRequestDetail.Key}: ");
                foreach (var val in badRequestDetail.Value)
                {
                    messageBuilder.AppendLine($"{val}");
                }
                messageBuilder.AppendLine();
            }
            messageBuilder.AppendLine($"Veuillez transmettre cet identifiant à votre support: {errorDetails.Instance}");
            return (message: messageBuilder.ToString(), title: "L'action demandée n'est pas valide");
        }

        private static bool TryParseJson<T>(string jsonString, out T result) where T : class
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                result = null;
                return false;
            }

            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    success = false;
                    args.ErrorContext.Handled = true;
                }
            };
            result = JsonConvert.DeserializeObject<T>(jsonString, settings);
            Console.WriteLine($"json parsed {success} {JsonConvert.SerializeObject(result)}");
            return success;
        }
    }
}
