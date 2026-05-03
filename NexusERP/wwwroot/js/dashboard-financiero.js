/* =========================================================
   MÓDULO DE CONTABILIDAD: GRÁFICOS DEL DASHBOARD
========================================================= */
document.addEventListener("DOMContentLoaded", function () {

    // Colores globales de tu tema (NexusERP)
    const colorSecondary = '#3b9bad'; // Azul clarito
    const colorDanger = '#ef476f';    // Rojo/Rosa
    const colorWarning = '#ffd166';   // Amarillo
    const colorSuccess = '#60d394';   // Verde
    const colorPrimary = '#2d3e50';   // Azul oscuro

    // ==========================================
    // 1. GRÁFICO CIRCULAR (GASTOS POR NATURALEZA)
    // ==========================================
    const ctxGastos = document.getElementById('chartGastos');
    if (ctxGastos) {
        // Leemos los datos inyectados en el HTML
        const labelsData = ctxGastos.dataset.labels;
        const valuesData = ctxGastos.dataset.values;

        if (labelsData && valuesData) {
            const labelsGastos = JSON.parse(labelsData);
            const dataGastos = JSON.parse(valuesData);

            new Chart(ctxGastos, {
                type: 'doughnut',
                data: {
                    labels: labelsGastos,
                    datasets: [{
                        data: dataGastos,
                        backgroundColor: [colorDanger, colorWarning, colorSecondary, colorSuccess, colorPrimary],
                        borderWidth: 2,
                        borderColor: '#ffffff'
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { position: 'bottom' },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    let value = context.raw;
                                    return ' ' + new Intl.NumberFormat('es-ES', { style: 'currency', currency: 'EUR' }).format(value);
                                }
                            }
                        }
                    },
                    cutout: '65%' // Grosor del anillo
                }
            });
        }
    }

    // ==========================================
    // 2. GRÁFICO DE BARRAS (EVOLUCIÓN MENSUAL)
    // ==========================================
    const ctxEvolucion = document.getElementById('chartEvolucion');
    if (ctxEvolucion) {

        const labelsData = ctxEvolucion.dataset.labels;
        const ingresosData = ctxEvolucion.dataset.ingresos;
        const gastosData = ctxEvolucion.dataset.gastos;

        if (labelsData && ingresosData && gastosData) {
            const labelsMeses = JSON.parse(labelsData);
            const dataIngresos = JSON.parse(ingresosData);
            const dataGastosMes = JSON.parse(gastosData);

            new Chart(ctxEvolucion, {
                type: 'bar',
                data: {
                    labels: labelsMeses,
                    datasets: [
                        {
                            label: 'Ingresos',
                            data: dataIngresos,
                            backgroundColor: colorSecondary,
                            borderRadius: 4
                        },
                        {
                            label: 'Gastos',
                            data: dataGastosMes,
                            backgroundColor: colorDanger,
                            borderRadius: 4
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { position: 'top', align: 'end' }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            grid: { color: '#f3f4f6' },
                            border: { display: false }
                        },
                        x: {
                            grid: { display: false },
                            border: { display: false }
                        }
                    }
                }
            });
        }
    }
});