
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInput');
    const tableBody = document.getElementById('empleadosTableBody');

    if (!searchInput || !tableBody) return;

    searchInput.addEventListener('keyup', function () {
        const searchTerm = this.value.toLowerCase().trim();

        const rows = tableBody.getElementsByTagName('tr');

        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];

            if (row.cells.length === 1 && row.cells[0].hasAttribute('colspan')) continue;

            const cellName = row.cells[0];

            if (cellName) {
                const textContent = cellName.textContent || cellName.innerText;

                if (textContent.toLowerCase().includes(searchTerm)) {
                    row.style.display = "";
                } else {
                    row.style.display = "none";
                }
            }
        }
    });
});