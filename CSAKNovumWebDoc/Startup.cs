using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CSAKNovumWebDoc
{
    public class Startup
    {
        public WebClient client = new WebClient();
        public string xmlKonyvtar = "/AKDokumentumok/xml/";
        public string xslKonyvtar = "/AKDokumentumok/xsl/";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

        }
        public string TransformXMLToHTML(string inputXml, string xsltString)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            using (XmlReader reader = XmlReader.Create(new StringReader(xsltString)))
            {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(inputXml)))
            {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        "A Dokumentum eléréséhez menj a /dokumentacio url-re és paraméterként add meg a kívánt xml/xsl fájl-páros neveit."
                        );
                });

                endpoints.MapGet("/dokumentacio", async context =>
                    {
                        var xml = context.Request.Query["xml"];
                        var xsl = context.Request.Query["xsl"];
                        await context.Response.WriteAsync(
                            TransformXMLToHTML(
                                File.ReadAllText(xmlKonyvtar + xml + ".xml")
                                , File.ReadAllText(xslKonyvtar + xsl + ".xsl")));
                    }
                );
            });

            app.UseHttpsRedirection();
        }
    }
}
