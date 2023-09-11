function getCategory(productName) {
    var categories = {
        'pilsen 6.0': 'CatCervezas',
        'imperial': 'CatCervezas',
        'daiquiri de piña': 'CatdeLaCasa',
        'refresco especial de sandía': 'CatdeLaCasa',
        'xarpe': 'CatdeLaCasa',
        'cuervo amigable': 'CatMaterialCoct',
        'fresa ardiente': 'CatLicoresPremium'
    };

    return categories[productName];
}

document.addEventListener('DOMContentLoaded', function () {
    var filter = document.getElementById('filter');
    var searchInput = document.getElementById('search');
    var productList = document.getElementById('product-list');
    var cards = productList.getElementsByClassName('card');

    searchInput.addEventListener('input', function () {
        var searchValue = searchInput.value.toLowerCase();

        for (var i = 0; i < cards.length; i++) {
            var card = cards[i];
            var productName = card.querySelector('.card-title').textContent.toLowerCase();
            var category = getCategory(productName);

            if (productName.includes(searchValue) && (filter.value === 'todos' || category === filter.value)) {
                card.style.display = 'flex';
            } else {
                card.style.display = 'none';
            }
        }
    });

    filter.addEventListener('change', function () {
        var searchValue = searchInput.value.toLowerCase();
        var selectedValue = filter.value;

        for (var i = 0; i < cards.length; i++) {
            var card = cards[i];
            var productName = card.querySelector('.card-title').textContent.toLowerCase();
            var category = getCategory(productName);

            if (productName.includes(searchValue) && (selectedValue === 'todos' || category === selectedValue)) {
                card.style.display = 'flex';
            } else {
                card.style.display = 'none';
            }
        }
    });
});
