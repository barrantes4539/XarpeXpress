document.querySelector(".purchase--btn").addEventListener("click", function (event) {
    event.preventDefault();
    const fullName = document.querySelector(".input_field-pago[name='input-name']").value;
    const cardNumber = document.querySelector(".input_field-pago[name='input-name']").value;
    const expiryDate = document.querySelector(".input_field-pago[name='input-name']").value;
    const cvv = document.querySelector(".input_field-pago[name='cvv']").value;

    if (fullName === "" || cardNumber === "" || expiryDate === "" || cvv === "") {
        showAlert("Por favor rellene todos los campos.", "error");

    } else {
        showAlert("Compra exitosa!", "success");
        // Aquí puedes realizar otras acciones relacionadas con la compra exitosa
    }
});

function showAlert(message, type) {
    const alertContainer = document.getElementById("alert-container");
    alertContainer.innerHTML = `<div class="alert--${type}">${message}</div>`;
    alertContainer.style.display = "block";

    setTimeout(function () {
        alertContainer.style.display = "none";
    }, 3000);
}



