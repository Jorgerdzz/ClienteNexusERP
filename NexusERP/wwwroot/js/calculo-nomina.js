/* --- wwwroot/js/calculo-nomina.js --- */

const CONCEPTOS_MAP = {
    'Incentivos': 'Incentivos',
    'Plus especial dedicación': 'PlusDedicacion',
    'Plus antigüedad': 'PlusAntiguedad',
    'Plus actividad': 'PlusActividad',
    'Plus nocturnidad': 'PlusNocturnidad',
    'Plus responsabilidad': 'PlusResponsabilidad',
    'Plus convenio': 'PlusConvenio',
    'Plus idiomas': 'PlusIdiomas',
    'Horas extraordinarias': 'HorasExtraordinarias',
    'Salario en especie': 'SalarioEspecie'
};

const CONCEPTOS_NO_SALARIALES_MAP = {
    'Indemnizaciones o Suplidos': 'IndemnizacionesSuplidos',
    'Prestaciones e indemnizaciones S.S.': 'PrestacionesSS',
    'Indemnizaciones por despido': 'IndemnizacionesDespido',
    'Plus transporte': 'PlusTransporte',
    'Dietas': 'Dietas'
};

// ============================================
// FUNCIONES GLOBALES (Añadir/Eliminar)
// ============================================

function addConceptoSalarial() {
    const container = document.getElementById('conceptosSalarialesContainer');
    const newRow = document.createElement('div');
    newRow.className = 'concepto-row';
    newRow.innerHTML = `
        <div class="row align-items-center">
            <div class="col-md-5 mb-2 mb-md-0">
                <select class="form-control concepto-nombre-input concepto-selector" onchange="actualizarNombreInput(this)">
                    <option value="">-- Seleccionar concepto --</option>
                    <option value="Incentivos">Incentivos</option>
                    <option value="Plus especial dedicación">Plus especial dedicación</option>
                    <option value="Plus antigüedad">Plus antigüedad</option>
                    <option value="Plus actividad">Plus actividad</option>
                    <option value="Plus nocturnidad">Plus nocturnidad</option>
                    <option value="Plus responsabilidad">Plus responsabilidad</option>
                    <option value="Plus convenio">Plus convenio</option>
                    <option value="Plus idiomas">Plus idiomas</option>
                    <option value="Horas extraordinarias">Horas extraordinarias</option>
                    <option value="Horas complementarias">Horas complementarias</option>
                    <option value="Salario en especie">Salario en especie</option>
                </select>
            </div>
            <div class="col-md-5 mb-2 mb-md-0">
                <div class="input-group">
                    <input type="number" step="0.01" class="form-control concepto-input input-devengo" 
                           value="0.00" data-tributa-irpf="true" 
                           data-field-name="" disabled oninput="calcularNomina()">
                    <span class="input-group-text bg-light border-0">€</span>
                </div>
            </div>
            <div class="col-md-2 text-center">
                <button type="button" class="btn btn-delete-concepto" onclick="eliminarConcepto(this)" title="Eliminar concepto">
                    <svg width="18" height="18" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path></svg>
                </button>
            </div>
        </div>
    `;
    container.appendChild(newRow);
}

function addConceptoNoSalarial() {
    const container = document.getElementById('conceptosNoSalarialesContainer');
    const newRow = document.createElement('div');
    newRow.className = 'concepto-row';
    newRow.innerHTML = `
        <div class="row align-items-center">
            <div class="col-md-5 mb-2 mb-md-0">
                <select class="form-control concepto-nombre-input concepto-selector" onchange="actualizarNombreInput(this)">
                    <option value="">-- Seleccionar concepto --</option>
                    <option value="Indemnizaciones o Suplidos">Indemnizaciones o Suplidos</option>
                    <option value="Prestaciones e indemnizaciones S.S.">Prestaciones e indemnizaciones S.S.</option>
                    <option value="Indemnizaciones por despido">Indemnizaciones por despido</option>
                    <option value="Plus transporte">Plus transporte</option>
                    <option value="Dietas">Dietas</option>
                </select>
            </div>
            <div class="col-md-5 mb-2 mb-md-0">
                <div class="input-group">
                    <input type="number" step="0.01" class="form-control concepto-input input-devengo" 
                           value="0.00" data-tributa-irpf="false" 
                           data-field-name="" disabled oninput="calcularNomina()">
                    <span class="input-group-text bg-light border-0">€</span>
                </div>
            </div>
            <div class="col-md-2 text-center">
                <button type="button" class="btn btn-delete-concepto" onclick="eliminarConcepto(this)" title="Eliminar concepto">
                    <svg width="18" height="18" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path></svg>
                </button>
            </div>
        </div>
    `;
    container.appendChild(newRow);
}

function actualizarNombreInput(selectElement) {
    const row = selectElement.closest('.concepto-row');
    const input = row.querySelector('.concepto-input');
    const conceptoNombre = selectElement.value;

    if (conceptoNombre) {
        const isSalarial = selectElement.closest('#conceptosSalarialesContainer') !== null;
        const mapa = isSalarial ? CONCEPTOS_MAP : CONCEPTOS_NO_SALARIALES_MAP;

        const fieldName = mapa[conceptoNombre];
        input.setAttribute('name', fieldName);
        input.setAttribute('data-field-name', fieldName);
        input.disabled = false;
    } else {
        input.disabled = true;
        input.removeAttribute('name');
        input.setAttribute('data-field-name', '');
        input.value = "0.00"; // Reiniciamos si deselecciona
    }

    calcularNomina();
}

function eliminarConcepto(btn) {
    btn.closest('.concepto-row').remove();
    calcularNomina();
}

// ============================================
// MOTOR DE CÁLCULO
// ============================================

function formatMoneda(val) {
    return val.toLocaleString('es-ES', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

function calcularNomina() {
    let totalDevengado = 0;
    let baseIRPF = 0;
    let baseCC = 0;
    let horasExtras = 0;

    const inputSalarioBase = document.getElementById('SalarioBase');
    const valBase = parseFloat(inputSalarioBase?.value) || 0;
    const porcentajeIrpf = parseFloat(document.getElementById('porcentajeIrpfAplicado')?.value) || 0;

    // 1. Sumar todos los devengos
    const todosInputsDevengos = document.querySelectorAll('.input-devengo');

    todosInputsDevengos.forEach(input => {
        if (input.disabled) return;

        const importe = parseFloat(input.value) || 0;
        totalDevengado += importe;

        const fieldName = (input.getAttribute('name') || input.getAttribute('data-field-name') || '').toLowerCase();
        const tributaIrpf = (fieldName !== 'salarioespecie' && fieldName !== 'dietas');
        const isNoSalarial = input.getAttribute('data-tributa-irpf') === "false";
        const esHoraExtra = fieldName.includes('horas') && fieldName.includes('extra');

        if (tributaIrpf) {
            baseIRPF += importe;
        }

        if (esHoraExtra) {
            horasExtras += importe;
        } else if (!isNoSalarial) {
            baseCC += importe;
        }
    });

    // 2. Prorrateo 2 pagas extra
    const prorrateo = (valBase * 2) / 12;
    baseCC += prorrateo;

    const baseCP = baseCC + horasExtras;

    // 3. Trabajador
    const cuotaCC = baseCC * 0.0470;
    const cuotaMEI = baseCC * 0.0012;
    const cuotaDes = baseCP * 0.0155;
    const cuotaFP = baseCP * 0.0010;
    const cuotaIRPF = baseIRPF * (porcentajeIrpf / 100);

    const totalDeducciones = cuotaCC + cuotaMEI + cuotaDes + cuotaFP + cuotaIRPF;
    const liquido = totalDevengado - totalDeducciones;

    // 4. Empresa
    const cEmpCC = baseCC * 0.2360;
    const cEmpMEI = baseCC * 0.0058;
    const cEmpAT = baseCP * 0.0150;
    const cEmpDes = baseCP * 0.0550;
    const cEmpForm = baseCP * 0.0060;
    const cEmpFog = baseCP * 0.0020;
    const cEmpHorasExtras = horasExtras * 0.2360;

    const totalEmpSS = cEmpCC + cEmpMEI + cEmpAT + cEmpDes + cEmpForm + cEmpFog + cEmpHorasExtras;

    // 5. Pintar en pantalla (Si los elementos existen)
    const setVal = (id, val) => { const el = document.getElementById(id); if (el) el.value = val; };

    setVal('total_devengado', formatMoneda(totalDevengado));
    setVal('base_cc', formatMoneda(baseCC));
    setVal('base_mei', formatMoneda(baseCC));
    setVal('base_desempleo', formatMoneda(baseCP));
    setVal('base_fp', formatMoneda(baseCP));
    setVal('base_irpf', formatMoneda(baseIRPF));

    setVal('importe_cc', formatMoneda(cuotaCC));
    setVal('importe_mei', formatMoneda(cuotaMEI));
    setVal('importe_desempleo', formatMoneda(cuotaDes));
    setVal('importe_fp', formatMoneda(cuotaFP));
    setVal('importe_irpf', formatMoneda(cuotaIRPF));

    setVal('total_aportaciones', formatMoneda(cuotaCC + cuotaMEI + cuotaDes + cuotaFP));
    setVal('total_deducciones', formatMoneda(totalDeducciones));
    setVal('liquido_percibir', formatMoneda(liquido));

    setVal('base_empresa_cc', formatMoneda(baseCC));
    setVal('base_empresa_he', formatMoneda(horasExtras));
    setVal('base_empresa_at', formatMoneda(baseCP));
    setVal('base_empresa_desempleo', formatMoneda(baseCP));
    setVal('base_empresa_formacion', formatMoneda(baseCP));
    setVal('base_empresa_fogasa', formatMoneda(baseCP));
    setVal('base_empresa_mei', formatMoneda(baseCC));

    setVal('importe_empresa_cc', formatMoneda(cEmpCC));
    setVal('importe_empresa_he', formatMoneda(cEmpHorasExtras));
    setVal('importe_empresa_at', formatMoneda(cEmpAT));
    setVal('importe_empresa_desempleo', formatMoneda(cEmpDes));
    setVal('importe_empresa_formacion', formatMoneda(cEmpForm));
    setVal('importe_empresa_fogasa', formatMoneda(cEmpFog));
    setVal('importe_empresa_mei', formatMoneda(cEmpMEI));

    // CORRECCIÓN 1: Sustituimos el punto por coma en los campos ocultos calculados
    setVal('total_empresa_ss_hidden', totalEmpSS.toFixed(2).replace('.', ','));

    // Este formatMoneda ya viene con coma, no hay problema
    setVal('coste_total_empresa', formatMoneda(totalDevengado + totalEmpSS));
}

// ============================================
// INICIALIZACIÓN Y PARCHE DE ENVÍO
// ============================================
document.addEventListener('DOMContentLoaded', function () {
    const inputSalarioBase = document.getElementById('SalarioBase');
    const salarioSugeridoText = document.getElementById('salarioSugerido');

    // Prellenar salario base con el sugerido si está vacío
    if (inputSalarioBase && salarioSugeridoText) {
        let textoSugerido = salarioSugeridoText.innerText;
        let sugeridoVal = parseFloat(textoSugerido) || 0;

        if (!inputSalarioBase.value || parseFloat(inputSalarioBase.value) === 0) {
            inputSalarioBase.value = sugeridoVal.toFixed(2);
        }
    }

    // Escuchar cambios estáticos (Salario Base)
    if (inputSalarioBase) {
        inputSalarioBase.addEventListener('input', calcularNomina);
    }

    // Calcular por primera vez al entrar
    calcularNomina();

    // ============================================
    // CORRECCIÓN 2: Interceptor de formulario (Solución exclusiva Salario Base)
    // ============================================
    const formNomina = document.querySelector('form.form-crud') || document.querySelector('form');

    if (formNomina) {
        formNomina.addEventListener('submit', function () {

            // 1. Buscamos todos los inputs numéricos EXCEPTO el Salario Base
            const otrosInputs = this.querySelectorAll('input[type="number"]:not(#SalarioBase), .concepto-input:not(#SalarioBase)');

            // A los demás inputs les aplicamos tu lógica original (cambiar punto por coma)
            otrosInputs.forEach(input => {
                if (input.value) {
                    let valorConComa = input.value.replace('.', ',');
                    input.type = 'text';
                    input.value = valorConComa;
                }
            });

            // 2. Tratamiento EXCLUSIVO para el Salario Base
            const inputSalarioBase = document.getElementById('SalarioBase');
            if (inputSalarioBase && inputSalarioBase.value) {
                let valorSalario = inputSalarioBase.value.toString();

                // Si el salario tiene una coma visual (ej: "2500,50"), la convertimos al punto universal
                if (valorSalario.includes(',')) {
                    valorSalario = valorSalario.replace(/\./g, '').replace(',', '.');
                }

                // Lo pasamos a texto y forzamos que se envíe limpio al servidor como "2500.50"
                inputSalarioBase.type = 'text';
                inputSalarioBase.value = parseFloat(valorSalario).toFixed(2);
            }

        });
    }
});