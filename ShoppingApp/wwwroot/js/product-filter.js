
$('.js-reload-details').on('click', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();
    console.log("hi");
    var $detailDiv = $('#detailsDiv'),
        url = $(this).data('url');

    $.get(url, function (data) {
        $('#detailsDiv').html(data);
    });

});


function SearchProducts() {
    const input = document.getElementById("filter").value.toUpperCase();
    const CardContainer = document.getElementById("card-lists");

    const cards = CardContainer.getElementsByClassName("card");
    for (i = 0; i < cards.length; i++) {

        let title = cards[i].querySelector(".card-body h5.card-title");
        if (title.innerText.toUpperCase().indexOf(input) > -1) {
            cards[i].style.display = "";
        }
        else {
            cards[i].style.display = "none";
        }


    }

}