document.addEventListener("DOMContentLoaded", function () {

    const form = document.querySelector("#loginForm");

    if (!form) return;

    form.addEventListener("submit", function () {

        Swal.fire({
            title: "Iniciando sesión...",
            allowOutsideClick: false,
            allowEscapeKey: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

    });

});