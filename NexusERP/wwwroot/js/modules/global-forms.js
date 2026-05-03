
document.addEventListener("DOMContentLoaded", function () {

    const crudForms = document.querySelectorAll(".form-crud");

    crudForms.forEach(form => {
        form.addEventListener("submit", async function (e) {

            if (!this.checkValidity()) {
                return; s
            }

            const necesitaConfirmacion = this.dataset.confirm === "true";
            const mensajeConfirmacion = this.dataset.confirmText || "¿Estás seguro de guardar los cambios?";

            if (necesitaConfirmacion) {
                e.preventDefault(); 

                const confirmado = await SweetAlertService.confirm("Confirmar acción", mensajeConfirmacion);

                if (confirmado) {
                    SweetAlertService.loading("Guardando datos...");
                    this.submit(); 
                }
            } else {
                SweetAlertService.loading("Procesando...");
            }
        });
    });

});