﻿using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;
using ComputerScience.Server.Web.Models.Response;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace ComputerScience.Server.Web.Formatters
{
    public class StandardOutputFormatter : JsonOutputFormatter
    {
        public StandardOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool) 
            : base(serializerSettings, charPool)
        {
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (selectedEncoding == null)
                throw new ArgumentException(nameof(selectedEncoding));

            var o = context.Object as StandardResponse;
            var standard = o ?? StandardResponse.Create(context.Object);

            var response = context.HttpContext.Response;
            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                WriteObject(writer, standard);
                await writer.FlushAsync();
            }
        }
    }
}
