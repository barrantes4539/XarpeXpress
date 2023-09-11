// Obtener todos los elementos del progress bar
const progressBars = document.querySelectorAll('.progress');

// Obtener todos los elementos de porcentaje
const percentages = document.querySelectorAll('.percentage');

// Valores de porcentaje iniciales y velocidades de incremento
const progressValues = [
    { value: 0, increment: 75 },   // Valor inicial y velocidad de incremento para la primera barra
    { value: 0, increment: 15 },  // Valor inicial y velocidad de incremento para la segunda barra
    { value: 0, increment: 85 }   // Valor inicial y velocidad de incremento para la tercera barra
];

// Función para actualizar los valores de porcentaje y los anchos de los progress bars
function updateProgressBars() {
    for (let i = 0; i < progressBars.length; i++) {
        const progressBar = progressBars[i];
        const percentage = percentages[i];

        progressValues[i].value += progressValues[i].increment; // Aumentar el valor del porcentaje

        // Limitar el valor del porcentaje a 100
        if (progressValues[i].value > 100) {
            progressValues[i].value = 100;
        }

        // Actualizar el texto del porcentaje
        percentage.textContent = progressValues[i].value + '%';

        // Actualizar el ancho del progress bar
        progressBar.style.width = progressValues[i].value + '%';
    }
}

// Llamar a la función para actualizar los progress bars (por ejemplo, cada vez que haya un evento de aumento de métrica)
updateProgressBars();
