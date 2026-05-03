using NexusERP.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NexusERP.Helpers
{
    public class FacturaDocument: IDocument
    {
        private Factura factura;

        public FacturaDocument(Factura factura)
        {
            this.factura = factura;
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
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Arial));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                // Datos Emisor (Tu Empresa)
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(this.factura.Empresa?.NombreComercial ?? "Tu Empresa").SemiBold().FontSize(14).FontColor(Colors.Blue.Darken2);
                    column.Item().Text($"CIF: {this.factura.Empresa?.Cif}");
                });

                // Datos Factura
                row.RelativeItem().AlignRight().Column(column =>
                {
                    column.Item().Text("FACTURA").Bold().FontSize(20).FontColor(Colors.Grey.Darken3);
                    column.Item().Text($"Nº: {this.factura.NumeroFactura}").SemiBold().FontSize(12);
                    column.Item().Text($"Fecha: {this.factura.FechaEmision:dd/MM/yyyy}");
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                // Datos Cliente
                column.Item().PaddingBottom(20).Background(Colors.Grey.Lighten4).Padding(15).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("FACTURAR A:").SemiBold().FontColor(Colors.Grey.Darken2).FontSize(9);
                        col.Item().Text(this.factura.Cliente?.RazonSocial).Bold().FontSize(12);
                        col.Item().Text($"NIF/CIF: {this.factura.Cliente?.CifNif}");
                    });
                });

                // Tabla de Líneas
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(4); // Concepto
                        columns.RelativeColumn(1); // Cantidad
                        columns.RelativeColumn(1.5f); // Precio Unitario
                        columns.RelativeColumn(1.5f); // Total Fila
                    });

                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).PaddingBottom(5).Text("DESCRIPCIÓN").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("CANT.").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("PRECIO UD.").SemiBold();
                        header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("IMPORTE").SemiBold();
                    });

                    decimal baseImponible = 0;
                    foreach (var linea in this.factura.FacturaDetalles)
                    {
                        decimal importeFila = linea.Cantidad * linea.PrecioUnitario;
                        baseImponible += importeFila;

                        table.Cell().PaddingVertical(5).Text(linea.Concepto);
                        table.Cell().PaddingVertical(5).AlignRight().Text($"{linea.Cantidad:N2}");
                        table.Cell().PaddingVertical(5).AlignRight().Text($"{linea.PrecioUnitario:N2} €");
                        table.Cell().PaddingVertical(5).AlignRight().Text($"{importeFila:N2} €");
                    }

                    // Cálculos finales
                    decimal total = baseImponible + this.factura.IvaTotal;

                    // Totales (Alineados a la derecha)
                    table.Cell().ColumnSpan(4).PaddingTop(15).Element(c => c.AlignRight()).Column(col =>
                    {
                        col.Item().Row(r => { r.RelativeItem().AlignRight().Text("Base Imponible:"); r.ConstantItem(100).AlignRight().Text($"{baseImponible:N2} €"); });
                        col.Item().Row(r => { r.RelativeItem().AlignRight().Text($"IVA:"); r.ConstantItem(100).AlignRight().Text($"{this.factura.IvaTotal:N2} €"); });

                        col.Item().PaddingTop(5).Row(r =>
                        {
                            r.RelativeItem().AlignRight().Text("TOTAL FACTURA:").Bold().FontSize(12).FontColor(Colors.Blue.Darken2);
                            r.ConstantItem(100).AlignRight().Text($"{total:N2} €").Bold().FontSize(12).FontColor(Colors.Blue.Darken2);
                        });
                    });
                });
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(10).AlignCenter().Text(x =>
            {
                x.Span("Gracias por confiar en nosotros. ");
                x.Span($"Página ").FontColor(Colors.Grey.Medium);
                x.CurrentPageNumber().FontColor(Colors.Grey.Medium);
                x.Span(" de ").FontColor(Colors.Grey.Medium);
                x.TotalPages().FontColor(Colors.Grey.Medium);
            });
        }
    }
}
