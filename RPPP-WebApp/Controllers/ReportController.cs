using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;
using Org.BouncyCastle.Utilities;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;
using RPPP_WebApp.Extensions;
using RPPP_WebApp.Models;
using System.Linq;
using RPPP_WebApp.UtilClasses;
using Microsoft.Data.SqlClient;
using RPPP_WebApp.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;
using RPPP_WebApp.ModelsPartials;
using OfficeOpenXml.Style;

namespace RPPP_WebApp.Controllers;
/// <summary>
/// Web API servis za ispis reportova u Excelu i PDF-u 
/// </summary>
public class ReportController : Controller
{
    private readonly RPPP05Context ctx;
    private readonly IWebHostEnvironment environment;
    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ReportController(RPPP05Context ctx, IWebHostEnvironment environment)
        {
            this.ctx = ctx;
            this.environment = environment;
        }

    #region CreateReport
        private PdfReport CreateReport(string naslov)
        {
            var pdf = new PdfReport();

            pdf.DocumentPreferences(doc =>
                {
                    doc.Orientation(PageOrientation.Portrait);
                    doc.PageSize(PdfPageSize.A4);
                    doc.DocumentMetadata(new DocumentMetadata
                    {
                        Author = "Anton Macan",
                        Application = "RPP-WebApp.MVC Core",
                        Title = naslov
                    });
                    doc.Compression(new CompressionSettings
                    {
                        EnableCompression = true,
                        EnableFullCompression = true
                    });
                })
                //fix za linux https://github.com/VahidN/PdfReport.Core/issues/40
                .DefaultFonts(fonts =>
                {
                  fonts.Path(Path.Combine("wwwroot", "fonts", "verdana.ttf"),
                    Path.Combine("wwwroot", "fonts", "tahoma.ttf"));
                    fonts.Size(9);
                    fonts.Color(System.Drawing.Color.Black);
                })
                //
                .MainTableTemplate(template => { template.BasicTemplate(BasicTemplate.ProfessionalTemplate); })
                .MainTablePreferences(table =>
                {
                    table.ColumnsWidthsType(TableColumnWidthType.Relative);
                    //table.NumberOfDataRowsPerPage(20);
                    table.GroupsPreferences(new GroupsPreferences
                    {
                        GroupType = GroupType.HideGroupingColumns,
                        RepeatHeaderRowPerGroup = true,
                        ShowOneGroupPerPage = true,
                        SpacingBeforeAllGroupsSummary = 5f,
                        NewGroupAvailableSpacingThreshold = 150,
                        SpacingAfterAllGroupsSummary = 5f
                    });
                    table.SpacingAfter(4f);
                });

            return pdf;
        }
        #endregion
  /// <summary>
  /// Vraća PDF file svih kamera i slika
  /// </summary>
  /// <returns></returns>
  public async Task<IActionResult> Kamera()
    {
      string naslov = $"Kamere";
      var slike = await ctx.SlikaDenorms().OrderBy(k => k.KameraId).ThenBy(k => k.Datum).ToListAsync();
      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); // It's a default setting to improve the performance.
              header.CustomHeader(new MasterDetailsHeaders(naslov)
        {
          PdfRptFont = header.PdfFont
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(slike));

      report.MainTableColumns(columns =>
      {
        #region Stupci po kojima se grupira
        columns.AddColumn(column =>
        {
          column.PropertyName<SlikaDenorm>(s => s.KameraId);
          column.Group(
              (val1, val2) =>
              {
                return (int)val1 == (int)val2;
              });
        });
        #endregion
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Right);
        });
        columns.AddColumn(column =>
        {
          column.PropertyName<SlikaDenorm>(s => s.Url);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Width(4);
          column.HeaderCell("URL", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<SlikaDenorm>(s => s.Datum);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);
          column.Width(1);
          column.HeaderCell("Datum", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<SlikaDenorm>(s => s.Smjer);
          column.CellsHorizontalAlignment(HorizontalAlignment.Right);
          column.IsVisible(true);
          column.Width(1);
          column.HeaderCell("Smjer", horizontalAlignment: HorizontalAlignment.Center);
        });
        });
      #endregion
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=dokumenti.pdf");
        return File(pdf, "application/pdf");
      }
      else
        return NotFound();
    }
  /// <summary>
  /// Vraća EXCEL file svih kamera i slika
  /// </summary>
  /// <returns></returns>
  public async Task<IActionResult> KameraExcel()
  {
    var kamere = await ctx.Kamere
      .Select(k => new KameraViewModel
      {
        Id = k.Id,
        DionicaNaziv = Converters.GetDionicaName(k.Dionica.UlaznaPostajaNavigation.Ime,
          k.Dionica.IzlaznaPostajaNavigation.Ime, k.Dionica.OznakaAutoceste),
        Naziv = k.Naziv,
        GeografskaDuzina = k.GeografskaDuzina,
        GeografskaSirina = k.GeografskaSirina
      }).ToListAsync();
    
    byte[] content;

    using (ExcelPackage excel = new ExcelPackage())
    {
      excel.Workbook.Properties.Title = "Popis kamera";
      excel.Workbook.Properties.Author = "Anton Macan";
      var worksheet = excel.Workbook.Worksheets.Add("Kamere");

      //First add the headers
      worksheet.Cells[1, 1].Value = "Id kamere";
      worksheet.Cells[1, 2].Value = "Naziv dionice";
      worksheet.Cells[1, 3].Value = "Naziv kamere";
      worksheet.Cells[1, 4].Value = "Geografska sirina";
      worksheet.Cells[1, 5].Value = "Geografska duzina";

      for (int i = 0; i < kamere.Count; i++)
      {
        worksheet.Cells[i + 2, 1].Value = kamere[i].Id;
        worksheet.Cells[i + 2, 2].Value = kamere[i].DionicaNaziv;
        worksheet.Cells[i + 2, 3].Value = kamere[i].Naziv;
        worksheet.Cells[i + 2, 4].Value = kamere[i].GeografskaSirina;
        worksheet.Cells[i + 2, 5].Value = kamere[i].GeografskaDuzina;
      }

      worksheet.Cells[1, 1, kamere.Count + 1, 4].AutoFitColumns();

      content = excel.GetAsByteArray();
    }
    return File(content, ExcelContentType, "kamere.xlsx");
  } 
  /// <summary>
  /// Vraća EXCEL file svih slika
  /// </summary>
  /// <returns></returns>
  public async Task<IActionResult> SlikeExcel()
  {
    var slike = await ctx.SlikaDenorms()
      .ToListAsync();
    byte[] content;
    using (ExcelPackage excel = slike.CreateExcel("Slike"))
    {
      content = excel.GetAsByteArray();
    }
    return File(content, ExcelContentType, "Slike.xlsx");
  }
        
    #region Master-detail header
    public class MasterDetailsHeaders : IPageHeader
    {
      private string naslov;
      public MasterDetailsHeaders(string naslov)
      {
        this.naslov = naslov;
      }
      public IPdfFont PdfRptFont { set; get; }

      public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
      {
        var KameraId = newGroupInfo.GetSafeStringValueOf(nameof(SlikaDenorm.KameraId));
        var Naziv = newGroupInfo.GetSafeStringValueOf(nameof(SlikaDenorm.Naziv));
        var GeografskaSirina =(double)newGroupInfo.GetValueOf(nameof(SlikaDenorm.GeografskaSirina));
        var GeografskaDuzina = (double)newGroupInfo.GetValueOf(nameof(SlikaDenorm.GeografskaDuzina));

        var table = new PdfGrid(relativeWidths: new[] { 2f, 5f, 2f, 3f }) { WidthPercentage = 100 };

        table.AddSimpleRow(
            (cellData, cellProperties) =>
            {
              cellData.Value = "Id kamere:";
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            },
            (cellData, cellProperties) =>
            {
              cellData.Value = KameraId;
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            },
            (cellData, cellProperties) =>
            {
              cellData.Value = "Naziv:";
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            },
            (cellData, cellProperties) =>
            {
              cellData.Value = Naziv;
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            });

        table.AddSimpleRow(
            (cellData, cellProperties) =>
            {
              cellData.Value = "Geografska sirina:";
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            },
            (cellData, cellProperties) =>
            {
              cellData.Value = UtilClasses.Converters.koordinateToWGS(GeografskaSirina);
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            },
            (cellData, cellProperties) =>
            {
              cellData.Value = "Geografska duzina:";
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            },
            (cellData, cellProperties) =>
            {
              cellData.Value = Converters.koordinateToWGS(GeografskaDuzina);
              cellProperties.PdfFont = PdfRptFont;
              cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
            });
        return table.AddBorderToTable(borderColor: BaseColor.LightGray, spacingBefore: 5f);
      }

      public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
      {
        var table = new PdfGrid(numColumns: 1) { WidthPercentage = 100 };
        table.AddSimpleRow(
           (cellData, cellProperties) =>
           {
             cellData.Value = naslov;
             cellProperties.PdfFont = PdfRptFont;
             cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
             cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
           });
        return table.AddBorderToTable();
      }
    }
    #endregion
    /// <summary>
    /// Vraća stranicu na kojoj su linkovi za reportove
    /// </summary>
    /// <returns></returns>
        public IActionResult IndexMacan()
        {
            return View();
        }

    /// <summary>
    /// Vraća stranicu na kojoj su linkovi za izvješća člana Fran Hruza
    /// </summary>
    /// <returns></returns>
    public IActionResult IndexHruza()
    {
        return View();
    }

    /// <summary>
    /// Vraca stranicu s linkovima izvjesca clana Josko Vrsalovic
    /// </summary>
    /// <returns></returns>
    public IActionResult IndexVrsalovic()
    {
        return View();
    }

    /// <summary>
    /// Vraca pdf s autocestama i dionicama
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Autoceste()
    {
        int n = 20;

        var autoceste = await ctx.Autoceste
                            .Include(a => a.Dionice)
                            .ThenInclude(d => d.UlaznaPostajaNavigation)
                            .Include(a => a.Dionice)
                            .ThenInclude(d => d.IzlaznaPostajaNavigation)
                            .OrderBy(a => a.Oznaka)
                            .ThenBy(a => a.DuljinaKm)
                            .Take(n)
                            .ToListAsync();
        

        List<AutocestaDionicaRow> ad = new List<AutocestaDionicaRow>();
        n = 0;
        foreach(var a in autoceste)
        {
            if (a.Dionice.Count > 0) { n++; }
            foreach (var d in a.Dionice)
            {
                ad.Add(new AutocestaDionicaRow
                {
                    Oznaka = a.Oznaka,
                    Koncesionar = a.Koncesionar,
                    Pocetak = a.Pocetak,
                    Kraj = a.Kraj,
                    DuljinaKm = a.DuljinaKm,
                    DionicaId = d.Id,
                    DionicaDuljinaKm = d.DuljinaKm,
                    DionicaUlaznaPostaja = d.UlaznaPostajaNavigation.Ime,
                    DionincaIzlaznaPostaja = d.IzlaznaPostajaNavigation.Ime,
                    DionicaNaziv = Converters.GetDionicaName(d)
                });
            }
        }
        string naslov = $"{n} autocesta i njihovih dionica";

        PdfReport report = CreateReport(naslov);
        #region Podnožje i zaglavlje

        report.PagesFooter(footer =>
        {
            footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
        })
        .PagesHeader(header =>
        {
            header.CacheHeader(cache: true); // It's a default setting to improve the performance.
            header.CustomHeader(new MasterDetailsHeadersAutoceste(naslov)
            {
                PdfRptFont = header.PdfFont
            });
        });
        #endregion
        #region Postavljanje izvora podataka i stupaca
        report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(ad));

        report.MainTableColumns(columns =>
        {
            #region Stupci po kojima se grupira
            columns.AddColumn(column =>
            {
                column.PropertyName<AutocestaDionicaRow>(a => a.Oznaka);
                column.Group(
                    (val1, val2) =>
                    {
                        return (string)val1 == (string)val2;
                    });
            });
            #endregion
            columns.AddColumn(column =>
            {
                column.IsRowNumber(true);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(2);
                column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Right);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<AutocestaDionicaRow>(d => d.DionicaNaziv);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(6);
                column.HeaderCell("Naziv dionice", horizontalAlignment: HorizontalAlignment.Right);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<AutocestaDionicaRow>(d => d.DionicaDuljinaKm);
                column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                column.IsVisible(true);
                column.Width(3);
                column.HeaderCell("Duljina (km)", horizontalAlignment: HorizontalAlignment.Center);
            });

            columns.AddColumn(column =>
            {

                column.PropertyName<AutocestaDionicaRow>(d => d.DionicaUlaznaPostaja);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Ulazna postaja", horizontalAlignment: HorizontalAlignment.Center);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<AutocestaDionicaRow>(d => d.DionincaIzlaznaPostaja);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Izlazna postaja", horizontalAlignment: HorizontalAlignment.Center);
            });
        });
        #endregion
        byte[] pdf = report.GenerateAsByteArray();

        if (pdf != null)
        {
            Response.Headers.Add("content-disposition", "inline; filename=autoceste.pdf");
            return File(pdf, "application/pdf");
        }
        else
        {
            return NotFound();

        }
    }

    /// <summary>
    /// Stvara PDF izvješće 20 naplatnih postaja i njihovih cijenika
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Naplatne()
    {
        int n = 20;   
        string naslov = $"{n} naplatnih postaja i njihovi izlazi";

        var naplatne =  await ctx.StavkaCjenikaDenorms(n)        
                            .OrderBy(np => np.NaplatnaID)
                            .ThenBy(np => np.ImeIzlaza)
                            .ToListAsync();




        PdfReport report = CreateReport(naslov);
        #region Podnožje i zaglavlje

        report.PagesFooter(footer =>
        {
            footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
        })
        .PagesHeader(header =>
        {
            header.CacheHeader(cache: true); // It's a default setting to improve the performance.
            header.CustomHeader(new MasterDetailsHeadersNaplatne(naslov)
            {
                PdfRptFont = header.PdfFont
            });
        });
        #endregion
        #region Postavljanje izvora podataka i stupaca
        report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(naplatne));

        report.MainTableColumns(columns =>
        {
            #region Stupci po kojima se grupira
            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.NaplatnaID);
                column.Group(
                    (val1, val2) =>
                    {
                        return (int)val1 == (int)val2;
                    });
            });
            #endregion
            columns.AddColumn(column =>
            {
                column.IsRowNumber(true);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(2);
                column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Right);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.IzlazId);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(2);
                column.HeaderCell("ID", horizontalAlignment: HorizontalAlignment.Right);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.ImeIzlaza);
                column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Ime", horizontalAlignment: HorizontalAlignment.Center);
            });

            columns.AddColumn(column =>
            {
                
                column.PropertyName<StavkaCjenikaDenorm>(n =>n.GeoDuzinaIzlaza);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Geografska dužina", horizontalAlignment: HorizontalAlignment.Center);
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => Converters.koordinateToWGS((double)obj));
                });
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.GeoSirinaIzlaza);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Geografska širina", horizontalAlignment: HorizontalAlignment.Center);
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => Converters.koordinateToWGS((double)obj));
                });
            });
         

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.CijenaIa);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Cijena IA", horizontalAlignment: HorizontalAlignment.Right);
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => Converters.convertAndFormatPrice((int)obj));
                });
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.CijenaI);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Cijena I", horizontalAlignment: HorizontalAlignment.Right);
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => Converters.convertAndFormatPrice((int)obj));
                });
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.CijenaIi);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Cijena II", horizontalAlignment: HorizontalAlignment.Right);
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => Converters.convertAndFormatPrice((int)obj));
                });
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.CijenaIii);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Cijena III", horizontalAlignment: HorizontalAlignment.Right);
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => Converters.convertAndFormatPrice((int)obj));
                });
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<StavkaCjenikaDenorm>(n => n.CijenaIv);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(5);
                column.HeaderCell("Cijena IV", horizontalAlignment: HorizontalAlignment.Right);
                column.ColumnItemsTemplate(template =>
                {
                    template.TextBlock();
                    template.DisplayFormatFormula(obj => Converters.convertAndFormatPrice((int)obj));
                });
            });
        });
        #endregion
        byte[] pdf = report.GenerateAsByteArray();

        if (pdf != null)
        {
            Response.Headers.Add("content-disposition", "inline; filename=naplatnePostaje.pdf");
            return File(pdf, "application/pdf");
        }
        else
        {
            return NotFound();

        }
    }

    /// <summary>
    /// header za pdf izvjesca autocesta
    /// </summary>
    public class MasterDetailsHeadersAutoceste : IPageHeader
    {
        private string naslov;
        public MasterDetailsHeadersAutoceste(string naslov)
        {
            this.naslov = naslov;
        }
        public IPdfFont PdfRptFont { set; get; }

        public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
        {
            var oznaka = newGroupInfo.GetSafeStringValueOf(nameof(AutocestaDionicaRow.Oznaka));
            var koncesionar = newGroupInfo.GetSafeStringValueOf(nameof(AutocestaDionicaRow.Koncesionar));
            var poc = newGroupInfo.GetSafeStringValueOf(nameof(AutocestaDionicaRow.Pocetak));
            var kraj = newGroupInfo.GetSafeStringValueOf(nameof(AutocestaDionicaRow.Kraj));
            var len = newGroupInfo.GetSafeStringValueOf(nameof(AutocestaDionicaRow.DuljinaKm));

            var table = new PdfGrid(relativeWidths: new[] { 2f, 5f, 2f, 5f }) { WidthPercentage = 100 };

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Oznaka:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = oznaka;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Pocetak: ";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = poc;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Koncesionar:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = koncesionar;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Kraj:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = kraj;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Duljina (km):";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = len;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });
            return table.AddBorderToTable(borderColor: BaseColor.LightGray, spacingBefore: 5f);
        }

        public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
        {
            var table = new PdfGrid(numColumns: 1) { WidthPercentage = 100 };
            table.AddSimpleRow(
               (cellData, cellProperties) =>
               {
                   cellData.Value = naslov;
                   cellProperties.PdfFont = PdfRptFont;
                   cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                   cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
               });
            return table.AddBorderToTable();
        }
    }

    /// <summary>
    /// Header za PDF izvješća za naplatne postaje
    /// </summary>
    public class MasterDetailsHeadersNaplatne : IPageHeader
    {
        private string naslov;
        public MasterDetailsHeadersNaplatne(string naslov)
        {
            this.naslov = naslov;
        }
        public IPdfFont PdfRptFont { set; get; }

        public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
        {
            var NaplatnaID = newGroupInfo.GetSafeStringValueOf(nameof(StavkaCjenikaDenorm.NaplatnaID));
            var Ime = newGroupInfo.GetSafeStringValueOf(nameof(StavkaCjenikaDenorm.Ime));
            var GeografskaSirina = (double)newGroupInfo.GetValueOf(nameof(StavkaCjenikaDenorm.GeoSirina));
            var GeografskaDuzina = (double)newGroupInfo.GetValueOf(nameof(StavkaCjenikaDenorm.GeoDuzina));

            var table = new PdfGrid(relativeWidths: new[] { 2f, 5f, 2f, 3f }) { WidthPercentage = 100 };

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Id:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = NaplatnaID;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Naziv:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = Ime;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Geografska širina:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = Converters.koordinateToWGS(GeografskaSirina);
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Geografska dužina:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = Converters.koordinateToWGS(GeografskaDuzina);
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });
            return table.AddBorderToTable(borderColor: BaseColor.LightGray, spacingBefore: 5f);
        }

        public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
        {
            var table = new PdfGrid(numColumns: 1) { WidthPercentage = 100 };
            table.AddSimpleRow(
               (cellData, cellProperties) =>
               {
                   cellData.Value = naslov;
                   cellProperties.PdfFont = PdfRptFont;
                   cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                   cellProperties.HorizontalAlignment = HorizontalAlignment.Center;
               });
            return table.AddBorderToTable();
        }
    }


    /// <summary>
    /// Stvara Excel tablicu svih parkirališta gospodarskih vozila
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> ParkGospVozExcel()
    {
        var parkiralista = await ctx.ParkGospVoz
                                     .Select(p => new ParkiralisteGospVozViewModel
                                     {
                                         ParkingId = p.ParkingId,
                                         Stacionaža = p.Stacionaža,
                                         Naziv = p.Naziv,
                                         Dionica = Converters.GetDionicaName(p.Dionica.UlaznaPostajaNavigation.Ime,
                                                  p.Dionica.IzlaznaPostajaNavigation.Ime,
                                                  p.Dionica.OznakaAutoceste),

                                         GeoDuzinaUlaz = p.GeoDuzinaUlaz,
                                         GeoSirinaUlaz = p.GeoSirinaUlaz,
                                         BrojMjesta = p.BrojMjesta,
                                         CijenaPoSatu = p.CijenaPoSatu,
                                         StranaCesteUlaz = p.StranaCesteUlaz,
                                     })                                
                                     .ToListAsync();

        byte[] content;

        using (ExcelPackage excel = new ExcelPackage())
        {
            excel.Workbook.Properties.Title = "Popis parkirališta gospodarskih vozila";
            excel.Workbook.Properties.Author = "Fran Hruza";
            var worksheet = excel.Workbook.Worksheets.Add("Kamere");

            //First add the headers
            worksheet.Cells[1, 1].Value = "Id parkirališta";
            worksheet.Cells[1, 2].Value = "Stacionaža";
            worksheet.Cells[1, 3].Value = "Naziv parkirališta";
            worksheet.Cells[1, 4].Value = "Naziv dionice";
            worksheet.Cells[1, 5].Value = "Geografska širina";
            worksheet.Cells[1, 6].Value = "Geografska dužina";
            worksheet.Cells[1, 7].Value = "Broj mjesta";
            worksheet.Cells[1, 8].Value = "Cijena po satu";
            worksheet.Cells[1, 9].Value = "Strana ceste ulaza";


            for (int i = 0; i < parkiralista.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = parkiralista[i].ParkingId;
                worksheet.Cells[i + 2, 2].Value = parkiralista[i].Stacionaža;
                worksheet.Cells[i + 2, 3].Value = parkiralista[i].Naziv;
                worksheet.Cells[i + 2, 4].Value = parkiralista[i].Dionica;
                worksheet.Cells[i + 2, 5].Value = Converters.koordinateToWGS(parkiralista[i].GeoSirinaUlaz);
                worksheet.Cells[i + 2, 6].Value = Converters.koordinateToWGS(parkiralista[i].GeoDuzinaUlaz);
                worksheet.Cells[i + 2, 7].Value = parkiralista[i].BrojMjesta;
                worksheet.Cells[i + 2, 8].Value = parkiralista[i].CijenaPoSatu != null ? Converters.convertAndFormatPrice(parkiralista[i].CijenaPoSatu) : "-";
                worksheet.Cells[i + 2, 9].Value = parkiralista[i].StranaCesteUlaz;


            }

            worksheet.Cells[1, 1, parkiralista.Count + 1, 4].AutoFitColumns();

            content = excel.GetAsByteArray();
        }
        return File(content, ExcelContentType, "parkiralista.xlsx");
    }

    /// <summary>
    /// Vraca excel tablicu autocesta i koncesionara
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> AutocesteKoncesionariExcel()
    {
        var autoceste = await ctx.Autoceste
                                     .Include(a => a.KoncesionarNavigation)
                                     .ToListAsync();

        byte[] content;

        using (ExcelPackage excel = new ExcelPackage())
        {
            excel.Workbook.Properties.Title = "Popis autocesta i njihovih koncesionara";
            excel.Workbook.Properties.Author = "Joško Vrsalović";
            var worksheet = excel.Workbook.Worksheets.Add("autoceste");

            //First add the headers
            worksheet.Cells[1, 1].Value = "Oznaka autoceste";
            worksheet.Cells[1, 2].Value = "Pocetak";
            worksheet.Cells[1, 3].Value = "Kraj";
            worksheet.Cells[1, 4].Value = "Duljina (km)";
            worksheet.Cells[1, 5].Value = "Naziv koncesionara";
            worksheet.Cells[1, 6].Value = "Url koncesionara";

            for (int i = 0; i < autoceste.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = autoceste[i].Oznaka;
                worksheet.Cells[i + 2, 2].Value = autoceste[i].Pocetak;
                worksheet.Cells[i + 2, 3].Value = autoceste[i].Kraj;
                worksheet.Cells[i + 2, 4].Value = autoceste[i].DuljinaKm;
                worksheet.Cells[i + 2, 5].Value = autoceste[i].Koncesionar;
                worksheet.Cells[i + 2, 6].Value = autoceste[i].KoncesionarNavigation.Url;
            }

            worksheet.Cells[1, 1, autoceste.Count + 1, 6].AutoFitColumns();

            content = excel.GetAsByteArray();
        }
        return File(content, ExcelContentType, "autocesteKoncesionari.xlsx");
    }

    /// <summary>
    /// Vraca master detail tablicu autocesta i dionica
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> AutocesteDioniceExcel()
    {
        var autoceste = await ctx.Autoceste
                                     .Include(a => a.Dionice)
                                     .ThenInclude(d => d.UlaznaPostajaNavigation)
                                     .Include(a => a.Dionice)
                                     .ThenInclude(d => d.IzlaznaPostajaNavigation)
                                     .ToListAsync();

        byte[] content;

        using (ExcelPackage excel = new ExcelPackage())
        {
            excel.Workbook.Properties.Title = "Popis autocesta i njihovih dionica";
            excel.Workbook.Properties.Author = "Joško Vrsalović";
            var worksheet = excel.Workbook.Worksheets.Add("autoceste");

            int row = 1;
            foreach (var autocesta in autoceste)
            {
                worksheet.Cells[row, 3].Value = "Naziv dionice";
                worksheet.Cells[row, 4].Value = "Ulazna postaja";
                worksheet.Cells[row, 5].Value = "Izlazna postaja";
                worksheet.Cells[row, 6].Value = "Duljina (km)";

                worksheet.Cells[row + 1, 1].Value = "Oznaka autoceste";
                worksheet.Cells[row + 2, 1].Value = "Naziv koncesionara";
                worksheet.Cells[row + 3, 1].Value = "Pocetak";
                worksheet.Cells[row + 4, 1].Value = "Kraj";
                worksheet.Cells[row + 5, 1].Value = "Duljina (km)";

                worksheet.Cells[row + 1, 2].Value = autocesta.Oznaka;
                worksheet.Cells[row + 2, 2].Value = autocesta.Koncesionar;
                worksheet.Cells[row + 3, 2].Value = autocesta.Pocetak;
                worksheet.Cells[row + 4, 2].Value = autocesta.Kraj;
                worksheet.Cells[row + 5, 2].Value = autocesta.DuljinaKm;

                worksheet.Cells[row + 1, 1, row + 5, 2].Style.Fill.SetBackground(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent1);
                worksheet.Cells[row, 3, row, 6].Style.Fill.SetBackground(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent1);

                int rowInner = row + 1;
                foreach(var dionica in autocesta.Dionice)
                {
                    worksheet.Cells[rowInner, 3].Value = Converters.GetDionicaName(dionica);
                    worksheet.Cells[rowInner, 4].Value = dionica.UlaznaPostajaNavigation.Ime;
                    worksheet.Cells[rowInner, 5].Value = dionica.IzlaznaPostajaNavigation.Ime;
                    worksheet.Cells[rowInner, 6].Value = dionica.DuljinaKm;
                    rowInner++;
                }

                row = row + Math.Max(7, autocesta.Dionice.Count + 2);
            }

            worksheet.Cells[1, 1, row, 6].AutoFitColumns();

            content = excel.GetAsByteArray();
        }
        return File(content, ExcelContentType, "autocesteDionice.xlsx");
    }
}