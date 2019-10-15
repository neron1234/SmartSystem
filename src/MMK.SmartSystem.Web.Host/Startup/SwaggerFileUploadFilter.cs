
using Microsoft.AspNetCore.Http;
using MMK.CNC.Application;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Web.Host.Startup
{
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                !context.ApiDescription.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            MethodInfo method = null;
            var isAuthorized = context.ApiDescription.ActionAttributes().Any(a => a.GetType() == typeof(SwaggerFileUploadAttribute));
            if (!isAuthorized)
            {
                return;
            }
            //var res = context.ApiDescription.TryGetMethodInfo(out method);
            //if (!res)
            //{
            //    return;
            //}
            //var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();
            //if (fileParameters.Count <= 0)
            //{
            //    return;
            //}
            //foreach (var fileParameter in fileParameters)
            //{

            //    operation.Parameters.Add(new NonBodyParameter
            //    {
            //        Name = fileParameter.Name,
            //        In = "formData",
            //        Description = "File to upload",
            //        Required = true,
            //        Type = "file"
            //    });
            //}
            //var fileUpload = method.GetCustomAttributes<SwaggerFileUploadAttribute>().FirstOrDefault();
            //if (fileUpload != null)
            //{
            var fileParameters = method.GetParameters()[0].ParameterType.GetProperties().Where(d => d.PropertyType == typeof(IFormFile)).ToList();
            // var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();

            operation.Consumes.Add("multipart/form-data");


            foreach (var fileParameter in fileParameters)
            {

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = fileParameter.Name,
                    In = "formData",
                    Description = "File to upload",
                    Required = true,
                    Type = "file"
                });
            }
            //}



        }
    }
}
