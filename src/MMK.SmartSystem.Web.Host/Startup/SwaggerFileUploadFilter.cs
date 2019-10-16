
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

            FileUploadByFileType(operation, context);
            SwaggerFileUploadByAttribute(operation, context);
        }
        void FileUploadByFileType(Operation operation, OperationFilterContext context)
        {
            var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();

            if (fileParameters.Count() <= 0)
            {
                return;
            }
            operation.Parameters.Clear();
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
            operation.Consumes.Add("multipart/form-data");
        }

        void SwaggerFileUploadByAttribute(Operation operation, OperationFilterContext context)
        {

            MethodInfo method = null;
            context.ApiDescription.TryGetMethodInfo(out method);

            var listParm = method.GetParameters().Where(d => d.GetCustomAttribute<SwaggerFileUploadAttribute>() != null).FirstOrDefault();

            if (listParm == null)
            {
                return;
            }

            var fileParameters = listParm.ParameterType.GetProperties().Where(d => d.PropertyType == typeof(IFormFile)).ToList();

            operation.Parameters.Clear();
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
            operation.Consumes.Add("multipart/form-data");
        }
    }
}
