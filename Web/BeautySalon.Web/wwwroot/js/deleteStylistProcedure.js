function deleteStylistProcedure() {
    let deleteButtons = [...document.getElementsByClassName("delete-button")];

    deleteButtons.forEach(button => {
        button.addEventListener("click", deleteProcedure);
    });

    function deleteProcedure(event) {
        let procedureId = event.target.parentElement.parentElement.children[1].children[0].id;
        let stylistId = document.getElementById("stylist-id").innerText;

        var token = $("#form-delete-procedure input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Administration/Stylists/DeleteStylistProcedure/",
            type: "POST",
            data: JSON.stringify({ procedureId, stylistId }),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '';
                let counter = 1;
                data.procedures.forEach(procedure => {
                    result += '<tr class="content">' + `<th scope="row">${counter}</th>` + `<td><a id="${procedure.Id}" >${procedure.name}</a></td>` + '<td><a class="btn btn-outline-danger p-2 px-2 py-2 ml-md-2 delete-button">Delete</a></td>' + '</tr>';

                    counter++;
                });

                $('#tbody-stylist-procedures').html(result);
                window.location.reload();
            }
        });
    }
}