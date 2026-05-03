document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("#modal-add-department form");

    if (!form) return;

    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        const btn = document.getElementById("btnCrearDepartamento");
        const urlPost = this.getAttribute('data-url-post');
        const urlRedirect = this.getAttribute('data-url-redirect');

        const formData = new FormData(form);

        const nombre = formData.get("Nombre");
        const presupuestoAnual = formData.get("PresupuestoAnual");

        if (!nombre || !presupuestoAnual) {
            Swal.fire({
                icon: "warning",
                title: "Campos obligatorios",
                text: "Debes rellenar todos los campos."
            });
            return;
        }

        try {
            const response = await fetch(urlPost, {
                method: "POST",
                body: formData
            });

            if (response.ok) {
                await Swal.fire({
                    icon: "success",
                    title: "¡Éxito!",
                    text: "Departamento creado correctamente.",
                    timer: 3000,
                    showConfirmButton: true,
                    showProgressTimerBar: true
                });
                window.location.href = urlRedirect;
            }

        } catch (error) {
            Swal.fire({
                icon: "error",
                title: "Error inesperado",
                text: "Ha ocurrido un error en la petición."
            });

        }

    })
})