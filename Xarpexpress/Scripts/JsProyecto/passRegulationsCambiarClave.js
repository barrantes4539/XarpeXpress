// Función para verificar las regulaciones de la contraseña
function verificarRegulacionesContraseña() {
    const contraseña = document.querySelector('input[type="password"]').value;
    const checkmarks = document.querySelectorAll('.checkmark');
    const crossmarks = document.querySelectorAll('.crossmark');
    const formulario = document.querySelector('.auth-form');
    const botonRegistrar = document.querySelector('.btnCambiarClave-auth');

    // Verificar longitud de la contraseña
    if (contraseña.length >= 8) {
        checkmarks[0].style.display = 'block';
        crossmarks[0].style.display = 'none';
    } else {
        checkmarks[0].style.display = 'none';
        crossmarks[0].style.display = 'block';
    }

    // Verificar letras mayúsculas y minúsculas
    if (/[a-z]/.test(contraseña) && /[A-Z]/.test(contraseña)) {
        checkmarks[1].style.display = 'block';
        crossmarks[1].style.display = 'none';
    } else {
        checkmarks[1].style.display = 'none';
        crossmarks[1].style.display = 'block';
    }

    // Verificar carácter especial y número
    if (/[!@#$%^&*(),.?":{}|<>]/.test(contraseña) && /\d/.test(contraseña)) {
        checkmarks[2].style.display = 'block';
        crossmarks[2].style.display = 'none';
    } else {
        checkmarks[2].style.display = 'none';
        crossmarks[2].style.display = 'block';
    }

    // Habilitar o deshabilitar el botón de submit según las regulaciones
    if (
        checkmarks[0].style.display === 'block' &&
        checkmarks[1].style.display === 'block' &&
        checkmarks[2].style.display === 'block'
    ) {
        botonRegistrar.disabled = false;
    } else {
        botonRegistrar.disabled = true;
    }
}

// Agregar evento de escucha al campo de contraseña
const contraseñaInput = document.querySelector('input[type="password"]');
contraseñaInput.addEventListener('input', verificarRegulacionesContraseña);
