document.addEventListener("DOMContentLoaded", function () {
    const editButtons = document.querySelectorAll(".btn-edit");

    editButtons.forEach(btn => {
        btn.addEventListener("click", function () {
            // Cogemos los datos del botón
            const id = this.dataset.id;
            const nombre = this.dataset.nombre;
            const presupuesto = this.dataset.presupuesto;

            // Rellenamos el Modal de Edición
            document.getElementById("editId").value = id;
            document.getElementById("editNombre").value = nombre;
            document.getElementById("editPresupuesto").value = presupuesto;
        });
    });
});