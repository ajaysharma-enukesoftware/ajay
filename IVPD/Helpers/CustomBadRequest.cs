using java.lang;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Helpers
{
    public class CustomBadRequest : ValidationProblemDetails
    {
        public CustomBadRequest(ActionContext context)
        {
            Title = "Argumentos inválidos para a API";
            Detail = "As entradas fornecidas para a API são inválidas";
            Status = 400;
            string[] b = { "Pedido não aceito" };
             Errors.Add("Message", b);

            //    ConstructErrorMessages("sasa");

            Type = context.HttpContext.TraceIdentifier;
        }
        private void ConstructErrorMessages(ActionContext context)
        {
            foreach (var keyModelStatePair in context.ModelState)
            {
                var key = keyModelStatePair.Key;
                key = key.Replace("$.", "");
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        var errorMessage = GetErrorMessage(errors[0]);
                        Errors.Add(key, new[] { errorMessage });
                    }
                    else
                    {
                        var errorMessages = new string[errors.Count];
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages[i] = GetErrorMessage(errors[i]);
                        }
                        Errors.Add(key, errorMessages);
                    }
                }
            }
        }
        string GetErrorMessage(ModelError error)
        {
            return string.IsNullOrEmpty(error.ErrorMessage) ?
            "" :
           Translate.TranslateText(error.ErrorMessage);
        }
        /*
          private void ConstructErrorMessages(ActionContext context)
          {
              foreach (var keyModelStatePair in context.ModelState)
              {
                  var key = keyModelStatePair.Key;
                  key = key.Replace("$.", "");
                  var errors = keyModelStatePair.Value.Errors;
                  if (errors != null && errors.Count > 0)
                  {
                      if (errors.Count == 1)
                      {
                          var errorMessage = GetErrorMessage(errors[0]);
                          Errors.Add(key, new[] { errorMessage });
                      }
                      else
                      {
                          var errorMessages = new string[errors.Count];
                          for (var i = 0; i < errors.Count; i++)
                          {
                              errorMessages[i] = GetErrorMessage(errors[i]);
                          }
                          Errors.Add(key, errorMessages);
                      }
                  }
              }
          }
          string GetErrorMessage(ModelError error)
          {
              return string.IsNullOrEmpty(error.ErrorMessage) ?
              "" :
             Translate.TranslateText(error.ErrorMessage);
          }*/
    }
}
