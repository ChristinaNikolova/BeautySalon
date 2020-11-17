function book() {
    $(function () {
        $("#datepicker").datepicker({
            startDate: "+1d",
            endDate: "+4m",
            weekStart: 1,
            daysOfWeekDisabled: "0",
            defaultDate: "",
            autoclose: true
        });
    });

    let categoryId = "";
    let clientSkinTypeId = document.getElementById("skin-type-id").innerText;

    document.getElementById("clear-appointment-search").addEventListener("click", clear);
    document.getElementById("category").addEventListener("change", loadStylists);
    document.getElementById("stylist").addEventListener("change", loadProcedures);

    function clear() {
        window.location.reload();
    }

    function clearData() {
        document.getElementById("procedure").innerHTML = "";
        document.getElementById("datepicker").value = "";
        document.getElementById("time").innerHTML = "";
        document.getElementById("smart-search-message").style.display = "none";
        document.getElementById("success-smart-search").style.display = "none";
    }

    function loadStylists(event) {
        clearData();

        Array.from(event.target.children).forEach(child => {
            if (child.selected === true) {
                categoryId = child.value;
            }
        });

        var token = $("#form input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Stylists/GetStylistsByCategory/",
            type: "POST",
            data: JSON.stringify(categoryId),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '<option selected>Please select stylist</option>';

                data.stylistNames.forEach(stylyst => {
                    result += `<option value="${stylyst.id}">${stylyst.fullName}</option>`
                });

                $('#stylist').html(result);
            }
        });
    }

    function loadProcedures(event) {
        clearData();

        if (categoryId === '5c18b9b0-edf7-4257-89d4-9000f2c1d0c3' && clientSkinTypeId !== "") {
            document.getElementById("smart-search-message").style.display = "block";
        } else {
            document.getElementById("smart-search-message").style.display = "none";
        }

        document.getElementById("smart-search-button").addEventListener("click", showSmartSearchNavBar);

        function showSmartSearchNavBar() {
            document.getElementById("success-smart-search").style.display = "block";

            let isSkinSensitive = document.getElementById("sensitive-skin").innerText;

            let stylistId = "";
            let parentStylist = document.getElementById("parent-stylist");

            Array.from(parentStylist.children[1].children).forEach(child => {
                if (child.selected === true) {
                    stylistId = child.value;
                }
            });

            var token = $("#form input[name=__RequestVerificationToken]").val();

            $.ajax({
                url: "/Procedures/SmartSearchProcedures/",
                type: "POST",
                data: JSON.stringify({ clientSkinTypeId, isSkinSensitive, stylistId }),
                contentType: "application/json; charset=utf-8",
                headers: { 'X-CSRF-TOKEN': token },
                dataType: "json",
                success: function (data) {
                    let result = '<option selected>Please select procedure</option>';

                    data.procedureNames.forEach(procedure => {
                        result += `<option value="${procedure.procedureId}">${procedure.procedureName}</option>`
                    });

                    $('#procedure').html(result);
                }
            });
        }

        let stylistId = "";

        Array.from(event.target.children).forEach(child => {
            if (child.selected === true) {
                stylistId = child.value;
            }
        });

        var token = $("#form input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Procedures/GetProceduresByStylist/",
            type: "POST",
            data: JSON.stringify(stylistId),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '<option selected>Please select procedure</option>';

                data.procedureNames.forEach(procedure => {
                    result += `<option value="${procedure.procedureId}">${procedure.procedureName}</option>`
                });

                $('#procedure').html(result);
            }
        });
    }

    $('#datepicker').on('changeDate', function () {
        let selectedDate = document.getElementById("datepicker").value;
        let selectedStylistId = document.getElementById("stylist").value;

        var token = $("#form input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/Appointments/GetFreeTimes/",
            type: "POST",
            data: JSON.stringify({ selectedDate, selectedStylistId }),
            contentType: "application/json; charset=utf-8",
            headers: { 'X-CSRF-TOKEN': token },
            dataType: "json",
            success: function (data) {
                let result = '<option value="" > Please select Time</option>';

                data.freeHours.forEach(hour => {
                    result += `<option value="${hour}">${hour}</option>`
                })

                $('#time').html(result);
            }
        });
    });
}
