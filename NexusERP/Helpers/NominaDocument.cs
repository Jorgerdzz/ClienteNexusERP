using NexusERP.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NexusERP.Helpers
{
    public class NominaDocument: IDocument
    {
        private Nomina nomina;

        public NominaDocument(Nomina nomina)
        {
            this.nomina = nomina;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Arial));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Datos Empresa (Izquierda)
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(this.nomina.Empresa?.NombreComercial ?? "Datos de Empresa").SemiBold().FontSize(12);
                    column.Item().Text($"CIF: {this.nomina.Empresa?.Cif}");
                });

                // Título (Derecha)
                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Item().Text("RECIBO INDIVIDUAL DE SALARIOS").Bold().FontSize(14).FontColor(Colors.Blue.Darken2);
                    column.Item().Text($"Período de liquidación: {this.nomina.FechaInicio:dd/MM/yyyy} - {this.nomina.FechaFin:dd/MM/yyyy}");
                    column.Item().Text($"Total días: {(this.nomina.FechaFin.Day - this.nomina.FechaInicio.Day + 1)}");
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(15).Column(column =>
            {
                // Caja de Datos del Trabajador
                column.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Trabajador:").SemiBold();
                        col.Item().Text($"{this.nomina.Empleado?.Nombre} {this.nomina.Empleado?.Apellidos}");
                        col.Item().Text($"DNI/NIE: {this.nomina.Empleado?.Dni}");
                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Nº Afiliación S.S.:").SemiBold();
                        col.Item().Text(this.nomina.Empleado?.NumSeguridadSocial);
                        col.Item().Text($"Grupo Cotización: {this.nomina.Empleado?.GrupoCotizacion}");
                    });
                });

                column.Item().PaddingVertical(10);

                // Tabla de Conceptos
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Concepto
                        columns.RelativeColumn(1); // Devengos
                        columns.RelativeColumn(1); // Deducciones
                    });

                    // Cabecera de la tabla
                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("CONCEPTO").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("DEVENGOS").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("DEDUCCIONES").SemiBold();
                    });

                    // Líneas de detalle (Devengos = Tipo 1, Deducciones = Tipo 2)
                    foreach (var detalle in this.nomina.NominaDetalles.OrderBy(d => d.Tipo))
                    {
                        table.Cell().PaddingVertical(3).Text(detalle.ConceptoNombre);

                        if (detalle.Tipo == 1) // Devengo
                        {
                            table.Cell().PaddingVertical(3).AlignRight().Text($"{detalle.Importe:N2} €");
                            table.Cell().PaddingVertical(3).AlignRight().Text("");
                        }
                        else // Deducción
                        {
                            table.Cell().PaddingVertical(3).AlignRight().Text("");
                            table.Cell().PaddingVertical(3).AlignRight().Text($"{detalle.Importe:N2} €");
                        }
                    }

                    // Fila de Totales
                    table.Cell().ColumnSpan(3).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);

                    table.Cell().PaddingTop(5).Text("TOTALES").Bold();
                    table.Cell().PaddingTop(5).AlignRight().Text($"{this.nomina.TotalDevengado:N2} €").Bold();
                    table.Cell().PaddingTop(5).AlignRight().Text($"{this.nomina.TotalDeducciones:N2} €").Bold();
                });

                column.Item().PaddingVertical(15);

                // Líquido a percibir gigante
                column.Item().AlignRight().Text(text =>
                {
                    text.Span("LÍQUIDO A PERCIBIR: ").SemiBold().FontSize(12);
                    text.Span($"{this.nomina.LiquidoApercibir:N2} €").Bold().FontSize(16).FontColor(Colors.Blue.Darken2);
                });
            });
        }

        void ComposeFooter(IContainer container)
        {
            // Pie de página: Bases de Cotización
            container.BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(10).Column(column =>
            {
                column.Item().Text("DETERMINACIÓN DE LAS BASES DE COTIZACIÓN AL RÉGIMEN GENERAL Y BASE DE I.R.P.F.").SemiBold().FontSize(8);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(col =>
                    {
                        col.RelativeColumn();
                        col.RelativeColumn();
                        col.RelativeColumn();
                    });

                    table.Cell().Text($"Base Contingencias Comunes: {this.nomina.BaseCotizacionCc:N2} €").FontSize(8);
                    table.Cell().Text($"Base Contingencias Profesionales: {this.nomina.BaseCotizacionCp:N2} €").FontSize(8);
                    table.Cell().Text($"Base Sujeta a I.R.P.F.: {this.nomina.BaseIrpf:N2} €").FontSize(8);
                });

                column.Item().PaddingTop(10).AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        }
    }

}
