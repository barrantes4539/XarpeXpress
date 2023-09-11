const header = document.querySelector("header");

window.addEventListener("scroll", function () {
    header.classList.toggle("sticky", this.window.scrollY > 0);
});

let menu = document.querySelector('#menu-icon');
let navmenu = document.querySelector('.navmenu');

menu.onclick = () => {
    navmenu.classList.toggle('hidden');
};

// Agrega el evento de clic para abrir o cerrar el menú desplegable
document.querySelectorAll('.dropdown-toggle').forEach(function (toggle) {
    toggle.addEventListener('click', function (event) {
        event.preventDefault();
        var parent = toggle.parentNode;
        parent.classList.toggle('open');
    });
});

// Cierra el menú desplegable cuando se hace clic fuera de él
document.addEventListener('click', function (event) {
    var target = event.target;
    if (!target.matches('.dropdown-toggle')) {
        document.querySelectorAll('.dropdown.open').forEach(function (dropdown) {
            dropdown.classList.remove('open');
        });
    }
});
