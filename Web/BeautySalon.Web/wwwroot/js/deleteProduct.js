function deleteProduct() {
    let deleteButtons = [...document.getElementsByClassName("delete-button")];

    deleteButtons.forEach(button => {
        button.addEventListener("click", deleteProduct);
    });

    function deleteProduct(event) {
        let productId = event.target.parentElement.parentElement.children[1].children[0].id;
        let procedureId = document.getElementById("procedure-id").innerText;

        var token = $("#form-delete-products input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Administration/Procedures/DeleteProcedureProduct/",
            type: "POST",
            data: JSON.stringify({ productId, procedureId }),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '';
                let counter = 1;
                data.products.forEach(product => {
                    result += '<tr class="content">' + `<th scope="row">${counter}</th>` + `<td><a id="${product.Id}" >${product.name}</a></td>` + '<td><a class="btn btn-outline-danger p-2 px-2 py-2 ml-md-2 delete-button">Delete</a></td>' + '</tr>';

                    counter++;
                });

                $('#tbody-procedure-products').html(result);
                window.location.reload();
            }
        });
    }
}
