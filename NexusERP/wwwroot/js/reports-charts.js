/* --- wwwroot/js/reports-charts.js --- */

document.addEventListener("DOMContentLoaded", function () {

    // 1. Localizar el contenedor de datos (el main o el div que los guarda)
    const dataContainer = document.getElementById('chartDataContainer');

    if (!dataContainer) {
        console.error("No se ha encontrado el contenedor de datos para los gráficos.");
        return;
    }

    // 2. Extraer y parsear los datos JSON desde los atributos data-*
    let datosIngresos, datosGastos, deptLabels, deptDatos;

    try {
        datosIngresos = JSON.parse(dataContainer.dataset.ingresos);
        datosGastos = JSON.parse(dataContainer.dataset.gastos);
        deptLabels = JSON.parse(dataContainer.dataset.labelsDept);
        deptDatos = JSON.parse(dataContainer.dataset.datosDept);
    } catch (e) {
        console.error("Error al parsear los datos JSON para los gráficos:", e);
        return;
    }

    const mesesLabels = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];

    // ==========================================
    // GRÁFICO 1: LÍNEAS (Ingresos vs Gastos)
    // ==========================================
    const canvasEvolucion = document.getElementById('evolucionChart');
    if (canvasEvolucion) {
        const ctxEvolucion = canvasEvolucion.getContext('2d');

        // Calcular valores mínimos y máximos para ajustar mejor la escala
        const todosLosValores = [...datosIngresos, ...datosGastos].filter(val => val > 0);
        const valorMinimo = todosLosValores.length > 0 ? Math.min(...todosLosValores) : 0;
        const valorMaximo = todosLosValores.length > 0 ? Math.max(...todosLosValores) : 100;
        
        // Ajustar el mínimo con un margen del 20% hacia abajo
        const margenInferior = valorMinimo * 0.8;
        const yMin = Math.floor(margenInferior / 1000) * 1000; // Redondear a miles
        
        // Ajustar el máximo con un margen del 10% hacia arriba
        const margenSuperior = valorMaximo * 1.1;
        const yMax = Math.ceil(margenSuperior / 1000) * 1000;

        // Crear gradientes para las líneas
        const gradientIngresos = ctxEvolucion.createLinearGradient(0, 0, 0, 400);
        gradientIngresos.addColorStop(0, 'rgba(16, 185, 129, 0.3)');
        gradientIngresos.addColorStop(1, 'rgba(16, 185, 129, 0.01)');

        const gradientGastos = ctxEvolucion.createLinearGradient(0, 0, 0, 400);
        gradientGastos.addColorStop(0, 'rgba(239, 68, 68, 0.3)');
        gradientGastos.addColorStop(1, 'rgba(239, 68, 68, 0.01)');

        new Chart(ctxEvolucion, {
            type: 'line',
            data: {
                labels: mesesLabels,
                datasets: [
                    {
                        label: 'Ingresos (Facturación)',
                        data: datosIngresos,
                        borderColor: '#10b981',
                        backgroundColor: gradientIngresos,
                        borderWidth: 3,
                        fill: true,
                        tension: 0.4,
                        pointBackgroundColor: '#10b981',
                        pointBorderColor: '#fff',
                        pointBorderWidth: 3,
                        pointRadius: 6,
                        pointHoverRadius: 9,
                        pointHoverBackgroundColor: '#10b981',
                        pointHoverBorderColor: '#fff',
                        pointHoverBorderWidth: 3
                    },
                    {
                        label: 'Costes (Salariales)',
                        data: datosGastos,
                        borderColor: '#ef4444',
                        backgroundColor: gradientGastos,
                        borderWidth: 3,
                        fill: true,
                        tension: 0.4,
                        pointBackgroundColor: '#ef4444',
                        pointBorderColor: '#fff',
                        pointBorderWidth: 3,
                        pointRadius: 6,
                        pointHoverRadius: 9,
                        pointHoverBackgroundColor: '#ef4444',
                        pointHoverBorderColor: '#fff',
                        pointHoverBorderWidth: 3
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    mode: 'index',
                    intersect: false,
                },
                plugins: {
                    legend: {
                        position: 'top',
                        labels: {
                            usePointStyle: true,
                            padding: 20,
                            font: { size: 14, weight: '600' },
                            boxWidth: 12,
                            boxHeight: 12
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(0, 0, 0, 0.85)',
                        padding: 16,
                        titleFont: { size: 15, weight: 'bold' },
                        bodyFont: { size: 14 },
                        bodySpacing: 8,
                        borderColor: 'rgba(255, 255, 255, 0.15)',
                        borderWidth: 1,
                        cornerRadius: 8,
                        displayColors: true,
                        callbacks: {
                            label: function (context) {
                                return ' ' + context.dataset.label + ': ' + context.parsed.y.toLocaleString('es-ES', {
                                    minimumFractionDigits: 2,
                                    maximumFractionDigits: 2
                                }) + ' €';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        min: yMin,
                        max: yMax,
                        grid: { 
                            color: 'rgba(0, 0, 0, 0.06)', 
                            drawBorder: false,
                            lineWidth: 1
                        },
                        ticks: {
                            font: { size: 12, weight: '500' },
                            padding: 10,
                            callback: function (value) { 
                                return value.toLocaleString('es-ES', {
                                    minimumFractionDigits: 0,
                                    maximumFractionDigits: 0
                                }) + ' €'; 
                            }
                        },
                        border: {
                            display: false
                        }
                    },
                    x: {
                        grid: { display: false },
                        ticks: { 
                            font: { size: 13, weight: '600' },
                            padding: 8
                        },
                        border: {
                            display: false
                        }
                    }
                }
            }
        });
    }

    // ==========================================
    // GRÁFICO 2: DONUT (Departamentos)
    // ==========================================
    const canvasDept = document.getElementById('deptChart');
    if (canvasDept) {
        const ctxDept = canvasDept.getContext('2d');
        const colores = ['#3b9bad', '#8b5cf6', '#ec4899', '#f59e0b', '#10b981', '#6366f1'];

        new Chart(ctxDept, {
            type: 'doughnut',
            data: {
                labels: deptLabels,
                datasets: [{
                    data: deptDatos,
                    backgroundColor: colores,
                    borderWidth: 3,
                    borderColor: '#ffffff',
                    hoverOffset: 8,
                    hoverBorderWidth: 3
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '70%',
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            padding: 15,
                            font: { size: 12, weight: '600' }
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(0, 0, 0, 0.8)',
                        padding: 12,
                        titleFont: { size: 14, weight: 'bold' },
                        bodyFont: { size: 13 },
                        borderColor: 'rgba(255, 255, 255, 0.1)',
                        borderWidth: 1,
                        callbacks: {
                            label: function (context) {
                                let valor = context.parsed.toLocaleString('es-ES') + ' €';
                                let total = context.dataset.data.reduce((a, b) => a + b, 0);
                                let porcentaje = total > 0 ? ((context.parsed / total) * 100).toFixed(1) : 0;
                                return ' ' + context.label + ': ' + valor + ' (' + porcentaje + '%)';
                            }
                        }
                    }
                }
            }
        });
    }
});