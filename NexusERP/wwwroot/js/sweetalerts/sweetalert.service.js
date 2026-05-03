window.SweetAlertService = {

    show: function (alert) {

        if (alert.toast) {

            Swal.fire({
                toast: true,
                position: 'top-end',
                icon: alert.type,
                title: alert.text,
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true
            });

            return;
        }

        Swal.fire({
            icon: alert.type,
            title: alert.title,
            text: alert.text,
            confirmButtonText: "Aceptar"
        });

    },


    success: async function (title, text, redirectUrl = null) {

        await Swal.fire({
            icon: "success",
            title: title,
            text: text,
            confirmButtonText: "Aceptar",
            confirmButtonColor: "#3085d6"
        });

        if (redirectUrl) {
            window.location.href = redirectUrl;
        }
    },

    error: function (title, text) {

        Swal.fire({
            icon: "error",
            title: title,
            text: text,
            confirmButtonColor: "#d33"
        });
    },

    warning: function (title, text) {

        Swal.fire({
            icon: "warning",
            title: title,
            text: text
        });
    },

    confirm: async function (title, text) {

        const result = await Swal.fire({
            icon: "question",
            title: title,
            text: text,
            showCancelButton: true,
            confirmButtonText: "Confirmar",
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#3085d6"
        });

        return result.isConfirmed;
    },

    loading: function (text = "Procesando...") {

        Swal.fire({
            title: text,
            allowOutsideClick: false,
            allowEscapeKey: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

    },

    confirmDelete: async function (name = "") {

        const result = await Swal.fire({
            icon: "warning",
            title: "Eliminar registro",
            text: name
                ? `¿Seguro que quieres eliminar "${name}"?`
                : "¿Seguro que quieres eliminar este registro?",
            showCancelButton: true,
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#d33"
        });

        return result.isConfirmed;
    },

};