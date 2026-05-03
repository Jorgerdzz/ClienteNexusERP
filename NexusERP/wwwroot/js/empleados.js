document.addEventListener("DOMContentLoaded", function () {

    const editButtons = document.querySelectorAll(".btn-edit");

    editButtons.forEach(btn => {
        btn.addEventListener("click", function () {

            // 1. Averiguamos qué modal se va a abrir según el botón pulsado
            const targetModalId = this.getAttribute("data-bs-target");
            const modal = document.querySelector(targetModalId);

            if (!modal) return;

            // 2. Creamos un buscador que solo mira dentro de ESE modal específico
            const setValue = (inputName, value) => {
                const input = modal.querySelector(`[name="${inputName}"]`);
                if (input) {
                    input.value = value || "";
                }
            };

            // 3. Rellenamos los datos usando los nombres de los inputs
            setValue("Id", this.dataset.id);

            // Datos Personales
            setValue("Nombre", this.dataset.nombre);
            setValue("Apellidos", this.dataset.apellidos);
            setValue("DNI", this.dataset.dni);
            setValue("EmailCorporativo", this.dataset.email);
            setValue("Telefono", this.dataset.telefono);
            setValue("FechaNacimiento", this.dataset.fechanacimiento);

            // Datos Laborales
            setValue("DepartamentoId", this.dataset.departamentoid);
            setValue("NumSeguridadSocial", this.dataset.numss);
            setValue("FechaAntiguedad", this.dataset.fechaantiguedad);
            setValue("GrupoCotizacion", this.dataset.grupo);

            // Ojo: Asegúrate de que el salario no viene con comas de miles desde C#.
            // En tu botón tienes .ToString("0.00", CultureInfo.InvariantCulture) lo cual es CORRECTO.
            setValue("SalarioBrutoAnual", this.dataset.salario);

            setValue("IBAN", this.dataset.iban);
            setValue("Activo", this.dataset.activo);

            // Datos IRPF
            setValue("EstadoCivil", this.dataset.estadocivil);
            setValue("NumeroHijos", this.dataset.hijos || "0");
            setValue("PorcentajeDiscapacidad", this.dataset.discapacidad || "0");

        });
    });

});