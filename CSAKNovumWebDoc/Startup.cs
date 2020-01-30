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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CSAKNovumWebDoc
{
    public class Startup
    {
        public WebClient client = new WebClient();

        public void ConfigureServices(IServiceCollection services)
        {
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
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        TransformXMLToHTML(
                                client.DownloadString("https://localhost:44349/xml")
                                , client.DownloadString("https://localhost:44349/xsl")
                            )
                        );
                });

                endpoints.MapGet("/xsl", async context =>
                {
                    await context.Response.WriteAsync("<?xml version='1.0' encoding='UTF-8'?> <xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'> <xsl:template match='/'> <html> <body> <xsl:apply-templates select='TaroltEljaras'/> </body> </html> </xsl:template> <xsl:template match='TaroltEljaras'> <table border='1'> <tr> <td>PublikusK�d</td> <td> <xsl:value-of select='Kod'/> </td> </tr> </table> <xsl:value-of select='Kod'/> <xsl:apply-templates select='ArgumentumLista'/> <xsl:value-of select='Kod'/> <xsl:apply-templates select='MuveletLista'/> </xsl:template> <xsl:template match='MuveletLista'> <xsl:apply-templates select='Muvelet'/> </xsl:template> <xsl:template match='Muvelet'> <xsl:value-of select='Kod'/> <xsl:apply-templates select='Valasz'/> </xsl:template> <xsl:template match='Valasz'> <xsl:apply-templates select='AdatElemLista'/> </xsl:template> <xsl:template match='AdatElemLista'> <xsl:apply-templates select='AdatElem'/> </xsl:template> <xsl:template match='AdatElem'> <xsl:value-of select='Kod'/> </xsl:template> <xsl:template match='ArgumentumLista'> <table border='1'> <tr> <td>PublikusK�d</td> </tr> <xsl:apply-templates select='TaroltEljarasArgumentum'/> </table> </xsl:template> <xsl:template match='TaroltEljarasArgumentum'> <tr> <td> <xsl:value-of select='Kod'/> </td> </tr> <xsl:value-of select='Kod'/> </xsl:template> </xsl:stylesheet> ");
                });

                endpoints.MapGet("/xml", async context =>
                    {
                        await context.Response.WriteAsync("<?xml version='1.0' encoding='utf-16'?> <TaroltEljaras xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'> <Kod>Tor_Szemely_Kereses</Kod> <Id>0</Id> <MuveletLista> <Muvelet> <Kod>ADO</Kod> <Leiras>Szem�ly lek�rdez�se ad�sz�m alapj�n</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.SzemelyLekerdezeseAdoszamAlapjanValasz</Kod> <PublikusKod>SzemelyLekerdezeseAdoszamAlapjanValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyLekerdezeseAdoszamAlapjanValasz.SzemAZ</Kod> <PublikusKod>SzemAZ</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>BELEP</Kod> <Leiras>Bel�p�s lek�rdez�se</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.BelepesLekerdezeseValasz</Kod> <PublikusKod>BelepesLekerdezeseValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.BelepesLekerdezeseValasz.TagBEKod</Kod> <PublikusKod>TagBEKod</PublikusKod> <Id>0</Id> <Adattipus>Byte</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.BelepesLekerdezeseValasz.TagBEText</Kod> <PublikusKod>TagBEText</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>KILEP</Kod> <Leiras>Kil�p�s lek�rdez�se</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.KilepesLekerdezeseValasz</Kod> <PublikusKod>KilepesLekerdezeseValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.KilepesLekerdezeseValasz.TagKilepKod</Kod> <PublikusKod>TagKilepKod</PublikusKod> <Id>0</Id> <Adattipus>Int16</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.KilepesLekerdezeseValasz.TagKilepText</Kod> <PublikusKod>TagKilepText</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>ADOSZEMAZ</Kod> <Leiras>Ad�sz�m felhaszn�l�s ellenorz�se</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.AdoszamFelhasznalasEllenorzeseValasz</Kod> <PublikusKod>AdoszamFelhasznalasEllenorzeseValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.AdoszamFelhasznalasEllenorzeseValasz.Column1</Kod> <PublikusKod>Column1</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>AZONOSITO</Kod> <Leiras>Szem�ly azonos�t� lek�rdez�se</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.SzemelyAzonositoLekerdezeseValasz</Kod> <PublikusKod>SzemelyAzonositoLekerdezeseValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyAzonositoLekerdezeseValasz.SzemAZ</Kod> <PublikusKod>SzemAZ</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>KERESES</Kod> <Leiras>Szem�ly keres�se</Leiras> <Id>0</Id> <ValaszTipus>1</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz</Kod> <PublikusKod>SzemelyKereseseValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Azonos�t�</Kod> <PublikusKod>Azonos�t�</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Tags�gk�d</Kod> <PublikusKod>Tags�gk�d</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.N�v</Kod> <PublikusKod>N�v</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Ad�azonos�t�</Kod> <PublikusKod>Ad�azonos�t�</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Partnerk�d</Kod> <PublikusKod>Partnerk�d</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.St�tusz</Kod> <PublikusKod>St�tusz</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Irsz</Kod> <PublikusKod>Irsz</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.V�ros</Kod> <PublikusKod>V�ros</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Utca</Kod> <PublikusKod>Utca</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Bel�pd�t</Kod> <PublikusKod>Bel�pd�t</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Kil�pd�t</Kod> <PublikusKod>Kil�pd�t</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Banksz�mlasz�m</Kod> <PublikusKod>Banksz�mlasz�m</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseValasz.Tagcsoport</Kod> <PublikusKod>Tagcsoport</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista> <TaroltEljarasArgumentum> <Kod>@tagaz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@tagaz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@statusz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@statusz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> </ArgumentumLista> </Muvelet> <Muvelet> <Kod>KERESESPU</Kod> <Leiras>Szem�ly keres�s p�nz�gyi adatok alapj�n</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz</Kod> <PublikusKod>SzemelyKeresesPenzugyiAdatokAlapjanValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Azonos�t�</Kod> <PublikusKod>Azonos�t�</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Tags�gk�d</Kod> <PublikusKod>Tags�gk�d</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.N�v</Kod> <PublikusKod>N�v</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Ad�azonos�t�</Kod> <PublikusKod>Ad�azonos�t�</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Partnerk�d</Kod> <PublikusKod>Partnerk�d</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.St�tusz</Kod> <PublikusKod>St�tusz</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Irsz</Kod> <PublikusKod>Irsz</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.V�ros</Kod> <PublikusKod>V�ros</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Utca</Kod> <PublikusKod>Utca</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Bel�pd�t</Kod> <PublikusKod>Bel�pd�t</PublikusKod> <Id>0</Id> <Adattipus>DateTime</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Kil�pd�t</Kod> <PublikusKod>Kil�pd�t</PublikusKod> <Id>0</Id> <Adattipus>DateTime</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Banksz�mlasz�m</Kod> <PublikusKod>Banksz�mlasz�m</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesPenzugyiAdatokAlapjanValasz.Tagcsoport</Kod> <PublikusKod>Tagcsoport</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>SZULETES</Kod> <Leiras>Szem�ly keres�s sz�let�si adatok alapj�n</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.SzemelyKeresesSzuletesiAdatokAlapjanValasz</Kod> <PublikusKod>SzemelyKeresesSzuletesiAdatokAlapjanValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKeresesSzuletesiAdatokAlapjanValasz.Column1</Kod> <PublikusKod>Column1</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>TAGSAG</Kod> <Leiras>Tags�gok darabsz�m�nak lek�rdez�se</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.TagsagokDarabszamanakLekerdezeseValasz</Kod> <PublikusKod>TagsagokDarabszamanakLekerdezeseValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.TagsagokDarabszamanakLekerdezeseValasz.SzemTAJ</Kod> <PublikusKod>SzemTAJ</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> <AdatElem> <Kod>Tor_Szemely_Kereses.TagsagokDarabszamanakLekerdezeseValasz.SzemAdoszam</Kod> <PublikusKod>SzemAdoszam</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>TAJ</Kod> <Leiras>Szem�ly keres�se tajsz�m alapj�n</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.SzemelyKereseseTajszamAlapjanValasz</Kod> <PublikusKod>SzemelyKereseseTajszamAlapjanValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseTajszamAlapjanValasz.SzemAZ</Kod> <PublikusKod>SzemAZ</PublikusKod> <Id>0</Id> <Adattipus>Int32</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> <Muvelet> <Kod>TAJSZEMAZ</Kod> <Leiras>Szem�ly keres�se tajsz�m �s azonos�t� alapj�n</Leiras> <Id>0</Id> <ValaszTipus>0</ValaszTipus> <Valasz> <Kod>Tor_Szemely_Kereses.SzemelyKereseseTajszamEsAzonositoAlapjanValasz</Kod> <PublikusKod>SzemelyKereseseTajszamEsAzonositoAlapjanValasz</PublikusKod> <Id>0</Id> <AdatElemLista> <AdatElem> <Kod>Tor_Szemely_Kereses.SzemelyKereseseTajszamEsAzonositoAlapjanValasz.Column1</Kod> <PublikusKod>Column1</PublikusKod> <Id>0</Id> <Adattipus>String</Adattipus> <Opcionalis>false</Opcionalis> </AdatElem> </AdatElemLista> </Valasz> <ArgumentumLista /> </Muvelet> </MuveletLista> <MetodusLista> <Metodus> <Nev>Szem�ly lek�rdez�se ad�sz�m alapj�n</Nev> <Kod>Tor_Szemely_Kereses.Szem�ly lek�rdez�se ad�sz�m alapj�n</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Bel�p�s lek�rdez�se</Nev> <Kod>Tor_Szemely_Kereses.Bel�p�s lek�rdez�se</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Kil�p�s lek�rdez�se</Nev> <Kod>Tor_Szemely_Kereses.Kil�p�s lek�rdez�se</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Ad�sz�m felhaszn�l�s ellenorz�se</Nev> <Kod>Tor_Szemely_Kereses.Ad�sz�m felhaszn�l�s ellenorz�se</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Szem�ly azonos�t� lek�rdez�se</Nev> <Kod>Tor_Szemely_Kereses.Szem�ly azonos�t� lek�rdez�se</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Szem�ly keres�se</Nev> <Kod>Tor_Szemely_Kereses.Szem�ly keres�se</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista> <TaroltEljarasArgumentum> <Kod>@tagaz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@tagaz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@statusz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@statusz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> </ArgumentumLista> </Metodus> <Metodus> <Nev>Szem�ly keres�s p�nz�gyi adatok alapj�n</Nev> <Kod>Tor_Szemely_Kereses.Szem�ly keres�s p�nz�gyi adatok alapj�n</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Szem�ly keres�s sz�let�si adatok alapj�n</Nev> <Kod>Tor_Szemely_Kereses.Szem�ly keres�s sz�let�si adatok alapj�n</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Tags�gok darabsz�m�nak lek�rdez�se</Nev> <Kod>Tor_Szemely_Kereses.Tags�gok darabsz�m�nak lek�rdez�se</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Szem�ly keres�se tajsz�m alapj�n</Nev> <Kod>Tor_Szemely_Kereses.Szem�ly keres�se tajsz�m alapj�n</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> <Metodus> <Nev>Szem�ly keres�se tajsz�m �s azonos�t� alapj�n</Nev> <Kod>Tor_Szemely_Kereses.Szem�ly keres�se tajsz�m �s azonos�t� alapj�n</Kod> <Id>0</Id> <Uzemmod>0</Uzemmod> <Skalar>false</Skalar> <ArgumentumLista /> </Metodus> </MetodusLista> <ArgumentumLista> <TaroltEljarasArgumentum> <Kod>@RETURN_VALUE</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@RETURN_VALUE</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@muvelet</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@muvelet</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szemaz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@szemaz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@tagaz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@tagaz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@taj</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@taj</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@tagsag</Kod> <Id>0</Id> <Adattipus>Bit</Adattipus> <BelsoNev>@tagsag</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@nev</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@nev</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@adoazonosito</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@adoazonosito</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@paraz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@paraz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@statusz</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@statusz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@irsz</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@irsz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@varos</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@varos</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@utca</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@utca</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@belepevtol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@belepevtol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@belephotol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@belephotol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@belepnaptol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@belepnaptol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@belepevig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@belepevig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@belephoig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@belephoig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@belepnapig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@belepnapig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@kilepevtol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@kilepevtol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@kilephotol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@kilephotol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@kilepnaptol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@kilepnaptol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@kilepevig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@kilepevig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@kilephoig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@kilephoig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@kilepnapig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@kilepnapig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@bankszamla</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@bankszamla</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@csoport</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@csoport</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@penztar</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@penztar</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@anyjaneve</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@anyjaneve</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szuletesihely</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@szuletesihely</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szuletesiido</Kod> <Id>0</Id> <Adattipus>SmallDateTime</Adattipus> <BelsoNev>@szuletesiido</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szulevtol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@szulevtol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szulhotol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@szulhotol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szulnaptol</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@szulnaptol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szulevig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@szulevig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szulhoig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@szulhoig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szulnapig</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@szulnapig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@tagszervkulsoaz</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@tagszervkulsoaz</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@szuletesinev</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@szuletesinev</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@ugynok</Kod> <Id>0</Id> <Adattipus>Int</Adattipus> <BelsoNev>@ugynok</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@elszevtol</Kod> <Id>0</Id> <Adattipus>SmallInt</Adattipus> <BelsoNev>@elszevtol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@elszhotol</Kod> <Id>0</Id> <Adattipus>SmallInt</Adattipus> <BelsoNev>@elszhotol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@elsznaptol</Kod> <Id>0</Id> <Adattipus>SmallInt</Adattipus> <BelsoNev>@elsznaptol</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@elszevig</Kod> <Id>0</Id> <Adattipus>SmallInt</Adattipus> <BelsoNev>@elszevig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@elszhoig</Kod> <Id>0</Id> <Adattipus>SmallInt</Adattipus> <BelsoNev>@elszhoig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@elsznapig</Kod> <Id>0</Id> <Adattipus>SmallInt</Adattipus> <BelsoNev>@elsznapig</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@megjegyzes</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@megjegyzes</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> <TaroltEljarasArgumentum> <Kod>@regiadat</Kod> <Id>0</Id> <Adattipus>VarChar</Adattipus> <BelsoNev>@regiadat</BelsoNev> <Opcionalis>false</Opcionalis> </TaroltEljarasArgumentum> </ArgumentumLista> </TaroltEljaras>");
                    });
            });
        }
    }
}
