document.addEventListener("DOMContentLoaded", function () {

    const deleteButtons = document.querySelectorAll(".btn-delete");

    if (!deleteButtons.length) return;

    // Buscamos el token de forma segura
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : '';

    deleteButtons.forEach(btn => {

        btn.addEventListener("click", async function (e) {

            e.preventDefault();

            const url = this.dataset.url;
            const name = this.dataset.name;

            const confirmado = await SweetAlertService.confirmDelete(name);

            if (!confirmado) return;

            SweetAlertService.loading("Eliminando...");

            try {
                // SOLUCIÓN: Empaquetamos el token en un FormData simulando un formulario real
                const formData = new FormData();
                if (token) {
                    formData.append("__RequestVerificationToken", token);
                }

                const response = await fetch(url, {
                    method: "POST",
                    body: formData // Enviamos el FormData en el body, NO usamos headers
                });

                if (response.ok) {
                    window.location.reload();
                } else {
                    // Si el servidor manda un 400 por integridad referencial (ej. tiene nóminas)
                    const errorText = await response.text();

                    Swal.fire({
                        icon: "error",
                        title: "No se pudo eliminar",
                        text: errorText || "El registro no se ha podido eliminar. Es posible que tenga datos asociados."
                    });
                }

            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: "error",
                    title: "Error inesperado",
                    text: "No se ha podido conectar con el servidor."
                });
            }

        });

    });

});