using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using ZDY.DMS.AspNetCore.Mvc;

namespace ZDY.DMS.API.Swagger
{
    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null || context == null)
                return;
            operation.Parameters = operation.Parameters ?? new List<OpenApiParameter>();

            var isAllowAnonymous = context.ApiDescription.CustomAttributes().Any(e => e.GetType() == typeof(AllowAnonymousAttribute));
            if (!isAllowAnonymous)
            {
                operation.Parameters.Insert(0, new OpenApiParameter()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    AllowEmptyValue = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    Required = true
                });
            }

            var version = operation.Parameters.FirstOrDefault(e => e.Name == "Version") as OpenApiParameter;
            if (version != null)
            {
                operation.Parameters.Remove(version);
                version.Reference = new OpenApiReference
                {
                    ExternalResource = context.ApiDescription.GroupName ?? typeof(ApiVersions).GetEnumNames().LastOrDefault()
                };
                version.Description = "Interface Version";
                operation.Parameters.Insert(0, version);
            }

            var fileAttribute = context.ApiDescription.CustomAttributes().OfType<SwaggerFileUploadAttribute>().FirstOrDefault();
            if (fileAttribute != null)
            {
                //operation.Parameters.Add("multipart/form-data");
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Upload File",
                    In = ParameterLocation.Header,
                    Description = "上传图片",
                    Required = (fileAttribute as SwaggerFileUploadAttribute).Required,
                    Schema = new OpenApiSchema
                    {
                        Type = "file"
                    }
                });
            }
        }
    }
}
