document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById('globalSearchInput');
    const dropdown = document.getElementById('searchResultsDropdown');
    let debounceTimer;

    if (!searchInput || !dropdown) return;

    // Cuando el usuario teclea
    searchInput.addEventListener('input', function () {
        clearTimeout(debounceTimer);
        const query = this.value.trim();
        const url = this.getAttribute('data-url') + `?q=${encodeURIComponent(query)}`;

        if (query.length < 2) {
            dropdown.classList.add('d-none');
            return;
        }

        // "Debounce": Esperamos 300ms después de que deje de teclear para no acribillar al servidor
        debounceTimer = setTimeout(async () => {
            try {
                // Pequeño indicador de carga (opcional)
                dropdown.innerHTML = '<div class="p-3 text-center text-muted small"><i class="fas fa-spinner fa-spin"></i> Buscando...</div>';
                dropdown.classList.remove('d-none');

                const response = await fetch(url);
                const data = await response.json();

                renderSearchResults(data);
            } catch (error) {
                console.error("Error en búsqueda global:", error);
                dropdown.innerHTML = '<div class="p-3 text-center text-danger small">Error de conexión</div>';
            }
        }, 300);
    });

    // Cerrar el desplegable si hace clic fuera
    document.addEventListener('click', function (e) {
        if (!searchInput.contains(e.target) && !dropdown.contains(e.target)) {
            dropdown.classList.add('d-none');
        }
    });

    // =========================================================
    // DICCIONARIO DE ICONOS SVG NATIVOS (Profesionales y Sobrios)
    // =========================================================
    const SVG_ICONS = {
        'persona': `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path></svg>`,

        'cliente': `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"></path></svg>`,

        'factura': `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>`,

        // Icono por defecto por si acaso
        'default': `<svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>`
    };

    // =========================================================
    // Función para pintar el HTML con estilo profesional y SVGs
    // =========================================================
    function renderSearchResults(resultados) {
        if (resultados.length === 0) {
            dropdown.innerHTML = '<div class="p-4 text-center text-muted small"><i class="fas fa-search me-2 opacity-50"></i> No se han encontrado resultados coincidentes.</div>';
            return;
        }

        let html = '';
        let currentCategory = '';

        resultados.forEach(item => {
            // Agrupamos por categoría con el nuevo estilo CSS
            if (item.categoria !== currentCategory) {
                html += `<div class="search-result-category">${item.categoria}s</div>`;
                currentCategory = item.categoria;
            }

            // Obtenemos el SVG correspondiente del diccionario
            const svgIcon = SVG_ICONS[item.icono] || SVG_ICONS['default'];

            // Generamos la fila de resultado con la nueva estructura HTML profesional
            html += `
                <a href="${item.url}" class="search-result-item">
                    <div class="search-result-icon-wrapper">
                        ${svgIcon}
                    </div>
                    <div class="search-result-text-container">
                        <div class="search-result-title">${item.titulo}</div>
                        <div class="search-result-subtitle">${item.subtitulo}</div>
                    </div>
                </a>
            `;
        });

        dropdown.innerHTML = html;
    }
});