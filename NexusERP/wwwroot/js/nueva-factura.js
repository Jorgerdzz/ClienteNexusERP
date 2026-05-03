/* =========================================================
   FUNCIONES DE LÍNEAS Y CÁLCULOS
========================================================= */
function addLinea() {
    const tbody = document.getElementById('lineasBody');
    const newRow = document.createElement('tr');
    newRow.className = 'linea-factura';
    newRow.innerHTML = `
        <td><input type="text" class="form-control input-clean concepto" placeholder="Descripción..." required></td>
        <td><input type="number" class="form-control input-clean text-center cantidad" value="1" min="0.01" step="0.01" required onchange="calcularTotales()"></td>
        <td><input type="number" class="form-control input-clean text-end precio" value="0.00" min="0.00" step="0.01" required onchange="calcularTotales()" onkeyup="calcularTotales()"></td>
        <td class="text-end align-middle fw-medium text-dark total-fila">0.00 €</td>
        <td class="text-center align-middle">
            <button type="button" class="btn btn-link text-danger p-0" onclick="eliminarLinea(this)"><i class="far fa-trash-alt"></i></button>
        </td>
    `;
    tbody.appendChild(newRow);
}

function eliminarLinea(btn) {
    const row = btn.closest('tr');
    const tbody = document.getElementById('lineasBody');
    if (tbody.children.length > 1) {
        row.remove();
        calcularTotales();
    } else {
        row.querySelector('.concepto').value = '';
        row.querySelector('.cantidad').value = '1';
        row.querySelector('.precio').value = '0.00';
        calcularTotales();
    }
}

function calcularTotales() {
    let subtotal = 0;
    const filas = document.querySelectorAll('.linea-factura');

    filas.forEach(fila => {
        const cantidad = parseFloat(fila.querySelector('.cantidad').value) || 0;
        const precio = parseFloat(fila.querySelector('.precio').value) || 0;
        const totalFila = cantidad * precio;

        fila.querySelector('.total-fila').textContent = totalFila.toFixed(2) + ' €';
        subtotal += totalFila;
    });

    const iva = subtotal * 0.21;
    const total = subtotal + iva;

    document.getElementById('txtSubtotal').textContent = subtotal.toFixed(2) + ' €';
    document.getElementById('txtIva').textContent = iva.toFixed(2) + ' €';
    document.getElementById('txtTotal').textContent = total.toFixed(2) + ' €';
}

/* =========================================================
   ENVÍO AJAX (FETCH) Y SWEETALERT
========================================================= */
document.addEventListener("DOMContentLoaded", function () {
    const btnGuardar = document.getElementById('btnGuardarFactura');

    if (btnGuardar) {
        btnGuardar.addEventListener('click', async function () {
            // 1. Validaciones básicas
            const clienteId = document.getElementById('clienteId').value;
            const numeroFactura = document.getElementById('numeroFactura').value;
            const fechaEmision = document.getElementById('fechaEmision').value;

            if (!clienteId) { Swal.fire('Atención', 'Debes seleccionar un cliente.', 'warning'); return; }
            if (!numeroFactura) { Swal.fire('Atención', 'El número de factura es obligatorio.', 'warning'); return; }

            // 2. Recopilar Líneas
            const lineasData = [];
            let errorLineas = false;

            document.querySelectorAll('.linea-factura').forEach(fila => {
                const concepto = fila.querySelector('.concepto').value.trim();
                const cantidad = parseFloat(fila.querySelector('.cantidad').value) || 0;
                const precio = parseFloat(fila.querySelector('.precio').value) || 0;

                if (concepto !== "" && cantidad > 0 && precio >= 0) {
                    lineasData.push({ Concepto: concepto, Cantidad: cantidad, PrecioUnitario: precio });
                } else {
                    errorLineas = true;
                }
            });

            if (lineasData.length === 0 || errorLineas) {
                Swal.fire('Atención', 'Revisa que todas las líneas tengan concepto, cantidad y precio válido.', 'warning');
                return;
            }

            // 3. Montar el Payload JSON
            const facturaPayload = {
                ClienteId: parseInt(clienteId),
                NumeroFactura: numeroFactura,
                FechaEmision: fechaEmision,
                PorcentajeIva: 21,
                Lineas: lineasData
            };

            const urlPost = this.getAttribute('data-url-post');

            // 4. Cambiar estado del botón
            const textoOriginal = this.innerHTML;
            this.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i> Procesando...';
            this.disabled = true;

            // 5. Petición AJAX (Fetch)
            try {
                const response = await fetch(urlPost, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(facturaPayload)
                });

                // Esperamos la respuesta JSON de tu controlador
                const data = await response.json();

                if (response.ok && data.exito) {
                    Swal.fire({
                        title: '¡Éxito!',
                        text: 'Factura emitida y contabilizada.',
                        icon: 'success',
                        allowOutsideClick: false,
                        confirmButtonText: 'Ver Factura'
                    }).then(() => {
                        // Redirigimos usando la URL que nos dio el controlador
                        window.location.href = data.urlRedireccion;
                    });
                } else {
                    Swal.fire('Error', data.mensaje || 'Hubo un error al procesar la factura.', 'error');
                    this.innerHTML = textoOriginal;
                    this.disabled = false;
                }
            } catch (error) {
                console.error('Error:', error);
                Swal.fire('Error de Red', 'No se pudo conectar con el servidor.', 'error');
                this.innerHTML = textoOriginal;
                this.disabled = false;
            }
        });
    }
});