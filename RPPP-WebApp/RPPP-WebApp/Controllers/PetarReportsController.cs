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
using RPPP_WebApp.UtilClasses;
using RPPP_WebApp.ViewModels;

namespace RPPP_WebApp.Controllers;

/// <summary>
/// Servis za ispis reportova u Excelu i PDF-u 
/// </summary>
public class PetarReportsController : Controller
{
    private readonly RPPP05Context ctx;
    private readonly IWebHostEnvironment environment;
    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public PetarReportsController(RPPP05Context ctx, IWebHostEnvironment environment)
    {
        this.ctx = ctx;
        this.environment = environment;
    }

    /// <summary>
    /// Vraca excel datoteku s popisom svih odmorista
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OdmoristaExcel()
    {
        var odmorista = ctx.Odmoriste
                    .Include(o => o.Dionica)
                    .ThenInclude(d => d.UlaznaPostajaNavigation)
                    .Include(o => o.Dionica)
                    .ThenInclude(d => d.IzlaznaPostajaNavigation)
                    .AsNoTracking();

        List<OdmoristeViewModel> odmoristeViewModels = new List<OdmoristeViewModel>();
        foreach (var odmoriste in odmorista)
        {
            odmoristeViewModels.Add(
                new OdmoristeViewModel
                {
                    Id = odmoriste.Id,
                    Naziv = odmoriste.Naziv,
                    Opis = odmoriste.Opis,
                    Smjer = odmoriste.Smjer,
                    NadmorskaVisina = odmoriste.NadmorskaVisina,
                    Stacionaza = odmoriste.StacionazaKm + "+" + odmoriste.StacionazaM + " km",
                    GeografskaDuzina = odmoriste.GeografskaDuzina,
                    GeografskaSirina = odmoriste.GeografskaSirina,
                    DionicaNaziv = Converters.GetDionicaName(odmoriste.Dionica)
                }
                );
        }


        byte[] content;

        using (ExcelPackage excel = new ExcelPackage())
        {
            excel.Workbook.Properties.Title = "Popis odmorišta";
            excel.Workbook.Properties.Author = "Petar Novak";
            var worksheet = excel.Workbook.Worksheets.Add("Odmorista");

            //First add the headers
            worksheet.Cells[1, 1].Value = "Id odmorišta";
            worksheet.Cells[1, 2].Value = "Naziv dionice";
            worksheet.Cells[1, 3].Value = "Naziv odmorišta";
            worksheet.Cells[1, 4].Value = "Geografska sirina";
            worksheet.Cells[1, 5].Value = "Geografska duzina";
            worksheet.Cells[1, 6].Value = "Smjer";
            worksheet.Cells[1, 7].Value = "Stacionaža";
            worksheet.Cells[1, 8].Value = "Nadmorska visina";
            worksheet.Cells[1, 9].Value = "Opis";

            for (int i = 0; i < odmoristeViewModels.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = odmoristeViewModels[i].Id;
                worksheet.Cells[i + 2, 2].Value = odmoristeViewModels[i].DionicaNaziv;
                worksheet.Cells[i + 2, 3].Value = odmoristeViewModels[i].Naziv;
                worksheet.Cells[i + 2, 4].Value = odmoristeViewModels[i].GeografskaSirina;
                worksheet.Cells[i + 2, 5].Value = odmoristeViewModels[i].GeografskaDuzina;
                worksheet.Cells[i + 2, 6].Value = odmoristeViewModels[i].Smjer;
                worksheet.Cells[i + 2, 7].Value = odmoristeViewModels[i].Stacionaza;
                worksheet.Cells[i + 2, 8].Value = odmoristeViewModels[i].NadmorskaVisina;
                worksheet.Cells[i + 2, 9].Value = odmoristeViewModels[i].Opis;

            }

            worksheet.Cells[1, 1, odmoristeViewModels.Count + 1, 4].AutoFitColumns();

            content = excel.GetAsByteArray();
        }
        return File(content, ExcelContentType, "odmorista.xlsx");
    }

    /// <summary>
    /// Vraca excel datoteku s popisom svih odmorista i njihovih sadrzaja 
    /// u master detail obliku
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OdmoristaSadrzajiExcel()
    {
        var odmorista = ctx.Odmoriste
                    .Include(o => o.Dionica)
                    .ThenInclude(d => d.UlaznaPostajaNavigation)
                    .Include(o => o.Dionica)
                    .ThenInclude(d => d.IzlaznaPostajaNavigation)
                    .AsNoTracking();

        List<OdmoristeViewModel> odmoristeViewModels = new List<OdmoristeViewModel>();
        foreach (var odmoriste in odmorista)
        {
            odmoristeViewModels.Add(
                new OdmoristeViewModel
                {
                    Id = odmoriste.Id,
                    Naziv = odmoriste.Naziv,
                    Opis = odmoriste.Opis,
                    Smjer = odmoriste.Smjer,
                    NadmorskaVisina = odmoriste.NadmorskaVisina,
                    Stacionaza = odmoriste.StacionazaKm + "+" + odmoriste.StacionazaM + " km",
                    GeografskaDuzina = odmoriste.GeografskaDuzina,
                    GeografskaSirina = odmoriste.GeografskaSirina,
                    DionicaNaziv = Converters.GetDionicaName(odmoriste.Dionica)
                }
                );
        }

        byte[] content;

        using (ExcelPackage excel = new ExcelPackage())
        {
            excel.Workbook.Properties.Title = "Popis odmorišta";
            excel.Workbook.Properties.Author = "Petar Novak";
            var worksheet = excel.Workbook.Worksheets.Add("Odmorista");

            int redak = 1;

            foreach (var odmoriste in odmoristeViewModels)
            {
                worksheet.Cells[redak, 1].Value = "Id odmorišta";
                worksheet.Cells[redak, 2].Value = odmoriste.Id;
                redak++;

                worksheet.Cells[redak, 1].Value = "Naziv dionice";
                worksheet.Cells[redak, 2].Value = odmoriste.DionicaNaziv;
                redak++;

                worksheet.Cells[redak, 1].Value = "Smjer";
                worksheet.Cells[redak, 2].Value = odmoriste.Smjer;
                redak++;

                worksheet.Cells[redak, 1].Value = "Stacionaža";
                worksheet.Cells[redak, 2].Value = odmoriste.Stacionaza;
                redak++;

                worksheet.Cells[redak, 1].Value = "Nadmorska visina";
                worksheet.Cells[redak, 2].Value = odmoriste.NadmorskaVisina;
                redak++;

                worksheet.Cells[redak, 1].Value = "g.š. odmorišta";
                worksheet.Cells[redak, 2].Value = odmoriste.GeografskaSirina;
                redak++;

                worksheet.Cells[redak, 1].Value = "g.d. odmorišta";
                worksheet.Cells[redak, 2].Value = odmoriste.GeografskaDuzina;
                redak++;

                redak++;

                worksheet.Cells[redak, 3].Value = "Naziv sadržaja";
                worksheet.Cells[redak, 4].Value = "Tip sadržaja";
                worksheet.Cells[redak, 5].Value = "g.š. sadržaja";
                worksheet.Cells[redak, 6].Value = "g.d. sadržaja";
                redak++;

                var sadrzaji = ctx.Sadrzaj
                    .Where(s => s.OdmoristeId == odmoriste.Id)
                    .Include(s => s.TipSadrzaja)
                    .AsNoTracking();

                foreach (var sadrzaj in sadrzaji)
                {
                    worksheet.Cells[redak, 3].Value = sadrzaj.Naziv;
                    worksheet.Cells[redak, 4].Value = sadrzaj.TipSadrzaja.Naziv;
                    worksheet.Cells[redak, 5].Value = sadrzaj.GeografskaSirina;
                    worksheet.Cells[redak, 6].Value = sadrzaj.GeografskaDuzina;
                    redak++;
                }
                redak++;
            }


            worksheet.Cells[1, 1, redak + 1, 6].AutoFitColumns();

            content = excel.GetAsByteArray();
        }
        return File(content, ExcelContentType, "odmorista i sadrzaji.xlsx");

    }

    /// <summary>
    /// Vraca pdf datoteku s popisom svih odmorista i njihovih sadrzaja 
    /// u master detail obliku
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OdmoristeSadrzaji()
    {
        string naslov = "Popis svih odmorišta i njihovih sadržaja";
        PdfReport report = CreateReport(naslov);

        //var sadrzaji = ctx.SadrzajDenorms().OrderBy(s => s.OdmoristeId).ThenBy(s => s.Naziv);
        var sadrzaji = new List<OdmoristeSadrzajViewModel>();

        var query = ctx.Sadrzaj
            .Include(s => s.TipSadrzaja)
            .Include(s => s.Odmoriste)
            .ThenInclude(o => o.Dionica)
            .ThenInclude(d => d.IzlaznaPostajaNavigation)
            .Include(s => s.Odmoriste)
            .ThenInclude(o => o.Dionica)
            .ThenInclude(d => d.UlaznaPostajaNavigation)
            .AsNoTracking();

        foreach (var sadrzaj in query)
        {
            sadrzaji.Add(new OdmoristeSadrzajViewModel
            {
                NazivSadrzaja = sadrzaj.Naziv,
                GeografskaSirinaSadrzaja = sadrzaj.GeografskaSirina,
                GeografskaDuzinaSadrzaja = sadrzaj.GeografskaDuzina,
                TipSadrzajaNaziv = sadrzaj.TipSadrzaja.Naziv,
                OdmoristeId = sadrzaj.OdmoristeId,
                NazivOdmorista = sadrzaj.Odmoriste.Naziv,
                Opis = sadrzaj.Odmoriste.Opis,
                Smjer = sadrzaj.Odmoriste.Smjer,
                NadmorskaVisina = sadrzaj.Odmoriste.NadmorskaVisina,
                GeografskaDuzinaOdmorista = sadrzaj.Odmoriste.GeografskaDuzina,
                GeografskaSirinaOdmorista = sadrzaj.Odmoriste.GeografskaSirina,
                Stacionaza = sadrzaj.Odmoriste.StacionazaKm + "+" + sadrzaj.Odmoriste.StacionazaM + " km",
                NazivDionice = Converters.GetDionicaName(sadrzaj.Odmoriste.Dionica)
            });
        }

        //header and footer
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


        #region Postavljanje izvora podataka i stupaca
        report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(sadrzaji));

        report.MainTableColumns(columns =>
        {
            #region Stupci po kojima se grupira
            columns.AddColumn(column =>
            {
                column.PropertyName<OdmoristeSadrzajViewModel>(s => s.OdmoristeId);
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
                column.PropertyName<OdmoristeSadrzajViewModel>(s => s.NazivSadrzaja);
                column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                column.IsVisible(true);
                column.Width(4);
                column.HeaderCell("Naziv sadržaja", horizontalAlignment: HorizontalAlignment.Center);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<OdmoristeSadrzajViewModel>(s => s.TipSadrzajaNaziv);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(1);
                column.HeaderCell("Tip sadržaja", horizontalAlignment: HorizontalAlignment.Center);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<OdmoristeSadrzajViewModel>(s => s.GeografskaSirinaSadrzaja);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(1);
                column.HeaderCell("Geografska širina sadržaja", horizontalAlignment: HorizontalAlignment.Center);
            });

            columns.AddColumn(column =>
            {
                column.PropertyName<OdmoristeSadrzajViewModel>(s => s.GeografskaDuzinaSadrzaja);
                column.CellsHorizontalAlignment(HorizontalAlignment.Right);
                column.IsVisible(true);
                column.Width(1);
                column.HeaderCell("Geografska dužina sadržaja", horizontalAlignment: HorizontalAlignment.Center);
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

        return null;
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
                Author = "Petar Novak",
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

    #region Master-detail header
    /// <summary>
    /// Header za PDF izvješća za naplatne postaje
    /// </summary>
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
            var NazivDionice = newGroupInfo.GetSafeStringValueOf(nameof(OdmoristeSadrzajViewModel.NazivDionice));
            var NazivOdmorista = newGroupInfo.GetSafeStringValueOf(nameof(OdmoristeSadrzajViewModel.NazivOdmorista));

            var Smjer = newGroupInfo.GetSafeStringValueOf(nameof(OdmoristeSadrzajViewModel.Smjer));
            var Stacionaza = newGroupInfo.GetSafeStringValueOf(nameof(OdmoristeSadrzajViewModel.Stacionaza));

            var Opis = newGroupInfo.GetSafeStringValueOf(nameof(OdmoristeSadrzajViewModel.Opis));            
            var NadmorskaVisina = newGroupInfo.GetSafeStringValueOf(nameof(OdmoristeSadrzajViewModel.NadmorskaVisina));
            
            var GeografskaSirinaOdmorista = (double)newGroupInfo.GetValueOf(nameof(OdmoristeSadrzajViewModel.GeografskaSirinaOdmorista));
            var GeografskaDuzinaOdmorista = (double)newGroupInfo.GetValueOf(nameof(OdmoristeSadrzajViewModel.GeografskaDuzinaOdmorista));

            var table = new PdfGrid(relativeWidths: new[] { 2f, 5f, 2f, 3f }) { WidthPercentage = 100 };

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Naziv odmorišta:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = NazivOdmorista;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Naziv dionice:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = NazivDionice;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Smjer:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = Smjer;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Stacionaža:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = Stacionaza;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Opis:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = Opis;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Nadmorska visina:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = NadmorskaVisina;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                });

            table.AddSimpleRow(
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Geografska širina odmorišta:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = GeografskaSirinaOdmorista;
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = "Geografska dužina odmorišta:";
                    cellProperties.PdfFont = PdfRptFont;
                    cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
                    cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
                },
                (cellData, cellProperties) =>
                {
                    cellData.Value = GeografskaDuzinaOdmorista;
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

    public IActionResult Index()
    {
        return View();
    }
}